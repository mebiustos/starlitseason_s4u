#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UCore.csx"
#nullable enable

public enum CameraTarget
{
    No1 = Keys.F1,
    No2 = Keys.F2,
    No3 = Keys.F3,
    No4 = Keys.F4,
    No5 = Keys.F5
}

public enum CameraMode
{
    Normal,
    Follow
}

public enum CameraType
{
    N_見下ろし_アップ,
    N_見下ろし_普通,
    N_見下ろし_引き,
    N_ステージ左から_アップ,
    N_ステージ左から_普通,
    N_ステージ左から_引き,
    N_ステージ右から_アップ,
    N_ステージ右から_普通,
    N_ステージ右から_引き,
    N_正面_アップ,
    N_正面_普通,
    N_正面_引き,
    F_ズーム_正面からアップして止め,
    F_ズーム_正面からアウトして止め,
    F_ズーム_ステージ左からアップして止め,
    F_ズーム_ステージ右からアップして止め,
    F_スライド_左からアップして右へアウト,
    F_スライド_右からアップして左へアウト,
    F_スライド_左からアップして右で止め,
    F_スライド_右からアップして左で止め,
    F_スライド_左からアップして右へゆっくり回転,
    F_スライド_右からアップして左へゆっくり回転,
    F_スライド_全員を左からアップでスライド,
    F_スライド_全員を右からアップでスライド,
    F_ロング_俯瞰,
    F_ロング_正面,
    F_アップ_足から頭へ,
    F_アップ_頭から足へ
}

public class S4UCam : S4UCore
{
    private int targetAfterWaitMs = 25;

    private CameraMode camMode = CameraMode.Normal;
    private Boolean isReserve = false;

    private CameraTarget currentCameraTarget = CameraTarget.No1;
    private Keys currentTreeKey = Keys.Up;

    private bool camDisable = false;
    private int camGlobalTargetLeadMs = 0;

    public S4UCam(IGlobals globals, string? songName = null) : base(globals, songName)
    {
    }

    public void SetTargetAfterWaitMs(int msec)
    {
        this.targetAfterWaitMs = msec;
    }

    public void SetCamGlobalTargetLeadMs(int leadMs)
    {
        if (leadMs < 0)
        {
            throw new ArgumentException("offsetMs is only positive integers are allowed.");
        }
        this.camGlobalTargetLeadMs = leadMs;
    }

    public void CamEnable()
    {
        this.camDisable = false;
    }

    public void CamDisable()
    {
        this.camDisable = true;
    }

    public void CamReserve(long msec, bool value = true)
    {
        if (this.camDisable)
        {
            return;
        }

        if (this.isReserve != value)
        {
            this.ETap(msec, Keys.G);
            this.isReserve = value;
        }
    }

    public void CamTarget(long msec, CameraTarget target)
    {
        if (this.camDisable)
        {
            return;
        }

        this.ETapCamTarget(msec, target);
    }

    public void CamMode(long msec, CameraMode camMode)
    {
        if (this.camDisable)
        {
            return;
        }

        if (this.camMode != camMode)
        {
            this.ETap(msec, Keys.Tab);
            this.camMode = camMode;
        }
    }

    public void CamAuto(long msec)
    {
        if (this.camDisable)
        {
            return;
        }

        this.PreNormal(msec);
        this.ETap(msec, Keys.D);
    }

    public void Cam(long msec, CameraType type)
    {
        if (this.camDisable)
        {
            return;
        }

        this.Cam(msec, this.currentCameraTarget, type);
    }

    public void Cam(long msec, CameraTarget target, CameraType type)
    {
        this.Cam(msec, target, this.camGlobalTargetLeadMs, type);
    }

    public void Cam(long msec, CameraTarget target, int targetLeadMs, CameraType type)
    {
        if (this.camDisable)
        {
            return;
        }

        long targetMsec = msec - targetLeadMs;

        this.CamReserve(targetMsec);

        switch (type)
        {
            case CameraType.N_見下ろし_アップ:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Up);
                this.ETap(msec, Keys.W);
                break;
            case CameraType.N_見下ろし_普通:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Up);
                this.ETap(msec, Keys.A);
                break;
            case CameraType.N_見下ろし_引き:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Up);
                this.ETap(msec, Keys.S);
                break;
            case CameraType.N_ステージ左から_アップ:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Left);
                this.ETap(msec, Keys.W);
                break;
            case CameraType.N_ステージ左から_普通:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Left);
                this.ETap(msec, Keys.A);
                break;
            case CameraType.N_ステージ左から_引き:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Left);
                this.ETap(msec, Keys.S);
                break;
            case CameraType.N_ステージ右から_アップ:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Right);
                this.ETap(msec, Keys.W);
                break;
            case CameraType.N_ステージ右から_普通:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Right);
                this.ETap(msec, Keys.A);
                break;
            case CameraType.N_ステージ右から_引き:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Right);
                this.ETap(msec, Keys.S);
                break;
            case CameraType.N_正面_アップ:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Down);
                this.ETap(msec, Keys.W);
                break;
            case CameraType.N_正面_普通:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Down);
                this.ETap(msec, Keys.A);
                break;
            case CameraType.N_正面_引き:
                this.PreNormal(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Down);
                this.ETap(msec, Keys.S);
                break;
            case CameraType.F_ズーム_正面からアップして止め:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Up);
                this.ETap(msec, Keys.W);
                break;
            case CameraType.F_ズーム_正面からアウトして止め:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Down);
                this.ETap(msec, Keys.W);
                break;
            case CameraType.F_ズーム_ステージ左からアップして止め:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Left);
                this.ETap(msec, Keys.W);
                break;
            case CameraType.F_ズーム_ステージ右からアップして止め:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Right);
                this.ETap(msec, Keys.W);
                break;
            case CameraType.F_スライド_左からアップして右へアウト:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Up);
                this.ETap(msec, Keys.A);
                break;
            case CameraType.F_スライド_右からアップして左へアウト:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Down);
                this.ETap(msec, Keys.A);
                break;
            case CameraType.F_スライド_左からアップして右で止め:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Left);
                this.ETap(msec, Keys.A);
                break;
            case CameraType.F_スライド_右からアップして左で止め:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Right);
                this.ETap(msec, Keys.A);
                break;
            case CameraType.F_スライド_左からアップして右へゆっくり回転:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Left);
                this.ETap(msec, Keys.S);
                break;
            case CameraType.F_スライド_右からアップして左へゆっくり回転:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Right);
                this.ETap(msec, Keys.S);
                break;
            case CameraType.F_スライド_全員を左からアップでスライド:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Left);
                this.ETap(msec, Keys.D);
                break;
            case CameraType.F_スライド_全員を右からアップでスライド:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Right);
                this.ETap(msec, Keys.D);
                break;
            case CameraType.F_ロング_俯瞰:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Up);
                this.ETap(msec, Keys.S);
                break;
            case CameraType.F_ロング_正面:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Down);
                this.ETap(msec, Keys.S);
                break;
            case CameraType.F_アップ_足から頭へ:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Up);
                this.ETap(msec, Keys.D);
                break;
            case CameraType.F_アップ_頭から足へ:
                this.PreFollow(targetMsec);
                this.ETapCamTarget(targetMsec, target);
                this.ETapCamTree(msec, Keys.Down);
                this.ETap(msec, Keys.D);
                break;
        }
    }

    private void ETapCamTarget(long msec, CameraTarget target)
    {
        if (this.currentCameraTarget != target)
        {
            this.ETap(msec, (Keys)target, 0, 0, targetAfterWaitMs);
            this.currentCameraTarget = target;
        }
    }

    private void ETapCamTree(long msec, Keys treeKey)
    {
        if (this.currentTreeKey != treeKey)
        {
            this.ETap(msec, treeKey);
            this.currentTreeKey = treeKey;
        }
    }

    private void PreNormal(long msec)
    {
        //this.CamReserve(msec);
        if (this.camMode != CameraMode.Normal)
        {
            this.ETap(msec, Keys.Tab);
            this.camMode = CameraMode.Normal;
        }
    }

    private void PreFollow(long msec)
    {
        //this.CamReserve(msec);
        if (this.camMode != CameraMode.Follow)
        {
            this.ETap(msec, Keys.Tab);
            this.camMode = CameraMode.Follow;
        }
    }
}