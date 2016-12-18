using UnityEngine;
using System.Collections;


public class LoopState_PickRinshanHai : MahjongState 
{
    public override void Enter() 
    {
        base.Enter();

        logicOwner.PickRinshanHai();


    }
}
