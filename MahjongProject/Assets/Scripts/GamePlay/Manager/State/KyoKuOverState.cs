using UnityEngine;
using System.Collections;


public class KyoKuOverState : MahjongState 
{

    public override void Enter() {
        base.Enter();

        if( logicOwner.IsLastKyoku() ) {
            owner.ChangeState<GameOverState>();
        }
        else {
            logicOwner.GoToNextKyoku();
            Debug.Log( "Go To Next Kyoku" );
            owner.ChangeState<InitGameState>();
        }
    }
}
