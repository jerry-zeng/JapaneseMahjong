using System;
using System.Collections.Generic;


public class Man : Player 
{
    public Man(string name) : base(name){
        
    }

    public override bool IsAI
    {
        get{ return false; }
    }

    public override void HandleEvent(EventID evtID, EKaze kazeFrom, EKaze kazeTo, Action<EventID> onAction) 
    {
        this._onAction = onAction;

        switch (evtID) 
        {
            case EventID.PickHai:
            {
                eventPickHai(kazeFrom, kazeTo);
            }
            break;

            case EventID.Select_SuteHai:
            {
                eventSelectSuteHai(kazeFrom, kazeTo);
            }
            break;

            case EventID.Ron_Check:
            {
                eventRonCheck(kazeFrom, kazeTo);
            }
            break;

            case EventID.SuteHai:
            case EventID.Reach:
            {
                eventReach(kazeFrom, kazeTo);
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
        // 手牌をコピーする。
        MahjongAgent.copyTehai(Tehai, MahjongAgent.getJikaze());
        Hai tsumoHai = MahjongAgent.getTsumoHai();

        // check enable Reach
        List<int> haiIndexList = new List<int>();
        if( !MahjongAgent.isReach() && MahjongAgent.getTsumoRemain() >= 4 ) 
        {
            if( MahjongAgent.tryGetReachIndexs(Tehai, tsumoHai, out haiIndexList) )
            {
                _action.IsValidReach = true;
                _action.HaiIndexList = haiIndexList;
                _action.MenuList.Add( EventID.Reach );
            }
        }

        // check enable Tsumo
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai);
        if (agariScore > 0) {
            _action.IsValidTsumo = true;
            _action.IsDisplayingMenu = true;
            _action.MenuList.Add( EventID.Tsumo_Agari );
        }

        // 制限事項。リーチ後のカンをさせない。/
        Hai[] kanHais = new Hai[4];
        int kanCount = 0;
        if( !MahjongAgent.isReach() ) 
        {
            kanCount = Tehai.validKan(tsumoHai, kanHais);
            if (kanCount > 0) {
                _action.setValidKan(true, kanHais, kanCount);
                _action.MenuList.Add( EventID.Ankan );
            }
        }
        else
        {
            if( _action.MenuList.Count == 0) {
                _action.Reset();
                _action.SutehaiIndex = Tehai.getJyunTehai().Length-1;

                return EventID.SuteHai;
            }
        }

        int sutehaiIndex = 0;
        while (true) 
        {
            // 入力を待つ。
            _action.State = EActionState.Select_Sutehai;
            MahjongAgent.PostUiEvent(UIEventID.UI_Input_Player_Action, kazeFrom, kazeTo);
            _action.Waiting();

            if (_action.IsDisplayingMenu) 
            {
                int menuSelect = _action.MenuSelectIndex;

                if( menuSelect >= 0 && menuSelect < _action.MenuList.Count ) 
                {
                    _action.Reset();

                    if( _action.MenuList[menuSelect] == EventID.Reach )
                    {
                        _action.HaiIndexList = haiIndexList;
                        _action.ReachSelectIndex = 0;

                        while (true) 
                        {
                            // 入力を待つ。
                            _action.State = EActionState.Select_Reach;
                            MahjongAgent.PostUiEvent(UIEventID.UI_Input_Player_Action, kazeFrom, kazeTo);
                            _action.Waiting();

                            sutehaiIndex = _action.ReachSelectIndex;
                            if( sutehaiIndex >= 0 && sutehaiIndex < haiIndexList.Count )
                                break;
                        }

                        _action.Reset();
                        _action.SutehaiIndex = haiIndexList[sutehaiIndex];
                    } 
                    else if( _action.MenuList[menuSelect] == EventID.Ankan || 
                            _action.MenuList[menuSelect] == EventID.Kakan) 
                    {
                        if (kanCount > 1) 
                        {
                            while (true) 
                            {
                                _action.Reset();
                                // 入力を待つ。
                                _action.setValidKan(false, kanHais, kanCount);

                                _action.State = EActionState.Select_Kan;
                                MahjongAgent.PostUiEvent(UIEventID.UI_Input_Player_Action, kazeFrom, kazeTo);
                                _action.Waiting();

                                int kanSelect = _action.KanSelectIndex;
                                _action.Reset();
                                _action.SutehaiIndex = kanSelect;

                                return _action.MenuList[menuSelect];
                            }
                        } 
                        else 
                        {
                            _action.SutehaiIndex = 0;
                        }
                    }

                    return _action.MenuList[menuSelect];
                }

                _action.Reset();
            } 
            else 
            {
                sutehaiIndex = _action.SutehaiIndex;
                if( sutehaiIndex >= 0 && sutehaiIndex < Tehai.getJyunTehai().Length )
                    break;
            }

        } // end while(true).

        _action.Reset();
        _action.SutehaiIndex = sutehaiIndex;

        return DoAction(EventID.SuteHai);
    }

    protected EventID eventSelectSuteHai(EKaze kazeFrom, EKaze kazeTo)
    {
        MahjongAgent.copyTehai(Tehai, MahjongAgent.getJikaze());
        int jyunTehaiLength = Tehai.getJyunTehai().Length;

        int sutehaiIdx = 0;
        while (true) 
        {
            // 入力を待つ。
            _action.State = EActionState.Select_Sutehai;
            _action.Waiting();
            sutehaiIdx = _action.SutehaiIndex;

            if (sutehaiIdx >= 0 && sutehaiIdx <= jyunTehaiLength)
                break;
        }

        _action.Reset();
        _action.SutehaiIndex = sutehaiIdx;

        return DoAction(EventID.SuteHai);
    }

    protected EventID eventRonCheck(EKaze kazeFrom, EKaze kazeTo)
    {
        MahjongAgent.copyTehai(Tehai, MahjongAgent.getJikaze());

        List<Hai> machiHais;
        if( MahjongAgent.tryGetMachiHais(Tehai, out machiHais) )
        {
            MahjongAgent.copyHou(Hou, MahjongAgent.getJikaze());

            // chekc hou
            SuteHai[] suteHais = Hou.getSuteHais();

            for (int i = 0; i < suteHais.Length; i++)
            {
                SuteHai suteHaiTemp = suteHais[i];

                for (int j = 0; j < machiHais.Count; j++)
                {
                    if (suteHaiTemp.ID == machiHais[j].ID)
                        return DoAction(EventID.Nagashi);
                }
            }

            // check sutehais
            suteHais = MahjongAgent.getSuteHaiList();
            int playerSuteHaisCount = MahjongAgent.getPlayerSuteHaisCount();

            for(; playerSuteHaisCount < suteHais.Length - 1; playerSuteHaisCount++)
            {
                SuteHai suteHaiTemp = suteHais[playerSuteHaisCount];

                for (int j = 0; j < machiHais.Count; j++) 
                {
                    if (suteHaiTemp.ID == machiHais[j].ID)
                        return DoAction(EventID.Nagashi);
                }
            }

        }

        // didn't fruiten
        Hai suteHai = MahjongAgent.getSuteHai();
        int agariScore = MahjongAgent.getAgariScore(Tehai, suteHai);

        if (agariScore > 0) 
        {
            _action.IsDisplayingMenu = true;
            _action.IsValidRon = true;
            _action.MenuList.Add( EventID.Ron_Agari );
            _action.State = EActionState.Select_Agari;

            MahjongAgent.PostUiEvent(UIEventID.UI_Input_Player_Action, kazeFrom, kazeTo);

            _action.Waiting();

            int menuSelect = _action.MenuSelectIndex;
            if( menuSelect < 1 ) {
                _action.Reset();
                return DoAction(EventID.Ron_Agari);
            }

            _action.Reset();
        }

        return DoAction(EventID.Nagashi);
    }

    protected EventID eventReach(EKaze kazeFrom, EKaze kazeTo)
    {
        if( kazeFrom == MahjongAgent.getJikaze() )
            return DoAction(EventID.Nagashi);

        MahjongAgent.copyTehai(Tehai, MahjongAgent.getJikaze());


        bool furiten = false;

        List<Hai> machiHais;
        if( MahjongAgent.tryGetMachiHais(Tehai, out machiHais) )
        {
            MahjongAgent.copyHou(Hou, MahjongAgent.getJikaze());

            SuteHai[] suteHais = Hou.getSuteHais();

            for (int i = 0; i < suteHais.Length; i++)
            {
                SuteHai suteHaiTemp = suteHais[i];
                for (int j = 0; j < machiHais.Count; j++) 
                {
                    if (suteHaiTemp.ID == machiHais[j].ID)
                    {
                        furiten = true;
                        goto End_REACH_CHECK;
                    }
                }
            }
            End_REACH_CHECK: {
                // go out of double for().
            }

            if( furiten == false ) 
            {
                suteHais = MahjongAgent.getSuteHaiList();

                int playerSuteHaisCount = MahjongAgent.getPlayerSuteHaisCount();
                for(; playerSuteHaisCount < suteHais.Length - 1; playerSuteHaisCount++ )
                {
                    Hai suteHaiTemp = suteHais[playerSuteHaisCount];

                    for (int j = 0; j < machiHais.Count; j++)
                    {
                        if (suteHaiTemp.ID == machiHais[j].ID) {
                            furiten = true;
                            goto End_REACH_CHECK_2;
                        }
                    }
                }
                End_REACH_CHECK_2: {
                    // go out of double for().
                }
            } // end if (!furiten).
        }


        Hai suteHai = MahjongAgent.getSuteHai();
        if( furiten == false ) 
        {
            int agariScore = MahjongAgent.getAgariScore(Tehai, suteHai);
            if (agariScore > 0) {
                _action.IsValidRon = true;
                _action.MenuList.Add(EventID.Ron_Agari);
            }
        }

        Hai[] sarashiHaiLeft = new Hai[2];
        Hai[] sarashiHaiCenter = new Hai[2];
        Hai[] sarashiHaiRight = new Hai[2];

        int chiiCount = 0;
        int chiiIndex = 0;
        int relation = kazeFrom - kazeTo;

        if( !MahjongAgent.isReach() && MahjongAgent.getTsumoRemain() > 0 ) 
        {
            if (Tehai.validPon(suteHai)) {
                _action.IsValidPon = true;
                _action.MenuList.Add(EventID.Pon);
            }

            if( relation == -1 || relation == 3 ) 
            {
                if (Tehai.validChiiRight(suteHai, sarashiHaiRight))
                {
                    _action.setValidChiiRight(true, sarashiHaiRight);
                    if( chiiCount == 0 ){
                        chiiIndex = _action.MenuList.Count;
                        _action.MenuList.Add(EventID.Chii_Right);
                    }
                    chiiCount++;
                }

                if (Tehai.validChiiCenter(suteHai, sarashiHaiCenter))
                {
                    _action.setValidChiiCenter(true, sarashiHaiCenter);
                    if( chiiCount == 0 ){
                        chiiIndex = _action.MenuList.Count;
                        _action.MenuList.Add(EventID.Chii_Center);
                    }
                    chiiCount++;
                }

                if (Tehai.validChiiLeft(suteHai, sarashiHaiLeft))
                {
                    _action.setValidChiiLeft(true, sarashiHaiLeft);
                    if( chiiCount == 0 ){
                        chiiIndex = _action.MenuList.Count;
                        _action.MenuList.Add(EventID.Chii_Left);
                    }
                    chiiCount++;
                }
            }

            if (Tehai.validDaiMinKan(suteHai)) {
                _action.IsValidDaiMinKan = true;
                _action.MenuList.Add(EventID.DaiMinKan);
            }
        }

        if( _action.MenuList.Count > 0 ) 
        {
            _action.MenuSelectIndex = 5;
            MahjongAgent.PostUiEvent(UIEventID.UI_Input_Player_Action, kazeFrom, kazeTo);
            _action.Waiting();

            int menuSelect = _action.MenuSelectIndex;
            if (menuSelect < _action.MenuList.Count) 
            {
                if( _action.MenuList[menuSelect] == EventID.Chii_Left || 
                   _action.MenuList[menuSelect] == EventID.Chii_Center || 
                   _action.MenuList[menuSelect] == EventID.Chii_Right ) 
                {
                    if (chiiCount > 1) 
                    {
                        while (true) 
                        {
                            _action.Reset();
                            // 入力を待つ。
                            _action.ChiiSelectType = (int)_action.MenuList[chiiIndex];
                            _action.State = EActionState.Select_Chii;

                            MahjongAgent.PostUiEvent(UIEventID.UI_Input_Player_Action, kazeFrom, kazeTo);

                            _action.Waiting();
                            EventID chiiEventId = (EventID)_action.ChiiSelectType;
                            _action.Reset();

                            return DoAction(chiiEventId);
                        }
                    }
                }

                _action.Reset();

                return DoAction( _action.MenuList[menuSelect] );
            }

            _action.Reset();
        }

        return DoAction(EventID.SuteHai);
    }

}