using System;
using System.Collections.Generic;


public class AI : Player 
{
    public AI(string name) : base(name){

    }

    public override bool IsAI
    {
        get{ return true; }
    }


    protected override EResponse OnHandle_TsumoHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        if(inTest){
            _action.Reset();
            _action.SutehaiIndex = Utils.GetRandomNum(0, Tehai.getJyunTehaiCount());
            //_action.SutehaiIndex = Tehai.getJyunTehaiCount()-1;
            return DoResponse(EResponse.SuteHai);
        }

        Hai tsumoHai = haiToHandle;

        // ツモあがりの場合は、イベント(ツモあがり)を返す。
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai);
        if( agariScore > 0 )
            return DoResponse(EResponse.Tsumo_Agari);

        // リーチの場合は、ツモ切りする。
        if( MahjongAgent.isReach(JiKaze) ){
            _action.SutehaiIndex = PlayerAction.Sutehai_Index_Max;
            return DoResponse(EResponse.SuteHai);
        }

        thinkSutehai(tsumoHai);

        // 捨牌を決めたので手牌を更新します。
        if( _action.SutehaiIndex != PlayerAction.Sutehai_Index_Max )
        {
            Tehai.removeJyunTehaiAt(_action.SutehaiIndex);
            Tehai.addJyunTehai(tsumoHai);
        }

        // リーチする場合はイベント（リーチ）を返します。
        if( thinkReach(Tehai) )
            return DoResponse(EResponse.Reach);

        return DoResponse(EResponse.SuteHai);
    }

    protected override EResponse OnHandle_KakanHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        if(inTest){
            return DoResponse(EResponse.Nagashi);
        }

        if( isFuriten() == false )
        {
            int agariScore = MahjongAgent.getAgariScore(Tehai, haiToHandle);
            if(agariScore > 0)
                return DoResponse(EResponse.Ron_Agari);
        }

        return DoResponse(EResponse.Nagashi);
    }

    protected override EResponse OnHandle_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        return DoResponse(EResponse.Nagashi);
    }


    protected override EResponse OnSelect_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();
        _action.SutehaiIndex = Utils.GetRandomNum(0, Tehai.getJyunTehaiCount());
        return DoResponse(EResponse.SuteHai);
    }


    protected void thinkSutehai(Hai addHai)
    {
        _action.SutehaiIndex = PlayerAction.Sutehai_Index_Max;
        FormatWorker.setCounterFormat(Tehai, null);
        int maxScore = getCountFormatScore(FormatWorker);

        Hai[] jyunTehai = Tehai.getJyunTehai();

        Hai[] jyunTehaiCopy = new Hai[jyunTehai.Length];
        Tehai.copyJyunTehai(jyunTehaiCopy, jyunTehai);

        for( int i = 0; i < jyunTehai.Length; i++ )
        {
            Hai hai = Tehai.removeJyunTehaiAt(i);

            FormatWorker.setCounterFormat(Tehai, addHai);
            int score = getCountFormatScore(FormatWorker);

            if( score > maxScore ){
                maxScore = score;
                _action.SutehaiIndex = i;
            }

            Tehai.insertJyunTehai(i, hai);
        }
    }

    protected bool thinkReach(Tehai tehai)
    {
        if( !MahjongAgent.isReach(JiKaze) && 
           MahjongAgent.getTsumoRemain() >= GameSettings.PlayerCount && 
           Tenbou >= GameSettings.Reach_Cost ) 
        {
            for(int i = 0; i < MahjongMain.HaiTable.Length; i++) 
            {
                FormatWorker.setCounterFormat(tehai, MahjongMain.HaiTable[i]);

                if(FormatWorker.calculateCombisCount( null ) > 0)
                    return true;
            }
        }
        return false;
    }


    protected readonly static int HYOUKA_SHUU = 1;

    protected int getCountFormatScore(CountFormat countFormat)
    {
        int score = 0;
        HaiCounterInfo[] countArr = countFormat.getCounterArray();

        for (int i = 0; i < countArr.Length; i++) 
        {
            if((countArr[i].numKind & Hai.KIND_SHUU) != 0)
                score += countArr[i].count * HYOUKA_SHUU;

            if(countArr[i].count == 2)
                score += 4;

            if(countArr[i].count >= 3)
                score += 8;

            if((countArr[i].numKind & Hai.KIND_SHUU) > 0)
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
