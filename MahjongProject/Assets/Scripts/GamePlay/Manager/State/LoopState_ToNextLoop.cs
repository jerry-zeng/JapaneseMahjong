using UnityEngine;
using System.Collections;


public class LoopState_ToNextLoop : MahjongState 
{

    public override void Enter() {
        base.Enter();

        logicOwner.SetNextPlayer();

        owner.ChangeState<LoopState_PickHai>();
        //StartCoroutine( PickTsumoHai() );
    }

    IEnumerator PickTsumoHai() {
        yield return new WaitForSeconds(0.5f);

        owner.ChangeState<LoopState_PickHai>();
    }
}
