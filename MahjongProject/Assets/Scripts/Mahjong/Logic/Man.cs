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
        _action.State = EActionState.Select_Sutehai;

        if(inTest){
            //_action.State = EActionState.Select_Sutehai;
            //return DisplayMenuList();
        }

        // 手牌をコピーする。
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
        if( !MahjongAgent.isReach(JiKaze) ) 
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
            //return DoResponse(EResponse.Nagashi);
        }

        bool furiten = false;

        List<Hai> machiHais;
        if( MahjongAgent.tryGetMachiHais(Tehai, out machiHais) )
        {
            SuteHai[] suteHais = Hou.getSuteHais();

            for (int i = 0; i < suteHais.Length; i++)
            {
                SuteHai suteHaiTemp = suteHais[i];

                if( machiHais.Exists( h => h.ID == suteHaiTemp.ID ) ){
                    furiten = true;
                    break;
                }
            }

            if( furiten == false ) 
            {
                suteHais = MahjongAgent.getSuteHaiList();

                int playerSuteHaisCount = MahjongAgent.getPlayerSuteHaisCount(JiKaze);
                for(; playerSuteHaisCount < suteHais.Length - 1; playerSuteHaisCount++ )
                {
                    Hai suteHaiTemp = suteHais[playerSuteHaisCount];

                    if( machiHais.Exists( h => h.ID == suteHaiTemp.ID ) ){
                        furiten = true;
                        break;
                    }
                }
            } // end if(furiten == false).
        }

        // didn't fruiten
        if( furiten == false ) 
        {
            int agariScore = MahjongAgent.getAgariScore(Tehai, haiToHandle);
            if (agariScore > 0) 
            {
                _action.IsValidRon = true;
                _action.MenuList.Add( EActionType.Agari );
                _action.MenuList.Add( EActionType.Nagashi );

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
            //return DoResponse(EResponse.Nagashi);
        }

        Hai suteHai = haiToHandle;

        // also check ron.
        if( MahjongAgent.isReach() || MahjongAgent.getTsumoRemain() <= 0 )
        {
            int agariScore = MahjongAgent.getAgariScore(Tehai, suteHai);
            if (agariScore > 0) // Ron
            {
                _action.IsValidRon = true;
                _action.MenuList.Add( EActionType.Agari );
                _action.MenuList.Add( EActionType.Nagashi );

                return DisplayMenuList();
            }
            return DoResponse(EResponse.Nagashi);
        }

        // check menu Kan
        if( Tehai.validDaiMinKan(suteHai) ) {
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

        if( _action.MenuList.Count > 0 ){
            _action.MenuList.Add( EActionType.Nagashi );
            return DisplayMenuList();
        }

        return DoResponse(EResponse.Nagashi);
    }


    protected override EResponse OnSelect_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();
        _action.State = EActionState.Select_Sutehai;

        return DisplayMenuList();
    }


    protected EResponse DisplayMenuList()
    {
        MahjongAgent.PostUiEvent(UIEventType.DisplayMenuList);

        return EResponse.Nagashi;
    }
}