
public static class GameSettings 
{
    // 食断.
    public static bool UseKuitan = true;

    // 红Dora.
    public static bool UseRedDora = true;

    // if allow furiten
    public static bool AllowFuriten = true;

    public static bool AllowRon3 = false;
    public static bool AllowReach4 = false;
    public static bool AllowSuteFonHai4 = false;

    // 局の最大値
    public static int Kyoku_Max = (int)EKyoku.Ton_4;

    public static int PlayerCount = 4;

    public const int KanCountMax = 4;

    // 持ち点の初期値
    public const int Init_Tenbou = 25000;
    public const int Back_Tenbou = 30000; // used for calculating final pt

    public const int Reach_Cost = 1000;
    public const int HonBa_Cost = 100;

}
