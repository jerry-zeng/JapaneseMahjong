using UnityEngine;
using System.Collections;


public class KyoKuOverState : GameStateBase 
{
    public override void Enter() {
        base.Enter();

        if( logicOwner.EndKyoku() )
        {
            owner.ChangeState<GameStartState>();
        }
        else
        {
            owner.ChangeState<GameOverState>();
        }
    }
}
