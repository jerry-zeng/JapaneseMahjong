using UnityEngine;
using System.Collections;


public class GameOverState : MahjongState 
{

    public override void Enter() {
        base.Enter();

        Debug.Log("Game over, show the score list.");
    }

}
