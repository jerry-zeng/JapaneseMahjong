using UnityEngine;
using System.Collections;


public class LoopState_HandleRyuuKyoKu : MahjongState 
{
    public override void Enter()
    {
        base.Enter();
        Debug.LogWarning("## Ryuu KyoKu 流局 ##");

        int tenpaiCount = 0;
        for( int i = 0; i < logicOwner.PlayerList.Count; i++ )
        {
            if( logicOwner.PlayerList[i].isTenpai() == true ) 
                tenpaiCount++;
        }

        EventManager.Get().SendEvent(UIEventType.RyuuKyoku, tenpaiCount);

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
