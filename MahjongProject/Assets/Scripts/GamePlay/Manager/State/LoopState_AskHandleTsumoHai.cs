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

                owner.ChangeState<LoopState_AgariTsumo>();
            }
            break;
            case EResponse.Ankan:
            {
                logicOwner.Handle_AnKan();

                EventManager.Get().SendEvent(UIEventType.Ankan, activePlayer);

                owner.ChangeState<LoopState_PickRinshanHai>();
            }
            break;
            case EResponse.Kakan:
            {
                logicOwner.Handle_KaKan();

                EventManager.Get().SendEvent(UIEventType.Kakan, activePlayer, logicOwner.KakanHai);

                owner.ChangeState<LoopState_AskHandleKakanHai>();
            }
            break;
            case EResponse.Reach:
            {
                logicOwner.Handle_Reach();

                EventManager.Get().SendEvent(UIEventType.Reach, activePlayer, logicOwner.SuteHaiIndex, logicOwner.SuteHai);

                owner.ChangeState<LoopState_AskHandleSuteHai>();
            }
            break;
            case EResponse.SuteHai:
            {
                logicOwner.Handle_SuteHai();

                EventManager.Get().SendEvent(UIEventType.SuteHai, activePlayer, logicOwner.SuteHaiIndex, logicOwner.SuteHai);

                owner.ChangeState<LoopState_AskHandleSuteHai>();
            }
            break;
        }
    }
}
