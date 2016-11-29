public class AgariInfo 
{
    public int m_han;
    public int m_fu;
    public string[] m_yakuNames;
    public AgariScore.Score m_score;
    public int m_agariScore;
}


/**
 * Agari(あがり) = 胡牌.
 */
public class AgariScore 
{

    #region internal classes.
    public class Score {
        public int m_oyaRon;
        public int m_oyaTsumo;
        public int m_koRon;
        public int m_koTsumo;

        public Score(Score a_score) {
            this.m_oyaRon = a_score.m_oyaRon;
            this.m_oyaTsumo = a_score.m_oyaTsumo;
            this.m_koRon = a_score.m_koRon;
            this.m_koTsumo = a_score.m_koTsumo;
        }

        public Score(int a_oyaRon, int a_oyaTsumo, int a_koRon, int a_koTsumo) {
            this.m_oyaRon = a_oyaRon;
            this.m_oyaTsumo = a_oyaTsumo;
            this.m_koRon = a_koRon;
            this.m_koTsumo = a_koTsumo;
        }
    }
    #endregion internal classes.


    // fields.
    private Score[,] SCORE = new Score[13,13]
    {
        {new Score(    0,    0,    0,    0),new Score(    0,    0,    0,    0),new Score( 1500,  500, 1000,  300),new Score( 2000,  700, 1300,  400),new Score( 2400,  800, 1600,  400),new Score( 2900, 1000, 2000,  500),new Score( 3400, 1200, 2300,  600),new Score( 3900, 1300, 2600,  700),new Score( 4400, 1500, 2900,  800),new Score( 4800, 1600, 3200,  800),new Score( 5300,    0, 3600,    0),new Score( 5800,    0, 3900,    0),new Score( 6300,    0, 2100,    0)},
        {new Score( 2000,  700, 1300,  400),new Score( 2400,    0, 1600,    0),new Score( 2900, 1000, 2000,  500),new Score( 3900, 1300, 2600,  700),new Score( 4800, 1600, 3200,  800),new Score( 5800, 2000, 3900, 1000),new Score( 6800, 2300, 4500, 1200),new Score( 7700, 2600, 5200, 1300),new Score( 8700, 2900, 5800, 1500),new Score( 9600, 3200, 6400, 1600),new Score(10600, 3600, 7100, 1800),new Score(11600, 3900, 7700, 2000),new Score(12000, 4000, 8000, 2000)},
        {new Score( 3900, 1300, 2600,  700),new Score( 4800, 1600, 3200,  800),new Score( 5800, 2000, 3900, 1000),new Score( 7700, 2600, 5200, 1300),new Score( 9600, 3200, 6400, 1600),new Score(11600, 3900, 7700, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000)},
        {new Score( 7700, 2600, 5200, 1300),new Score( 9600, 3200, 6400, 1600),new Score(11600, 3900, 7700, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000)},
        {new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000),new Score(12000, 4000, 8000, 2000)},
        {new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000)},
        {new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000),new Score(18000, 6000,12000, 3000)},
        {new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000)},
        {new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000)},
        {new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000),new Score(24000, 8000,16000, 4000)},
        {new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000)},
        {new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000),new Score(36000,12000,24000, 6000)},
        {new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000),new Score(48000,16000,32000, 8000)}
    };

    private Score m_scoreWork;
    private CountFormat countFormat;

    public AgariScore() {
        countFormat = new CountFormat();
    }

    /**
     * 符を計算します。
     */
    public int countHu(Tehai tehai, Hai addHai, Combi combi, Yaku yaku, AgariSetting setting) {
        int countHu = 20;
        Hai[] checkHais;

        //七対子の場合は２５符
        //      if(yaku.checkTeetoitu() == true){
        //          return 25;
        //      }

        //頭の牌を取得
        Hai atamaHai = new Hai(Hai.NumKindToID(combi.m_atamaNumKind));

        // ３元牌なら２符追加
        if (atamaHai.getKind() == Hai.KIND_SANGEN) {
            countHu += 2;
        }

        // 場風なら２符追加
        if( (atamaHai.getID() - Hai.ID_TON) == setting.getBakaze() ) {
            countHu += 2;
        }

        // 自風なら２符追加
        if( (atamaHai.getID() - Hai.ID_TON) == setting.getJikaze() ) {
            countHu += 2;
        }

        //平和が成立する場合は、待ちによる２符追加よりも優先される
        if( yaku.checkPinfu() == false ){
            // 単騎待ちの場合２符追加
            if(addHai.getNumKind() == combi.m_atamaNumKind){
                countHu += 2;
            }

            // 嵌張待ちの場合２符追加
            //数牌の２～８かどうか判定
            if(addHai.isYaochuu() == false){
                for(int i = 0 ; i < combi.m_shunNum ; i++){
                    if((addHai.getNumKind()-1) == combi.m_shunNumKinds[i]){
                        countHu += 2;
                    }
                }
            }

            // 辺張待ち(3)の場合２符追加
            if( (addHai.isYaochuu() == false) && (addHai.getNum() == Hai.NUM_3) ) {
                for(int i = 0 ; i < combi.m_shunNum ; i++){
                    if( (addHai.getNumKind()-2) == combi.m_shunNumKinds[i]){
                        countHu += 2;
                    }
                }
            }

            // 辺張待ち(7)の場合２符追加
            if( (addHai.isYaochuu() == false) && (addHai.getNum() == Hai.NUM_7) ) {
                for(int i = 0 ; i < combi.m_shunNum ; i++){
                    if( addHai.getNumKind() == combi.m_shunNumKinds[i]){
                        countHu += 2;
                    }
                }
            }
        }

        // 暗刻による加点
        for (int i = 0; i < combi.m_kouNum; i++) {
            Hai checkHai = new Hai(Hai.NumKindToID(combi.m_kouNumKinds[i]));
            // 牌が字牌もしくは1,9
            if (checkHai.isYaochuu() == true) {
                countHu += 8;
            } 
            else {
                countHu += 4;
            }
        }

        Fuuro[] fuuros = tehai.getFuuros();
        int fuuroNum = tehai.getFuuroNum();
        int type;

        for (int i = 0; i < fuuroNum; i++) {
            type = fuuros[i].getType();
            switch (type) {
                case Fuuro.TYPE_MINKOU:
                //明刻の牌をチェック
                checkHais = fuuros[i].getHais();
                // 牌が字牌もしくは1,9
                if (checkHais[0].isYaochuu() == true) {
                    countHu += 4;
                } 
                else {
                    countHu += 2;
                }
                break;
                case Fuuro.TYPE_DAIMINKAN:
                case Fuuro.TYPE_KAKAN:
                //明槓の牌をチェック
                checkHais = fuuros[i].getHais();
                // 牌が字牌もしくは1,9
                if (checkHais[0].isYaochuu() == true) {
                    countHu += 16;
                } 
                else {
                    countHu += 8;
                }
                break;
                case Fuuro.TYPE_ANKAN:
                //暗槓の牌をチェック
                checkHais = fuuros[i].getHais();
                // 牌が字牌もしくは1,9
                if (checkHais[0].isYaochuu() == true) {
                    countHu += 32;
                } 
                else {
                    countHu += 16;
                }
                break;
                default:
                break;
            }
        }

        // ツモ上がりで平和が成立していなければ２譜追加
        if(setting.getYakuFlag( (int)YakuflgName.TUMO )== true){
            if(yaku.checkPinfu() == false){
                countHu += 2;
            }
        }

        // 面前ロン上がりの場合は１０符追加
        if( setting.getYakuFlag((int)YakuflgName.TUMO) == false ) {
            if (yaku.GetNakiflg() == false) {
                countHu += 10;
            }
        }

        // 一の位がある場合は、切り上げ
        if (countHu % 10 != 0) {
            countHu = countHu - (countHu % 10) + 10;
        }

        return countHu;
    }

    /**
     * 上がり点数を取得します。
     */
    public int getScore(int hanSuu, int huSuu) {
        if (hanSuu == 0) {
            return 0;
        }

        int iFu;
        if (huSuu == 20) {
            iFu = 0;
        } 
        else if (huSuu == 25) {
            iFu = 1;
        } 
        else if (huSuu > 120) {
            iFu = 12;
        } 
        else {
            iFu = (huSuu / 10) - 1;
        }

        int iHan;
        if (hanSuu > 13) {
            iHan = 13 - 1;
        } 
        else {
            iHan = hanSuu - 1;
        }

        m_scoreWork = new Score(SCORE[iHan,iFu]);

        return m_scoreWork.m_koRon;

        //      int score;
        //      // 符　× ２の　（翻数　+　場ゾロの2翻)乗
        //      score = huSuu * (int) Math.pow(2, hanSuu + 2);
        //      // 子は上の4倍が基本点(親は6倍)
        //      score *= 4;
        //
        //      // 100で割り切れない数がある場合100点繰上げ
        //      if (score % 100 != 0) {
        //          score = score - (score % 100) + 100;
        //      }
        //      // 7700以上は8000とする
        ////        if (score >= 7700) {
        ////            score = 8000;
        ////        }
        //
        //      if (hanSuu >= 13) { // 13翻以上は役満
        //          score = 32000;
        //      } 
        //      else if (hanSuu >= 11) { // 11翻以上は3倍満
        //          score = 24000;
        //      } 
        //      else if (hanSuu >= 8) { // 8翻以上は倍満
        //          score = 16000;
        //      } 
        //      else if (hanSuu >= 6) { // 6翻以上は跳満
        //          score = 12000;
        //      } 
        //      else if (hanSuu >= 5) { // 5翻以上は満貫
        //          score = 8000;
        //      }
        //
        //      return score;
    }

    // 流局满贯 //
    public void setNagashiMangan(AgariInfo a_agariInfo) {
        getScore(5, 30);
        a_agariInfo.m_score = m_scoreWork;
        a_agariInfo.m_fu = 30;
        a_agariInfo.m_han = 5;

        string[] strings = new string[1];
        strings[0] = Yaku.GetYakuName(48);
        a_agariInfo.m_yakuNames = strings;
    }

    public int getAgariScore(Tehai tehai, Hai addHai, Combi[] combis, AgariSetting setting, AgariInfo a_agariInfo) {
        // カウントフォーマットを取得します。
        countFormat.setCountFormat(tehai, addHai);

        // あがりの組み合わせを取得します。
        int combisCount = countFormat.getCombis(combis);
        combis = countFormat.getCombis();
        combisCount = countFormat.getCombiNum();

        if (countFormat.isChiitoitsu()) {
            Yaku yaku = new Yaku(tehai, addHai, combis[0], setting, 0);
            string[] yakuNames = yaku.getYakuName();
            int hanSuu = yaku.getHan();
            int agariScore = getScore(hanSuu, 25);
            a_agariInfo.m_score = m_scoreWork;
            a_agariInfo.m_fu = 25;
            a_agariInfo.m_han = hanSuu;
            a_agariInfo.m_yakuNames = yakuNames;

            return agariScore;
        }

        if (countFormat.isKokushi()) {
            Yaku yaku = new Yaku(tehai, addHai, setting);
            if( yaku.Kokushi ) {
                int hanSuu = 13;
                int agariScore = getScore(hanSuu, 20);
                a_agariInfo.m_score = m_scoreWork;
                a_agariInfo.m_fu = 0;
                a_agariInfo.m_han = hanSuu;
                a_agariInfo.m_yakuNames = yaku.getYakuName();

                return agariScore;
            }

            return 0;
        }

        // あがりの組み合わせがない場合は0点
        if (combisCount == 0) {
            return 0;
        }

        // 役
        int[] hanSuuArr = new int[combisCount];
        // 符
        int[] huSuu = new int[combisCount];
        // 点数（子のロン上がり）
        int[] agariScoreArr = new int[combisCount];
        // 最大の点数
        int maxagariScore = 0;

        for (int i = 0; i < combisCount; i++) {
            Yaku yaku = new Yaku(tehai, addHai, combis[i], setting);
            hanSuuArr[i] = yaku.counttHanSuu();
            huSuu[i] = countHu(tehai, addHai, combis[i],yaku,setting);
            agariScoreArr[i] = getScore(hanSuuArr[i], huSuu[i]);

            if( maxagariScore < agariScoreArr[i] ) {
                a_agariInfo.m_score = m_scoreWork;
                maxagariScore = agariScoreArr[i];
                a_agariInfo.m_fu = huSuu[i];
                a_agariInfo.m_han = hanSuuArr[i];
                a_agariInfo.m_yakuNames = yaku.getYakuName();
            }
        }

        // 最大値を探す
        maxagariScore = agariScoreArr[0];
        for (int i = 0; i < combisCount; i++) {
            maxagariScore = (maxagariScore >= agariScoreArr[i]? maxagariScore : agariScoreArr[i]);
        }

        return maxagariScore;
    }

    public string[] getYakuName(Tehai tehai, Hai addHai, Combi[] combis, AgariSetting setting) {
        //和了り役の名前
        string[] yakuNames = {""};
        // カウントフォーマットを取得します。
        countFormat.setCountFormat(tehai, addHai);

        // あがりの組み合わせを取得します。
        int combisCount = countFormat.getCombis(combis);

        // あがりの組み合わせがない場合は0点
        if (combisCount == 0){
            return yakuNames;
        }

        // 役
        int[] hanSuu = new int[combisCount];
        // 符
        int[] huSuu = new int[combisCount];
        // 点数（子のロン上がり）
        int[] agariScore = new int[combisCount];
        // 最大の点数
        int maxagariScore = 0;

        for (int i = 0; i < combisCount; i++) {
            Yaku yaku = new Yaku(tehai, addHai, combis[i], setting);
            hanSuu[i] = yaku.counttHanSuu();
            huSuu[i] = countHu(tehai, addHai, combis[i],yaku,setting);
            agariScore[i] = getScore(hanSuu[i], huSuu[i]);

            if(maxagariScore < agariScore[i]){
                maxagariScore = agariScore[i];
                yakuNames = yaku.getYakuName();
            }
        }

        return yakuNames;
    }
}
