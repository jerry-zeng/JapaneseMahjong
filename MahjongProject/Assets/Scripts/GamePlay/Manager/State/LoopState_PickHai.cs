using UnityEngine;
using System.Collections;


public class LoopState_PickHai : MahjongState 
{

    public override void Enter() {
        base.Enter();

        logicOwner.PickNewTsumoHai();

        if( logicOwner.IsRyuukyoku() ) {
            owner.ChangeState<LoopState_HandleRyuuKyoKu>();
            //StartCoroutine( HandleRyuuKyoku() );
        }
        else {
            Hai tsumoHai = logicOwner.TsumoHai;
            logicOwner.ActivePlayer.Tehai.addJyunTehai( tsumoHai );

            int lastPickIndex = logicOwner.Yama.getLastTsumoHaiIndex();
            EventManager.Get().SendEvent(UIEventType.PickTsumoHai, logicOwner.ActivePlayer, lastPickIndex, tsumoHai );

            owner.ChangeState<LoopState_AskHandleTsumoHai>();
            //StartCoroutine( AskHandleTsumoHai() );
        }
    }

    IEnumerator HandleRyuuKyoku()
    {
        yield return new WaitForSeconds(0.1f);

        owner.ChangeState<LoopState_HandleRyuuKyoKu>();
    }

    IEnumerator AskHandleTsumoHai()
    {
        yield return new WaitForSeconds(0.1f);

        owner.ChangeState<LoopState_AskHandleTsumoHai>();
    }

}
