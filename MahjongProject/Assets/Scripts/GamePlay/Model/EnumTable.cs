
public enum EFuuroType 
{
    MinShun = 0,   // 明顺.
    MinKou = 1,    // 明刻.
    DaiMinKan = 2, // 大明杠.
    KaKan = 3,     // 加杠.
    AnKan = 4      // 暗杠.
}

// Kaze = 风 //
public enum EKaze 
{
    East = 0,
    South = 1,
    West = 2,
    North = 3,

    None = 100,
}

public enum EOrientation 
{
    Landscape_Left,
    Landscape_Right,
    Portrait,
    Portrait_Down
}

public enum EFrontBack 
{
    Front,
    Back
}


public static class Kaze 
{
    /** 風(東) */
    public readonly static int KAZE_TON = 0;
    /** 風(南) */
    public readonly static int KAZE_NAN = 1;
    /** 風(西) */
    public readonly static int KAZE_SYA = 2;
    /** 風(北) */
    public readonly static int KAZE_PE = 3;
    /** 風(なし) */
    public readonly static int KAZE_NONE = 4;
}
