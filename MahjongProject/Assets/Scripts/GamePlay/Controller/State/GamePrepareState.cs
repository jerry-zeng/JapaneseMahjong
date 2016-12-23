using UnityEngine;
using System.Collections;


/// <summary>
/// Sai sai furi for deciding Qin jia.
/// </summary>

public class GamePrepareState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        if( logicOwner.needSelectOya() )
            EventManager.Get().SendEvent(UIEventType.Saifuri_For_Oya);
        else
            OnSaifuriForQinEnd();
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        switch(evtID)
        {
            case UIEventType.On_Saifuri_For_Oya_End:
            {
                logicOwner.SetOyaChiicha();

                OnSaifuriForQinEnd();
            }
            break;
        }
    }

    void OnSaifuriForQinEnd()
    {
        logicOwner.PrepareKyoku();

        EventManager.Get().SendEvent(UIEventType.Init_PlayerInfoUI);
        EventManager.Get().SendEvent(UIEventType.SetYama_BeforeHaipai);

        owner.ChangeState<HaiPaiState>();
    }

}
