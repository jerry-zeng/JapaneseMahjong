using UnityEngine;
using System.Collections;


public class LoopState_PickHai : MahjongState 
{

    public override void Enter() {
        base.Enter();

        logicOwner.PickNewTsumoHai();

        if( logicOwner.IsLastHai() ) {
            owner.ChangeState<LoopState_CheckRyuuKyoKu>();
        }
        else {
            Debug.Log( string.Format( "player {0} pick a new tsumo hai", logicOwner.getActivePlayer()) );
            owner.ChangeState<LoopState_CheckTsumo>();
        }
    }
}
