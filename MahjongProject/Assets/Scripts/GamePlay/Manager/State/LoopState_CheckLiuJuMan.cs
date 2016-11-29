using UnityEngine;
using System.Collections;


public class LoopState_CheckRyuuKyoKu : MahjongState 
{

    public override void Enter() {
        base.Enter();

        if( logicOwner.HasRyuukyokuMan() ) {
            StartCoroutine( HandleRyuukyokuMan() );
        }
        else {
            owner.ChangeState<LoopState_CheckLiuJuTenpai>();
        }
    }

    IEnumerator HandleRyuukyokuMan() {
        yield return new WaitForSeconds(0.1f);

        logicOwner.HandleRyuukyokuMan();

        owner.ChangeState<KyoKuOverState>();
    }
}
