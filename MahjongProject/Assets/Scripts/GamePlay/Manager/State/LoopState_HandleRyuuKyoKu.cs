using UnityEngine;
using System.Collections;


public class LoopState_HandleRyuuKyoKu : MahjongState 
{
    public override void Enter() {
        base.Enter();
        Debug.LogWarning("## Ryuu KyoKu 流局 ##");
        /*
        if( logicOwner.HasRyuukyokuMan() ) {
            StartCoroutine( HandleRyuukyokuMan() );
        }
        else {
            if( logicOwner.HasRyuukyokuTenpai() ) {
                StartCoroutine( HandleRyuukyokuTenpai() );
            }
            else {
                StartCoroutine( RyuukyokuOver() );

            }
        }
        */
    }

    IEnumerator HandleRyuukyokuMan() {
        yield return new WaitForSeconds(0.1f);

        logicOwner.HandleRyuukyokuMan();
        Debug.Log("-> HandleRyuukyokuMan()");

        owner.ChangeState<KyoKuOverState>();
    }

    IEnumerator HandleRyuukyokuTenpai() {
        yield return new WaitForSeconds(0.1f);

        logicOwner.HandleRyuukyokuTenpai();
        Debug.Log("-> HandleRyuukyokuTenpai()");

        owner.ChangeState<KyoKuOverState>();
    }

    IEnumerator RyuukyokuOver() {
        yield return new WaitForSeconds(0.1f);

        Debug.Log("-> RyuukyokuOver()");

        owner.ChangeState<KyoKuOverState>();
    }

}
