
/** 特殊系役*/
public enum YakuflgName 
{
    /** 立直 */
    REACH,
    /** 一発 */
    IPPATU,
    /** ツモ */
    TUMO,
    /** 海底 */
    HAITEI,
    /** 河底 */
    HOUTEI,
    /** 嶺上開放 */
    RINSYAN,
    /** 搶槓 */
    CHANKAN,
    /** ダブル立直 */
    DOUBLEREACH,
    /** 天和 */
    TENHOU,
    /** 地和 */
    TIHOU,
    /** 人和  */
    RENHOU,
    /** 十三不塔 */
    TIISANPUTOU,
    /** 流し満貫 */
    NAGASHIMANGAN,
    /** 八連荘 */
    PARENCHAN,
    /** 喰いタン */
    KUITAN,
    /** 役の数 */
    YAKUFLGNUM
}


/**
 * 手牌全体の役を判定するクラスです。
 * 计算役.
 */
public class Yaku 
{

    public readonly static int JIKAZE_TON = 0;
    public readonly static int JIKAZE_NAN = 1;
    public readonly static int JIKAZE_SYA = 2;
    public readonly static int JIKAZE_PEI = 3;

    Tehai m_tehai;
    Hai m_addHai;
    Combi m_combi;
    AgariSetting m_setting;
    YakuHantei[] yakuHantei;
    bool nakiFlag = false;
    int m_doraCount;

    bool m_kokushi = false;

    public bool Kokushi { get { return m_kokushi; } }

    public bool GetNakiflg() {
        return nakiFlag;
    }

    /**
     * Yakuクラスのコンストラクタ。
     * 引数を保存し、YakuHanteiクラスの配列を作成する。
     * @param tehai 手牌　addHai 上がった牌  combi 手牌 の組み合わせ info 情報
     */
    public Yaku(Tehai tehai, Hai addHai, Combi combi, AgariSetting setting){
        this.m_tehai = tehai;
        this.m_addHai = addHai;
        this.m_combi  = combi;
        this.m_setting = setting;

        //鳴きがある場合
        nakiFlag = tehai.isNaki();

        YakuHantei[] buffer = 
        {
            new CheckTanyao(this),
            new CheckPinfu(this),
            new CheckIpeikou(this),
            new CheckReach(this),
            new CheckIppatu(this),
            new CheckTumo(this),
            new CheckTon(this),
            new CheckNan(this),
            new CheckSya(this),
            new CheckPei(this),
            new CheckHaku(this),
            new CheckHatu(this),
            new CheckCyun(this),
            new CheckHaitei(this),
            new CheckHoutei(this),
            new CheckRinsyan(this),
            new CheckCyankan(this),
            new CheckDoubleReach(this),
            //          new CheckTeetoitu(this),
            new CheckCyanta(this),
            new CheckIkkituukan(this),
            new CheckSansyokuDoukou(this),
            new CheckSansyokuDoujun(this),
            new CheckToitoi(this),
            new CheckSanankou(this),
            new CheckSankantu(this),
            new CheckRyanpeikou(this),
            new CheckHonitu(this),
            new CheckJunCyan(this),
            new CheckSyousangen(this),
            new CheckHonroutou(this),
            new CheckTinitu(this),
            new CheckSuuankou(this),
            new CheckSuukantu(this),
            new CheckDaisangen(this),
            new CheckSyousuushi(this),
            new CheckDaisuushi(this),
            new CheckTuuisou(this),
            new CheckChinroutou(this),
            new CheckRyuuisou(this),
            new CheckCyuurennpoutou(this),
            new CheckKokushi(this),
            new CheckTenhou(this),
            new CheckTihou(this),
            new CheckDora(this)
        };

        yakuHantei = buffer;

        yakuHantei[yakuHantei.Length - 1].setHanSuu(m_doraCount);

        //役満成立時は他の一般役は切り捨てる
        for(int i = 0 ; i < yakuHantei.Length ; i++)
        {
            if((yakuHantei[i].getYakuman() == true) && (yakuHantei[i].getYakuHantei() == true)) 
            {
                for(int j = 0 ; j < yakuHantei.Length; j++)
                {
                    if(yakuHantei[j].getYakuman() == false){
                        yakuHantei[j].setYakuHantei(false);
                    }
                }
            }
        } // end for(int i).
    }

    /**
     * Yakuクラスのコンストラクタ。
     * 引数を保存し、YakuHanteiクラスの配列を作成する。
     * @param tehai 手牌　addHai 上がった牌  combi 手牌 の組み合わせ info 情報
     */
    public Yaku(Tehai tehai, Hai addHai, AgariSetting setting){
        this.m_tehai = tehai;
        this.m_addHai = addHai;
        this.m_setting = setting;

        this.m_kokushi = false;

        YakuHantei[] buffer = 
        {
            new CheckKokushi(this),
            new CheckTenhou(this),
            new CheckTihou(this)
        };

        this.m_kokushi = buffer[0].getYakuHantei();
        yakuHantei = buffer;
    }

    public Yaku(Tehai tehai, Hai addHai, Combi combi, AgariSetting setting, int a_status) {
        this.m_tehai = tehai;
        this.m_addHai = addHai;
        this.m_combi  = combi;
        this.m_setting = setting;

        this.nakiFlag = false;

        YakuHantei[] buffer = 
        {
            new CheckTanyao(this),
            new CheckReach(this),
            new CheckIppatu(this),
            new CheckTumo(this),
            new CheckHaitei(this),
            new CheckHoutei(this),
            new CheckRinsyan(this),
            new CheckDoubleReach(this),
            new CheckTeetoitu(this),
            new CheckHonroutouChiitoitsu(this),
            new CheckHonitu(this),
            new CheckTinitu(this),
            new CheckTuuisou(this),
            new CheckTenhou(this),
            new CheckTihou(this),
            new CheckDora(this)
        };

        yakuHantei = buffer;

        yakuHantei[yakuHantei.Length - 1].setHanSuu(m_doraCount);
    }

    public AgariSetting getAgariSetting() {
        return this.m_setting;
    }
    public bool getNakiflg() {
        return this.nakiFlag;
    }

    /**
     * 手牌全体の翻数を取得します。
     *
     * @return 手牌全体の翻数
     */
    public int counttHanSuu() {
        int hanSuu = 0;
        for(int i = 0 ; i < yakuHantei.Length ; i++){
            if( yakuHantei[i].getYakuHantei() == true){
                hanSuu+= yakuHantei[i].getHanSuu();
            }
        }

        // ドラのみは無し
        if (hanSuu == yakuHantei[yakuHantei.Length - 1].getHanSuu()) {
            return 0;
        }

        return hanSuu;
    }

    public int getHan() {
        int hanSuu = 0;
        for(int i = 0 ; i < yakuHantei.Length ; i++){
            if( yakuHantei[i].getYakuHantei() == true){
                hanSuu += yakuHantei[i].getHanSuu();
            }
        }

        return hanSuu;
    }

    /**
     * 成立している役の名前を取得します。
     *
     * @return 成立している役の名前の配列
     */
    public string[] getYakuName() {
        int count = 0;
        int hanSuu;

        //成立している役の数をカウント
        for(int i = 0 ; i < yakuHantei.Length ; i++){
            if( yakuHantei[i].getYakuHantei() == true){
                count++;
            }
        }

        string[] yakuNames = new string[count];
        count = 0;
        for(int i = 0 ; i < yakuHantei.Length ; i++){
            if( yakuHantei[i].getYakuHantei() == true){
                hanSuu = yakuHantei[i].getHanSuu();
                if (hanSuu >= 13) {
                    yakuNames[count] = yakuHantei[i].getYakuName() + " " + ResManager.getString("yakuman");
                } 
                else {
                    yakuNames[count] = yakuHantei[i].getYakuName() + " " + ResManager.getString("Han");
                }
                count++;
            }
        }

        return yakuNames;
    }

    /**
     * 役満が成立しているかを取得します。
     */
    public bool checkIfYakuMan() {
        for(int i = 0 ; yakuHantei[i] != null ; i++){
            if( yakuHantei[i].getYakuman() == true){
                return true;
            }
        }
        return false;
    }


    // 断幺九. //
    public bool checkTanyao() {
        int num;
        Hai[] jyunTehai = m_tehai.getJyunTehai();
        Hai[] checkHai;

        Fuuro[] fuuros;
        fuuros = m_tehai.getFuuros();
        int fuuroNum;
        fuuroNum = m_tehai.getFuuroNum();

        // 喰いタンなしで、鳴いていたら不成立。
        if( m_setting.getYakuFlag( (int)YakuflgName.KUITAN ) == false) {
            if (fuuroNum > 0) {
                return false;
            }
        }

        //純手牌をチェック
        int jyunTehaiLength = m_tehai.getJyunTehaiLength();
        for (int i = 0; i < jyunTehaiLength; i++) {
            //１９字牌ならば不成立
            if (jyunTehai[i].isYaochuu() == true){
                return false;
            }
        }

        // 追加牌をチェック

        //１９字牌ならば不成立
        if (m_addHai.isYaochuu() == true){
            return false;
        }

        int type;
        for (int i = 0; i < fuuroNum; i++) {
            type = fuuros[i].getType();
            switch (type) {
                case Fuuro.TYPE_MINSHUN:
                //明順の牌をチェック
                checkHai = fuuros[i].getHais();
                num = checkHai[0].getNum();
                //123 と 789 の順子があれば不成立
                if ((num == 1) || (num == 7)){
                    return false;
                }
                break;
                case Fuuro.TYPE_MINKOU:
                //明刻の牌をチェック
                checkHai = fuuros[i].getHais();
                if (checkHai[0].isYaochuu() == true){
                    return false;
                }
                break;
                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                //明槓の牌をチェック
                checkHai = fuuros[i].getHais();
                if (checkHai[0].isYaochuu() == true){
                    return false;
                }
                break;
                case Fuuro.TYPE_ANKAN:
                //暗槓の牌をチェック
                checkHai = fuuros[i].getHais();
                if (checkHai[0].isYaochuu() == true){
                    return false;
                }
                break;
                default:
                break;
            }
        }

        return true;
    }

    // 平和 //
    public bool checkPinfu() {
        Hai atamaHai;
        //鳴きが入っている場合は成立しない
        if(nakiFlag == true){
            return false;
        }

        //面子が順子だけではない
        if(m_combi.m_shunNum != 4){
            return false;
        }

        //頭が三元牌
        atamaHai = new Hai(Hai.NumKindToID(m_combi.m_atamaNumKind));
        if( atamaHai.getKind() == Hai.KIND_SANGEN ){
            return false;
        }

        //頭が場風
        if( atamaHai.getKind() == Hai.KIND_FON
           && (atamaHai.getNum() - 1) == m_setting.getBakaze())
        {
            return false;
        }

        //頭が自風
        if( atamaHai.getKind() == Hai.KIND_FON
           && (atamaHai.getNum() - 1) == m_setting.getJikaze())
        {
            return false;
        }

        //字牌の頭待ちの場合は不成立//
        if(m_addHai.isTsuu() == true){
            return false;
        }

        //待ちが両面待ちか判定//
        bool ryanmenFlag = false;
        int addHaiid = m_addHai.getNumKind();

        //上がり牌の数をチェックして場合分け
        switch(m_addHai.getNum())
        {
            //上がり牌が1,2,3の場合は123,234,345の順子ができているかどうかチェック
            case 1:
            case 2:
            case 3:
            for(int i = 0 ; i < m_combi.m_shunNum ; i++)
            {
                if(addHaiid == m_combi.m_shunNumKinds[i]){
                    ryanmenFlag = true;
                }
            }
            break;
            //上がり牌が4,5,6の場合は456か234,567か345,678か456の順子ができているかどうかチェック
            case 4:
            case 5:
            case 6:
            for(int i = 0 ; i < m_combi.m_shunNum ; i++)
            {
                if((addHaiid == m_combi.m_shunNumKinds[i])
                       ||(addHaiid-2 == m_combi.m_shunNumKinds[i]))
                {
                    ryanmenFlag = true;
                }
            }
            break;
            //上がり牌が7,8,9の場合は567,678,789の順子ができているかどうかチェック
            case 7:
            case 8:
            case 9:
            for(int i = 0 ; i < m_combi.m_shunNum ; i++)
            {
                if(addHaiid-2 == (m_combi.m_shunNumKinds[i])){
                    ryanmenFlag = true;
                }
            }
            break;
            default:
            break;
        }

        if(ryanmenFlag == false){
            return false;
        }

        //条件を満たしていれば、約成立
        return true;
    }

    // 一杯口 //
    public bool checkIpeikou() {
        //鳴きが入っている場合は成立しない
        if(nakiFlag == true){
            return false;
        }

        //順子の組み合わせを確認する
        for (int i = 0; i < m_combi.m_shunNum -1; i++) {
            if(m_combi.m_shunNumKinds[i] == m_combi.m_shunNumKinds[i+1]){
                return true;
            }
        }

        return false;
    }

    // 立直 //
    public bool checkReach() {
        return m_setting.getYakuFlag( (int)YakuflgName.REACH );
    }

    // 一发(立直后，轮牌内和了) //
    public bool checkIppatu() {
        return m_setting.getYakuFlag( (int)YakuflgName.IPPATU );
    }

    // 门前清自摸和 //
    public bool checkTumo() {
        //鳴きが入っている場合は成立しない
        if(nakiFlag == true){
            return false;
        }
        return m_setting.getYakuFlag((int)YakuflgName.TUMO);
    }


    //役牌ができているかどうかの判定に使う補助メソッド
    public bool checkYakuHai(Tehai tehai, Combi combi, int yakuHaiId) {
        int id;
        Hai[] checkHai;

        //純手牌をチェック
        for(int i = 0; i < combi.m_kouNum ; i++){
            //IDと役牌のIDをチェック
            id = Hai.NumKindToID(combi.m_kouNumKinds[i]);
            if( id == yakuHaiId ){
                return true;
            }
        }

        Fuuro[] fuuros = tehai.getFuuros();
        int fuuroNum = tehai.getFuuroNum();
        int type;

        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();

            switch (type) {
                case Fuuro.TYPE_MINKOU:
                //明刻の牌をチェック
                checkHai = fuuros[i].getHais();
                id = checkHai[0].getID();
                //IDと役牌のIDをチェック
                if( id == yakuHaiId ){
                    return true;
                }
                break;
                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                //明槓の牌をチェック
                checkHai = fuuros[i].getHais();
                id = checkHai[0].getID();
                //IDと役牌のIDをチェック
                if( id == yakuHaiId ){
                    return true;
                }
                break;
                case Fuuro.TYPE_ANKAN:
                //暗槓の牌をチェック
                checkHai = fuuros[i].getHais();
                id = checkHai[0].getID();
                //IDと役牌のIDをチェック
                if( id == yakuHaiId ){
                    return true;
                }
                break;
                default:
                break;
            }
        }
        return false;
    }

    // 东 //
    public bool checkTon() {
        if((m_setting.getJikaze() == JIKAZE_TON) || (m_setting.getBakaze() == JIKAZE_TON)){
            return checkYakuHai(m_tehai,m_combi, Hai.ID_TON);
        }
        else{
            return false;
        }
    }

    // 南 //
    public bool checkNan() {
        if((m_setting.getJikaze() == JIKAZE_NAN) || (m_setting.getBakaze() == JIKAZE_NAN)){
            return checkYakuHai(m_tehai,m_combi, Hai.ID_NAN);
        }
        else{
            return false;
        }
    }

    // 西 //
    public bool checkSya() {
        if(m_setting.getJikaze() == JIKAZE_SYA){
            return checkYakuHai(m_tehai, m_combi, Hai.ID_SYA);
        }
        else{
            return false;
        }
    }

    // 北 //
    public bool checkPei() {
        if(m_setting.getJikaze() == JIKAZE_PEI){
            return checkYakuHai(m_tehai, m_combi, Hai.ID_PE);
        }
        else{
            return false;
        }
    }

    // 白 //
    public bool checkHaku() {
        return checkYakuHai(m_tehai, m_combi, Hai.ID_HAKU);
    }

    // 发 //
    public bool checkHatu() {
        return checkYakuHai(m_tehai, m_combi, Hai.ID_HATSU);
    }

    // 中 //
    public bool checkCyun() {
        return checkYakuHai(m_tehai, m_combi, Hai.ID_CHUN);
    }

    // 海底捞月 //
    public bool checkHaitei() {
        return m_setting.getYakuFlag((int)YakuflgName.HAITEI);
    }

    // 河底捞鱼 //
    public bool checkHoutei() {
        return m_setting.getYakuFlag((int)YakuflgName.HOUTEI);
    }

    // 岭上开花 //
    public bool checkRinsyan() {
        return m_setting.getYakuFlag((int)YakuflgName.RINSYAN);
    }

    // 抢杠 //
    public bool checkCyankan() {
        return m_setting.getYakuFlag((int)YakuflgName.CHANKAN);
    }

    // 双立直 //
    public bool checkDoubleReach() {
        return m_setting.getYakuFlag((int)YakuflgName.DOUBLEREACH);
    }

    // 七对子 //
    public bool checkTeetoitu() {
        //鳴きが入っている場合は成立しない
        if(nakiFlag == true){
            return false;
        }

        return true;
    }

    // 混全带幺九 //
    public bool checkCyanta() {
        Hai[] checkHais;
        Hai checkHai;

        //純手牌の刻子をチェック
        for(int i = 0; i < m_combi.m_kouNum ; i++){
            checkHai = new Hai(Hai.NumKindToID(m_combi.m_kouNumKinds[i]));
            //数牌の場合は数字をチェック
            if (checkHai.isYaochuu() == false){
                return false;
            }
        }

        //純手牌の順子をチェック
        for(int i = 0; i < m_combi.m_shunNum ; i++){
            checkHai = new Hai(Hai.NumKindToID(m_combi.m_shunNumKinds[i]));
            //数牌の場合は数字をチェック
            if (checkHai.isTsuu() == false){
                if ((checkHai.getNum() > 1) && (checkHai.getNum() < 7))
                    return false;
            }
        }

        //純手牌の頭をチェック
        checkHai = new Hai(Hai.NumKindToID(m_combi.m_atamaNumKind));
        if (checkHai.isYaochuu() == false){
            return false;
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int type;

        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();
            switch (type) 
            {
                case Fuuro.TYPE_MINSHUN:
                //明順の牌をチェック
                checkHais = fuuros[i].getHais();
                //123 と　789 以外の順子があれば不成立
                if ((checkHais[0].getNum() > 1) && (checkHais[0].getNum() < 7))
                    return false;
                break;

                case Fuuro.TYPE_MINKOU:
                //明刻の牌をチェック
                checkHais = fuuros[i].getHais();
                //数牌の場合は数字をチェック
                if (checkHais[0].isYaochuu() == false){
                    return false;
                }
                break;

                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                //明槓の牌をチェック
                checkHais = fuuros[i].getHais();
                //数牌の場合は数字をチェック
                if (checkHais[0].isYaochuu() == false){
                    return false;
                }
                break;

                case Fuuro.TYPE_ANKAN:
                //暗槓の牌をチェック
                checkHais = fuuros[i].getHais();
                //数牌の場合は数字をチェック
                if (checkHais[0].isYaochuu() == false){
                    return false;
                }
                break;

                default:
                break;
            }
        }

        return true;
    }

    // 一气通贯 //
    public bool checkIkkituukan() {
        int id;
        Hai[] checkHai;
        bool[] ikkituukanflg = {false,false,false,false,false,false,false,false,false};
        //萬子、筒子、索子の1,4,7をチェック
        int[] checkId = { Hai.ID_WAN_1, Hai.ID_WAN_4, Hai.ID_WAN_7, Hai.ID_PIN_1, Hai.ID_PIN_4, Hai.ID_PIN_7, Hai.ID_SOU_1, Hai.ID_SOU_4, Hai.ID_SOU_7 };

        //手牌の順子をチェック
        for(int i = 0 ; i < m_combi.m_shunNum ; i++){
            id = Hai.NumKindToID(m_combi.m_shunNumKinds[i]);
            for(int j =0 ; j < checkId.Length ; j++){
                if(id == checkId[j]){
                    ikkituukanflg[j] = true;
                }
            }
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int type;

        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();
            switch (type) {
                case Fuuro.TYPE_MINSHUN:
                //鳴いた牌をチェック
                checkHai = fuuros[i].getHais();
                id = checkHai[0].getID();
                for(int j =0 ; j < checkId.Length ; j++){
                    if(id == checkId[j]){
                        ikkituukanflg[j] = true;
                    }
                }
                break;
                default:
                break;
            }
        }

        //一気通貫が出来ているかどうかチェック
        if( (ikkituukanflg[0] == true && ikkituukanflg[1] == true && ikkituukanflg[2] == true ) ||
           (ikkituukanflg[3] == true && ikkituukanflg[4] == true && ikkituukanflg[5] == true ) ||
           (ikkituukanflg[6] == true && ikkituukanflg[7] == true && ikkituukanflg[8] == true ))
        {
            return true;
        }
        else{
            return false;
        }
    }

    //三色ができているかどうかの判定に使う補助メソッド
    private static void checkSansyoku(int id, bool[][] sansyokuFlag) {
        //萬子、筒子、索子をチェック
        int[] checkId = { Hai.ID_WAN_1, Hai.ID_PIN_1, Hai.ID_SOU_1 };

        for(int i =0 ; i < sansyokuFlag.Length ; i++){
            for(int j = 0; j < sansyokuFlag[i].Length; j++){
                if(id == (checkId[i] + j) ){
                    sansyokuFlag[i][j] = true;
                }
            }
        }
    }

    // 三色同顺 //
    public bool checkSansyokuDoujun() {
        int id;
        Hai[] checkHai;

        const int Col = 9;
        bool[][] sansyokuFlag = new bool[3][];

        //フラグの初期化
        for(int i = 0 ; i < sansyokuFlag.Length; i++){
            for( int k = 0; k < Col; k++ ) {
                sansyokuFlag[i][k] = false;
            }
        }

        //手牌の順子をチェック
        for(int i = 0 ; i < m_combi.m_shunNum ; i++){
            id = Hai.NumKindToID(m_combi.m_shunNumKinds[i]);
            checkSansyoku(id,sansyokuFlag);
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int type;

        for (int i = 0; i < fuuroNum; i++) {
            type = fuuros[i].getType();
            switch (type) {
                case Fuuro.TYPE_MINSHUN:
                //鳴いた牌をチェック
                checkHai = fuuros[i].getHais();
                id = checkHai[0].getID();
                checkSansyoku(id,sansyokuFlag);
                break;
                default:
                break;
            }
        }

        //三色同順が出来ているかどうかチェック
        for(int i = 0 ; i < sansyokuFlag[0].Length ; i++){
            if( (sansyokuFlag[0][i] == true) && (sansyokuFlag[1][i] == true ) && (sansyokuFlag[2][i] == true)){
                return true;
            }
        }
        //出来ていない場合 falseを返却
        return false;
    }

    // 三色同刻 //
    public bool checkSansyokuDoukou() {
        int id;
        Hai[] checkHai;

        const int Col = 9;
        bool[][] sansyokuFlag = new bool[3][];

        //フラグの初期化
        for(int i = 0 ; i<sansyokuFlag.Length; i++){
            for (int k = 0; k < Col ; k++){
                sansyokuFlag[i][k] = false;
            }
        }

        //手牌の刻子をチェック
        for(int i = 0 ; i < m_combi.m_kouNum ; i++){
            id = Hai.NumKindToID(m_combi.m_kouNumKinds[i]);
            checkSansyoku(id, sansyokuFlag);
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();

        int type;

        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();
            switch (type) {
                case Fuuro.TYPE_MINKOU:
                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                case Fuuro.TYPE_ANKAN:
                //鳴いた牌の明刻をチェック
                //鳴いた牌の明槓をチェック
                //鳴いた牌の暗槓をチェック
                checkHai = fuuros[i].getHais();
                id = checkHai[0].getID();
                checkSansyoku(id,sansyokuFlag);
                break;

                default:
                break;
            }
        }

        //三色同刻が出来ているかどうかチェック
        for(int i = 0 ; i < sansyokuFlag[0].Length ; i++){
            if( (sansyokuFlag[0][i] == true) && (sansyokuFlag[1][i] == true ) && (sansyokuFlag[2][i] == true)){
                return true;
            }
        }

        //出来ていない場合 falseを返却
        return false;
    }

    // 对对和 //
    public bool checkToitoi() {
        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int minShunNum = 0;

        for (int i = 0; i < fuuroNum; i++) 
        {
            if( fuuros[i].getType() == Fuuro.TYPE_MINSHUN ) {
                minShunNum++;
            }
        }

        //手牌に順子がある
        if((m_combi.m_shunNum != 0) || (minShunNum != 0) ){
            return false;
        }
        else{
            return true;
        }
    }

    // 三暗刻 //
    public bool checkSanankou() {
        //対々形で鳴きがなければ成立している【ツモ和了りや単騎の場合、四暗刻が優先される）
        if((checkToitoi() == true) && (nakiFlag == false)){
            return true;
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int anKanNum = 0;

        for (int i = 0; i < fuuroNum; i++) 
        {
            if( fuuros[i].getType() == Fuuro.TYPE_ANKAN ) {
                anKanNum++;
            }
        }

        //暗刻と暗槓の合計が３つではない場合は不成立
        if((m_combi.m_kouNum + anKanNum) != 3){
            return false;
        }

        //ツモ上がりの場合は成立
        if( m_setting.getYakuFlag((int)YakuflgName.TUMO) == true ) {
            return true;
        }
        else  //ロン上がりの場合、和了った牌と
        {
            int numKind = m_addHai.getNumKind();

            //ロン上がりで頭待ちの場合は成立
            if(numKind == m_combi.m_atamaNumKind){
                return true;
            }
            else
            {
                //和了った牌と刻子になっている牌が同じか確認
                bool checkFlag = false;
                for(int i = 0 ; i < m_combi.m_kouNum ; i++){
                    if(numKind == m_combi.m_kouNumKinds[i]){
                        checkFlag = true;
                    }
                }

                //刻子の牌で和了った場合
                if(checkFlag == true)
                {
                    //字牌ならば不成立
                    if(m_addHai.isTsuu() == true){
                        return false;
                    }
                    else
                    {
                        // 順子の待ちにもなっていないか確認する
                        // 例:11123 で1で和了り  , 45556 の5で和了り
                        bool checkshun = false;
                        for(int i = 0 ; i < m_combi.m_shunNum ; i++)
                        {
                            switch(m_addHai.getNum())
                            {
                                case 1:
                                if(numKind == m_combi.m_shunNumKinds[i]){
                                    checkshun = true;
                                }
                                break;

                                case 2:
                                if((numKind == m_combi.m_shunNumKinds[i])
                                   ||(numKind-1 == m_combi.m_shunNumKinds[i]))
                                {
                                    checkshun = true;
                                }
                                break;
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                if( (numKind == m_combi.m_shunNumKinds[i]) ||
                                   (numKind-1 == m_combi.m_shunNumKinds[i]) ||
                                   (numKind-2 == m_combi.m_shunNumKinds[i]) )
                                {
                                    checkshun = true;
                                }
                                break;
                                case 8:
                                if( (numKind-1 == m_combi.m_shunNumKinds[i])
                                   ||(numKind-2 == m_combi.m_shunNumKinds[i]))
                                {
                                    checkshun = true;
                                }
                                break;
                                case 9:
                                if(numKind-2 == m_combi.m_shunNumKinds[i]){
                                    checkshun = true;
                                }
                                break;
                                default:
                                break;
                            } // end switch().

                        } // end for().

                        //順子の牌でもあった場合は成立
                        if(checkshun == true){
                            return true;
                        }
                        else  //関係ある順子がなかった場合は不成立
                        {
                            return false;
                        }

                    } // end else.
                }
                else  //刻子と関係ない牌で和了った場合は成立
                {
                    return true;
                }

            } // end else.

        } // end else.

    }

    // 三杠子 //
    public bool checkSankantu() {
        int kansnumber = 0;

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int type;

        for (int i = 0; i < fuuroNum; i++) {
            type = fuuros[i].getType();
            switch (type) {
                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                case Fuuro.TYPE_ANKAN:
                kansnumber++;
                break;
                default:
                break;
            }
        }

        if(kansnumber == 3){
            return true;
        }
        else{
            return false;
        }
    }

    // 二杯口 //
    public bool checkRyanpeikou() {
        //鳴きが入っている場合は成立しない
        if(nakiFlag == true){
            return false;
        }

        //順子が４つである
        if(m_combi.m_shunNum < 4){
            return false;
        }

        //順子の組み合わせを確認する
        if( m_combi.m_shunNumKinds[0] == m_combi.m_shunNumKinds[1]
           && m_combi.m_shunNumKinds[2] == m_combi.m_shunNumKinds[3])
        {
            return true;
        }
        else{
            return false;
        }
    }

    // 混一色 //
    public bool checkHonitu() {
        Hai[] jyunTehai = m_tehai.getJyunTehai();
        Hai[] checkHai;

        //萬子、筒子、索子をチェック
        int[] checkId = { Hai.KIND_WAN, Hai.KIND_PIN, Hai.KIND_SOU };

        for (int i = 0 ; i < checkId.Length ; i++)
        {
            bool honituFlag = true;

            //純手牌をチェック
            int jyunTehaiLength = m_tehai.getJyunTehaiLength();
            for (int j = 0; j < jyunTehaiLength; j++) {
                //牌が(萬子、筒子、索子)以外もしくは字牌以外
                if ( (jyunTehai[j].getKind() != checkId[i]) && (jyunTehai[j].isTsuu() == false) ){
                    honituFlag = false;
                }
            }

            Fuuro[] fuuros = m_tehai.getFuuros();
            int fuuroNum = m_tehai.getFuuroNum();
            int type;

            for (int j = 0; j < fuuroNum; j++) {
                type = fuuros[j].getType();
                switch (type) {
                    case Fuuro.TYPE_MINSHUN:
                    case Fuuro.TYPE_MINKOU:
                    case Fuuro.TYPE_DAIMINKAN:
                    case Fuuro.TYPE_KAKAN:
                    case Fuuro.TYPE_ANKAN:
                    //明順の牌をチェック
                    //明刻の牌をチェック
                    //明槓の牌をチェック
                    //暗槓の牌をチェック
                    checkHai = fuuros[j].getHais();
                    //牌が(萬子、筒子、索子)以外もしくは字牌以外
                    if ((checkHai[0].getKind() != checkId[i]) && (checkHai[0].isTsuu() == false)){
                        honituFlag = false;
                    }
                    break;
                    default:
                    break;
                }
            }

            //混一が成立している
            if(honituFlag == true){
                return true;
            }

        } // end for(i < checkId.Length).

        //成立していなけらば不成立
        return false;
    }

    // 纯全带幺九 //
    public bool checkJunCyan() {
        Hai[] checkHais;
        Hai checkHai;

        //純手牌の刻子をチェック
        for(int i = 0; i < m_combi.m_kouNum ; i++)
        {
            checkHai = new Hai(Hai.NumKindToID(m_combi.m_kouNumKinds[i]));
            //字牌があれば不成立
            if( checkHai.isTsuu() == true){
                return false;
            }

            //中張牌ならば不成立
            if(checkHai.isYaochuu() == false ){
                return false;
            }
        }

        //純手牌の順子をチェック
        for(int i = 0; i < m_combi.m_shunNum ; i++)
        {
            checkHai = new Hai(Hai.NumKindToID(m_combi.m_shunNumKinds[i]));
            //字牌があれば不成立
            if( checkHai.isTsuu() == true){
                return false;
            }

            //数牌の場合は数字をチェック
            if( (checkHai.getNum() > Hai.NUM_1) && (checkHai.getNum() < Hai.NUM_7) ) {
                return false;
            }
        }

        //純手牌の頭をチェック
        checkHai = new Hai(Hai.NumKindToID(m_combi.m_atamaNumKind));

        //字牌があれば不成立
        if( checkHai.isTsuu() == true){
            return false;
        }

        //中張牌ならば不成立
        if(checkHai.isYaochuu() == false ){
            return false;
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int type;

        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();

            switch (type) {
                case Fuuro.TYPE_MINSHUN:
                //明順の牌をチェック
                checkHais = fuuros[i].getHais();
                //123 と　789 以外の順子があれば不成立
                if( (checkHais[0].getNum() > Hai.NUM_1) && (checkHais[0].getNum() < Hai.NUM_7) ) {
                    return false;
                }
                break;

                case Fuuro.TYPE_MINKOU:
                //明刻の牌をチェック
                checkHais = fuuros[i].getHais();
                //字牌があれば不成立
                if( checkHais[0].isTsuu() == true){
                    return false;
                }
                //中張牌ならば不成立
                if(checkHais[0].isYaochuu() == false ){
                    return false;
                }
                break;

                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                //明槓の牌をチェック
                checkHais = fuuros[i].getHais();
                //字牌があれば不成立
                if( checkHais[0].isTsuu() == true){
                    return false;
                }
                //中張牌ならば不成立
                if(checkHais[0].isYaochuu() == false ){
                    return false;
                }
                break;

                case Fuuro.TYPE_ANKAN:
                //暗槓の牌をチェック
                checkHais = fuuros[i].getHais();
                //字牌があれば不成立
                if( checkHais[0].isTsuu() == true){
                    return false;
                }
                //中張牌ならば不成立
                if(checkHais[0].isYaochuu() == false ){
                    return false;
                }
                break;

                default:
                break;
            }
        }

        return true;
    }

    // 小三元 //
    public bool checkSyousangen() {
        //三元牌役が成立している個数を調べる
        int countSangen = 0;
        //白が刻子
        if(checkHaku() == true){
            countSangen++;
        }
        //発が刻子
        if(checkHatu() == true){
            countSangen++;
        }
        //中が刻子
        if(checkCyun() == true){
            countSangen++;
        }
        //頭が三元牌 かつ、三元牌役が2つ成立
        //Hai atamaHai = new Hai(combi.m_atamaNoKind);
        if( ((m_combi.m_atamaNumKind & Hai.KIND_SANGEN) == Hai.KIND_SANGEN) && (countSangen == 2) ) {
            return true;
        }

        return false;
    }

    // 混老头 //
    public bool checkHonroutou() {
        //トイトイが成立している
        if(checkToitoi() == false){
            return false;
        }

        //チャンタが成立している
        if(checkCyanta() == true){
            return true;
        }
        else{
            return false;
        }
    }

    // 混老头七对子 //
    public bool checkHonroutouChiitoitsu() {
        Hai[] jyunTehai = m_tehai.getJyunTehai();

        //純手牌をチェック
        int jyunTehaiLength = m_tehai.getJyunTehaiLength();
        for (int i = 0; i < jyunTehaiLength; i++) {
            //１９字牌ならば成立
            if (jyunTehai[i].isYaochuu() == false){
                return false;
            }
        }

        // 追加牌をチェック

        //１９字牌ならば成立
        if (m_addHai.isYaochuu() == false){
            return false;
        }

        return true;
    }

    // 人和 //
    public bool checkRenhou() {
        if( m_setting.getYakuFlag((int)YakuflgName.RENHOU) ) {
            return true;
        }
        else{
            return false;
        }
    }

    // 清一色 //
    public bool checkTinitu() {
        Hai[] jyunTehai = m_tehai.getJyunTehai();
        Hai[] checkHai;

        //萬子、筒子、索子をチェック
        int[] checkId = { Hai.KIND_WAN, Hai.KIND_PIN, Hai.KIND_SOU };

        for (int i = 0 ; i < checkId.Length ; i++)
        {
            bool tinituFlag = true;

            //純手牌をチェック
            int jyunTehaiLength = m_tehai.getJyunTehaiLength();
            for (int j = 0; j < jyunTehaiLength; j++) {
                //牌が(萬子、筒子、索子)以外
                if (jyunTehai[j].getKind() != checkId[i]){
                    tinituFlag = false;
                    break;
                }
            }

            Fuuro[] fuuros = m_tehai.getFuuros();
            int fuuroNum = m_tehai.getFuuroNum();

            for (int j = 0; j < fuuroNum; j++) {
                checkHai = fuuros[j].getHais();
                //牌が(萬子、筒子、索子)以外
                if (checkHai[0].getKind() != checkId[i]){
                    tinituFlag = false;
                    break;
                }
            }

            //清一が成立している
            if(tinituFlag == true){
                return true;
            }

        } // end for().

        //成立していなければ不成立
        return false;
    }

    // 四暗刻 //
    public bool checkSuuankou() {
        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int anKanNum = 0;

        for (int i = 0; i < fuuroNum; i++) 
        {
            if( fuuros[i].getType() == Fuuro.TYPE_ANKAN ) {
                anKanNum++;
            }
        }

        //手牌の暗刻が4つ
        if((m_combi.m_kouNum + anKanNum) < 4){
            return false;
        }
        else
        {
            //ツモ和了りの場合は成立
            if( m_setting.getYakuFlag((int)YakuflgName.TUMO) ) {
                return true;
            }
            else //ロン和了りの場合
            {
                //頭待ちならば成立 (四暗刻単騎待ち)
                if(m_addHai.getNumKind() == m_combi.m_atamaNumKind){
                    return true;
                }
                else{
                    return false;
                }
            }
        }
    }

    // 四杠子 //
    public bool checkSuukantu() {
        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int kansnumber = 0;
        int type;

        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();

            switch (type) {
                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                case Fuuro.TYPE_ANKAN:
                kansnumber++;
                break;

                default:
                break;
            }
        }

        if(kansnumber == 4){
            return true;
        }
        else{
            return false;
        }
    }

    // 大三元 //
    public bool checkDaisangen() {
        //三元牌役が成立している個数を調べる
        int countSangen = 0;
        //白が刻子
        if(checkHaku() == true){
            countSangen++;
        }
        //発が刻子
        if(checkHatu() == true){
            countSangen++;
        }
        //中が刻子
        if(checkCyun() == true){
            countSangen++;
        }

        //３元牌が３つ揃っている
        if(countSangen == 3){
            return true;
        }
        else{
            return false;
        }
    }

    // 天和 //
    public bool checkTenhou() {
        return m_setting.getYakuFlag((int)YakuflgName.TENHOU);
    }

    // 地和 //
    public bool checkTihou() {
        return m_setting.getYakuFlag((int)YakuflgName.TIHOU);
    }

    // 小四喜 //
    public bool checkSyousuushi() {
        //風牌役が成立している個数を調べる
        int countFon = 0;
        //東が刻子
        if(checkYakuHai(m_tehai,m_combi, Hai.ID_TON) == true){
            countFon++;
        }
        //南が刻子
        if( checkYakuHai(m_tehai, m_combi, Hai.ID_NAN) == true ) {
            countFon++;
        }
        //西が刻子
        if( checkYakuHai(m_tehai, m_combi, Hai.ID_SYA) == true ) {
            countFon++;
        }
        //北が刻子
        if( checkYakuHai(m_tehai, m_combi, Hai.ID_PE) == true ) {
            countFon++;
        }

        //頭が風牌 かつ、風牌役が3つ成立
        if( ((m_combi.m_atamaNumKind & Hai.KIND_FON) == Hai.KIND_FON) && (countFon == 3) ) {
            return true;
        }
        else{
            return false;
        }
    }

    // 大四喜 //
    public bool checkDaisuushi() {
        //風牌役が成立している個数を調べる
        int countFon = 0;
        //東が刻子
        if( checkYakuHai(m_tehai, m_combi, Hai.ID_TON) == true ) {
            countFon++;
        }
        //南が刻子
        if( checkYakuHai(m_tehai, m_combi, Hai.ID_NAN) == true ) {
            countFon++;
        }
        //西が刻子
        if( checkYakuHai(m_tehai, m_combi, Hai.ID_SYA) == true ) {
            countFon++;
        }
        //北が刻子
        if( checkYakuHai(m_tehai, m_combi, Hai.ID_PE) == true ) {
            countFon++;
        }

        //風牌役が4つ成立
        if(countFon == 4){
            return true;
        }
        else{
            return false;
        }
    }

    // 字一色 //
    public bool checkTuuisou() {
        Hai[] jyunTehai = m_tehai.getJyunTehai();
        Hai[] checkHai;

        //順子があるかどうか確認
        if(checkToitoi() == false){
            return false;
        }

        //純手牌をチェック
        int jyunTehaiLength = m_tehai.getJyunTehaiLength();
        for (int j = 0; j < jyunTehaiLength; j++) {
            //牌が字牌ではない
            if (jyunTehai[j].isTsuu() == false){
                return false;
            }
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();

        for (int i = 0; i < fuuroNum; i++) {
            checkHai = fuuros[i].getHais();
            //牌が字牌ではない
            if (checkHai[0].isTsuu() == false){
                return false;
            }
        }

        return true;
    }

    // 清老头 //
    public bool checkChinroutou() {
        //順子があるかどうか確認
        if(checkToitoi() == false){
            return false;
        }

        //順子なしでジュンチャンが成立しているか（1と9のみで作成）
        if(checkJunCyan() == false){
            return false;
        }

        return true;
    }

    // 绿一色 //
    public bool checkRyuuisou() {
        int[] checkId = { Hai.ID_SOU_2, Hai.ID_SOU_3, Hai.ID_SOU_4, Hai.ID_SOU_6, Hai.ID_SOU_8, Hai.ID_HATSU };
        int id;
        bool ryuuisouFlag = false;
        Hai[] jyunTehai = m_tehai.getJyunTehai();
        Hai[] checkHai;

        //純手牌をチェック
        int jyunTehaiLength = m_tehai.getJyunTehaiLength();

        for (int i = 0; i < jyunTehaiLength; i++) 
        {
            id = jyunTehai[i].getID();
            ryuuisouFlag = false;

            for(int j = 0 ; j < checkId.Length ; j++){
                //緑一色に使用できる牌だった
                if(id == checkId[j]){
                    ryuuisouFlag = true;
                }
            }
            //該当する牌ではなかった
            if(ryuuisouFlag == false){
                return false;
            }
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int type;

        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();

            switch (type) {
                case Fuuro.TYPE_MINSHUN:
                //明順の牌をチェック
                checkHai = fuuros[i].getHais();
                id = checkHai[0].getID();
                //索子の2,3,4以外の順子があった場合不成立
                if( id != Hai.ID_SOU_2 ) {
                    return false;
                }
                break;

                case Fuuro.TYPE_MINKOU:
                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                case Fuuro.TYPE_ANKAN:
                checkHai = fuuros[i].getHais();
                id = checkHai[0].getID();
                ryuuisouFlag = false;
                for(int j = 0 ; j < checkId.Length ; j++){
                    //緑一色に使用できる牌だった
                    if(id == checkId[j]){
                        ryuuisouFlag = true;
                    }
                }
                //該当する牌ではなかった
                if(ryuuisouFlag == false){
                    return false;
                }
                break;

                default:
                break;
            }
        }

        //条件に該当した
        return true;
    }

    // 九莲宝灯 //
    public bool checkCyuurennpoutou() {
        //鳴きがある場合は成立しない
        if( nakiFlag == true ) {
            return false;
        }

        //牌の数を調べるための配列 (0番地は使用しない）
        int[] countNumber = {0,0,0,0,0,0,0,0,0,0};
        Hai[] checkHai = new Hai[Tehai.JYUN_TEHAI_LENGTH_MAX];

        //手牌が清一になっていない場合も成立しない
        if(checkTinitu() == false){
            return false;
        }

        //手牌をコピーする
        checkHai = m_tehai.getJyunTehai();

        //手牌にある牌の番号を調べる
        for(int i = 0 ; i < m_tehai.getJyunTehaiLength() ; i++){
            //数字の番号をインクリメントする
            countNumber[checkHai[i].getNum()]++;
        }

        //九蓮宝燈になっているか調べる（1と9が３枚以上 2～8が１枚以上)
        if(( countNumber[1] >= 3)
           && ( countNumber[2] >= 1)
           && ( countNumber[3] >= 1)
           && ( countNumber[4] >= 1)
           && ( countNumber[5] >= 1)
           && ( countNumber[6] >= 1)
           && ( countNumber[7] >= 1)
           && ( countNumber[8] >= 1)
           && ( countNumber[9] >= 3))
        {
            return true;
        }

        return false;
    }

    // 国士无双 //
    public bool checkKokushi() {
        //鳴きがある場合は成立しない
        if( nakiFlag == true ) {
            return false;
        }

        //牌の数を調べるための配列 (0番地は使用しない）
        int[] checkId = { Hai.ID_WAN_1, Hai.ID_WAN_9,Hai.ID_PIN_1,Hai.ID_PIN_9,Hai.ID_SOU_1,Hai.ID_SOU_9,
            Hai.ID_TON, Hai.ID_NAN,Hai.ID_SYA,Hai.ID_PE,Hai.ID_HAKU,Hai.ID_HATSU,Hai.ID_CHUN
        };
        int[] countHai = {0,0,0,0,0,0,0,0,0,0,0,0,0};
        Hai[] checkHai = new Hai[Tehai.JYUN_TEHAI_LENGTH_MAX];

        //手牌をコピーする
        checkHai = m_tehai.getJyunTehai();

        //手牌のIDを検索する
        for(int i = 0 ; i < m_tehai.getJyunTehaiLength() ; i++){
            for(int j = 0 ; j < checkId.Length ; j++){
                if(checkHai[i].getID() == checkId[j]){
                    countHai[j]++;
                }
            }
        }

        for(int j = 0 ; j < checkId.Length ; j++){
            if(m_addHai.getID() == checkId[j]){
                countHai[j]++;
            }
        }

        bool atama = false;

        //国士無双が成立しているか調べる(手牌がすべて1.9字牌 すべての１,９字牌を持っている）
        for(int i = 0 ; i < countHai.Length ; i++)
        {
            //0枚の牌があれば不成立
            if(countHai[i] == 0){
                return false;
            }
            if(countHai[i] == 2){
                atama = true;
            }
        }

        //条件を満たしていれば成立
        if (atama) {
            return true;
        } 
        else {
            return false;
        }
    }

    // 悬赏牌 //
    public bool checkDora() {
        int doraCount = 0;

        Hai[] doraHais = m_setting.getOmoteDoraHais();

        Hai[] jyunTehai = m_tehai.getJyunTehai();
        int jyunTehaiLength = m_tehai.getJyunTehaiLength();
        for (int i = 0; i < doraHais.Length; i++) {
            for (int j = 0; j < jyunTehaiLength; j++) {
                if (doraHais[i].getNextHaiId() == jyunTehai[j].getID()) {
                    doraCount++;
                }
            }
        }

        for (int j = 0; j < jyunTehaiLength; j++) {
            if (jyunTehai[j].isRed()) {
                doraCount++;
            }
        }

        for (int i = 0; i < doraHais.Length; i++) {
            if (doraHais[i].getNextHaiId() == m_addHai.getID()) {
                doraCount++;
                break;
            }
        }

        if (m_addHai.isRed()) {
            doraCount++;
        }

        Fuuro[] fuuros = m_tehai.getFuuros();
        int fuuroNum = m_tehai.getFuuroNum();
        int type;

        // common dora.
        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();

            switch (type) {
                case Fuuro.TYPE_MINKOU:
                for (int j = 0; j < doraHais.Length; j++) {
                    if (doraHais[j].getNextHaiId() == fuuros[i].getHais()[0].getID()) {
                        doraCount += 3;
                        break;
                    }
                }
                break;

                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                case Fuuro.TYPE_ANKAN:
                for (int j = 0; j < doraHais.Length; j++) {
                    if (doraHais[j].getNextHaiId() == fuuros[i].getHais()[0].getID()) {
                        doraCount += 4;
                        break;
                    }
                }
                break;

                case Fuuro.TYPE_MINSHUN:
                for (int j = 0; j < doraHais.Length; j++) {
                    for (int k = 0; k < 3; k++) {
                        if (doraHais[j].getNextHaiId() == fuuros[i].getHais()[k].getID()) {
                            doraCount += 1;
                            goto End_MINSHUNLOOP;
                        }
                    }
                }
                End_MINSHUNLOOP: {
                    // go out of double for().
                }
                break;
                default:
                break;
            }
        }

        // red dora.
        for (int i = 0; i < fuuroNum; i++) 
        {
            type = fuuros[i].getType();

            switch (type) {
                case Fuuro.TYPE_MINKOU:
                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                case Fuuro.TYPE_ANKAN:
                for (int j = 0; j < 4; j++) {
                    if (fuuros[i].getHais()[j].isRed()) {
                        doraCount++;
                    }
                }
                break;

                case Fuuro.TYPE_MINSHUN:
                for (int j = 0; j < 3; j++) {
                    if (fuuros[i].getHais()[j].isRed()) {
                        doraCount++;
                    }
                }
                break;
                default:
                break;
            }
        }

        if (doraCount > 0) {
            m_doraCount = doraCount;
            return true;
        }

        return false;
    }

    public struct YakuName {
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

    public static string GetYakuName(int yakuID) {
        if( yakuID <= 0 )
            return "";

        string name = "Yaku-" + yakuID.ToString();

        for( int i = 0; i < Yaku_IdNames.Length; i ++ ) {
            if( Yaku_IdNames[i].ID == yakuID ) {
                name = Yaku_IdNames[i].Key; // should be translated to exact name.
                break;
            }
        }

        return name;
    }

    //===================================== Yaku end =================================================//


    #region YakuHantei SubClass.
    /**
     * 個別の役を判定するクラスです。
     */
    private class YakuHantei {
        protected int YakuID = 0;

        /** 役の成立判定フラグ */
        protected bool hantei = false;
        /** 役満の判定フラグ */
        protected bool yakuman = false;
        /** 役の名前 */
        protected string yakuName;
        /** 役の翻数 */
        protected int hanSuu;

        public YakuHantei() {

        }

        /**
         * 役の成立判定フラグを取得&設定します。
         */
        public bool getYakuHantei() {
            return hantei;
        }
        public void setYakuHantei(bool hantei) {
            this.hantei = hantei;
        }

        /**
         * 役の翻数を取得します。
         */
        public int getHanSuu() {
            return hanSuu;
        }
        public void setHanSuu(int han) {
            this.hanSuu = han;
        }

        /**
         * 役のIDを取得します。
         */
        public int getYakuID() {
            return this.YakuID;
        }
        /**
         * 役の名前を取得します。
         */
        public string getYakuName() {
            yakuName = Yaku.GetYakuName(this.YakuID);
            return yakuName;
        }

        /**
         * 役満の判定フラグを取得します。
         */
        public bool getYakuman() {
            return yakuman;
        }
    }

    private class CheckTanyao : YakuHantei {
        public CheckTanyao(Yaku owner) {
            this.YakuID = 1;
            hantei = owner.checkTanyao();
            //yakuName = a_res.getString(R.string.yaku_tanyao);
            hanSuu = 1;
        }
    }

    private class CheckPinfu : YakuHantei {
        public CheckPinfu(Yaku owner) {
            this.YakuID = 2;
            hantei = owner.checkPinfu();
            //yakuName = a_res.getString(R.string.yaku_pinfu);
            hanSuu = 1;
        }
    }

    private class CheckIpeikou : YakuHantei {
        public CheckIpeikou(Yaku owner) {
            this.YakuID = 3;
            hantei = owner.checkIpeikou() && !owner.checkRyanpeikou();
            //yakuName = a_res.getString(R.string.yaku_ipeikou);
            hanSuu = 1;
        }
    }

    private class CheckReach : YakuHantei {
        public CheckReach(Yaku owner) {
            this.YakuID = 4;
            hantei = owner.checkReach() && !owner.checkDoubleReach();
            //yakuName = a_res.getString(R.string.yaku_reach);
            hanSuu = 1;
        }
    }

    private class CheckIppatu : YakuHantei {
        public CheckIppatu(Yaku owner) {
            this.YakuID = 5;
            hantei = owner.checkIppatu();
            //yakuName = a_res.getString(R.string.yaku_ippatu);
            hanSuu = 1;
        }
    }

    private class CheckTumo : YakuHantei {
        public CheckTumo(Yaku owner) {
            this.YakuID = 6;
            hantei = owner.checkTumo();
            //yakuName = a_res.getString(R.string.yaku_tumo);
            hanSuu = 1;
        }
    }

    private class CheckTon : YakuHantei {
        public CheckTon(Yaku owner) {
            hantei = owner.checkTon();

            if( (owner.getAgariSetting().getJikaze() == JIKAZE_TON)
               && (owner.getAgariSetting().getBakaze() == JIKAZE_TON) ) 
            {
                //yakuName = a_res.getString(R.string.yaku_doubleton);
                this.YakuID = 7;
                hanSuu = 2;
            }
            else {
                this.YakuID = 8;
                //yakuName = a_res.getString(R.string.yaku_ton);
                hanSuu = 1;
            }
        }
    }

    private class CheckNan : YakuHantei {
        public CheckNan(Yaku owner) {
            hantei = owner.checkNan();
            if( (owner.getAgariSetting().getJikaze() == JIKAZE_NAN)
               && (owner.getAgariSetting().getBakaze() == JIKAZE_NAN) ) 
            {
                this.YakuID = 9;
                //yakuName = a_res.getString(R.string.yaku_doublenan);
                hanSuu = 2;
            }
            else {
                this.YakuID = 10;
                //yakuName = a_res.getString(R.string.yaku_nan);
                hanSuu = 1;
            }
        }
    }

    private class CheckSya : YakuHantei {
        public CheckSya(Yaku owner) {
            this.YakuID = 11;
            hantei = owner.checkSya();
            //yakuName = a_res.getString(R.string.yaku_sya);
            hanSuu = 1;
        }
    }

    private class CheckPei : YakuHantei {
        public CheckPei(Yaku owner) {
            this.YakuID = 12;
            hantei = owner.checkPei();
            //yakuName = a_res.getString(R.string.yaku_pei);
            hanSuu = 1;
        }
    }

    private class CheckHaku : YakuHantei {
        public CheckHaku(Yaku owner) {
            this.YakuID = 13;
            hantei = owner.checkHaku();
            //yakuName = a_res.getString(R.string.yaku_haku);
            hanSuu = 1;
        }
    }

    private class CheckHatu : YakuHantei {
        public CheckHatu(Yaku owner) {
            this.YakuID = 14;
            hantei = owner.checkHatu();
            //yakuName = a_res.getString(R.string.yaku_hatu);
            hanSuu = 1;
        }
    }

    private class CheckCyun : YakuHantei {
        public CheckCyun(Yaku owner) {
            this.YakuID = 15;
            hantei = owner.checkCyun();
            //yakuName = a_res.getString(R.string.yaku_cyun);
            hanSuu = 1;
        }
    }

    private class CheckHaitei : YakuHantei {
        public CheckHaitei(Yaku owner) {
            this.YakuID = 16;
            hantei = owner.checkHaitei();
            //yakuName = a_res.getString(R.string.yaku_haitei);
            hanSuu = 1;
        }
    }

    private class CheckHoutei : YakuHantei {
        public CheckHoutei(Yaku owner) {
            this.YakuID = 17;
            hantei = owner.checkHoutei();
            //yakuName = a_res.getString(R.string.yaku_houtei);
            hanSuu = 1;
        }
    }

    private class CheckRinsyan : YakuHantei {
        public CheckRinsyan(Yaku owner) {
            this.YakuID = 18;
            hantei = owner.checkRinsyan();
            //yakuName = a_res.getString(R.string.yaku_rinsyan);
            hanSuu = 1;
        }
    }

    private class CheckCyankan : YakuHantei {
        public CheckCyankan(Yaku owner) {
            this.YakuID = 19;
            hantei = owner.checkCyankan();
            //yakuName = a_res.getString(R.string.yaku_cyankan);
            hanSuu = 1;
        }
    }

    private class CheckDoubleReach : YakuHantei {
        public CheckDoubleReach(Yaku owner) {
            this.YakuID = 20;
            hantei = owner.checkDoubleReach();
            //yakuName = a_res.getString(R.string.yaku_doublereach);
            hanSuu = 2;
        }
    }

    private class CheckTeetoitu : YakuHantei {
        public CheckTeetoitu(Yaku owner) {
            this.YakuID = 21;
            hantei = owner.checkTeetoitu();
            //yakuName = a_res.getString(R.string.yaku_teetoitu);
            hanSuu = 2;
        }
    }

    private class CheckCyanta : YakuHantei {
        public CheckCyanta(Yaku owner) {
            this.YakuID = 22;
            hantei = owner.checkCyanta() && !owner.checkJunCyan() && !owner.checkHonroutou();
            //yakuName = a_res.getString(R.string.yaku_cyanta);
            if( owner.getNakiflg() == true ) {
                hanSuu = 1;
            }
            else {
                hanSuu = 2;
            }
        }
    }

    private class CheckIkkituukan : YakuHantei {
        public CheckIkkituukan(Yaku owner) {
            this.YakuID = 23;
            hantei = owner.checkIkkituukan();
            //yakuName = a_res.getString(R.string.yaku_ikkituukan);
            if( owner.getNakiflg() == true ) {
                hanSuu = 1;
            }
            else {
                hanSuu = 2;
            }
        }
    }

    private class CheckSansyokuDoujun : YakuHantei {
        public CheckSansyokuDoujun(Yaku owner) {
            this.YakuID = 24;
            hantei = owner.checkSansyokuDoujun();
            //yakuName = a_res.getString(R.string.yaku_sansyokudoujyun);
            if( owner.getNakiflg() == true ) {
                hanSuu = 1;
            }
            else {
                hanSuu = 2;
            }
        }
    }

    private class CheckSansyokuDoukou : YakuHantei {
        public CheckSansyokuDoukou(Yaku owner) {
            this.YakuID = 25;
            hantei = owner.checkSansyokuDoukou();
            //yakuName = a_res.getString(R.string.yaku_sansyokudoukou);
            hanSuu = 2;
        }
    }

    private class CheckToitoi : YakuHantei {
        public CheckToitoi(Yaku owner) {
            this.YakuID = 26;
            hantei = owner.checkToitoi();
            //yakuName = a_res.getString(R.string.yaku_toitoi);
            hanSuu = 2;
        }
    }

    private class CheckSanankou : YakuHantei {
        public CheckSanankou(Yaku owner) {
            this.YakuID = 27;
            hantei = owner.checkSanankou();
            //yakuName = a_res.getString(R.string.yaku_sanankou);
            hanSuu = 2;
        }
    }

    private class CheckSankantu : YakuHantei {
        public CheckSankantu(Yaku owner) {
            this.YakuID = 28;
            hantei = owner.checkSankantu();
            //yakuName = a_res.getString(R.string.yaku_sankantu);
            hanSuu = 2;
        }
    }

    private class CheckRyanpeikou : YakuHantei {
        public CheckRyanpeikou(Yaku owner) {
            this.YakuID = 29;
            hantei = owner.checkRyanpeikou();
            //yakuName = a_res.getString(R.string.yaku_ryanpeikou);
            hanSuu = 3;
        }
    }

    private class CheckHonitu : YakuHantei {
        public CheckHonitu(Yaku owner) {
            this.YakuID = 30;
            hantei = owner.checkHonitu() && !owner.checkTinitu();

            //yakuName = a_res.getString(R.string.yaku_honitu);
            if( owner.getNakiflg() == true ) {
                hanSuu = 2;
            }
            else {
                hanSuu = 3;
            }
        }
    }

    private class CheckJunCyan : YakuHantei {
        public CheckJunCyan(Yaku owner) {
            this.YakuID = 31;
            hantei = owner.checkJunCyan();
            //yakuName = a_res.getString(R.string.yaku_juncyan);
            if( owner.getNakiflg() == true ) {
                hanSuu = 2;
            }
            else {
                hanSuu = 3;
            }
        }
    }

    private class CheckSyousangen : YakuHantei {
        public CheckSyousangen(Yaku owner) {
            this.YakuID = 32;
            hantei = owner.checkSyousangen();
            //yakuName = a_res.getString(R.string.yaku_syousangen);
            hanSuu = 2;
        }
    }

    private class CheckHonroutou : YakuHantei {
        public CheckHonroutou(Yaku owner) {
            this.YakuID = 33;
            hantei = owner.checkHonroutou();
            //yakuName = a_res.getString(R.string.yaku_honroutou);
            hanSuu = 2;
        }
    }

    private class CheckHonroutouChiitoitsu : YakuHantei {
        public CheckHonroutouChiitoitsu(Yaku owner) {
            this.YakuID = 33; // the same name to Honroutou.
            hantei = owner.checkHonroutouChiitoitsu();
            //yakuName = a_res.getString(R.string.yaku_honroutou);
            hanSuu = 2;
        }
    }

    private class CheckTinitu : YakuHantei {
        public CheckTinitu(Yaku owner) {
            this.YakuID = 34;
            hantei = owner.checkTinitu();
            //yakuName = a_res.getString(R.string.yaku_tinitu);
            if( owner.getNakiflg() == true ) {
                hanSuu = 5;
            }
            else {
                hanSuu = 6;
            }
        }
    }

    private class CheckSuuankou : YakuHantei {
        public CheckSuuankou(Yaku owner) {
            this.YakuID = 35;
            hantei = owner.checkSuuankou();
            //yakuName = a_res.getString(R.string.yaku_suuankou);
            hanSuu = 13;
            yakuman = true;
        }
    }

    private class CheckSuukantu : YakuHantei {
        public CheckSuukantu(Yaku owner) {
            this.YakuID = 36;
            hantei = owner.checkSuukantu();
            //yakuName = a_res.getString(R.string.yaku_suukantu);
            hanSuu = 13;
            yakuman = true;
        }
    }

    private class CheckDaisangen : YakuHantei {
        public CheckDaisangen(Yaku owner) {
            this.YakuID = 37;
            hantei = owner.checkDaisangen();
            //yakuName = a_res.getString(R.string.yaku_daisangen);
            hanSuu = 13;
            yakuman = true;
        }
    }

    private class CheckSyousuushi : YakuHantei {
        public CheckSyousuushi(Yaku owner) {
            this.YakuID = 38;
            hantei = owner.checkSyousuushi();
            //yakuName = a_res.getString(R.string.yaku_syousuushi);
            hanSuu = 13;
            yakuman = true;
        }
    }

    private class CheckDaisuushi : YakuHantei {
        public CheckDaisuushi(Yaku owner) {
            this.YakuID = 39;
            hantei = owner.checkDaisuushi();
            //yakuName = a_res.getString(R.string.yaku_daisuushi);
            hanSuu = 13;
            yakuman = true;
        }
    }

    private class CheckTuuisou : YakuHantei {
        public CheckTuuisou(Yaku owner) {
            this.YakuID = 40;
            hantei = owner.checkTuuisou();
            //yakuName = a_res.getString(R.string.yaku_tuuisou);
            hanSuu = 13;
            yakuman = true;
        }
    }

    private class CheckChinroutou : YakuHantei {
        public CheckChinroutou(Yaku owner) {
            this.YakuID = 41;
            hantei = owner.checkChinroutou();
            //yakuName = a_res.getString(R.string.yaku_chinroutou);
            hanSuu = 13;
            yakuman = true;
        }
    }

    private class CheckRyuuisou : YakuHantei {
        public CheckRyuuisou(Yaku owner) {
            this.YakuID = 42;
            hantei = owner.checkRyuuisou();
            //yakuName = a_res.getString(R.string.yaku_ryuuisou);
            hanSuu = 13;
            yakuman = true;
        }
    }
    private class CheckCyuurennpoutou : YakuHantei {
        public CheckCyuurennpoutou(Yaku owner) {
            this.YakuID = 43;
            hantei = owner.checkCyuurennpoutou();
            //yakuName = a_res.getString(R.string.yaku_cyuurennpoutou);
            hanSuu = 13;
            yakuman = true;
        }
    }
    private class CheckKokushi : YakuHantei {
        public CheckKokushi(Yaku owner) {
            this.YakuID = 44;
            hantei = owner.checkKokushi();
            //yakuName = a_res.getString(R.string.yaku_kokushi);
            hanSuu = 13;
            yakuman = true;
        }
    }
    private class CheckTenhou : YakuHantei {
        public CheckTenhou(Yaku owner) {
            this.YakuID = 45;
            hantei = owner.checkTenhou();
            //yakuName = a_res.getString(R.string.yaku_tenhou);
            hanSuu = 13;
            yakuman = true;
        }
    }
    private class CheckTihou : YakuHantei {
        public CheckTihou(Yaku owner) {
            this.YakuID = 46;
            hantei = owner.checkTihou();
            //yakuName = a_res.getString(R.string.yaku_tihou);
            hanSuu = 13;
            yakuman = true;
        }
    }
    private class CheckDora : YakuHantei {
        public CheckDora(Yaku owner) {
            this.YakuID = 47;
            hantei = owner.checkDora();
            //yakuName = a_res.getString(R.string.yaku_dora);
            hanSuu = 1;
            yakuman = false;
        }
    }
    #endregion YakuHantei Class.
}
