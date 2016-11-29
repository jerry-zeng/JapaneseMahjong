
/**
 * Agari(あがり) = 胡牌.
 */
public class AgariSetting 
{

    /** 役成立フラグの配列 */
    private bool[] yakuFlag = new bool[(int)YakuflgName.YAKUFLGNUM];
    /** 自風の設定 */
    private int jikaze = Kaze.KAZE_NONE;
    /** 場風の設定 */
    private int bakaze = Kaze.KAZE_TON;
    /** 表ドラ表示牌 */
    private Hai[] omoteDoraHais = new Hai[4];
    /** 裏ドラ */
    private Hai[] uraDoraHais = new Hai[4];

    public AgariSetting(){
        for(int i = 0 ; i < yakuFlag.Length ; i++){
            yakuFlag[i] = false;
        }
    }

    /** コンストラクタ(Constructor) */
    public AgariSetting(Mahjong game) : this() {
        this.omoteDoraHais = game.getOmotoDoras();
        this.uraDoraHais = game.getUraDoras();
        this.jikaze = game.getJiKaze();
        this.bakaze = game.getBaKaze();
    }

    /** 特殊役成立の設定 */
    public void setYakuflg(int yakuNum, bool flg) {
        yakuFlag[yakuNum] = flg;
    }
    /** 特殊役成立の取得 */
    public bool getYakuFlag(int yakuNum) {
        return yakuFlag[yakuNum];
    }

    /** 自風の設定 */
    public void setJikaze(int jikaze) {
        this.jikaze = jikaze;
    }
    /** 自風の取得 */
    public int getJikaze() {
        return this.jikaze;
    }

    /** 場風の設定 */
    public void setBakaze(int bakaze) {
        this.bakaze = bakaze;
    }
    /** 場風の取得 */
    public int getBakaze() {
        return this.bakaze;
    }


    /** ドラ表示牌の設定 */
    public void setOmoteDoraHais(Hai[] doraHais) {
        this.omoteDoraHais = doraHais;
    }
    /** ドラ表示牌の取得 */
    public Hai[] getOmoteDoraHais() {
        return this.omoteDoraHais;
    }

    /** 裏ドラ表示牌の設定 */
    public void setUraDoraHais(Hai[] uraDoraHais) {
        this.omoteDoraHais = uraDoraHais;
    }
    /** 裏ドラ表示牌の取得 */
    public Hai[] getUraDoraHais() {
        return this.uraDoraHais;
    }
}
