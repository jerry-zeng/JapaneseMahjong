using UnityEngine;
using System.Collections;


public class LoopState_PickRinshanHai : GameStateBase 
{
    public override void Enter() 
    {
        base.Enter();

        logicOwner.PickRinshanHai();

        Hai rinshanHai = logicOwner.TsumoHai;

        int lastPickIndex = logicOwner.Yama.getPreRinshanHaiIndex();
        int newDoraHaiIndex = logicOwner.Yama.getLastOmoteHaiIndex();

        EventManager.Get().SendEvent(UIEventType.PickRinshanHai, logicOwner.ActivePlayer, lastPickIndex, rinshanHai, newDoraHaiIndex );

        StartCoroutine( AskHandleRinshanHai() );

    }

    IEnumerator AskHandleRinshanHai()
    {
        yield return new WaitForSeconds( MahjongView.NakiAnimationTime );

        owner.ChangeState<LoopState_AskHandleTsumoHai>();
    }

}
