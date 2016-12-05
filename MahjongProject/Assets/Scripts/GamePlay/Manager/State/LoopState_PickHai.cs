using UnityEngine;
using System.Collections;


public class LoopState_PickHai : MahjongState 
{

    public override void Enter() {
        base.Enter();

        logicOwner.PickNewTsumoHai();

        if( logicOwner.IsRyuukyoku() ) {
            //owner.ChangeState<LoopState_CheckRyuuKyoKu>();
        }
        else {
            int lastPickIndex = logicOwner.getYama().getLastTsumoHaiIndex();
            EventManager.Get().SendEvent(UIEventType.PickHai, logicOwner.getActivePlayer(), lastPickIndex);

            Debug.LogWarningFormat( "Player in kaze {0} picked a new tsumo hai", logicOwner.getActivePlayer().JiKaze.ToString() );

            //owner.ChangeState<LoopState_CheckTsumo>();
        }
    }
}
