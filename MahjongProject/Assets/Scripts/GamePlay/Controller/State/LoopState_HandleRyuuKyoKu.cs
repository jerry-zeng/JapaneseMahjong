using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_HandleRyuuKyoKu : GameStateBase 
{
    public override void Enter()
    {
        base.Enter();

        Debug.LogWarning("## Ryuu KyoKu 流局 ##");

        List<int> tenpaiPlayers = logicOwner.GetTenpaiPlayerIndex();

        EventManager.Get().SendEvent(UIEventType.RyuuKyoku, ERyuuKyokuReason.NoTsumoHai, tenpaiPlayers);

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

    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        if( evtID == UIEventType.End_RyuuKyoku )
        {
            if( logicOwner.EndRyuuKyoku() )
            {
                owner.ChangeState<GameStartState>();
            }
            else
            {
                owner.ChangeState<GameOverState>();
            }
        }
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
