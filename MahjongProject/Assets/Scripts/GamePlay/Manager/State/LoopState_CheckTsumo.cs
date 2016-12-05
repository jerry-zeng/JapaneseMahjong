using UnityEngine;
using System.Collections;


public class LoopState_CheckTsumo : MahjongState 
{

    public override void Enter() {
        base.Enter();

        StartCoroutine(CheckTsumo());
    }

    IEnumerator CheckTsumo() {
        yield return new WaitForEndOfFrame();

        bool hasTsumo = false;
        if( hasTsumo ) {
            owner.ChangeState<KyoKuOverState>();
        }
        else {
            owner.ChangeState<LoopState_ToNextLoop>();
        }
    }
}
