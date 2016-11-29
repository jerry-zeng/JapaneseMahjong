using UnityEngine;
using System.Collections;


public class LoopState_ToNextLoop : MahjongState 
{

    public override void Enter() {
        base.Enter();

        logicOwner.GoToNextLoop();

        owner.ChangeState<LoopState_PickHai>();
    }
}
