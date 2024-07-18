#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UUtil.csx"

//#define STAGE_FOR_YOUメイン画面チェック

public class S4UEffect
{
    private const string CONSOLE_WINDOW_TITLE = "KeyToKey - Console ／ 非表示：Ctrl + C";

    private readonly IGlobals _globals;

    private int waitTimeAfterDown = 10;
    private int waitTimeAfterUp = 50;
    private int waitTimeAfterEnteringItemList = 100;
    private int waitTimeBeforeEnteringCustom = 400;
    private int waitTimeAfterEnteringCustom = 2000;
    private int waitSearchCurrentItemAfterScroll = 300;

    private readonly Point customIconPoint = new Point(2056, 1475);

    private int page = 0;
    private int currentX = 1;
    private int currentY = 1;

    public S4UEffect(IGlobals globals)
    {
        this._globals = globals;
        this._globals.Disposing += (sender, e) =>
        {
            _globals.UnblockMouseMovement();
            _globals.UnblockAllKeys();
        };
    }

    public void SetWaitTimeAfterDown(int msec)
    {
        waitTimeAfterDown = msec;
    }
    public void SetWaitTimeAfterUp(int msec)
    {
        waitTimeAfterUp = msec;
    }
    public void SetWaitTimeAfterEnteringItemList(int msec)
    {
        waitTimeAfterEnteringItemList = msec;
    }
    public void SetWaitTimeBeforeEnteringCustom(int msec)
    {
        waitTimeBeforeEnteringCustom = msec;
    }
    public void SetWaitTimeAfterEnteringCustom(int msec)
    {
        waitTimeAfterEnteringCustom = msec;
    }
    public void SetWaitSearchCurrentItemAfterScroll(int msec)
    {
        waitSearchCurrentItemAfterScroll = msec;
    }

    public void CreateCode()
    {
#if STAGE_FOR_YOUメイン画面チェック
        if (!_globals.Match("STAGE_FOR_YOU", out var result)) {
            Console.WriteLine($"!!! STARGE FOR YOU のメイン画面で開始する必要があります。(0) {result.Score}");
            return;
        }
        if (result.Score > 1) {
            Console.WriteLine($"!!! STARGE FOR YOU のメイン画面で開始する必要があります。(1) {result.Score}");
            return;
        }
#endif

        var consoleWindowVisible = false;
        var consoleWindowMinimize = false;

        try
        {
            var consoleWindow = _globals.FindWindow(windowTitle: CONSOLE_WINDOW_TITLE);
            if (consoleWindow.Handle != IntPtr.Zero)
            {
                Console.WriteLine("KeyToKeyコンソールウィンドウを見つけました。");
                consoleWindowVisible = consoleWindow.IsWindowVisible;
                if (consoleWindowVisible && !consoleWindow.IsMinimized)
                {
                    consoleWindowMinimize = true;
                    consoleWindow.Minimize();
                }
            }

            var (lastMouseX, lastMouseY) = _globals.GetCursorPosition();

            _globals.Move(customIconPoint.X, customIconPoint.Y);
            _globals.BlockMouseMovement();
            _globals.Wait(waitTimeBeforeEnteringCustom);
            Tap(Keys.Enter);

            var bounds = _globals.Screen.PrimaryScreen.Bounds;
            _globals.UnblockMouseMovement();
            _globals.Move(bounds.Right, bounds.Bottom);
            _globals.BlockMouseMovement();
            _globals.Wait(waitTimeAfterEnteringCustom);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("var s4u = new S4U(@this);");
            sb.AppendLine("var effect = s4u.GetEffect();");
            sb.AppendLine("effect.StartSetting();");
            // スロット１ マークライト
            for (int slot = 1; slot < 9; slot++)
            {
                sb.Append($"effect.Slot({slot}, ");
                for (int type = 1; type < 8; type++)
                {
                    var point = CreateCodeCore(slot, type);
                    sb.Append($"effect.Item({point.X}, {point.Y})");
                    if (type < 7)
                    {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine(");");
            }
            sb.AppendLine("effect.EndSetting();");

            // コンソールに出力
            Console.Clear();
            Console.Write(sb.ToString());

            // クリップボードにコピー
            _globals.Clipboard.SetText(sb.ToString());

            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("演出復元コードをクリップボードにコピーしました。");
            Console.ResetColor(); ;

            // S4Uメイン画面に戻る
            Tap(Keys.Escape);

            // カーソル位置復元
            _globals.UnblockMouseMovement();
            _globals.Move(lastMouseX, lastMouseY);
        }
        finally
        {
            var consoleWindow = _globals.FindWindow(windowTitle: CONSOLE_WINDOW_TITLE);
            if (consoleWindow.Handle != IntPtr.Zero)
            {
                if (consoleWindowVisible && consoleWindowMinimize)
                {
                    consoleWindow.Restore();
                }
            }
        }
    }

    private Point CreateCodeCore(int slot, int type)
    {
        SelectPage(slot);
        SelectTargetSlot(slot);
        SelectTargetType(type);
        Tap(Keys.Enter);
        _globals.Wait(waitTimeAfterEnteringItemList);
        var point = SearchCurrentSettingItemPoint();
        Tap(Keys.Escape);
        return point;
    }

    private Point SearchCurrentSettingItemPoint()
    {
        var itemHeight = 153;

        // line 0-2
        var lineCount = 0;
        for (int i = 0; i < 3; i++)
        {
            var x = SearchCurrentSettingItemPointCore(1205 + (itemHeight * i));
            if (x > -1)
            {
                return new Point(x, lineCount);
            }
            lineCount++;
        }

        // line 3-
        Tap(Keys.Down);
        Tap(Keys.Down);
        while (true)
        {
            Tap(Keys.Down);
            _globals.Wait(waitSearchCurrentItemAfterScroll);
            var x = SearchCurrentSettingItemPointCore(1524);
            if (x > -1)
            {
                return new Point(x, lineCount);
            }
            lineCount++;
        }
    }

    private int SearchCurrentSettingItemPointCore(int y)
    {
        int itemWidth = 258;
        int baseX = 657;
        int ABOUT = 20;
        int R = 37;
        int G = 186;
        int B = 213;
        for (int i = 0; i < 10; i++)
        {
            var x = baseX + (itemWidth * i);
            var color = GetPixelColor(x, y);
            //Console.WriteLine($"x:{x} y:{y} R:{color.R} G:{color.G} B:{color.B}");
            if (color.R >= R - ABOUT && color.R <= R + ABOUT && color.G >= G - ABOUT && color.G <= G + ABOUT && color.B >= B - ABOUT && color.B <= B + ABOUT)
            {
                return i;
            }
        }
        return -1;
    }

    public void StartSetting()
    {
#if STAGE_FOR_YOUメイン画面チェック
        if (!_globals.Match("STAGE_FOR_YOU", out var result)) {
            Console.WriteLine($"!!! STARGE FOR YOU のメイン画面で開始する必要があります。(0) {result.Score}");
            return;
        }
        if (result.Score > 1) {
            Console.WriteLine($"!!! STARGE FOR YOU のメイン画面で開始する必要があります。(1) {result.Score}");
            return;
        }
#endif

        _globals.Move(customIconPoint.X, customIconPoint.Y);
        _globals.Wait(waitTimeBeforeEnteringCustom);
        _globals.BlockMouseMovement();
        Tap(Keys.Enter);
        _globals.UnblockMouseMovement();
        _globals.Move(0, 0, 10);
        _globals.BlockMouseMovement();
        _globals.Wait(waitTimeAfterEnteringCustom);
    }

    public void EndSetting()
    {
        Tap(Keys.Escape);
        _globals.UnblockMouseMovement();
        _globals.UnblockAllKeys();
        Console.WriteLine("カスタム演出スロットの設定処理が終了しました。");
    }

    public void SetAllNone()
    {
        for (int slot = 1; slot < 9; slot++)
        {
            for (int i = 1; i < 8; i++)
            {
                SetItem(slot, i, new Point(0, 0));
            }
        }
    }

    public void SetAllReset()
    {
        for (int slot = 1; slot < 9; slot++)
        {
            for (int i = 1; i < 8; i++)
            {
                SetItem(slot, i, new Point(1, 0));
            }
        }
    }

    public void SetPreset1()
    {
        this.Slot(1, this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(2, 4), this.Item(0, 0));
        this.Slot(2, this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(5, 3), this.Item(0, 0));
        this.Slot(3, this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(7, 3), this.Item(0, 0));
        this.Slot(4, this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(7, 0), this.Item(0, 0));
        this.Slot(5, this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(9, 0), this.Item(0, 0));
        this.Slot(6, this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(1, 1), this.Item(0, 0));
        this.Slot(7, this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 0), this.Item(0, 2), this.Item(0, 0));
        this.Slot(8, this.Item(1, 0), this.Item(1, 0), this.Item(1, 0), this.Item(1, 0), this.Item(1, 0), this.Item(1, 0), this.Item(1, 0));
    }

    public Point Item(int x, int y)
    {
        return new Point(x, y);
    }

    private void SetItem(int slot, int type, Point point)
    {
        SelectPage(slot);
        SelectTargetSlot(slot);

        SelectTargetType(type);
        Tap(Keys.Enter);
        _globals.Wait(waitTimeAfterEnteringItemList);
        SelectItem(point.X, point.Y);
    }

    public void Slot(int slot, Point markLight, Point monitor, Point effect, Point floorGimmick, Point upperGimmick, Point wipe, Point lightColor)
    {
        SetItem(slot, 1, markLight);
        SetItem(slot, 2, monitor);
        SetItem(slot, 3, effect);
        SetItem(slot, 4, floorGimmick);
        SetItem(slot, 5, upperGimmick);
        SetItem(slot, 6, wipe);
        SetItem(slot, 7, lightColor);
    }

    private void SelectPage(int slot)
    {
        if (
            (1 <= slot && slot <= 4 && page == 1) ||
            (5 <= slot && slot <= 8 && page == 0)
        )
        {
            Tap(Keys.Z);
            page = page == 0 ? 1 : 0;
        }
    }

    private void SelectTargetSlot(int slot)
    {
        var slotInPage = slot < 5 ? slot : slot - 4;
        int diff = slotInPage - currentY;
        if (diff == 0)
        {
            return;
        }
        if (diff < 0)
        {
            for (int i = 0; i < (diff * -1); i++)
            {

                Tap(Keys.Up);
                this.currentY--;
            }
            return;
        }
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                Tap(Keys.Down);
                this.currentY++;
            }
            return;
        }
    }

    private void SelectTargetType(int effect)
    {
        int diff = effect - currentX;
        if (diff == 0)
        {
            return;
        }
        if (diff < 0)
        {
            for (int i = 0; i < (diff * -1); i++)
            {

                Tap(Keys.Left);
                this.currentX--;
            }
            return;
        }
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                Tap(Keys.Right);
                this.currentX++;
            }
            return;
        }
    }

    private void SelectItem(int x, int y)
    {
        for (int i = 1; i < y + 1; i++)
        {
            Tap(Keys.Down);
        }

        for (int i = 1; i < x + 1; i++)
        {
            Tap(Keys.Right);
        }
        Tap(Keys.Enter);
    }

    private void Tap(Keys key)
    {
        _globals.Tap(key, waitTimeAfterDown, waitTimeAfterUp);
    }
}