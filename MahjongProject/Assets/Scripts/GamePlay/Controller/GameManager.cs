using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameManager : StateMachine, IObserver
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    private MahjongMain mahjong;
    public MahjongMain MahjongMain
    {
        get { return mahjong; }
    }


    void OnEnable() 
    {
        EventManager.Get().addObserver(this);
    }
    void OnDisable() 
    {
        EventManager.Get().removeObserver(this);
    }


    void Awake() {
        _instance = this;
        mahjong = new MahjongMain();
    }

    void Start() {
        ChangeState<InitGameState>();
    }

    void OnApplicationQuit()
    {
        ResManager.ClearMahjongPaiPool();
    }


    public void OnHandleEvent(UIEventType evtID, object[] args) 
    {
        if( CurrentState is GameStateBase )
            (CurrentState as GameStateBase).OnHandleEvent(evtID, args);
    }

}
