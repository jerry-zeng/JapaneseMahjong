
/// <summary>
/// Agari setting.
/// Agari(あがり) = 胡牌
/// </summary>

public class AgariParam 
{
    // 自風の設定
    private EKaze _jiKaze = EKaze.Ton;

    // 場風の設定
    private EKaze _baKaze = EKaze.Ton;


    // 役成立フラグの配列
    private bool[] _yakuFlag = new bool[(int)EYakuFlagType.Count];

    // 表ドラ
    private Hai[] _omoteDoraHais = null;

    // 裏ドラ
    private Hai[] _uraDoraHais = null;


    public AgariParam(Mahjong game)
    {     
        _jiKaze = game.getJiKaze();
        _baKaze = game.getBaKaze();

        for(int i = 0; i < _yakuFlag.Length; i++){
            _yakuFlag[i] = false;
        }

        /// as dora hais will change, set them on Tsumo or Ron.
        //_omoteDoraHais = game.getOmotoDoras();
        //_uraDoraHais = game.getUraDoras();
    }


    public void setYakuFlag(EYakuFlagType yakuNum, bool flg) {
        _yakuFlag[(int)yakuNum] = flg;
    }

    public bool getYakuFlag(EYakuFlagType yakuFlag) {
        return _yakuFlag[(int)yakuFlag];
    }

    // 表ドラ
    public void setOmoteDoraHais(Hai[] omoteDoraHais) {
        _omoteDoraHais = omoteDoraHais;
    }
    public Hai[] getOmoteDoraHais() {
        return _omoteDoraHais;
    }

    // 裏ドラ
    public void setUraDoraHais(Hai[] uraDoraHais) {
        _uraDoraHais = uraDoraHais;
    }
    public Hai[] getUraDoraHais() {
        return _uraDoraHais;
    }


    public void setJikaze(EKaze jikaze) {
        _jiKaze = jikaze;
    }
    public EKaze getJikaze() {
        return _jiKaze;
    }

    public void setBakaze(EKaze bakaze) {
        _baKaze = bakaze;
    }
    public EKaze getBakaze() {
        return _baKaze;
    }
}
