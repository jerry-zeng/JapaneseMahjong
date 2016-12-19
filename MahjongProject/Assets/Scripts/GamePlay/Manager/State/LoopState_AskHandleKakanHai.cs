using UnityEngine;
using System.Collections;


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
        yield return new WaitForSeconds(0.5f);

        logicOwner.Ask_Handle_KaKanHai();
    }


    void OnHandle_ResponseKakanHai()
    {
        if( logicOwner.CheckMultiRon() == true )
        {
            logicOwner.Handle_KaKan_Ron();

            // show ron ui.
        }
        else
        {
            //logicOwner.Handle_KaKan_Nagashi();

            owner.ChangeState<LoopState_PickRinshanHai>();
        }
    }
}
