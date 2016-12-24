using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : StateMachine, IObserver
{
    private static GameManager _instance;
    public static GameManager Get()
    {
        return _instance;
    }

    private MahjongMain mahjong;
    public MahjongMain LogicMain
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
        ChangeState<GameStartState>();
    }

    void OnDestroy()
    {
        ResManager.ClearMahjongPaiPool();
    }

    void OnApplicationQuit()
    {
        ResManager.ClearMahjongPaiPool();
    }


    public void Restart()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }


    public void OnHandleEvent(UIEventType evtID, object[] args) 
    {
        if( CurrentState is GameStateBase )
            (CurrentState as GameStateBase).OnHandleEvent(evtID, args);
    }

}
