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
            DisplayMenuList();
            return EResponse.SuteHai;
        }

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
                _action.MenuList.Add( EActionType.Reach );
            }
        }

        // check enable Tsumo
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai);
        if (agariScore > 0) {
            _action.IsValidTsumo = true;
            _action.IsDisplayingMenu = true;
            _action.MenuList.Add( EActionType.Agari );
        }

        // 制限事項。リーチ後のカンをさせない。/
        Hai[] kanHais = new Hai[4];
        int kanCount = 0;
        if( !MahjongAgent.isReach() ) 
        {
            kanCount = Tehai.validKan(tsumoHai, kanHais);
            if (kanCount > 0) {
                _action.setValidKan(true, kanHais, kanCount);
                _action.MenuList.Add( EActionType.Kan );
            }
        }
        else
        {
            if( _action.MenuList.Count == 0) {
                _action.Reset();
                _action.SutehaiIndex = Tehai.getJyunTehaiCount()-1;

                return DoResponse(EResponse.SuteHai);
            }
        }

        // display menu ui.
        DisplayMenuList();
        return EResponse.SuteHai;
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
                _action.IsDisplayingMenu = true;
                _action.IsValidRon = true;
                _action.MenuList.Add( EActionType.Agari );
                _action.MenuList.Add(EActionType.Nagashi);

                _action.State = EActionState.Select_Agari;

                DisplayMenuList();
                return EResponse.Nagashi;
            }
        }

        return DoResponse(EResponse.Nagashi);
    }

    protected override EResponse OnHandle_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();

        if(inTest){
            //_action.MenuList.Add(EActionType.Nagashi);
            //DisplayMenuList();
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
        int chiiCount = 0;

        if( relation == (int)ERelation.KaMiCha ) 
        {
            Hai[] sarashiHaiRight = new Hai[2];

            if (Tehai.validChiiRight(suteHai, sarashiHaiRight))
            {
                _action.setValidChiiRight(true, sarashiHaiRight);

                if( chiiCount == 0 )
                    _action.MenuList.Add(EActionType.Chii);
                
                chiiCount++;
            }


            Hai[] sarashiHaiCenter = new Hai[2];

            if (Tehai.validChiiCenter(suteHai, sarashiHaiCenter))
            {
                _action.setValidChiiCenter(true, sarashiHaiCenter);

                if( chiiCount == 0 )
                    _action.MenuList.Add(EActionType.Chii);

                chiiCount++;
            }


            Hai[] sarashiHaiLeft = new Hai[2];

            if (Tehai.validChiiLeft(suteHai, sarashiHaiLeft))
            {
                _action.setValidChiiLeft(true, sarashiHaiLeft);

                if( chiiCount == 0 )
                    _action.MenuList.Add(EActionType.Chii);
                
                chiiCount++;
            }
        }


        if( _action.MenuList.Count > 0 ){
            DisplayMenuList();
            return EResponse.Nagashi;
        }

        return DoResponse(EResponse.Nagashi);
    }


    protected void DisplayMenuList()
    {
        MahjongAgent.PostUiEvent(UIEventType.DisplayMenuList);
    }
}