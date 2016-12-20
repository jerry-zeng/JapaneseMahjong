using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_AskSelectSuteHai : GameStateBase 
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_Select_SuteHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_Select_SuteHai_Handler = OnHandle_ResponseSelectSuteHai;

        //logicOwner.Ask_Select_SuteHai();
        StartCoroutine( AskSelectSuteHai() );
    }

    IEnumerator AskSelectSuteHai()
    {
        // wait for naki animation time.
        yield return new WaitForSeconds( MahjongView.NakiAnimationTime + 0.1f );

        logicOwner.Ask_Select_SuteHai();
    }

    void OnHandle_ResponseSelectSuteHai()
    {
        logicOwner.Handle_SelectSuteHai();

        owner.ChangeState<LoopState_AskHandleSuteHai>();
    }
}
