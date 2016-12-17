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
        MahjongAgent.copyTehai(Tehai);
        Hai tsumoHai = haiToHandle;

        // ツモあがりの場合は、イベント(ツモあがり)を返す。
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai);
        if(agariScore > 0)
            return DoResponse(EResponse.Tsumo_Agari);

        // リーチの場合は、ツモ切りする。
        if(MahjongAgent.isReach()) {
            _action.SutehaiIndex = PlayerAction.Sutehai_Index_Max;
            return DoResponse(EResponse.SuteHai);
        }

        thinkSutehai(tsumoHai);

        // 捨牌を決めたので手牌を更新します。
        if( _action.SutehaiIndex != PlayerAction.Sutehai_Index_Max ) {
            Tehai.removeJyunTehai(_action.SutehaiIndex);
            Tehai.addJyunTehai(tsumoHai);
        }

        // リーチする場合はイベント（リーチ）を返します。
        if( thinkReach(Tehai) )
            return DoResponse(EResponse.Reach);

        return DoResponse(EResponse.SuteHai);
    }

    protected override EResponse OnHandle_KakanHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        if( fromPlayerKaze == MahjongAgent.getJikaze() )
            return DoResponse(EResponse.Nagashi);

        MahjongAgent.copyTehai(Tehai);
        MahjongAgent.copyHou(Hou, MahjongAgent.getJikaze());

        if(isFuriten() == false)
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



    protected void thinkSutehai(Hai addHai)
    {
        _action.SutehaiIndex = PlayerAction.Sutehai_Index_Max;
        FormatWorker.setCounterFormat(Tehai, null);
        int maxScore = getCountFormatScore(FormatWorker);

        Hai[] jyunTehai = Tehai.getJyunTehai();

        Hai[] jyunTehaiCopy = new Hai[jyunTehai.Length];
        Tehai.copyJyunTehai(jyunTehaiCopy, jyunTehai);

        for (int i = 0; i < jyunTehai.Length; i++)
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
    }

    protected bool thinkReach(Tehai tehai)
    {
        if (MahjongAgent.getTsumoRemain() >= GameSettings.PlayerCount) 
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


    // 振听.
    protected bool isFuriten()
    {
        List<Hai> machiHais;
        if( MahjongAgent.tryGetMachiHais(Tehai, out machiHais) )
        {
            // check hou
            SuteHai[] suteHais = Hou.getSuteHais();

            for (int i = 0; i < suteHais.Length; i++)
            {
                SuteHai suteHaiTemp = suteHais[i];

                for (int j = 0; j < machiHais.Count; j++)
                {
                    if(suteHaiTemp.ID == machiHais[j].ID)
                        return true;
                }
            }

            // check sutehai
            suteHais = MahjongAgent.getSuteHaiList();

            int playerSuteHaisCount = MahjongAgent.getPlayerSuteHaisCount();
            for(; playerSuteHaisCount < suteHais.Length - 1; playerSuteHaisCount++)
            {
                SuteHai suteHaiTemp = suteHais[playerSuteHaisCount];

                for (int j = 0; j < machiHais.Count; j++)
                {
                    if(suteHaiTemp.ID == machiHais[j].ID)
                        return true;
                }
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
