using UnityEngine;
using System.Collections;


public class LoopState_Start : MahjongState 
{

    public override void Enter() {
        base.Enter();

        logicOwner.PrepareToStart();

        owner.ChangeState<LoopState_PickHai>();
    }

}
