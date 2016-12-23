using UnityEngine;
using System.Collections;


public class GameOverState : GameStateBase 
{
    public override void Enter() 
    {
        base.Enter();

        logicOwner.GameOver();

        Debug.Log("show the score list.");

        EventManager.Get().SendEvent(UIEventType.End_Game);
    }

}
