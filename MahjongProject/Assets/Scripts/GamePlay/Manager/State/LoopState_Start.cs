using UnityEngine;
using System.Collections;


public class LoopState_Start : MahjongState 
{

    public override void Enter() {
        base.Enter();

        StartCoroutine( StartPickHai() );
    }

    IEnumerator StartPickHai()
    {
        logicOwner.PrepareToStart();

        yield return new WaitForEndOfFrame();

        owner.ChangeState<LoopState_PickHai>();
    }
}
