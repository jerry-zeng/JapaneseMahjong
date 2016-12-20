using UnityEngine;
using System.Collections;


public class LoopState_AgariTsumo : GameStateBase
{
    public override void Exit()
    {
        base.Exit();
    }

    public override void Enter()
    {
        base.Enter();

        StartCoroutine(HandleAgariTsumo());
    }

    IEnumerator HandleAgariTsumo() {
        yield return new WaitForSeconds( MahjongView.AgariAnimationTime );

        Debug.LogWarning("## Tsumo player is in kaze " + logicOwner.ActivePlayer.JiKaze.ToString());

    }
}
