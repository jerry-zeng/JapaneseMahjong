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

        waitingOperation = StartCoroutine(HandleAgariRon());
    }

    IEnumerator HandleAgariRon() {
        yield return new WaitForSeconds( MahjongView.AgariAnimationTime );

        yield return new WaitForSeconds(MahjongView.NormalWaitTime);

        OnAgariAnimEnd();
    }

    void OnAgariAnimEnd()
    {
        StopWaitingOperation();

        EventManager.Get().SendEvent(UIEventType.Display_Agari_Panel, logicOwner.AgariUpdateInfoList);
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        if( evtID == UIEventType.On_UIAnim_End )
        {
            OnAgariAnimEnd();
        }
        else if( evtID == UIEventType.End_Kyoku )
        {
            owner.ChangeState<KyoKuOverState>();
        }
    }
}
