
/// <summary>
/// Man.与AI相对
/// </summary>

public class Man : IPlayer 
{
    private string _name;

    // 捨牌のインデックス
    private int m_sutehaiIndex = 0;

    // プレイヤーに提供する情報
    private Info m_info;
    private PlayerAction m_playerAction;


    public Man(Info info, string name, PlayerAction playerAction)
    {
        this.m_info = info;
        this._name = name;
        this.m_playerAction = playerAction;

        m_sutehaiIndex = 0;
    }

    public string getName() {
        return _name;
    }
    public bool isAI() {
        return false;
    }
    public int getSutehaiIndex() {
        return m_sutehaiIndex;
    }

    // 手牌
    private Tehai m_tehai = new Tehai();
    private Hou m_hou = new Hou();

    public EventId HandleEvent(EventId evtID, EKaze kazeFrom, EKaze kazeTo) 
    {
        int sutehaiIdx = 0;
        int agariScore = 0;
        Hai tsumoHai = null;
        Hai suteHai = null;
        int indexNum = 0;
        int[] indexs = new int[14];
        int menuNum = 0;
        EventId[] eventId = new EventId[4];
        int jyunTehaiLength;

        Hai[] kanHais = new Hai[4];
        int kanNum = 0;
        Hai[] sarashiHaiLeft = new Hai[2];
        Hai[] sarashiHaiCenter = new Hai[2];
        Hai[] sarashiHaiRight = new Hai[2];
        //bool isChii = false;
        int chiiCount = 0;
        int iChii = 0;
        int relation = kazeFrom - kazeTo;
        bool furiten = false;
        Hai[] a_hais = new Hai[Hai.ID_ITEM_MAX];

        switch (evtID) 
        {
            case EventId.TSUMO:
            {
                // 手牌をコピーする。
                m_info.copyTehai(m_tehai, m_info.getJikaze());
                // ツモ牌を取得する。
                tsumoHai = m_info.getTsumoHai();

                if (!m_info.isReach() && (m_info.getTsumoRemain() >= 4)) {
                    indexNum = m_info.getReachIndexs(m_tehai, tsumoHai, indexs);
                    if (indexNum > 0) {
                        m_playerAction.setValidReach(true);
                        m_playerAction.m_indexs = indexs;
                        m_playerAction.m_indexNum = indexNum;
                        eventId[menuNum] = EventId.REACH;
                        menuNum++;
                    }
                }

                agariScore = m_info.getAgariScore(m_tehai, tsumoHai);
                if (agariScore > 0) {
                    m_playerAction.setValidTsumo(true);
                    m_playerAction.setDispMenu(true);
                    eventId[menuNum] = EventId.TSUMO_AGARI;
                    menuNum++;
                }

                // 制限事項。リーチ後のカンをさせない。/
                if (!m_info.isReach()) {
                    kanNum = m_tehai.validKan(tsumoHai, kanHais);
                    if (kanNum > 0) {
                        m_playerAction.setValidKan(true, kanHais, kanNum);
                        eventId[menuNum] = EventId.ANKAN;
                        menuNum++;
                    }
                }

                if (m_info.isReach()) {
                    if (menuNum == 0) {
                        m_playerAction.Init();
                        this.m_sutehaiIndex = 13;

                        return EventId.SUTEHAI;
                    }
                }

                m_playerAction.setMenuNum(menuNum);
                while (true) 
                {
                    // 入力を待つ。
                    m_playerAction.setState( EActionState.Sutehai_Select );
                    m_info.postUiEvent(EventId.UI_INPUT_PLAYER_ACTION, kazeFrom, kazeTo);
                    m_playerAction.actionWait();

                    if (m_playerAction.isDispMenu()) 
                    {
                        int menuSelect = m_playerAction.getMenuSelect();

                        if ((menuSelect >= 0) && (menuSelect < menuNum)) 
                        {
                            m_playerAction.Init();

                            if (eventId[menuSelect] == EventId.REACH) {
                                m_playerAction.m_indexs = indexs;
                                m_playerAction.m_indexNum = indexNum;
                                m_playerAction.setReachSelect(0);

                                while (true) 
                                {
                                    // 入力を待つ。
                                    m_playerAction.setState( EActionState.Reach_Select );
                                    m_info.postUiEvent(EventId.UI_INPUT_PLAYER_ACTION, kazeFrom, kazeTo);
                                    m_playerAction.actionWait();
                                    sutehaiIdx = m_playerAction.getReachSelect();

                                    if (sutehaiIdx != int.MaxValue) {
                                        if (sutehaiIdx >= 0 && sutehaiIdx < indexNum) {
                                            break;
                                        }
                                    }
                                }

                                m_playerAction.Init();
                                this.m_sutehaiIndex = indexs[sutehaiIdx];
                            } 
                            else if ((eventId[menuSelect] == EventId.ANKAN) ||
                                 (eventId[menuSelect] == EventId.KAN)) 
                            {
                                if (kanNum > 1) 
                                {
                                    while (true) {
                                        m_playerAction.Init();
                                        // 入力を待つ。
                                        m_playerAction.setValidKan(false, kanHais, kanNum);
                                        //m_playerAction.setChiiEventId(eventId[iChii]);
                                        m_playerAction.setState( EActionState.Kan_Select );
                                        m_info.postUiEvent(EventId.UI_INPUT_PLAYER_ACTION, kazeFrom, kazeTo);
                                        m_playerAction.actionWait();
                                        int kanSelect = m_playerAction.getKanSelect();
                                        m_playerAction.Init();
                                        this.m_sutehaiIndex = kanSelect;
                                        return eventId[menuSelect];
                                    }
                                } 
                                else {
                                    this.m_sutehaiIndex = 0;
                                }
                            }

                            return eventId[menuSelect];
                        }

                        m_playerAction.Init();
                    } 
                    else 
                    {
                        sutehaiIdx = m_playerAction.getSutehaiIndex();
                        if (sutehaiIdx != int.MaxValue) {
                            if (sutehaiIdx >= 0 && sutehaiIdx <= 13) {
                                break;
                            }
                        }
                    }

                } // end while(true).

                m_playerAction.Init();
                this.m_sutehaiIndex = sutehaiIdx;
            }
            return EventId.SUTEHAI;

            case EventId.SELECT_SUTEHAI:
            {
                m_info.copyTehai(m_tehai, m_info.getJikaze());
                jyunTehaiLength = m_tehai.getJyunTehaiLength();

                while (true) 
                {
                    // 入力を待つ。
                    m_playerAction.setState( EActionState.Sutehai_Select );
                    m_playerAction.actionWait();
                    sutehaiIdx = m_playerAction.getSutehaiIndex();

                    if (sutehaiIdx != int.MaxValue) {
                        if (sutehaiIdx >= 0 && sutehaiIdx <= jyunTehaiLength) {
                            break;
                        }
                    }
                }

                m_playerAction.Init();
                this.m_sutehaiIndex = sutehaiIdx;
            }
            return EventId.SUTEHAI;

            case EventId.RON_CHECK:
            {
                m_info.copyTehai(m_tehai, m_info.getJikaze());
                suteHai = m_info.getSuteHai();

                indexNum = m_info.getMachiIndexs(m_tehai, a_hais);
                if (indexNum > 0) 
                {
                    m_info.copyHou(m_hou, m_info.getJikaze());
                    SuteHai suteHaiTemp = new SuteHai();
                    SuteHai[] suteHais = m_hou.getSuteHais();
                    int kawaLength = m_hou.getSuteHaisLength();

                    for (int i = 0; i < kawaLength; i++) {
                        suteHaiTemp = suteHais[i];
                        for (int j = 0; j < indexNum; j++) {
                            if (suteHaiTemp.ID == a_hais[j].ID) {
                                furiten = true;
                                goto End_RON_CHECK;
                            }
                        }
                    }
                    End_RON_CHECK: {
                        // go out of double for().
                    }

                    if (!furiten) 
                    {
                        suteHais = m_info.getSuteHais();

                        int suteHaisCount = 0;
                        int playerSuteHaisCount = 0;

                        suteHaisCount = m_info.getSuteHaisCount();
                        playerSuteHaisCount = m_info.getPlayerSuteHaisCount();

                        for (; playerSuteHaisCount < suteHaisCount - 1; playerSuteHaisCount++) {
                            suteHaiTemp = suteHais[playerSuteHaisCount];
                            for (int j = 0; j < indexNum; j++) {
                                if (suteHaiTemp.ID == a_hais[j].ID) {
                                    furiten = true;
                                    goto End_RON_CHECK_2;
                                }
                            }
                        }
                        End_RON_CHECK_2: {
                            // go out of double for().
                        }
                    }
                }

                if (!furiten) 
                {
                    agariScore = m_info.getAgariScore(m_tehai, suteHai);

                    if (agariScore > 0) 
                    {
                        m_playerAction.setDispMenu(true);
                        m_playerAction.setValidRon(true);
                        m_playerAction.setMenuNum(1);
                        m_playerAction.setMenuSelect(5);
                        m_playerAction.setState( EActionState.Ron_Select );

                        m_info.postUiEvent(EventId.UI_INPUT_PLAYER_ACTION, kazeFrom, kazeTo);

                        m_playerAction.actionWait();

                        int menuSelect = m_playerAction.getMenuSelect();
                        if (menuSelect < 1) {
                            m_playerAction.Init();
                            return EventId.RON_AGARI;
                        }

                        m_playerAction.Init();
                    }
                }
            }
            break;

            case EventId.SUTEHAI:
            case EventId.REACH:
            {
                if (kazeFrom == m_info.getJikaze()) {
                    return EventId.NAGASHI;
                }

                m_info.copyTehai(m_tehai, m_info.getJikaze());
                suteHai = m_info.getSuteHai();

                indexNum = m_info.getMachiIndexs(m_tehai, a_hais);
                if (indexNum > 0) 
                {
                    m_info.copyHou(m_hou, m_info.getJikaze());
                    SuteHai suteHaiTemp = new SuteHai();
                    SuteHai[] suteHais = m_hou.getSuteHais();

                    int houLength = m_hou.getSuteHaisLength();

                    for (int i = 0; i < houLength; i++) {
                        suteHaiTemp = suteHais[i];
                        for (int j = 0; j < indexNum; j++) {
                            if (suteHaiTemp.ID == a_hais[j].ID) {
                                furiten = true;
                                goto End_REACH_CHECK;
                            }
                        }
                    }
                    End_REACH_CHECK: {
                        // go out of double for().
                    }

                    if (!furiten) 
                    {
                        suteHais = m_info.getSuteHais();

                        int suteHaisCount = 0;
                        int playerSuteHaisCount = 0;

                        suteHaisCount = m_info.getSuteHaisCount();            
                        playerSuteHaisCount = m_info.getPlayerSuteHaisCount();

                        for( ; playerSuteHaisCount < suteHaisCount - 1; playerSuteHaisCount++ ) {
                            suteHaiTemp = suteHais[playerSuteHaisCount];

                            for (int j = 0; j < indexNum; j++) {
                                if (suteHaiTemp.ID == a_hais[j].ID) {
                                    furiten = true;
                                    goto End_REACH_CHECK_2;
                                }
                            }
                        }
                        End_REACH_CHECK_2: {
                            // go out of double for().
                        }

                    } // end if (!furiten).
                } // end if (indexNum > 0).

                if (!furiten) 
                {
                    agariScore = m_info.getAgariScore(m_tehai, suteHai);
                    if (agariScore > 0) {
                        m_playerAction.setValidRon(true);
                        eventId[menuNum] = EventId.RON_AGARI;
                        menuNum++;
                    }
                }

                if (!m_info.isReach() && (m_info.getTsumoRemain() > 0)) 
                {
                    if (m_tehai.validPon(suteHai)) {
                        m_playerAction.setValidPon(true);
                        eventId[menuNum] = EventId.PON;
                        menuNum++;
                    }

                    if ((relation == -1) || (relation == 3)) 
                    {
                        if (m_tehai.validChiiRight(suteHai, sarashiHaiRight)) {
                            m_playerAction.setValidChiiRight(true, sarashiHaiRight);
                            if (chiiCount == 0) {
                                iChii = menuNum;
                                eventId[menuNum] = EventId.CHII_RIGHT;
                                menuNum++;
                            }
                            chiiCount++;
                        }

                        if (m_tehai.validChiiCenter(suteHai, sarashiHaiCenter)) {
                            m_playerAction.setValidChiiCenter(true, sarashiHaiCenter);
                            if (chiiCount == 0) {
                                iChii = menuNum;
                                eventId[menuNum] = EventId.CHII_CENTER;
                                menuNum++;
                            }
                            chiiCount++;
                        }

                        if (m_tehai.validChiiLeft(suteHai, sarashiHaiLeft)) {
                            m_playerAction.setValidChiiLeft(true, sarashiHaiLeft);
                            if (chiiCount == 0) {
                                iChii = menuNum;
                                eventId[menuNum] = EventId.CHII_LEFT;
                                menuNum++;
                            }
                            chiiCount++;
                        }
                    }

                    if (m_tehai.validDaiMinKan(suteHai)) {
                        m_playerAction.setValidDaiMinKan(true);
                        eventId[menuNum] = EventId.DAIMINKAN;
                        menuNum++;
                    }
                }

                if (menuNum > 0) 
                {
                    m_playerAction.setMenuNum(menuNum);
                    m_playerAction.setMenuSelect(5);
                    m_info.postUiEvent(EventId.UI_INPUT_PLAYER_ACTION, kazeFrom, kazeTo);
                    m_playerAction.actionWait();

                    int menuSelect = m_playerAction.getMenuSelect();
                    if (menuSelect < menuNum) 
                    {
                        if ((eventId[menuSelect] == EventId.CHII_LEFT) ||
                        (eventId[menuSelect] == EventId.CHII_CENTER) ||
                        (eventId[menuSelect] == EventId.CHII_RIGHT)) 
                        {
                            if (chiiCount > 1) 
                            {
                                while (true) 
                                {
                                    m_playerAction.Init();
                                    // 入力を待つ。
                                    m_playerAction.setChiiEventId(eventId[iChii]);
                                    m_playerAction.setState( EActionState.Chii_Select );

                                    m_info.postUiEvent(EventId.UI_INPUT_PLAYER_ACTION, kazeFrom, kazeTo);

                                    m_playerAction.actionWait();
                                    EventId chiiEventId = m_playerAction.getChiiEventId();
                                    m_playerAction.Init();

                                    return chiiEventId;
                                }
                            }
                        }

                        m_playerAction.Init();

                        return eventId[menuSelect];
                    }

                    m_playerAction.Init();
                }
            }
            break;
        }

        return EventId.NAGASHI;
    }

}