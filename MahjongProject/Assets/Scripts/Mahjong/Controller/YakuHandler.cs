using UnityEngine;
using System.Collections;


public class YakuHandler 
{
    protected int YakuID;

    // 役の成立判定フラグ 
    protected bool hantei = false;

    // 役満の判定フラグ 
    protected bool yakuman = false;

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

    public string getYakuName()
    {
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
        new YakuName( 1, "yaku_tanyao"),
        new YakuName( 2, "yaku_pinfu"),
        new YakuName( 3, "yaku_ipeikou"),
        new YakuName( 4, "yaku_reach"),
        new YakuName( 5, "yaku_ippatu"),
        new YakuName( 6, "yaku_tumo"),
        new YakuName( 7, "yaku_doubleton"),
        new YakuName( 8, "yaku_ton"),
        new YakuName( 9, "yaku_doublenan"),
        new YakuName(10, "yaku_nan"),
        new YakuName(11, "yaku_sya"),
        new YakuName(12, "yaku_pet"),
        new YakuName(13, "yaku_haku"),
        new YakuName(14, "yaku_hatu"),
        new YakuName(15, "yaku_cyun"),
        new YakuName(16, "yaku_haitei"),
        new YakuName(17, "yaku_houtei"),
        new YakuName(18, "yaku_rinsyan"),
        new YakuName(19, "yaku_cyankan"),

        new YakuName(20, "yaku_doublereach"),
        new YakuName(21, "yaku_teetoitu"),
        new YakuName(22, "yaku_cyanta"),
        new YakuName(23, "yaku_ikkituukan"),
        new YakuName(24, "yaku_sansyokudoujyun"),
        new YakuName(25, "yaku_sansyokudoukou"),
        new YakuName(26, "yaku_toitoi"),
        new YakuName(27, "yaku_sanankou"),
        new YakuName(28, "yaku_sankantu"),
        new YakuName(29, "yaku_ryanpeikou"),
        new YakuName(30, "yaku_honitu"),
        new YakuName(31, "yaku_juncyan"),
        new YakuName(32, "yaku_syousangen"),
        new YakuName(33, "yaku_honroutou"),

        new YakuName(34, "yaku_tinitu"),

        new YakuName(35, "yaku_suuankou"),
        new YakuName(36, "yaku_suukantu"),
        new YakuName(37, "yaku_daisangen"),
        new YakuName(38, "yaku_syousuushi"),
        new YakuName(39, "yaku_daisuushi"),
        new YakuName(40, "yaku_tuuisou"),
        new YakuName(41, "yaku_chinroutou"),
        new YakuName(42, "yaku_ryuuisou"),
        new YakuName(43, "yaku_cyuurennpoutou"),
        new YakuName(44, "yaku_kokushi"),
        new YakuName(45, "yaku_tenhou"),
        new YakuName(46, "yaku_tihou"),
        new YakuName(47, "yaku_dora"),
        new YakuName(48, "yaku_nagashimangan"),
    };

    public static string GetYakuName(int yakuID)
    {
        string key = Yaku_IdNames[ yakuID-1 ].Key;
        return ResManager.getString( key );
    }
    #endregion
}


public class CheckTanyao : YakuHandler {
    public CheckTanyao(Yaku owner) {
        this.YakuID = 1;
        hantei = owner.checkTanyao();
        hanSuu = 1;
    }
}

public class CheckPinfu : YakuHandler {
    public CheckPinfu(Yaku owner) {
        this.YakuID = 2;
        hantei = owner.checkPinfu();
        hanSuu = 1;
    }
}

public class CheckIpeikou : YakuHandler {
    public CheckIpeikou(Yaku owner) {
        this.YakuID = 3;
        hantei = owner.checkIpeikou() && !owner.checkRyanpeikou();
        hanSuu = 1;
    }
}

public class CheckReach : YakuHandler {
    public CheckReach(Yaku owner) {
        this.YakuID = 4;
        hantei = owner.checkReach() && !owner.checkDoubleReach();
        hanSuu = 1;
    }
}

public class CheckIppatu : YakuHandler {
    public CheckIppatu(Yaku owner) {
        this.YakuID = 5;
        hantei = owner.checkIppatu();
        hanSuu = 1;
    }
}

public class CheckTsumo : YakuHandler {
    public CheckTsumo(Yaku owner) {
        this.YakuID = 6;
        hantei = owner.checkTsumo();
        hanSuu = 1;
    }
}

public class CheckTon : YakuHandler {
    public CheckTon(Yaku owner) {
        hantei = owner.checkTon();

        if( (AgariSetting.getJikaze() == EKaze.Ton) && 
           (AgariSetting.getBakaze() == EKaze.Ton) ) 
        {
            this.YakuID = 7;
            hanSuu = 2;
        }
        else {
            this.YakuID = 8;
            hanSuu = 1;
        }
    }
}

public class CheckNan : YakuHandler {
    public CheckNan(Yaku owner) {
        hantei = owner.checkNan();
        if( (AgariSetting.getJikaze() == EKaze.Nan) && 
           (AgariSetting.getBakaze() == EKaze.Nan) ) 
        {
            this.YakuID = 9;
            hanSuu = 2;
        }
        else {
            this.YakuID = 10;
            hanSuu = 1;
        }
    }
}

public class CheckSya : YakuHandler {
    public CheckSya(Yaku owner) {
        this.YakuID = 11;
        hantei = owner.checkSya();
        hanSuu = 1;
    }
}

public class CheckPei : YakuHandler {
    public CheckPei(Yaku owner) {
        this.YakuID = 12;
        hantei = owner.checkPei();
        hanSuu = 1;
    }
}

public class CheckHaku : YakuHandler {
    public CheckHaku(Yaku owner) {
        this.YakuID = 13;
        hantei = owner.checkHaku();
        hanSuu = 1;
    }
}

public class CheckHatu : YakuHandler {
    public CheckHatu(Yaku owner) {
        this.YakuID = 14;
        hantei = owner.checkHatsu();
        hanSuu = 1;
    }
}

public class CheckCyun : YakuHandler {
    public CheckCyun(Yaku owner) {
        this.YakuID = 15;
        hantei = owner.checkCyun();
        hanSuu = 1;
    }
}

public class CheckHaitei : YakuHandler {
    public CheckHaitei(Yaku owner) {
        this.YakuID = 16;
        hantei = owner.checkHaitei();
        hanSuu = 1;
    }
}

public class CheckHoutei : YakuHandler {
    public CheckHoutei(Yaku owner) {
        this.YakuID = 17;
        hantei = owner.checkHoutei();
        hanSuu = 1;
    }
}

public class CheckRinsyan : YakuHandler {
    public CheckRinsyan(Yaku owner) {
        this.YakuID = 18;
        hantei = owner.checkRinsyan();
        hanSuu = 1;
    }
}

public class CheckCyankan : YakuHandler {
    public CheckCyankan(Yaku owner) {
        this.YakuID = 19;
        hantei = owner.checkCyankan();
        hanSuu = 1;
    }
}

public class CheckDoubleReach : YakuHandler {
    public CheckDoubleReach(Yaku owner) {
        this.YakuID = 20;
        hantei = owner.checkDoubleReach();
        hanSuu = 2;
    }
}

public class CheckTeetoitu : YakuHandler {
    public CheckTeetoitu(Yaku owner) {
        this.YakuID = 21;
        hantei = owner.checkTeetoitu();
        hanSuu = 2;
    }
}

public class CheckCyanta : YakuHandler {
    public CheckCyanta(Yaku owner) {
        this.YakuID = 22;
        hantei = owner.checkCyanta() && !owner.checkJunCyan() && !owner.checkHonroutou();

        if( owner.isNaki == true ) {
            hanSuu = 1;
        }
        else {
            hanSuu = 2;
        }
    }
}

public class CheckIkkituukan : YakuHandler {
    public CheckIkkituukan(Yaku owner) {
        this.YakuID = 23;
        hantei = owner.checkIkkituukan();

        if( owner.isNaki == true ) {
            hanSuu = 1;
        }
        else {
            hanSuu = 2;
        }
    }
}

public class CheckSansyokuDoujun : YakuHandler {
    public CheckSansyokuDoujun(Yaku owner) {
        this.YakuID = 24;
        hantei = owner.checkSansyokuDoujun();

        if( owner.isNaki == true ) {
            hanSuu = 1;
        }
        else {
            hanSuu = 2;
        }
    }
}

public class CheckSansyokuDoukou : YakuHandler {
    public CheckSansyokuDoukou(Yaku owner) {
        this.YakuID = 25;
        hantei = owner.checkSansyokuDoukou();
        hanSuu = 2;
    }
}

public class CheckToitoi : YakuHandler {
    public CheckToitoi(Yaku owner) {
        this.YakuID = 26;
        hantei = owner.checkToitoi();
        hanSuu = 2;
    }
}

public class CheckSanankou : YakuHandler {
    public CheckSanankou(Yaku owner) {
        this.YakuID = 27;
        hantei = owner.checkSanankou();
        hanSuu = 2;
    }
}

public class CheckSankantu : YakuHandler {
    public CheckSankantu(Yaku owner) {
        this.YakuID = 28;
        hantei = owner.checkSankantu();
        hanSuu = 2;
    }
}

public class CheckRyanpeikou : YakuHandler {
    public CheckRyanpeikou(Yaku owner) {
        this.YakuID = 29;
        hantei = owner.checkRyanpeikou();
        hanSuu = 3;
    }
}

public class CheckHonitu : YakuHandler {
    public CheckHonitu(Yaku owner) {
        this.YakuID = 30;
        hantei = owner.checkHonitu() && !owner.checkTinitu();

        if( owner.isNaki == true ) {
            hanSuu = 2;
        }
        else {
            hanSuu = 3;
        }
    }
}

public class CheckJunCyan : YakuHandler {
    public CheckJunCyan(Yaku owner) {
        this.YakuID = 31;
        hantei = owner.checkJunCyan();

        if( owner.isNaki == true ) {
            hanSuu = 2;
        }
        else {
            hanSuu = 3;
        }
    }
}

public class CheckSyousangen : YakuHandler {
    public CheckSyousangen(Yaku owner) {
        this.YakuID = 32;
        hantei = owner.checkSyousangen();
        hanSuu = 2;
    }
}

public class CheckHonroutou : YakuHandler {
    public CheckHonroutou(Yaku owner) {
        this.YakuID = 33;
        hantei = owner.checkHonroutou();
        hanSuu = 2;
    }
}

public class CheckHonroutouChiitoitsu : YakuHandler {
    public CheckHonroutouChiitoitsu(Yaku owner) {
        this.YakuID = 33; // the same name to Honroutou.
        hantei = owner.checkHonroutouChiitoitsu();
        hanSuu = 2;
    }
}

public class CheckTinitu : YakuHandler {
    public CheckTinitu(Yaku owner) {
        this.YakuID = 34;
        hantei = owner.checkTinitu();

        if( owner.isNaki == true ) {
            hanSuu = 5;
        }
        else {
            hanSuu = 6;
        }
    }
}

public class CheckSuuankou : YakuHandler {
    public CheckSuuankou(Yaku owner) {
        this.YakuID = 35;
        hantei = owner.checkSuuankou();
        hanSuu = 13;
        yakuman = true;
    }
}

public class CheckSuukantu : YakuHandler {
    public CheckSuukantu(Yaku owner) {
        this.YakuID = 36;
        hantei = owner.checkSuukantu();
        hanSuu = 13;
        yakuman = true;
    }
}

public class CheckDaisangen : YakuHandler {
    public CheckDaisangen(Yaku owner) {
        this.YakuID = 37;
        hantei = owner.checkDaisangen();
        hanSuu = 13;
        yakuman = true;
    }
}

public class CheckSyousuushi : YakuHandler {
    public CheckSyousuushi(Yaku owner) {
        this.YakuID = 38;
        hantei = owner.checkSyousuushi();
        hanSuu = 13;
        yakuman = true;
    }
}

public class CheckDaisuushi : YakuHandler {
    public CheckDaisuushi(Yaku owner) {
        this.YakuID = 39;
        hantei = owner.checkDaisuushi();
        hanSuu = 13;
        yakuman = true;
    }
}

public class CheckTuuisou : YakuHandler {
    public CheckTuuisou(Yaku owner) {
        this.YakuID = 40;
        hantei = owner.checkTuuisou();
        hanSuu = 13;
        yakuman = true;
    }
}

public class CheckChinroutou : YakuHandler {
    public CheckChinroutou(Yaku owner) {
        this.YakuID = 41;
        hantei = owner.checkChinroutou();
        hanSuu = 13;
        yakuman = true;
    }
}

public class CheckRyuuisou : YakuHandler {
    public CheckRyuuisou(Yaku owner) {
        this.YakuID = 42;
        hantei = owner.checkRyuuisou();
        hanSuu = 13;
        yakuman = true;
    }
}
public class CheckCyuurennpoutou : YakuHandler {
    public CheckCyuurennpoutou(Yaku owner) {
        this.YakuID = 43;
        hantei = owner.checkCyuurennpoutou();
        hanSuu = 13;
        yakuman = true;
    }
}
public class CheckKokushi : YakuHandler {
    public CheckKokushi(Yaku owner) {
        this.YakuID = 44;
        hantei = owner.checkKokushi();
        hanSuu = 13;
        yakuman = true;
    }
}
public class CheckTenhou : YakuHandler {
    public CheckTenhou(Yaku owner) {
        this.YakuID = 45;
        hantei = owner.checkTenhou();
        hanSuu = 13;
        yakuman = true;
    }
}
public class CheckTihou : YakuHandler {
    public CheckTihou(Yaku owner) {
        this.YakuID = 46;
        hantei = owner.checkTihou();
        hanSuu = 13;
        yakuman = true;
    }
}
public class CheckDora : YakuHandler {
    public CheckDora(Yaku owner) {
        this.YakuID = 47;
        hantei = owner.checkDora();
        hanSuu = 1;
        yakuman = false;
    }
}

