using System.Collections;


public class YakuHandler 
{
    protected int YakuID;

    // 役の成立判定フラグ 
    protected bool hantei = false;

    // 役満の判定フラグ 
    protected bool yakuman = false;

    // ダブル役満の判定フラグ 
    protected bool doubleYakuman = false;

    // 役の翻数 
    protected int hanSuu = 0;


    public bool isHantei() {
        return hantei;
    }
    public void setYakuHantei(bool hantei) {
        this.hantei = hantei;
    }

    public int getHanSuu() {
        return hanSuu;
    }
    public void setHanSuu(int han) {
        this.hanSuu = han;
    }


    public int getYakuID() {
        return this.YakuID;
    }

    public bool isYakuman() {
        return yakuman;
    }

    public bool isDoubleYakuman(){
        return doubleYakuman;
    }

    public string getYakuName(){
        return YakuHandler.GetYakuName(this.YakuID);
    }


    #region Yaku Name
    public struct YakuName 
    {
        public int ID;
        public string Key;

        public YakuName(int id, string key) {
            this.ID = id;
            this.Key = key;
        }
    }

    static YakuName[] Yaku_IdNames = new YakuName[] 
    {
        new YakuName( 1, "yaku_reach"),
        new YakuName( 2, "yaku_doublereach"),
        new YakuName( 3, "yaku_ippatu"),
        new YakuName( 4, "yaku_tsumo"),

        new YakuName( 5, "yaku_haitei"),
        new YakuName( 6, "yaku_houtei"),
        new YakuName( 7, "yaku_rinsyan"),
        new YakuName( 8, "yaku_chankan"),
        new YakuName( 9, "yaku_tanyao"),
        new YakuName(10, "yaku_pinfu"),
        new YakuName(11, "yaku_ipeikou"),

        new YakuName(12, "yaku_chiitoitu"),
        new YakuName(13, "yaku_cyanta"),
        new YakuName(14, "yaku_ikkituukan"),
        new YakuName(15, "yaku_sansyokudoujyun"),
        new YakuName(16, "yaku_sansyokudoukou"),
        new YakuName(17, "yaku_toitoi"),
        new YakuName(18, "yaku_sanankou"),
        new YakuName(19, "yaku_sankantsu"),
        new YakuName(20, "yaku_honroutou"),
        new YakuName(21, "yaku_syousangen"),

        new YakuName(22, "yaku_ryanpeikou"),
        new YakuName(23, "yaku_honisou"),
        new YakuName(24, "yaku_jyuncyan"),

        new YakuName(25, "yaku_tinisou"),

        new YakuName(26, "yaku_tenhou"),
        new YakuName(27, "yaku_tihou"),
        new YakuName(28, "yaku_renhou"),
        new YakuName(29, "yaku_chinroutou"),
        new YakuName(30, "yaku_ryuuisou"),
        new YakuName(31, "yaku_suukantsu"),
        new YakuName(32, "yaku_daisangen"),
        new YakuName(33, "yaku_syousuushi"),
        new YakuName(34, "yaku_tsuisou"),
        new YakuName(35, "yaku_suuankou"),
        new YakuName(36, "yaku_cyuurennpoutou"),
        new YakuName(37, "yaku_kokushi"),

        new YakuName(38, "yaku_daisuushi"),
        new YakuName(39, "yaku_suuankou_tanki"),
        new YakuName(40, "yaku_cyuurennpoutou_jyunsei"),
        new YakuName(41, "yaku_kokushi_13men"),

        new YakuName(42, "yaku_lenfonhai"),
        new YakuName(43, "yaku_yakuhai"),
        new YakuName(44, "yaku_dora"),

        new YakuName(45, "yaku_nagashimangan"), // handler is in AgariScoreManager.
    };

    public static string getNagashiManganYakuName()
    {
        return GetYakuName(45);
    }

    public static string GetYakuName(int yakuID)
    {
        string key = Yaku_IdNames[ yakuID-1 ].Key;
        return ResManager.getString( key );
    }
    #endregion
}


//立直.
public class CheckReach : YakuHandler {
    public CheckReach(Yaku owner) {
        this.YakuID = 1;
        hantei = owner.checkReach() && !owner.checkDoubleReach();
        hanSuu = 1;
    }
}
//双立直.
public class CheckDoubleReach : YakuHandler {
    public CheckDoubleReach(Yaku owner) {
        this.YakuID = 2;
        hantei = owner.checkDoubleReach();
        hanSuu = 2;
    }
}
//一发.
public class CheckIppatu : YakuHandler {
    public CheckIppatu(Yaku owner) {
        this.YakuID = 3;
        hantei = owner.checkIppatu();
        hanSuu = 1;
    }
}
//自摸.
public class CheckTsumo : YakuHandler {
    public CheckTsumo(Yaku owner) {
        this.YakuID = 4;
        hantei = owner.checkTsumo();
        hanSuu = 1;
    }
}
//海底捞月.
public class CheckHaitei : YakuHandler {
    public CheckHaitei(Yaku owner) {
        this.YakuID = 5;
        hantei = owner.checkHaitei();
        hanSuu = 1;
    }
}
//河底捞鱼.
public class CheckHoutei : YakuHandler {
    public CheckHoutei(Yaku owner) {
        this.YakuID = 6;
        hantei = owner.checkHoutei();
        hanSuu = 1;
    }
}
//杠上开花.
public class CheckRinsyan : YakuHandler {
    public CheckRinsyan(Yaku owner) {
        this.YakuID = 7;
        hantei = owner.checkRinsyan();
        hanSuu = 1;
    }
}
//抢扛.
public class CheckChankan : YakuHandler {
    public CheckChankan(Yaku owner) {
        this.YakuID = 8;
        hantei = owner.checkChankan();
        hanSuu = 1;
    }
}
//断幺.
public class CheckTanyao : YakuHandler {
    public CheckTanyao(Yaku owner) {
        this.YakuID = 9;
        hantei = owner.checkTanyao();
        hanSuu = 1;
    }
}
//平和.
public class CheckPinfu : YakuHandler {
    public CheckPinfu(Yaku owner) {
        this.YakuID = 10;
        hantei = owner.checkPinfu();
        hanSuu = 1;
    }
}
//一杯口.
public class CheckIpeikou : YakuHandler {
    public CheckIpeikou(Yaku owner) {
        this.YakuID = 11;
        hantei = owner.checkIpeikou() && !owner.checkRyanpeikou();
        hanSuu = 1;
    }
}

//七对子.
public class CheckChiitoitsu : YakuHandler {
    public CheckChiitoitsu(Yaku owner) {
        this.YakuID = 12;
        hantei = owner.checkChiitoitsu();
        hanSuu = 2;
    }
}
//混全带幺九.
public class CheckCyanta : YakuHandler {
    public CheckCyanta(Yaku owner) {
        this.YakuID = 13;
        hantei = owner.checkCyanta() && !owner.checkJyunCyan() && !owner.checkHonroutou();

        if( owner.isNaki == true ) {
            hanSuu = 1;
        }
        else {
            hanSuu = 2;
        }
    }
}
//一气通贯.
public class CheckIkkituukan : YakuHandler {
    public CheckIkkituukan(Yaku owner) {
        this.YakuID = 14;
        hantei = owner.checkIkkituukan();

        if( owner.isNaki == true ) {
            hanSuu = 1;
        }
        else {
            hanSuu = 2;
        }
    }
}
//三色同顺.
public class CheckSansyokuDoujun : YakuHandler {
    public CheckSansyokuDoujun(Yaku owner) {
        this.YakuID = 15;
        hantei = owner.checkSansyokuDoujun();

        if( owner.isNaki == true ) {
            hanSuu = 1;
        }
        else {
            hanSuu = 2;
        }
    }
}
//三色同刻.
public class CheckSansyokuDoukou : YakuHandler {
    public CheckSansyokuDoukou(Yaku owner) {
        this.YakuID = 16;
        hantei = owner.checkSansyokuDoukou();
        hanSuu = 2;
    }
}
//对对和.
public class CheckToitoi : YakuHandler {
    public CheckToitoi(Yaku owner) {
        this.YakuID = 17;
        hantei = owner.checkToitoi();
        hanSuu = 2;
    }
}
//三暗刻.
public class CheckSanankou : YakuHandler {
    public CheckSanankou(Yaku owner) {
        this.YakuID = 18;
        hantei = owner.checkSanankou();
        hanSuu = 2;
    }
}
//三杠子.
public class CheckSankantsu : YakuHandler {
    public CheckSankantsu(Yaku owner) {
        this.YakuID = 19;
        hantei = owner.checkSankantsu();
        hanSuu = 2;
    }
}
//混老头.
public class CheckHonroutou : YakuHandler {
    public CheckHonroutou(Yaku owner) {
        this.YakuID = 20;
        hantei = owner.checkHonroutou();
        hanSuu = 2;
    }
}
//混老头七对子.
public class CheckHonroutou_Chiitoitsu : YakuHandler {
    public CheckHonroutou_Chiitoitsu(Yaku owner) {
        this.YakuID = 20; // the same name to Honroutou.
        hantei = owner.checkHonroutou_Chiitoitsu();
        hanSuu = 2;
    }
}
//小三元.
public class CheckSyousangen : YakuHandler {
    public CheckSyousangen(Yaku owner) {
        this.YakuID = 21;
        hantei = owner.checkSyousangen();
        hanSuu = 2;
    }
}

//二杯口.
public class CheckRyanpeikou : YakuHandler {
    public CheckRyanpeikou(Yaku owner) {
        this.YakuID = 22;
        hantei = owner.checkRyanpeikou();
        hanSuu = 3;
    }
}
//混一色.
public class CheckHonisou : YakuHandler {
    public CheckHonisou(Yaku owner) {
        this.YakuID = 23;
        hantei = owner.checkHonisou() && !owner.checkTinisou();

        if( owner.isNaki == true ) {
            hanSuu = 2;
        }
        else {
            hanSuu = 3;
        }
    }
}
//纯全带幺九.
public class CheckJyunCyan : YakuHandler {
    public CheckJyunCyan(Yaku owner) {
        this.YakuID = 24;
        hantei = owner.checkJyunCyan();

        if( owner.isNaki == true ) {
            hanSuu = 2;
        }
        else {
            hanSuu = 3;
        }
    }
}

//清一色.
public class CheckTinisou : YakuHandler {
    public CheckTinisou(Yaku owner) {
        this.YakuID = 25;
        hantei = owner.checkTinisou();

        if( owner.isNaki == true ) {
            hanSuu = 5;
        }
        else {
            hanSuu = 6;
        }
    }
}

//天和.
public class CheckTenhou : YakuHandler {
    public CheckTenhou(Yaku owner) {
        this.YakuID = 26;
        hantei = owner.checkTenhou();
        hanSuu = 13;
        yakuman = true;
    }
}
//地和.
public class CheckTihou : YakuHandler {
    public CheckTihou(Yaku owner) {
        this.YakuID = 27;
        hantei = owner.checkTihou();
        hanSuu = 13;
        yakuman = true;
    }
}
//人和.
public class CheckRenhou : YakuHandler{
    public CheckRenhou(Yaku owner) {
        this.YakuID = 28;
        hantei = owner.checkRenhou();
        hanSuu = 13;
        yakuman = true;
    }
}
//清老头.
public class CheckChinroutou : YakuHandler {
    public CheckChinroutou(Yaku owner) {
        this.YakuID = 29;
        hantei = owner.checkChinroutou();
        hanSuu = 13;
        yakuman = true;
    }
}
//绿一色.
public class CheckRyuuisou : YakuHandler {
    public CheckRyuuisou(Yaku owner) {
        this.YakuID = 30;
        hantei = owner.checkRyuuisou();
        hanSuu = 13;
        yakuman = true;
    }
}
//四杠子.
public class CheckSuukantsu : YakuHandler {
    public CheckSuukantsu(Yaku owner) {
        this.YakuID = 31;
        hantei = owner.checkSuukantsu();
        hanSuu = 13;
        yakuman = true;
    }
}
//大三元.
public class CheckDaisangen : YakuHandler {
    public CheckDaisangen(Yaku owner) {
        this.YakuID = 32;
        hantei = owner.checkDaisangen();
        hanSuu = 13;
        yakuman = true;
    }
}
//小四喜.
public class CheckSyousuushi : YakuHandler {
    public CheckSyousuushi(Yaku owner) {
        this.YakuID = 33;
        hantei = owner.checkSyousuushi();
        hanSuu = 13;
        yakuman = true;
    }
}
//字一色(对对和)
public class CheckTsuisou : YakuHandler {
    public CheckTsuisou(Yaku owner) {
        this.YakuID = 34;
        hantei = owner.checkTsuisou();
        hanSuu = 13;
        yakuman = true;
    }
}
//字一色(七对子)
public class CheckTsuisou_Chiitoitsu : YakuHandler {
    public CheckTsuisou_Chiitoitsu(Yaku owner) {
        this.YakuID = 34;
        hantei = owner.checkTsuisou_Chiitoitsu();
        hanSuu = 13;
        yakuman = true;
    }
}

//四暗刻.
public class CheckSuuankou : YakuHandler {
    public CheckSuuankou(Yaku owner) {
        this.YakuID = 35;
        hantei = owner.checkSuuankou() && !owner.checkSuuankou_Tanki();
        hanSuu = 13;
        yakuman = true;
    }
}
//九连宝灯.
public class CheckCyuurennpoutou : YakuHandler {
    public CheckCyuurennpoutou(Yaku owner) {
        this.YakuID = 36;
        hantei = owner.checkCyuurennpoutou() && !owner.checkCyuurennpoutou_Jyunsei();
        hanSuu = 13;
        yakuman = true;
    }
}
//国士无双.
public class CheckKokushi : YakuHandler {
    public CheckKokushi(Yaku owner) {
        this.YakuID = 37;
        hantei = owner.checkKokushi() && !owner.checkKokushi_13Men();
        hanSuu = 13;
        yakuman = true;
    }
}

//大四喜.
public class CheckDaisuushi : YakuHandler {
    public CheckDaisuushi(Yaku owner) {
        this.YakuID = 38;
        hantei = owner.checkDaisuushi();
        hanSuu = 13;
        yakuman = true;
        doubleYakuman = true;
    }
}
//四暗刻单骑.
public class CheckSuuankou_Tanki : YakuHandler {
    public CheckSuuankou_Tanki(Yaku owner) {
        this.YakuID = 39;
        hantei = owner.checkSuuankou_Tanki();
        hanSuu = 13;
        yakuman = true;
        doubleYakuman = true;
    }
}
//纯正九连宝灯.
public class CheckCyuurennpoutou_Jyunsei : YakuHandler {
    public CheckCyuurennpoutou_Jyunsei(Yaku owner) {
        this.YakuID = 40;
        hantei = owner.checkCyuurennpoutou_Jyunsei();
        hanSuu = 13;
        yakuman = true;
        doubleYakuman = true;
    }
}
//国士无双十三面.
public class CheckKokushi_13Men : YakuHandler {
    public CheckKokushi_13Men(Yaku owner) {
        this.YakuID = 41;
        hantei = owner.checkKokushi_13Men();
        hanSuu = 13;
        yakuman = true;
        doubleYakuman = true;
    }
}


// 连风牌.
public class CheckLenFonHai : YakuHandler {
    public CheckLenFonHai(Yaku owner) 
    {
        this.YakuID = 42;

        EKaze jikaze = owner.AgariParam.getJikaze();
        EKaze bakaze = owner.AgariParam.getBakaze();

        if( jikaze == EKaze.Ton && bakaze == EKaze.Ton ){
            if( owner.checkTon() ){
                hantei = true;
                hanSuu = 2;
            }
        }
        else if( jikaze == EKaze.Nan ){
            if( owner.checkNan() ){
                hantei = true;
                hanSuu = 2;
            }
        } 
    }
}

// 役牌.
public class CheckYakuHai : YakuHandler {
    public CheckYakuHai(Yaku owner) 
    {
        this.YakuID = 43;

        if( owner.checkHaku() ){
            hanSuu++;
            hantei = true;
        }
        if( owner.checkHatsu() ){
            hanSuu++;
            hantei = true;
        }
        if( owner.checkCyun() ){
            hanSuu++;
            hantei = true;
        }

        EKaze jikaze = owner.AgariParam.getJikaze();
        CheckLenFonHai lenFonHaiChecker = new CheckLenFonHai(owner);

        if( jikaze == EKaze.Ton )
        {
            if( lenFonHaiChecker.isHantei() ){

            }
            else if( owner.checkTon() ){
                hanSuu++;
                hantei = true;
            }
        }
        else if( jikaze == EKaze.Nan )
        {
            if( lenFonHaiChecker.isHantei() ){

            }
            else if( owner.checkNan() ){
                hanSuu++;
                hantei = true;
            }
        }
        else if( jikaze == EKaze.Sya )
        {
            if( lenFonHaiChecker.isHantei() ){

            }
            else if( owner.checkSya() ){
                hanSuu++;
                hantei = true;
            }
        }
        else
        {
            if( lenFonHaiChecker.isHantei() ){

            }
            else if( owner.checkPei() ){
                hanSuu++;
                hantei = true;
            }
        }
    }
}

//宝牌.
public class CheckDora : YakuHandler {
    public CheckDora(Yaku owner) {
        this.YakuID = 44;
        hantei = owner.checkDora();
        hanSuu = 1;
        yakuman = false;
    }
}

// nagashimangan.
// YakuID = 45.

