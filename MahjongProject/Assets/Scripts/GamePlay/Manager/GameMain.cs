using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameMain : StateMachine, IUIObserver
{

    private static GameMain _instance;
    public static GameMain Instance {
        get {
            return _instance;
        }
    }

    private MahjongMain mahjong;
    public MahjongMain MahjongMain {
        get {
            return mahjong;
        }
    }

    private List<System.Action> eventDelegates = new List<System.Action>();

    void OnEnable() {
        EventManager.Get().addObserver(this);
    }
    void OnDisable() {
        EventManager.Get().removeObserver(this);
    }


    void Awake() {
        _instance = this;
        mahjong = new MahjongMain();
    }

    void Start() {
        ChangeState<InitGameState>();
    }

    

    public void SetDelegate(System.Action del) {
        eventDelegates.Add(del);
    }

    private void CallDelegates() {
        for( int i = 0; i < eventDelegates.Count; i++ ) {
            if( eventDelegates[i] != null ) {
                eventDelegates[i].Invoke();
            }
        }
        
        ClearDelegates();
    }

    private void ClearDelegates() {
        eventDelegates.Clear();
    }

    public void OnHandleEvent(UIEventID evtID, object[] args) 
    {
        switch(evtID)
        {
            case UIEventID.On_Saifuri_End:
            case UIEventID.On_Saifuri_For_Haipai_End:
                CallDelegates();
                break;
        }
    }
}
