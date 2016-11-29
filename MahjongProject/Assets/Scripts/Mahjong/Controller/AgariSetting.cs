
/// <summary>
/// Agari setting.
/// Agari(あがり) = 胡牌
/// </summary>

public class AgariSetting 
{
    // 役成立フラグの配列
    private bool[] yakuFlag = new bool[(int)EYakuFlagType.YAKUFLG_COUNT];

    // 自風の設定
    private EKaze jiKaze = EKaze.None;

    // 場風の設定
    private EKaze baKaze = EKaze.Ton;

    // 表ドラ
    private Hai[] omoteDoraHais = new Hai[4];

    // 裏ドラ
    private Hai[] uraDoraHais = new Hai[4];


    public AgariSetting()
    {
        for(int i = 0; i < yakuFlag.Length ; i++){
            yakuFlag[i] = false;
        }
    }

    public AgariSetting(Mahjong game) : this()
    {
        this.omoteDoraHais = game.getOmotoDoras();
        this.uraDoraHais = game.getUraDoras();
        this.jiKaze = game.getJiKaze();
        this.baKaze = game.getBaKaze();
    }


    public void setYakuFlag(int yakuNum, bool flg) {
        yakuFlag[yakuNum] = flg;
    }

    public bool getYakuFlag(int yakuNum) {
        return yakuFlag[yakuNum];
    }


    public void setJikaze(EKaze jikaze) {
        this.jiKaze = jikaze;
    }

    public EKaze getJikaze() {
        return this.jiKaze;
    }


    public void setBakaze(EKaze bakaze) {
        this.baKaze = bakaze;
    }
    public EKaze getBakaze() {
        return this.baKaze;
    }


    // 表ドラ
    public void setOmoteDoraHais(Hai[] omoteDoraHais) {
        this.omoteDoraHais = omoteDoraHais;
    }
    public Hai[] getOmoteDoraHais() {
        return this.omoteDoraHais;
    }

    // 裏ドラ
    public void setUraDoraHais(Hai[] uraDoraHais) {
        this.omoteDoraHais = uraDoraHais;
    }
    public Hai[] getUraDoraHais() {
        return this.uraDoraHais;
    }
}
