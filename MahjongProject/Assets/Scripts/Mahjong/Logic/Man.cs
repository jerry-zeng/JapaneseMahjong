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

        // check enable Tsumo
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai);
        if( agariScore > 0 )
        {
            if( isFuriten() == false )
            {
                _action.IsValidTsumo = true;
                _action.MenuList.Add( EActionType.Agari );

                if( MahjongAgent.isReach(JiKaze) )
                    return DisplayMenuList();
            }
            else{
                Utils.LogWarningFormat( "Player {0} is enable tsumo but furiten...", JiKaze.ToString() );
            }
        }

        // check enable Reach
        if( !MahjongAgent.isReach(JiKaze) && MahjongAgent.getTsumoRemain() >= GameSettings.PlayerCount ) 
        {
            List<int> haiIndexList;
            if( MahjongAgent.tryGetReachIndexs(Tehai, tsumoHai, out haiIndexList) )
            {
                _action.IsValidReach = true;
                _action.ReachHaiIndexList = haiIndexList;
                _action.MenuList.Add( EActionType.Reach );
            }
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

            // can Ron or Ankan, sute hai automatically.
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

        Hai kanHai = haiToHandle;

        int agariScore = MahjongAgent.getAgariScore(Tehai, kanHai);
        if( agariScore > 0 ) 
        {
            if( isFuriten() == false ) 
            {
                _action.IsValidRon = true;
                _action.MenuList.Add( EActionType.Agari );
                _action.MenuList.Add( EActionType.Nagashi );

                return DisplayMenuList();
            }
            else{
                Utils.LogWarningFormat( "Player {0} is enable ron but furiten...", JiKaze.ToString() );
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
        }

        Hai suteHai = haiToHandle;

        // check Ron
        int agariScore = MahjongAgent.getAgariScore(Tehai, suteHai);
        if( agariScore > 0 ) // Ron
        {
            if( isFuriten() == false )
            {
                _action.IsValidRon = true;
                _action.MenuList.Add( EActionType.Agari );

                if( MahjongAgent.isReach(JiKaze) ){
                    //_action.MenuList.Add( EActionType.Nagashi );
                    return DisplayMenuList();
                }
                else{
                    _action.MenuList.Add( EActionType.Nagashi );
                }
            }
            else{
                Utils.LogWarningFormat( "Player {0} is enable ron but furiten...", JiKaze.ToString() );
            }
        }

        if( MahjongAgent.getTsumoRemain() <= 0 )
            return DoResponse(EResponse.Nagashi);

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
            if( Tehai.validChiiRight(suteHai, sarashiHaiRight) )
            {
                _action.setValidChiiRight(true, sarashiHaiRight);

                if( !_action.MenuList.Contains(EActionType.Chii) )
                    _action.MenuList.Add(EActionType.Chii);
            }

            List<Hai> sarashiHaiCenter = new List<Hai>();
            if( Tehai.validChiiCenter(suteHai, sarashiHaiCenter) )
            {
                _action.setValidChiiCenter(true, sarashiHaiCenter);

                if( !_action.MenuList.Contains(EActionType.Chii) )
                    _action.MenuList.Add(EActionType.Chii);
            }

            List<Hai> sarashiHaiLeft = new List<Hai>();
            if( Tehai.validChiiLeft(suteHai, sarashiHaiLeft) )
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