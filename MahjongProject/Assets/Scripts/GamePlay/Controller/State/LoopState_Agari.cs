using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_Agari : GameStateBase
{
    public override void Exit()
    {
        base.Exit();
    }

    public override void Enter()
    {
        base.Enter();

        StartCoroutine(HandleAgariRon());
    }

    IEnumerator HandleAgariRon() {
        yield return new WaitForSeconds( MahjongView.AgariAnimationTime );

        yield return new WaitForSeconds(0.5f);

        EventManager.Get().SendEvent(UIEventType.Display_Agari_Panel, logicOwner.AgariUpdateInfoList);
    }

    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        if( evtID == UIEventType.End_Kyoku )
        {
            owner.ChangeState<KyoKuOverState>();
        }
    }
}
