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


    private List<Action<EPlayerInputType, EKaze, object[]>> _onPlayerInput;


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

        _onPlayerInput = new List<Action<EPlayerInputType, EKaze, object[]>>();
    }

    void Start() {
        ChangeState<InitGameState>();
    }


    public void AddInputListener(Action<EPlayerInputType, EKaze, object[]> del)
    {
        if(del == null) return;

        if(!_onPlayerInput.Contains(del))
            _onPlayerInput.Add(del);
    }
    public void RemoveInputListener(Action<EPlayerInputType, EKaze, object[]> del)
    {
        if(del == null) return;

        if(_onPlayerInput.Contains(del))
            _onPlayerInput.Remove(del);
    }

    private void InvokeDelegates(EPlayerInputType type, EKaze kaze, object[] args)
    {
        for( int i = 0; i < _onPlayerInput.Count; i++ )
            _onPlayerInput[i].Invoke(type, kaze, args);
    }


    public void OnHandleEvent(UIEventType evtID, object[] args) 
    {
        switch(evtID)
        {
            case UIEventType.On_Saifuri_End:
            case UIEventType.On_Saifuri_For_Haipai_End:
            {
                InvokeDelegates(EPlayerInputType.Saifuri, MahjongMain.getManKaze(), null);
            }
            break;

            case UIEventType.OnPlayerInput:
            {
                EPlayerInputType type = (EPlayerInputType)args[0];
                EKaze kaze = (EKaze)args[1];
                object[] param = (object[])args[2];
                InvokeDelegates(type, kaze, param);
            }
            break;
        }
    }

}
