
/// <summary>
/// Agari setting.
/// Agari(あがり) = 胡牌
/// </summary>

public sealed class AgariSetting 
{
    // 役成立フラグの配列
    private static bool[] _yakuFlag = new bool[(int)EYakuFlagType.Count];

    // 自風の設定
    private static EKaze _jiKaze = EKaze.Ton;

    // 場風の設定
    private static EKaze _baKaze = EKaze.Ton;

    // 表ドラ
    private static Hai[] _omoteDoraHais = new Hai[4];

    // 裏ドラ
    private static Hai[] _uraDoraHais = new Hai[4];


    public static void Initialize(Mahjong game)
    {
        for(int i = 0; i < _yakuFlag.Length; i++){
            _yakuFlag[i] = false;
        }

        _omoteDoraHais = game.getOmotoDoras();
        _uraDoraHais = game.getUraDoras();
        _jiKaze = game.getJiKaze();
        _baKaze = game.getBaKaze();
    }


    public static void setYakuFlag(int yakuNum, bool flg) {
        _yakuFlag[yakuNum] = flg;
    }

    public static bool getYakuFlag(int yakuNum) {
        return _yakuFlag[yakuNum];
    }


    public static void setJikaze(EKaze jikaze) {
        _jiKaze = jikaze;
    }

    public static EKaze getJikaze() {
        return _jiKaze;
    }


    public static void setBakaze(EKaze bakaze) {
        _baKaze = bakaze;
    }
    public static EKaze getBakaze() {
        return _baKaze;
    }


    // 表ドラ
    public static void setOmoteDoraHais(Hai[] omoteDoraHais) {
        _omoteDoraHais = omoteDoraHais;
    }
    public static Hai[] getOmoteDoraHais() {
        return _omoteDoraHais;
    }

    // 裏ドラ
    public static void setUraDoraHais(Hai[] uraDoraHais) {
        _uraDoraHais = uraDoraHais;
    }
    public static Hai[] getUraDoraHais() {
        return _uraDoraHais;
    }
}
