using System;


public class AI : IPlayer 
{
    protected Player _owner;
    protected PlayerAction _action = new PlayerAction();
    protected Action<EventID> _onAction;

    // 手牌
    protected Tehai Tehai
    {
        get{ return _owner.Tehai; }
    }

    // 河
    protected Hou Hou
    {
        get{ return _owner.Hou; }
    }

    // Infoのコンストラクタ
    protected Info MahjongInfo
    {
        get{ return Mahjong.current.getInfo(); }
    }


    public bool IsAI
    {
        get{ return true; }
    }

    public void AttachToPlayer( Player owner )
    {
        this._owner = owner;
    }

    public PlayerAction getAction()
    {
        return _action;
    }

    public EventID DoAction(EventID result)
    {
        if(_onAction != null) _onAction.Invoke(result);
        return result;
    }


    public void HandleEvent(EventID evtID, EKaze kazeFrom, EKaze kazeTo, Action<EventID> onAction) 
    {
        this._onAction = onAction;

        switch(evtID) 
        {
            case EventID.PickHai:
            {
                eventPickHai(kazeFrom, kazeTo);
            }
            break;

            case EventID.SuteHai:
            case EventID.Pon:
            case EventID.Chii_Center:
            case EventID.Chii_Left:
            case EventID.Chii_Right:
            case EventID.DaiMinKan:            
            case EventID.Ron_Check:
            case EventID.Reach:
            {
                eventSutehai(kazeFrom, kazeTo);
            }               
            break;

            case EventID.Select_SuteHai:
            {
                MahjongInfo.copyTehai(Tehai);
                thinkSutehai(null);
            }
            break;

            default:
            {
                DoAction(EventID.Nagashi);
            }
            break;
        }
    }


    protected EventID eventPickHai(EKaze kazeFrom, EKaze kazeTo)
    {
        MahjongInfo.copyTehai(Tehai);
        Hai tsumoHai = MahjongInfo.getTsumoHai();

        // ツモあがりの場合は、イベント(ツモあがり)を返す。
        int agariScore = MahjongInfo.getAgariScore(Tehai, tsumoHai);
        if (agariScore > 0)
            return DoAction(EventID.Tsumo_Agari);

        // リーチの場合は、ツモ切りする。
        if (MahjongInfo.isReach()) {
            _action.SutehaiIndex = 13;
            return DoAction(EventID.SuteHai);
        }

        thinkSutehai(tsumoHai);

        // 捨牌を決めたので手牌を更新します。
        if (_action.SutehaiIndex != 13) {
            Tehai.removeJyunTehai(_action.SutehaiIndex);
            Tehai.addJyunTehai(tsumoHai);
        }

        // リーチする場合はイベント（リーチ）を返します。
        if (thinkReach(Tehai))
            return DoAction(EventID.Reach);

        return DoAction(EventID.SuteHai);
    }

    protected EventID eventSutehai(EKaze kazeFrom, EKaze kazeTo)
    {
        if (kazeFrom == MahjongInfo.getJikaze())
            return DoAction(EventID.Nagashi);

        MahjongInfo.copyTehai(Tehai);
        MahjongInfo.copyHou(Hou, MahjongInfo.getJikaze());

        if(isFuriten() == false)
        {
            Hai m_suteHai = MahjongInfo.getSuteHai();
            int agariScore = MahjongInfo.getAgariScore(Tehai, m_suteHai);

            if (agariScore > 0)
                return DoAction(EventID.Ron_Agari);
        }

        return DoAction(EventID.Nagashi);
    }

    // 振听.
    protected bool isFuriten()
    {
        bool furiten = false;

        Hai[] hais = new Hai[Hai.ID_MAX+1];
        int indexNum = MahjongInfo.getMachiIndexs(Tehai, hais);

        if (indexNum > 0) 
        {
            SuteHai[] suteHais = Hou.getSuteHais();

            for (int i = 0; i < suteHais.Length; i++)
            {
                SuteHai suteHaiTemp = suteHais[i];
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

            if( furiten == false ) 
            {
                suteHais = MahjongInfo.getSuteHaiList();
                int playerSuteHaisCount = MahjongInfo.getPlayerSuteHaisCount();

                for(; playerSuteHaisCount < suteHais.Length - 1; playerSuteHaisCount++)
                {
                    SuteHai suteHaiTemp = suteHais[playerSuteHaisCount];

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


    protected readonly static int HYOUKA_SHUU = 1;

    protected CountFormat FormatWorker
    {
        get{ return _owner.FormatWorker; }
    }

    #region table
    protected readonly static Hai[] haiTable = new Hai[] 
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
    #endregion

    protected EventID thinkSutehai(Hai addHai)
    {
        _action.SutehaiIndex = 13;
        FormatWorker.setCounterFormat(Tehai, null);
        int maxScore = getCountFormatScore(FormatWorker);

        Hai[] jyunTehaiCopy = new Hai[Tehai.JYUN_TEHAI_LENGTH_MAX];
        Tehai.copyJyunTehai(jyunTehaiCopy, Tehai.getJyunTehai());

        int jyunTehaiLength = Tehai.getJyunTehai().Length;

        for (int i = 0; i < jyunTehaiLength; i++)
        {
            Hai hai = new Hai();
            Tehai.copyJyunTehaiIndex(hai, i);
            Tehai.removeJyunTehai(i);

            FormatWorker.setCounterFormat(Tehai, addHai);
            int score = getCountFormatScore(FormatWorker);

            if( score > maxScore ){
                maxScore = score;
                _action.SutehaiIndex = i;
            }

            Tehai.addJyunTehai(hai);
        }

        return EventID.Nagashi;
    }

    protected bool thinkReach(Tehai tehai)
    {
        if (MahjongInfo.getTsumoRemain() >= GameSettings.PlayerCount) 
        {
            for(int i = 0; i < haiTable.Length; i++) 
            {
                FormatWorker.setCounterFormat(tehai, haiTable[i]);

                if(FormatWorker.calculateCombisCount( null ) > 0)
                    return true;
            }
        }
        return false;
    }

    protected int getCountFormatScore(CountFormat countFormat)
    {
        int score = 0;
        HaiCounterInfo[] countArr = countFormat.getCounterArray();

        for (int i = 0; i < countArr.Length; i++) 
        {
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
