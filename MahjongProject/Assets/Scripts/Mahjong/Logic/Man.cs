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


    protected override EResponse OnHandle_TsumoHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        // 手牌をコピーする。
        MahjongAgent.copyTehai(Tehai, MahjongAgent.getJikaze());
        Hai tsumoHai = haiToHandle;

        // check enable Reach
        List<int> haiIndexList = new List<int>();
        if( !MahjongAgent.isReach() && MahjongAgent.getTsumoRemain() >= 4 ) 
        {
            if( MahjongAgent.tryGetReachIndexs(Tehai, tsumoHai, out haiIndexList) )
            {
                _action.IsValidReach = true;
                _action.HaiIndexList = haiIndexList;
                _action.MenuList.Add( EResponse.Reach );
            }
        }

        // check enable Tsumo
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai);
        if (agariScore > 0) {
            _action.IsValidTsumo = true;
            _action.IsDisplayingMenu = true;
            _action.MenuList.Add( EResponse.Tsumo_Agari );
        }

        // 制限事項。リーチ後のカンをさせない。/
        Hai[] kanHais = new Hai[4];
        int kanCount = 0;
        if( !MahjongAgent.isReach() ) 
        {
            kanCount = Tehai.validKan(tsumoHai, kanHais);
            if (kanCount > 0) {
                _action.setValidKan(true, kanHais, kanCount);
                _action.MenuList.Add( EResponse.Ankan );
            }
        }
        else
        {
            if( _action.MenuList.Count == 0) {
                _action.Reset();
                _action.SutehaiIndex = Tehai.getJyunTehai().Length-1;

                return DoResponse(EResponse.SuteHai);
            }
        }

        int sutehaiIndex = 0;
        while (true) 
        {
            // 入力を待つ。
            _action.State = EActionState.Select_Sutehai;
            MahjongAgent.PostUiEvent(UIEventType.UI_Input_Player_Action);

            _action.Waiting();

            if (_action.IsDisplayingMenu) 
            {
                int menuSelect = _action.MenuSelectIndex;

                if( menuSelect >= 0 && menuSelect < _action.MenuList.Count ) 
                {
                    _action.Reset();

                    if( _action.MenuList[menuSelect] == EResponse.Reach )
                    {
                        _action.HaiIndexList = haiIndexList;
                        _action.ReachSelectIndex = 0;

                        while (true) 
                        {
                            // 入力を待つ。
                            _action.State = EActionState.Select_Reach;
                            MahjongAgent.PostUiEvent(UIEventType.UI_Input_Player_Action);
                            _action.Waiting();

                            sutehaiIndex = _action.ReachSelectIndex;
                            if( sutehaiIndex >= 0 && sutehaiIndex < haiIndexList.Count )
                                break;
                        }

                        _action.Reset();
                        _action.SutehaiIndex = haiIndexList[sutehaiIndex];
                    } 
                    else if( _action.MenuList[menuSelect] == EResponse.Ankan || 
                            _action.MenuList[menuSelect] == EResponse.Kakan) 
                    {
                        if (kanCount > 1) 
                        {
                            while (true) 
                            {
                                _action.Reset();
                                // 入力を待つ。
                                _action.setValidKan(false, kanHais, kanCount);

                                _action.State = EActionState.Select_Kan;
                                MahjongAgent.PostUiEvent(UIEventType.UI_Input_Player_Action);
                                _action.Waiting();

                                int kanSelect = _action.KanSelectIndex;
                                _action.Reset();
                                _action.SutehaiIndex = kanSelect;

                                return DoResponse( _action.MenuList[menuSelect] );
                            }
                        } 
                        else 
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

                            return DoResponse(EResponse.SuteHai);
                        }
                    }

                    return DoResponse( _action.MenuList[menuSelect] );
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

        return DoResponse(EResponse.SuteHai);
    }

    protected override EResponse OnHandle_KakanHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
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

        // didn't fruiten
        if( furiten == false ) 
        {
            int agariScore = MahjongAgent.getAgariScore(Tehai, haiToHandle);
            if (agariScore > 0) 
            {
                _action.IsDisplayingMenu = true;
                _action.IsValidRon = true;
                _action.MenuList.Add( EResponse.Ron_Agari );
                _action.State = EActionState.Select_Agari;

                while(true)
                {
                    MahjongAgent.PostUiEvent(UIEventType.UI_Input_Player_Action);
                    _action.Waiting();

                    int menuSelect = _action.MenuSelectIndex;
                    if( menuSelect < 1 ) {
                        _action.Reset();
                        return DoResponse(EResponse.Ron_Agari);
                    }
                }
            }
        }

        return DoResponse(EResponse.Nagashi);
    }

    protected override EResponse OnHandle_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        // also need check ron.
        if( MahjongAgent.isReach() || MahjongAgent.getTsumoRemain() <= 0 )
            return DoResponse(EResponse.Nagashi);

        // 手牌をコピーする。
        MahjongAgent.copyTehai(Tehai, MahjongAgent.getJikaze());
        Hai suteHai = haiToHandle;

        // check menu Kan
        if ( Tehai.validDaiMinKan(suteHai) ) {
            _action.IsValidDaiMinKan = true;
            _action.MenuList.Add( EResponse.DaiMinKan );
        }

        // check menu Pon
        if( Tehai.validPon(suteHai) ){
            _action.IsValidPon = true;
            _action.MenuList.Add( EResponse.Pon );
        }

        // check menu Chii
        int relation = Mahjong.getRelation(fromPlayerKaze, JiKaze);
        int chiiCount = 0;
        int chiiIndex = 0;

        if( relation == (int)ERelation.KaMiCha ) 
        {
            Hai[] sarashiHaiRight = new Hai[2];

            if (Tehai.validChiiRight(suteHai, sarashiHaiRight))
            {
                _action.setValidChiiRight(true, sarashiHaiRight);
                if( chiiCount == 0 ){
                    chiiIndex = _action.MenuList.Count;
                    _action.MenuList.Add(EResponse.Chii_Right);
                }
                chiiCount++;
            }


            Hai[] sarashiHaiCenter = new Hai[2];

            if (Tehai.validChiiCenter(suteHai, sarashiHaiCenter))
            {
                _action.setValidChiiCenter(true, sarashiHaiCenter);
                if( chiiCount == 0 ){
                    chiiIndex = _action.MenuList.Count;
                    _action.MenuList.Add(EResponse.Chii_Center);
                }
                chiiCount++;
            }


            Hai[] sarashiHaiLeft = new Hai[2];

            if (Tehai.validChiiLeft(suteHai, sarashiHaiLeft))
            {
                _action.setValidChiiLeft(true, sarashiHaiLeft);
                if( chiiCount == 0 ){
                    chiiIndex = _action.MenuList.Count;
                    _action.MenuList.Add(EResponse.Chii_Left);
                }
                chiiCount++;
            }
        }


        if( _action.MenuList.Count > 0 )
        {
            _action.IsDisplayingMenu = true;
            while(true)
            {
                if( _action.MenuList.Count > 0 ) 
                {
                    _action.MenuSelectIndex = 5;
                    MahjongAgent.PostUiEvent(UIEventType.UI_Input_Player_Action);
                    _action.Waiting();

                    int menuSelect = _action.MenuSelectIndex;
                    if (menuSelect < _action.MenuList.Count) 
                    {
                        if(_action.MenuList[menuSelect] == EResponse.Chii_Left || 
                            _action.MenuList[menuSelect] == EResponse.Chii_Center || 
                            _action.MenuList[menuSelect] == EResponse.Chii_Right ) 
                        {
                            if (chiiCount > 1) 
                            {
                                while (true) 
                                {
                                    _action.Reset();
                                    // 入力を待つ。
                                    _action.ChiiSelectType = (int)_action.MenuList[chiiIndex];
                                    _action.State = EActionState.Select_Chii;

                                    MahjongAgent.PostUiEvent(UIEventType.UI_Input_Player_Action);

                                    _action.Waiting();
                                    EResponse chiiEventId = (EResponse)_action.ChiiSelectType;
                                    _action.Reset();

                                    return DoResponse(chiiEventId);
                                }
                            }
                        }

                        _action.Reset();

                        return DoResponse( _action.MenuList[menuSelect] );
                    }

                    _action.Reset();
                }
            }
        }

        return DoResponse(EResponse.Nagashi);
    }

}