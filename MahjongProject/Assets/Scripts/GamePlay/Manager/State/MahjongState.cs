using UnityEngine;
using System.Collections;


public class MahjongState : State 
{

    protected MahjongMain logicOwner;
    protected GameManager owner;

    protected virtual void Awake() {
        owner = GetComponent<GameManager>();
        logicOwner = owner.MahjongMain;
    }

}
