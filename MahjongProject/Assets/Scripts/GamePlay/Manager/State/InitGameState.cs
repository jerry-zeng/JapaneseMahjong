using UnityEngine;
using System.Collections;


public class InitGameState : MahjongState 
{

    public override void Enter() {

        base.Enter();

        StartCoroutine( InitData() );
    }

    IEnumerator InitData() {
        logicOwner.initialize();

        yield return new WaitForEndOfFrame();

        EventManager.Get().SendEvent( EventId.Init_Game );

        owner.ChangeState<PrepareState>();
    }
}
