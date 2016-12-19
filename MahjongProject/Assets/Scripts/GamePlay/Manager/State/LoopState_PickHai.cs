using UnityEngine;
using System.Collections;


public class LoopState_PickHai : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        logicOwner.PickNewTsumoHai();

        if( logicOwner.IsRyuukyoku() ) {
            owner.ChangeState<LoopState_HandleRyuuKyoKu>();
        }
        else {
            Hai tsumoHai = logicOwner.TsumoHai;
            //logicOwner.ActivePlayer.Tehai.addJyunTehai( tsumoHai );//

            int lastPickIndex = logicOwner.Yama.getLastTsumoHaiIndex();
            EventManager.Get().SendEvent(UIEventType.PickTsumoHai, logicOwner.ActivePlayer, lastPickIndex, tsumoHai );

            owner.ChangeState<LoopState_AskHandleTsumoHai>();
        }
    }

}
