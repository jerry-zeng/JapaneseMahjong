using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_AskHandleKakanHai : GameStateBase
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_KakanHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_KakanHai_Handler = OnHandle_ResponseKakanHai;

        StartCoroutine(AskHandleKakanHai());
    }

    IEnumerator AskHandleKakanHai() {
        yield return new WaitForSeconds( MahjongView.NakiAnimationTime + 0.1f );

        logicOwner.Ask_Handle_KaKanHai();
    }


    void OnHandle_ResponseKakanHai()
    {
        List<EKaze> ronPlayers = logicOwner.GetRonPlayers();
        if( ronPlayers.Count > 0 )
        {
            logicOwner.Handle_KaKan_Ron();

            // show ron ui.
            EventManager.Get().SendEvent(UIEventType.Ron_Agari, ronPlayers, logicOwner.FromKaze, logicOwner.KakanHai);

            owner.ChangeState<LoopState_AgariRon>();
        }
        else
        {
            logicOwner.Handle_KaKan_Nagashi();

            owner.ChangeState<LoopState_PickRinshanHai>();
        }
    }
}
