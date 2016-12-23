using UnityEngine;
using System.Collections;


public class GameOverState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        Debug.Log("Game over, show the score list.");
    }

}
