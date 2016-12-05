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


    public override void HandleRequest(ERequest request, EKaze fromPlayerKaze, Hai haiToHandle, Action<EResponse> onResponse)
    {
        base.HandleRequest(request, fromPlayerKaze, haiToHandle, onResponse);
    }

    protected override EResponse Check_TsumoOrNot(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        MahjongAgent.copyTehai(Tehai);
        Hai tsumoHai = haiToHandle;

        // ツモあがりの場合は、イベント(ツモあがり)を返す。
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai);
        if (agariScore > 0)
            return DoResponse(EResponse.Tsumo_Agari);

        // リーチの場合は、ツモ切りする。
        if (MahjongAgent.isReach()) {
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

    protected override EResponse Check_RonOrNot(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        if( fromPlayerKaze == MahjongAgent.getJikaze() )
            return DoResponse(EResponse.Nagashi);

        MahjongAgent.copyTehai(Tehai);
        MahjongAgent.copyHou(Hou, MahjongAgent.getJikaze());

        if(isFuriten() == false)
        {
            int agariScore = MahjongAgent.getAgariScore(Tehai, haiToHandle);

            if (agariScore > 0)
                return DoResponse(EResponse.Ron_Agari);
        }

        return DoResponse(EResponse.Nagashi);
    }

    protected override EResponse Check_PonKanOrNot(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        return DoResponse(EResponse.Nagashi);
    }

    protected override EResponse Check_ChiiOrNot(EKaze fromPlayerKaze, Hai haiToHandle)
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
            for(int i = 0; i < haiTable.Length; i++) 
            {
                FormatWorker.setCounterFormat(tehai, haiTable[i]);

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
                    if (suteHaiTemp.ID == machiHais[j].ID)
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
                    if (suteHaiTemp.ID == machiHais[j].ID)
                        return true;
                }
            }

        }

        return false;
    }


    protected readonly static int HYOUKA_SHUU = 1;

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
