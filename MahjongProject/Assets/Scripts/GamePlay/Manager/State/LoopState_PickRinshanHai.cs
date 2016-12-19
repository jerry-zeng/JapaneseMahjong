using UnityEngine;
using System.Collections;


public class LoopState_PickRinshanHai : GameStateBase 
{
    public override void Enter() 
    {
        base.Enter();

        logicOwner.PickRinshanHai();


    }
}
