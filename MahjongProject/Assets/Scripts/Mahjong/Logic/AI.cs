
/// <summary>
/// AIを実装するクラスです
/// </summary>

public class AI : IPlayer 
{
    // Infoのコンストラクタ
    private Info m_info;

    // AIの名前
    private string m_name;

    // 捨牌のインデックス
    private int m_sutehaiIndex;

    // 手牌
    private Tehai m_tehai = new Tehai();

    // 河
    private Hou m_hou = new Hou();

    // 捨牌
    private Hai m_suteHai = new Hai();


    public AI(Info a_info, string a_name)
    {
        this.m_info = a_info;
        this.m_name = a_name;
    }

    public string getName() {
        return m_name;
    }
    public bool isAI() {
        return true;
    }
    public int getSutehaiIndex() {
        return m_sutehaiIndex;
    }


    public EventID HandleEvent(EventID evtID, EKaze kazeFrom, EKaze kazeTo) 
    {
        EventID result = EventID.Nagashi;

        switch(evtID) 
        {
            case EventID.PickHai:
                result = eventTsumo(kazeFrom, kazeTo);
            break;

            case EventID.Pon:
            case EventID.Chii_Center:
            case EventID.Chii_Left:
            case EventID.Chii_Right:
            case EventID.DaiMinKan:
            case EventID.SuteHai:
            case EventID.Ron_Check:
            case EventID.Reach:
                result = eventSutehai(kazeFrom, kazeTo);
            break;

            case EventID.Select_SuteHai:
            {
                m_info.copyTehai(m_tehai);
                thinkSutehai(null);
            }
            break;
        }

        return result;
    }

    private EventID eventTsumo(EKaze kazeFrom, EKaze kazeTo)
    {
        m_info.copyTehai(m_tehai);
        Hai tsumoHai = m_info.getTsumoHai();

        // ツモあがりの場合は、イベント(ツモあがり)を返す。
        int agariScore = m_info.getAgariScore(m_tehai, tsumoHai);
        if (agariScore > 0) 
            return EventID.Tsumo_Agari;

        // リーチの場合は、ツモ切りする。
        if (m_info.isReach()) {
            m_sutehaiIndex = 13;
            return EventID.SuteHai;
        }

        thinkSutehai(tsumoHai);

        // 捨牌を決めたので手牌を更新します。
        if (m_sutehaiIndex != 13) {
            m_tehai.removeJyunTehai(m_sutehaiIndex);
            m_tehai.addJyunTehai(tsumoHai);
        }

        // リーチする場合はイベント（リーチ）を返します。
        if (thinkReach(m_tehai))
            return EventID.Reach;

        return EventID.SuteHai;
    }

    private EventID eventSutehai(EKaze kazeFrom, EKaze kazeTo)
    {
        if (kazeFrom == m_info.getJikaze())
            return EventID.Nagashi;

        m_info.copyTehai(m_tehai);
        m_suteHai = m_info.getSuteHai();
        m_info.copyHou(m_hou, m_info.getJikaze());

        if(isFuriten() == false)
        {
            int agariScore = m_info.getAgariScore(m_tehai, m_suteHai);
            if (agariScore > 0)
                return EventID.Ron_Agari;
        }

        return EventID.Nagashi;
    }

    // 振听.
    private bool isFuriten()
    {
        bool furiten = false;
        Hai[] hais = new Hai[Hai.ID_ITEM_MAX];
        int indexNum = m_info.getMachiIndexs(m_tehai, hais);

        if (indexNum > 0) 
        {
            SuteHai suteHaiTemp = new SuteHai();
            SuteHai[] suteHais = m_hou.getSuteHais();
            int suteHaisLength = m_hou.getSuteHaisLength();

            for (int i = 0; i < suteHaisLength; i++)
            {
                suteHaiTemp = suteHais[i];
                for (int j = 0; j < indexNum; j++)
                {
                    if (suteHaiTemp.ID == hais[j].ID){
                        furiten = true;
                        goto END_FURITENLOOP;
                    }
                }
            }
            END_FURITENLOOP: {
                // go out of double for().
            }

            if (!furiten) 
            {
                suteHais = m_info.getSuteHais();

                int suteHaisCount = 0;
                int playerSuteHaisCount = 0;

                suteHaisCount = m_info.getSuteHaisCount();
                playerSuteHaisCount = m_info.getPlayerSuteHaisCount();

                for (; playerSuteHaisCount < suteHaisCount - 1; playerSuteHaisCount++)
                {
                    suteHaiTemp = suteHais[playerSuteHaisCount];
                    for (int j = 0; j < indexNum; j++)
                    {
                        if (suteHaiTemp.ID == hais[j].ID){
                            furiten = true;
                            goto End_FURITENLOOP_2;
                        }
                    }
                }
                End_FURITENLOOP_2: {
                    // go out of double for().
                }
            }
        }

        return furiten;
    }



    private CountFormat countFormat = new CountFormat();

    private readonly static int HYOUKA_SHUU = 1;

    private HaiCombi[] combis = new HaiCombi[10]
    {
        new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi(),
        new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi()
    };


    private void thinkSutehai(Hai addHai)
    {
        int score = 0;
        int maxScore = 0;

        m_sutehaiIndex = 13;
        countFormat.setCounterFormat(m_tehai, null);
        maxScore = getCountFormatScore(countFormat);

        Hai hai = new Hai();

        Hai[] jyunTehai = new Hai[Tehai.JYUN_TEHAI_LENGTH_MAX];
        for( int i = 0; i < Tehai.JYUN_TEHAI_LENGTH_MAX; i++ ) {
            jyunTehai[i] = new Hai();
        }

        int jyunTehaiLength = m_tehai.getJyunTehaiLength();
        Tehai.copyJyunTehai(jyunTehai, m_tehai.getJyunTehai(), jyunTehaiLength);

        for (int i = 0; i < jyunTehaiLength; i++)
        {
            m_tehai.copyJyunTehaiIndex(hai, i);
            m_tehai.removeJyunTehai(i);
            countFormat.setCounterFormat(m_tehai, addHai);
            score = getCountFormatScore(countFormat);

            if (score > maxScore) {
                maxScore = score;
                m_sutehaiIndex = i;
            }

            m_tehai.addJyunTehai(hai);
        }
    }


    private readonly static Hai[] haiTable = new Hai[] 
    {
        new Hai(Hai.ID_WAN_1), new Hai(Hai.ID_WAN_2),
        new Hai(Hai.ID_WAN_3), new Hai(Hai.ID_WAN_4),
        new Hai(Hai.ID_WAN_5), new Hai(Hai.ID_WAN_6),
        new Hai(Hai.ID_WAN_7), new Hai(Hai.ID_WAN_8),
        new Hai(Hai.ID_WAN_9),
        new Hai(Hai.ID_PIN_1), new Hai(Hai.ID_PIN_2),
        new Hai(Hai.ID_PIN_3), new Hai(Hai.ID_PIN_4),
        new Hai(Hai.ID_PIN_5), new Hai(Hai.ID_PIN_6),
        new Hai(Hai.ID_PIN_7), new Hai(Hai.ID_PIN_8),
        new Hai(Hai.ID_PIN_9),
        new Hai(Hai.ID_SOU_1), new Hai(Hai.ID_SOU_2),
        new Hai(Hai.ID_SOU_3), new Hai(Hai.ID_SOU_4),
        new Hai(Hai.ID_SOU_5), new Hai(Hai.ID_SOU_6),
        new Hai(Hai.ID_SOU_7), new Hai(Hai.ID_SOU_8),
        new Hai(Hai.ID_SOU_9),
        new Hai(Hai.ID_TON),  new Hai(Hai.ID_NAN),
        new Hai(Hai.ID_SYA),  new Hai(Hai.ID_PE),
        new Hai(Hai.ID_HAKU), new Hai(Hai.ID_HATSU),
        new Hai(Hai.ID_CHUN) 
    };

    private bool thinkReach(Tehai tehai)
    {
        if (m_info.getTsumoRemain() >= 4) 
        {
            for(int i = 0; i < haiTable.Length; i++) 
            {
                Hai hai = haiTable[i];
                countFormat.setCounterFormat(tehai, hai);

                if (countFormat.calculateCombisCount(combis) > 0)
                    return true;
            }
        }
        return false;
    }

    private int getCountFormatScore(CountFormat countFormat)
    {
        int score = 0;
        HaiCounterInfo[] countArr;

        for (int i = 0; i < countFormat.getCounterArrLength(); i++) 
        {
            countArr = countFormat.getCounterArray();

            if ((countArr[i].numKind & Hai.KIND_SHUU) != 0)
                score += countArr[i].count * HYOUKA_SHUU;

            if (countArr[i].count == 2)
                score += 4;

            if (countArr[i].count >= 3)
                score += 8;

            if ((countArr[i].numKind & Hai.KIND_SHUU) > 0)
            {
                if ((countArr[i].numKind + 1) == countArr[i + 1].numKind) {
                    score += 4;
                }

                if ((countArr[i].numKind + 2) == countArr[i + 2].numKind) {
                    score += 4;
                }
            }
        }

        return score;
    }
}
