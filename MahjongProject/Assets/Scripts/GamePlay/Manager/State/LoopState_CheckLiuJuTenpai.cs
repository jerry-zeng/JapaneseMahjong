using UnityEngine;
using System.Collections;


public class LoopState_CheckLiuJuTenpai : MahjongState 
{

    public override void Enter() {
        base.Enter();

        if( logicOwner.HasRyuukyokuTenpai() ) {
            StartCoroutine( HandleRyuukyokuTenpai() );
        }
        else { 
            owner.ChangeState<KyoKuOverState>();
        }
    }

    IEnumerator HandleRyuukyokuTenpai() {
        yield return new WaitForSeconds(0.1f);

        logicOwner.HandleRyuukyokuTenpai();

        owner.ChangeState<KyoKuOverState>();
    }
}
