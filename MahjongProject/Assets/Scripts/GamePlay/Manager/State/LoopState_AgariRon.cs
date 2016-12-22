using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_AgariRon : GameStateBase
{
    public override void Exit()
    {
        base.Exit();
    }

    public override void Enter()
    {
        base.Enter();

        StartCoroutine(HandleAgariRon());
    }

    IEnumerator HandleAgariRon() {
        yield return new WaitForSeconds( MahjongView.AgariAnimationTime );

        List<EKaze> ronPlayers = logicOwner.GetRonPlayers();
        Debug.LogWarning("## show agari result: Ron player count = " + ronPlayers.Count.ToString());

        if(ronPlayers.Count > 1)
            Debug.LogWarning("## not support multi-ron in current version.");
        
        logicOwner.ResetActivePlayer( ronPlayers[0] );
        logicOwner.HandleRon();

        EventManager.Get().SendEvent(UIEventType.Display_Agari_Panel);
    }

}
