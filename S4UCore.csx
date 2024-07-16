#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UEffect.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UUtil.csx"
#nullable enable

//#define CamAutoUsePad

public enum LoadingWaitMethod
{
    Legacy,
    Modan_A,
    Modan_B
}

public class S4UCore
{
    private const int LOOP_INTERVAL_MS = 50;
    
    private readonly S4UEffect s4uEffect;

    private int keyDownAfterWait = 0;
    private int keyUpAfterWait = 0;
    
    private static readonly Keys[] ABORT_KEYS = { Keys.Escape };

    protected readonly IGlobals _globals;
    private readonly string? songName;

    private readonly List<Data> datas = new List<Data>();

    private DateTime startTime;

    private Boolean recording = false;

    private Boolean processing = false;
    private Boolean abort = false;

    private long globalShift = 0;

    private readonly EventHandler<KeyStateChangedEventArgs> keyStateChangedHandler;

    private LoadingWaitMethod loadingWaitMethod = LoadingWaitMethod.Modan_B;
    private Point waitLoadingCheckColorPoint;


    private readonly MappingSources[] vxinputMappingSources = new[]
    {
        MappingSources.LeftStickX,
        MappingSources.LeftStickY,
        MappingSources.DPad
    };
    private readonly int vxInputDeviceNo = 0;
    private readonly IVirtualXInputDevice controller;
    
    private int idolUnitSize = 5;
    private Point[] idolCamTargetValue = {};
    
    private StringBuilder recordingClipboardStringBuilder = new StringBuilder();

    public S4UCore(IGlobals globals, string? songName = null)
    {
        this._globals = globals;
        this.songName = songName;
        this.keyStateChangedHandler = this.CreateKeyStateChangedHandler();
        this.s4uEffect = new S4UEffect(_globals);
        this.controller = _globals.VirtualXInput.GetController(userIndex: this.vxInputDeviceNo);
        var screenBounds = _globals.Screen.PrimaryScreen.Bounds;
        this.waitLoadingCheckColorPoint = new Point(screenBounds.CenterX, screenBounds.CenterY);
        this.SetIdolUnitSize(5);
    }

    private EventHandler<KeyStateChangedEventArgs> CreateKeyStateChangedHandler()
    {
        return (sender, e) =>
        {
            var now = DateTime.Now;
            if (e.IsInjected == true || e.IsCancel || e.IsPressed == false)
            {
                return;
            }

            if (this.processing == true)
            {
                foreach (var abortKey in ABORT_KEYS)
                {
                    if (e.Key == abortKey)
                    {
                        this.abort = true;
                        this.recording = false;
                        _globals.Cancel();
                        return;
                    }
                }
            }

            if (this.recording == false)
            {
                return;
            }
            
            if (e.Key == Keys.LButton || e.Key == Keys.RButton) {
                return;
            }

            TimeSpan elapsed = now - startTime;
            string text = this.LogETapCode(elapsed.TotalMilliseconds, e.Key);
            Console.WriteLine(text);
            this.recordingClipboardStringBuilder.AppendLine(text);
        };
    }
    
    private void SetRecording(bool recording) {
        if (recording) {
            this.recordingClipboardStringBuilder = new StringBuilder();
            this.recording = true;
        } else {
            this.recording = false;
            string text = this.recordingClipboardStringBuilder.ToString();
            if (text.Length > 0) {
                _globals.Clipboard.SetText(text);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("手動演出をクリップボードにコピーしました。");
                Console.ResetColor();
            }
        }
    }
    
    public void SetWaitLoadingCheckColorPoint(int x, int y) {
        this.waitLoadingCheckColorPoint = new Point(x, y);
    }
    
    public int GetIdolUnitSize() {
        return this.idolUnitSize;
    }
    
    public void SetIdolUnitSize(int size) {
        this.idolUnitSize = size;
        this.idolCamTargetValue = new Point[size];
        switch(size) {
            case 1:
                this.idolCamTargetValue[0] = new Point(0, 0);
                break;
            case 2:
                this.idolCamTargetValue[0] = new Point(1, 0);
                this.idolCamTargetValue[1] = new Point(-1, 0);
                break;
            case 3:
                this.idolCamTargetValue[0] = new Point(0, 1);
                this.idolCamTargetValue[1] = new Point(1, -1);
                this.idolCamTargetValue[2] = new Point(-1, -1);
                break;
            case 4:
                this.idolCamTargetValue[0] = new Point(1, 1);
                this.idolCamTargetValue[1] = new Point(1, -1);
                this.idolCamTargetValue[2] = new Point(-1, -1);
                this.idolCamTargetValue[3] = new Point(-1, 1);
                break;
            case 5:
                this.idolCamTargetValue[0] = new Point(0, 1);
                this.idolCamTargetValue[1] = new Point(1, 0);
                this.idolCamTargetValue[2] = new Point(1, -1);
                this.idolCamTargetValue[3] = new Point(-1, -1);
                this.idolCamTargetValue[4] = new Point(-1, 0);
                break;
        }
    }
    
    public S4UEffect GetEffect() {
        return this.s4uEffect;
    }

    public Keys[] KeysArray(params Keys[] keys)
    {
        return keys;
    }

    public void SetGlobalETapKeyDownAfterWait(int waitMs) {
        this.keyDownAfterWait = waitMs;
    }

    public void SetGlobalETapKeyUpAfterWait(int waitMs) {
        this.keyUpAfterWait = waitMs;
    }

    public void SetGlobalShift(long msec)
    {
        this.globalShift = msec;
    }

    public void SetLoadingWaitMethod(LoadingWaitMethod loadingWaitMethod)
    {
        this.loadingWaitMethod = loadingWaitMethod;
    }

    public LoadingWaitMethod GetLoadingWaitMethod()
    {
        return this.loadingWaitMethod;
    }

    public void ETap(long msec, Keys key, int loop = 0, int loopIntervalMs = LOOP_INTERVAL_MS, int keyUpMs = -1, int keyDownMs = -1)
    {
        this.ETap(msec, new Keys[] { key }, loop, loopIntervalMs, keyUpMs, keyDownMs);
    }

    public void ETap(long msec, Keys[] key, int loop = 0, int loopIntervalMs = LOOP_INTERVAL_MS, int keyUpMs = -1, int keyDownMs = -1)
    {
        int upMs = keyUpMs == -1 ? keyUpAfterWait : keyUpMs;
        int downMs = keyDownMs == -1 ? keyDownAfterWait : keyDownMs;
        
        this.datas.Add(new Data(msec + this.globalShift, key, loop, loopIntervalMs, upMs, downMs));
    }

    private class Data
    {
        public long msec;
        public Keys[] key;
        public int loop;
        public int loopIntervalMs;
        public int keyDownMs;
        public int keyUpMs;

        public Data(long msec, Keys[] key, int loop, int loopIntervalMs, int keyUpMs, int keyDownMs)
        {
            this.msec = msec;
            this.key = key;
            this.loop = loop;
            this.loopIntervalMs = loopIntervalMs;
            this.keyDownMs = keyDownMs;
            this.keyUpMs = keyUpMs;
        }
    }

    public void Recording()
    {
        this.ProcessCore(true);
    }

    public void Execute()
    {
        this.ProcessCore(false);
    }
    
    private void ProcessCore(bool rec) {
        Console.Clear();
        
        string logId;
        if (rec) {
            logId = "REC";
        } else {
            logId = "LIVE";
        }
        
        if (!rec) {
            if (this.songName != "")
            {
                Console.WriteLine("♪ " + this.songName);
                this.LogUnitSize();
            }
        }
        this.LogAbortKeyInfo();
        if (!rec) {
            this.CheckDataTimeline();
        }
        Console.WriteLine($"=== S4U {logId} Loading...");
        try
        {
            _globals.VirtualXInput.SetMappingState(this.vxInputDeviceNo, this.vxinputMappingSources, state: false);
            this.abort = false;
            this.processing = true;
            _globals.KeyStateChanged += this.keyStateChangedHandler;
            if (!this.WaitLoading()) {
                return;
            }
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"=== S4U {logId} Start");
            Console.ResetColor();
            
            if (rec) {
                this.SetRecording(true);
                while (this.recording)
                {
                    _globals.Wait(2000);
                }
            } else {
                this.SetRecording(true);
                this.Start();
            }
        }
        finally
        {
            if (this.abort) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("!!! ABORT DETECTED...");
                Console.ResetColor();
            }
            this.controller.LeftStick.SetValue(horizontal: 0, vertical: 0);
            _globals.VirtualXInput.SetMappingState(this.vxInputDeviceNo, this.vxinputMappingSources, state: true);
            _globals.KeyStateChanged -= this.keyStateChangedHandler;
            this.SetRecording(false);
            this.processing = false;
            if (!rec) {
                this.LogNumPadKeyUsage();
            }
            if (!rec) {
                this.CheckDataTimeline();
            }
            Console.WriteLine($"=== S4U {logId} End");
        }

    }

    private void Start()
    {
        foreach (Data data in this.datas)
        {
            var nextTime = this.startTime.AddMilliseconds(data.msec);
            var span = nextTime - DateTime.Now;
            if (span.TotalMilliseconds > 0)
            {
                _globals.Wait((int)span.TotalMilliseconds);
            }
            if (this.abort)
            {
                break;
            }
            if (data.loop > 0)
            {
                for (int i = 0; i < data.loop; i++)
                {
                    Tap(data.key, data.keyDownMs, data.keyUpMs);
                    if (data.loopIntervalMs > 0)
                    {
                        _globals.Wait(data.loopIntervalMs);
                    }
                }
            }
            else
            {
                Tap(data.key, data.keyDownMs, data.keyUpMs);
            }
        }
    }

    private void Tap(Keys[] keys, int keyDownMs, int keyUpMs)
    {
        var span = DateTime.Now - this.startTime;
        foreach (Keys key in keys)
        {
            if (Keys.F1 <= key && key <= Keys.F5) {
                Point point = this.idolCamTargetValue[key - Keys.F1];
                this.controller.LeftStick.SetValue(horizontal: point.X, vertical: point.Y);
                _globals.Wait(keyUpMs);
#if CamAutoUsePad
            } else if (key == Keys.D) {
                // Console.WriteLine($"{keyDownMs}, {keyUpMs}");
                this.controller.DPadRight.Tap(20, 0);
#endif
            } else {
                _globals.Tap(key, keyDownMs, keyUpMs);
            }
        }
        foreach (Keys key in keys)
        {
            Console.WriteLine(this.LogETapCode(span.TotalMilliseconds, key, "MACRO: "));
        }
    }

    private bool WaitLoading()
    {
        bool success = false;
        var (lastMouseX, lastMouseY) = _globals.GetCursorPosition();
        try {
            var bounds = _globals.Screen.PrimaryScreen.Bounds;
            _globals.Move(bounds.Right, bounds.Bottom, 10);
            _globals.BlockMouseMovement();

            switch(this.loadingWaitMethod) {
                case LoadingWaitMethod.Legacy:
                    success = this.WaitLoadingOldMethod();
                    break;
                case LoadingWaitMethod.Modan_A:
                    success = this.WaitLoadingNewMethodA();
                    break;
                case LoadingWaitMethod.Modan_B:
                    success = this.WaitLoadingNewMethodB();
                    break;
            }
            
            if (!success) {
                Console.WriteLine("!!! Loading の待ち合わせに失敗しました");
            }
        } finally {
            _globals.UnblockMouseMovement();
            //_globals.Move(lastMouseX, lastMouseY);
        }
        
        return success;
    }

    private bool WaitLoadingNewMethodA()
    {
        // この方式は1ループ平均15-17ms
        bool success = false;
        while (true)
        {
            var color = GetPixelColor(this.waitLoadingCheckColorPoint.X, this.waitLoadingCheckColorPoint.Y);
            if (color.R != 0 || color.G != 0 || color.B != 0)
            {
                break;
            }
            success = true;
        }
        this.startTime = DateTime.Now;
        return success;
    }

    private bool WaitLoadingNewMethodB()
    {
        // この方式は1ループ平均15-17ms
        bool success = false;
        while (true)
        {
            var color = GetPixelColor(this.waitLoadingCheckColorPoint.X, this.waitLoadingCheckColorPoint.Y);
            if (color.R != 0 || color.G != 0 || color.B != 0)
            {
                this.startTime = DateTime.Now;
                break;
            }
            success = true;
        }
        if (success) {
            return success;
        }
        
        // ローディング暗転前に実行してしまった場合は以下の処理を追加実行

        // まず、暗転を待って
        while (true)
        {
            var color = GetPixelColor(this.waitLoadingCheckColorPoint.X, this.waitLoadingCheckColorPoint.Y);
            if (color.R == 0 || color.G == 0 || color.B == 0)
            {
                break;
            }
            // この暗転待ちはリアルタイム性を必要としないので100ミリ待機
            _globals.Wait(100);
        }

        // 次にローディング終了を待つ
        success = false;
        while (true)
        {
            var color = GetPixelColor(this.waitLoadingCheckColorPoint.X, this.waitLoadingCheckColorPoint.Y);
            if (color.R != 0 || color.G != 0 || color.B != 0)
            {
                this.startTime = DateTime.Now;
                break;
            }
            success = true;
        }
        
        return success;
    }

    private bool WaitLoadingOldMethod()
    {
        // この方式は1ループ平均60ms
        bool success = false;
        while (true)
        {
            //var startLoopTime = DateTime.Now;
            if (_globals.Match("white", out var result))
            {
                //Console.WriteLine($"Score:{result.Score}");
                if (result.Score < 1000)
                {
                    break;
                }
                //if (LOAD_WAIT > 0)
                //{
                //    _globals.Wait(LOAD_WAIT);
                //}
            }
            else
            {
                break;
            }
            //var endLoopTime = DateTime.Now;
            //var loopDuration = endLoopTime - startLoopTime;
            //Console.WriteLine($"New Method Loop Duration: {loopDuration.TotalMilliseconds} ms");
            
            success = true;
        }
        this.startTime = DateTime.Now;
        return success;
    }

    private void CheckDataTimeline()
    {
        long lastTime = 0;
        int count = 0;
        Boolean warnFound = false;
        foreach (var data in this.datas)
        {
            count++;
            if (data.msec < lastTime)
            {
                if (!warnFound)
                {
                    Console.WriteLine();
                    warnFound = true;
                }
                string keysList = string.Join(",", data.key.Select(k => k.ToString()));
                Console.WriteLine($"!!! WARN: invalid data timeline: Data({data.msec - this.globalShift}, [ {keysList} ])");
            }
            lastTime = data.msec;
        }
        if (warnFound)
        {
            Console.WriteLine();
        }
    }
    
    private string LogETapCode(double msec, Keys key, string prefix = "")
    {
        string keyName = Enum.GetName(typeof(Keys), key);
        return $"{prefix}s4u.ETap({msec:#0}, Keys.{keyName});";
        //Console.WriteLine($"{prefix}s4u.ETap({msec:#0}, Keys.{keyName});");
    }

    private void LogAbortKeyInfo()
    {
        string keysList = string.Join(" or ", ABORT_KEYS.Select(k => k.ToString()));
        Console.WriteLine($"Abort press: {keysList}");
    }

    private void LogUnitSize() {
        Console.Write($"Unit {this.idolUnitSize} [");
        switch(this.idolUnitSize) {
            case 1:
                Console.Write("..1..");
                break;
            case 2:
                Console.Write(".12..");
                break;
            case 3:
                Console.Write(".213.");
                break;
            case 4:
                Console.Write("3124.");
                break;
            case 5:
                Console.Write("42135");
                break;
            default:
                Console.Write("!!!!!");
                break;
        }
        //for(int i = 0; i<this.idolUnitSize; i++) {
        //    Console.Write("@");
        //}
        //for(int i = 0; i<5-this.idolUnitSize; i++) {
        //    Console.Write(".");
        //}
        Console.WriteLine("]");
    }

    private void LogNumPadKeyUsage()
    {
        Console.WriteLine("=== S4U LIVE Used numpad key list");
        bool[] numPadKeysUsed = new bool[8];

        foreach (var data in this.datas)
        {
            foreach (var key in data.key)
            {
                if (key >= Keys.NumPad1 && key <= Keys.NumPad8)
                {
                    int index = key - Keys.NumPad1;
                    numPadKeysUsed[index] = true;
                }
            }
        }

        Console.Write("Used: ");
        for (int i = 0; i < numPadKeysUsed.Length; i++)
        {
            if (numPadKeysUsed[i])
            {
                Console.Write((i + 1).ToString());
            }
            else
            {
                Console.Write("*");
            }
        }
        Console.WriteLine();

        Console.Write("Free: ");
        for (int i = 0; i < numPadKeysUsed.Length; i++)
        {
            if (!numPadKeysUsed[i])
            {
                Console.Write((i + 1).ToString());
            }
            else
            {
                Console.Write("*");
            }
        }
        Console.WriteLine();
    }
}