
public class AgariInfo 
{
    public int m_han;
    public int m_fu;
    public string[] m_yakuNames;
    public AgariScore.Score m_score;
    public int m_agariScore;
}

/// <summary>
/// Agari(あがり) = 胡牌.
/// </summary>

public class AgariScore 
{
    #region internal classes.
    public class Score
    {
        // おや:亲，
        public int _oyaRon;
        public int _oyaTsumo;
        // こ:子
        public int _koRon;
        public int _koTsumo;

        public Score(Score score) {
            this._oyaRon = score._oyaRon;
            this._oyaTsumo = score._oyaTsumo;
            this._koRon = score._koRon;
            this._koTsumo = score._koTsumo;
        }

        public Score(int oyaRon, int oyaTsumo, int koRon, int koTsumo) {
            this._oyaRon = oyaRon;
            this._oyaTsumo = oyaTsumo;
            this._koRon = koRon;
            this._koTsumo = koTsumo;
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

    private Score _scoreWork;
    private CountFormat _countFormat;

    public AgariScore() {
        _countFormat = new CountFormat();
    }


    // 符を計算します
    public int countHu(Tehai tehai, Hai addHai, Combi combi, Yaku yaku, AgariSetting setting)
    {
        int countHu = 20;
        Hai[] checkHais;

        //七対子の場合は２５符
        //if(yaku.checkTeetoitu() == true){
        //    return 25;
        //}

        //頭の牌を取得
        Hai atamaHai = new Hai(Hai.NumKindToID(combi._atamaNumKind));

        // ３元牌なら２符追加
        if (atamaHai.getKind() == Hai.KIND_SANGEN)
            countHu += 2;

        // 場風なら２符追加
        if( (atamaHai.ID - Hai.ID_TON) == (int)setting.getBakaze() )
            countHu += 2;

        // 自風なら２符追加
        if( (atamaHai.ID - Hai.ID_TON) == (int)setting.getJikaze() )
            countHu += 2;

        //平和が成立する場合は、待ちによる２符追加よりも優先される
        if( yaku.checkPinfu() == false )
        {
            // 単騎待ちの場合２符追加
            if(addHai.getNumKind() == combi._atamaNumKind)
                countHu += 2;

            // 嵌張待ちの場合２符追加
            // 数牌の２～８かどうか判定
            if(addHai.isYaochuu() == false)
            {
                for(int i = 0 ; i < combi._shunNum ; i++)
                {
                    if((addHai.getNumKind()-1) == combi._shunNumKinds[i])
                        countHu += 2;
                }
            }

            // 辺張待ち(3)の場合２符追加
            if( (addHai.isYaochuu() == false) && (addHai.getNum() == Hai.NUM_3) )
            {
                for(int i = 0 ; i < combi._shunNum ; i++)
                {
                    if( (addHai.getNumKind()-2) == combi._shunNumKinds[i])
                        countHu += 2;
                }
            }

            // 辺張待ち(7)の場合２符追加
            if( (addHai.isYaochuu() == false) && (addHai.getNum() == Hai.NUM_7) )
            {
                for(int i = 0 ; i < combi._shunNum ; i++)
                {
                    if( addHai.getNumKind() == combi._shunNumKinds[i])
                        countHu += 2;
                }
            }
        }

        // 暗刻による加点
        for (int i = 0; i < combi._kouNum; i++)
        {
            Hai checkHai = new Hai(Hai.NumKindToID(combi._kouNumKinds[i]));

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

        for (int i = 0; i < fuuroNum; i++) 
        {
            switch( fuuros[i].Type ) 
            {
                case EFuuroType.MinKou:
                {
                    //明刻の牌をチェック
                    checkHais = fuuros[i].Hais;

                    // 牌が字牌もしくは1,9
                    if (checkHais[0].isYaochuu() == true) {
                        countHu += 4;
                    } 
                    else {
                        countHu += 2;
                    }
                }
                break;

                case EFuuroType.KaKan:
                case EFuuroType.DaiMinKan:
                {
                    //明槓の牌をチェック
                    checkHais = fuuros[i].Hais;

                    // 牌が字牌もしくは1,9
                    if (checkHais[0].isYaochuu() == true) {
                        countHu += 16;
                    } 
                    else {
                        countHu += 8;
                    }
                }
                break;

                case EFuuroType.AnKan:
                {
                    //暗槓の牌をチェック
                    checkHais = fuuros[i].Hais;

                    // 牌が字牌もしくは1,9
                    if (checkHais[0].isYaochuu() == true) {
                        countHu += 32;
                    } 
                    else {
                        countHu += 16;
                    }
                }
                break;
            }
        }

        // ツモ上がりで平和が成立していなければ２譜追加
        if(setting.getYakuFlag( (int)EYakuFlagType.TUMO )== true)
        {
            if(yaku.checkPinfu() == false){
                countHu += 2;
            }
        }

        // 面前ロン上がりの場合は１０符追加
        if( setting.getYakuFlag((int)EYakuFlagType.TUMO) == false )
        {
            if (yaku.isNaki == false) {
                countHu += 10;
            }
        }

        // 一の位がある場合は、切り上げ
        if (countHu % 10 != 0) {
            countHu = countHu - (countHu % 10) + 10;
        }

        return countHu;
    }

    // 上がり点数を取得します
    public int getScore(int hanSuu, int huSuu)
    {
        if (hanSuu == 0)
            return 0;

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

        _scoreWork = new Score( SCORE[iHan,iFu] );

        return _scoreWork._koRon;
    }

    // 流局满贯 //
    public void setNagashiMangan(AgariInfo agariInfo)
    {
        getScore(5, 30);

        agariInfo.m_score = _scoreWork;
        agariInfo.m_fu = 30;
        agariInfo.m_han = 5;

        string[] strings = new string[1];
        strings[0] = Yaku.GetYakuName(48);
        agariInfo.m_yakuNames = strings;
    }

    public int getAgariScore(Tehai tehai, Hai addHai, Combi[] combis, AgariSetting setting, AgariInfo agariInfo)
    {
        // カウントフォーマットを取得します。
        _countFormat.setCountFormat(tehai, addHai);

        // あがりの組み合わせを取得します。
        int combisCount = _countFormat.getCombis(combis);
        combis = _countFormat.getCombis();
        combisCount = _countFormat.getCombiNum();

        if (_countFormat.isChiitoitsu())
        {
            Yaku yaku = new Yaku(tehai, addHai, combis[0], setting, 0);

            string[] yakuNames = yaku.getYakuName();
            int hanSuu = yaku.getHan();
            int agariScore = getScore(hanSuu, 25);

            agariInfo.m_score = _scoreWork;
            agariInfo.m_fu = 25;
            agariInfo.m_han = hanSuu;
            agariInfo.m_yakuNames = yakuNames;

            return agariScore;
        }

        if (_countFormat.isKokushi())
        {
            Yaku yaku = new Yaku(tehai, addHai, setting);

            if( yaku.isKokushi )
            {
                int hanSuu = 13;
                int agariScore = getScore(hanSuu, 20);

                agariInfo.m_score = _scoreWork;
                agariInfo.m_fu = 0;
                agariInfo.m_han = hanSuu;
                agariInfo.m_yakuNames = yaku.getYakuName();

                return agariScore;
            }

            return 0;
        }

        // あがりの組み合わせがない場合は0点
        if (combisCount == 0)
            return 0;

        int[] hanSuuArr = new int[combisCount]; // 役
        int[] huSuu = new int[combisCount]; // 符
        int[] agariScoreArr = new int[combisCount]; // 点数（子のロン上がり）
        int maxAgariScore = 0; // 最大の点数

        for (int i = 0; i < combisCount; i++)
        {
            Yaku yaku = new Yaku(tehai, addHai, combis[i], setting);
            hanSuuArr[i] = yaku.counttHanSuu();
            huSuu[i] = countHu(tehai, addHai, combis[i],yaku,setting);
            agariScoreArr[i] = getScore(hanSuuArr[i], huSuu[i]);

            if( maxAgariScore < agariScoreArr[i] )
            {
                agariInfo.m_score = _scoreWork;
                maxAgariScore = agariScoreArr[i];
                agariInfo.m_fu = huSuu[i];
                agariInfo.m_han = hanSuuArr[i];
                agariInfo.m_yakuNames = yaku.getYakuName();
            }
        }

        // 最大値を探す
        maxAgariScore = agariScoreArr[0];

        for (int i = 0; i < combisCount; i++) 
        {
            maxAgariScore = (maxAgariScore >= agariScoreArr[i])? maxAgariScore : agariScoreArr[i];
        }

        return maxAgariScore;
    }

    public string[] getYakuName(Tehai tehai, Hai addHai, Combi[] combis, AgariSetting setting)
    {
        //和了り役の名前
        string[] yakuNames = {""};

        // カウントフォーマットを取得します。
        _countFormat.setCountFormat(tehai, addHai);

        // あがりの組み合わせを取得します。
        int combisCount = _countFormat.getCombis(combis);

        // あがりの組み合わせがない場合は0点
        if (combisCount == 0)
            return yakuNames;

        int[] hanSuu = new int[combisCount]; // 役
        int[] huSuu = new int[combisCount]; // 符
        int[] agariScore = new int[combisCount]; // 点数（子のロン上がり）
        int maxAgariScore = 0; // 最大の点数

        for (int i = 0; i < combisCount; i++)
        {
            Yaku yaku = new Yaku(tehai, addHai, combis[i], setting);
            hanSuu[i] = yaku.counttHanSuu();
            huSuu[i] = countHu(tehai, addHai, combis[i],yaku,setting);
            agariScore[i] = getScore(hanSuu[i], huSuu[i]);

            if(maxAgariScore < agariScore[i])
            {
                maxAgariScore = agariScore[i];
                yakuNames = yaku.getYakuName();
            }
        }

        return yakuNames;
    }
}
