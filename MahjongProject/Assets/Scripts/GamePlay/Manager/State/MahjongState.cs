using UnityEngine;
using System.Collections;


public class MahjongState : State 
{

    protected MahjongMain logicOwner;
    protected GameMain owner;

    protected virtual void Awake() {
        owner = GetComponent<GameMain>();
        logicOwner = owner.MahjongMain;
    }

}
