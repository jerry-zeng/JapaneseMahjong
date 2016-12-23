using UnityEngine;
using System.Collections;


public class GameStateBase : State 
{
    protected MahjongMain logicOwner;
    protected GameManager owner;

    protected virtual void Awake()
    {
        owner = GetComponent<GameManager>();
        logicOwner = owner.MahjongMain;
    }

    public virtual void OnHandleEvent(UIEventType evtID, object[] args) 
    {
        
    }
}
