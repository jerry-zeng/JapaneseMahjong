using UnityEngine;
using System.Collections;


public class LoopState_PickHai : MahjongState 
{

    public override void Enter() {
        base.Enter();

        logicOwner.PickNewTsumoHai();

        int lastPickIndex = logicOwner.getYama().getLastTsumoHaiIndex();
        EventManager.Get().SendUIEvent(UIEventID.PickHai, logicOwner.getActivePlayer(), lastPickIndex);

        Debug.LogWarningFormat( "Player in kaze {0} picked a new tsumo hai", logicOwner.getActivePlayer().JiKaze.ToString() );

        if( logicOwner.IsLastHai() ) {
            //owner.ChangeState<LoopState_CheckRyuuKyoKu>();
        }
        else {
            //owner.ChangeState<LoopState_CheckTsumo>();
        }
    }
}
