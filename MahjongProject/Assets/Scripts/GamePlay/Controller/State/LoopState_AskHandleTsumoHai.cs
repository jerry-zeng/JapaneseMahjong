using UnityEngine;
using System.Collections;


public class LoopState_AskHandleTsumoHai : GameStateBase 
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_TsumoHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_TsumoHai_Handler = OnHandle_ResponseTsumoHai;

        logicOwner.Ask_Handle_TsumoHai();
    }


    void OnHandle_ResponseTsumoHai()
    {
        Player activePlayer = logicOwner.ActivePlayer;

        switch( activePlayer.Action.Response )
        {
            case EResponse.Tsumo_Agari:
            {
                logicOwner.Handle_TsumoAgari();

                EventManager.Get().SendEvent(UIEventType.Tsumo_Agari, activePlayer);

                owner.ChangeState<LoopState_Agari>();
            }
            break;
            case EResponse.Ankan:
            {
                logicOwner.Handle_AnKan();

                EventManager.Get().SendEvent(UIEventType.Ankan, activePlayer);

                if( logicOwner.checkKanCountOverFlow() ){
                    throw new MahjongException("ERyuuKyokuReason.KanOver4");
                }
                else{
                    owner.ChangeState<LoopState_PickRinshanHai>();
                }
            }
            break;
            case EResponse.Kakan:
            {
                logicOwner.Handle_KaKan();

                EventManager.Get().SendEvent(UIEventType.Kakan, activePlayer, logicOwner.KakanHai);

                if( logicOwner.checkKanCountOverFlow() ){
                    throw new MahjongException("ERyuuKyokuReason.KanOver4");
                }
                else{
                    owner.ChangeState<LoopState_AskHandleKakanHai>();
                }
            }
            break;
            case EResponse.Reach:
            {
                logicOwner.Handle_Reach();

                EventManager.Get().SendEvent(UIEventType.Reach, activePlayer, logicOwner.SuteHaiIndex, logicOwner.SuteHai, logicOwner.isTedashi);

                // TODO: need to check after sute reach hai and no one Ron.
                if( logicOwner.checkReach4() && !GameSettings.AllowReach4 ){
                    throw new MahjongException("ERyuuKyokuReason.Reach4");
                }
                else{
                    owner.ChangeState<LoopState_AskHandleSuteHai>();
                }
            }
            break;
            case EResponse.SuteHai:
            {
                logicOwner.Handle_SuteHai();

                EventManager.Get().SendEvent(UIEventType.SuteHai, activePlayer, logicOwner.SuteHaiIndex, logicOwner.SuteHai, logicOwner.isTedashi);

                if( logicOwner.checkSuteFonHai4() && !GameSettings.AllowSuteFonHai4 ){
                    throw new MahjongException("ERyuuKyokuReason.SuteFonHai4");
                }
                else{
                    owner.ChangeState<LoopState_AskHandleSuteHai>();
                }
            }
            break;
            // This is only enable when any players select ERyuuKyokuReason.HaiTypeOver9
            case EResponse.Nagashi: 
            {
                if( logicOwner.checkHaiTypeOver9() ){
                    throw new MahjongException("ERyuuKyokuReason.HaiTypeOver9");
                }
                else{
                    throw new MahjongException("Invalid response");
                }
            }
            //break;
        }
    }
}
