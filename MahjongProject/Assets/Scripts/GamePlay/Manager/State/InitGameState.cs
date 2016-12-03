using UnityEngine;
using System.Collections;


public class InitGameState : MahjongState 
{

    public override void Enter() {

        base.Enter();

        StartCoroutine( InitData() );
    }

    IEnumerator InitData() {

        yield return new WaitForEndOfFrame();

        EventManager.Get().SendUIEvent( UIEventID.Init_Game );

        owner.ChangeState<PrepareState>();
    }
}
