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
        _action.Reset();

        if(inTest){
            _action.State = EActionState.Select_Sutehai;
            return DisplayMenuList();
        }

        // 手牌をコピーする。
        MahjongAgent.copyTehai(Tehai, MahjongAgent.getJikaze());
        Hai tsumoHai = haiToHandle;

        // check enable Reach
        if( !MahjongAgent.isReach() && MahjongAgent.getTsumoRemain() >= GameSettings.PlayerCount ) 
        {
            List<int> haiIndexList;
            if( MahjongAgent.tryGetReachIndexs(Tehai, tsumoHai, out haiIndexList) )
            {
                _action.IsValidReach = true;
                _action.ReachHaiIndexList = haiIndexList;
                _action.MenuList.Add( EActionType.Reach );
            }
        }

        // check enable Tsumo
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai);
        if (agariScore > 0) {
            _action.IsValidTsumo = true;
            _action.MenuList.Add( EActionType.Agari );
        }

        // 制限事項。リーチ後のカンをさせない
        if( !MahjongAgent.isReach() ) 
        {
            // tsumo kans
            List<Hai> kanHais = new List<Hai>();
            if( Tehai.validAnyTsumoKan(tsumoHai, kanHais) )
            {
                _action.setValidTsumoKan(true, kanHais);

                _action.MenuList.Add( EActionType.Kan );
            }
        }
        else
        {
            // TODO: enable AnKan after reach if the ankan MENTSU is not related to any MENTSU
            // such as: 23444 4 is disable, while 22444 4 is enable.

            //if( Tehai.validAnKan(tsumoHai) )

            if( _action.MenuList.Count == 0) {
                _action.Reset();
                _action.SutehaiIndex = Tehai.getJyunTehaiCount()-1;

                return DoResponse(EResponse.SuteHai);
            }
        }

        // always display menu on pick tsumo hai.
        return DisplayMenuList();
    }

    protected override EResponse OnHandle_KakanHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();

        if(inTest){
            return DoResponse(EResponse.Nagashi);
        }

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
                _action.IsValidRon = true;
                _action.MenuList.Add( EActionType.Agari );
                _action.State = EActionState.Select_Agari;

                return DisplayMenuList();
            }
        }

        return DoResponse(EResponse.Nagashi);
    }

    protected override EResponse OnHandle_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();

        if(inTest){
            //_action.MenuList.Add(EActionType.Nagashi);
            //return DisplayMenuList();
            return DoResponse(EResponse.Nagashi);
        }

        // also need check ron.
        if( MahjongAgent.isReach() )
            return DoResponse(EResponse.Nagashi);

        if( MahjongAgent.getTsumoRemain() <= 0 )
            return DoResponse(EResponse.Nagashi);

        // 手牌をコピーする。
        MahjongAgent.copyTehai(Tehai, MahjongAgent.getJikaze());
        Hai suteHai = haiToHandle;

        // check menu Kan
        if ( Tehai.validDaiMinKan(suteHai) ) {
            _action.IsValidDaiMinKan = true;
            _action.MenuList.Add( EActionType.Kan );
        }

        // check menu Pon
        if( Tehai.validPon(suteHai) ){
            _action.IsValidPon = true;
            _action.MenuList.Add( EActionType.Pon );
        }

        // check menu Chii
        int relation = Mahjong.getRelation(fromPlayerKaze, JiKaze);
        if( relation == (int)ERelation.KaMiCha ) 
        {
            List<Hai> sarashiHaiRight = new List<Hai>();
            if (Tehai.validChiiRight(suteHai, sarashiHaiRight))
            {
                _action.setValidChiiRight(true, sarashiHaiRight);

                if( !_action.MenuList.Contains(EActionType.Chii) )
                    _action.MenuList.Add(EActionType.Chii);
            }


            List<Hai> sarashiHaiCenter = new List<Hai>();
            if (Tehai.validChiiCenter(suteHai, sarashiHaiCenter))
            {
                _action.setValidChiiCenter(true, sarashiHaiCenter);

                if( !_action.MenuList.Contains(EActionType.Chii) )
                    _action.MenuList.Add(EActionType.Chii);
            }


            List<Hai> sarashiHaiLeft = new List<Hai>();
            if (Tehai.validChiiLeft(suteHai, sarashiHaiLeft))
            {
                _action.setValidChiiLeft(true, sarashiHaiLeft);

                if( !_action.MenuList.Contains(EActionType.Chii) )
                    _action.MenuList.Add(EActionType.Chii);
            }
        }


        if( _action.MenuList.Count > 0 )
            return DisplayMenuList();

        return DoResponse(EResponse.Nagashi);
    }


    protected EResponse DisplayMenuList()
    {
        MahjongAgent.PostUiEvent(UIEventType.DisplayMenuList);

        return EResponse.Nagashi;
    }
}