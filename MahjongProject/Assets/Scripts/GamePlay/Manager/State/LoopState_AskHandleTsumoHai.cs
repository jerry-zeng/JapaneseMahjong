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
            }
            break;
            case EResponse.Ankan:
            {
                logicOwner.Handle_AnKan();
            }
            break;
            case EResponse.Kakan:
            {
                logicOwner.Handle_KaKan();
            }
            break;
            case EResponse.Reach:
            {
                logicOwner.Handle_Reach();
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
