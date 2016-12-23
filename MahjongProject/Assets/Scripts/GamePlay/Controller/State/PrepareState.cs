using UnityEngine;
using System.Collections;


/// <summary>
/// Sai sai furi for deciding Qin jia.
/// </summary>

public class PrepareState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        EventManager.Get().SendEvent(UIEventType.Saifuri_For_Oya);
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        switch(evtID)
        {
            case UIEventType.On_Saifuri_For_Oya_End:
            {
                OnSaifuriForQinEnd();
            }
            break;
        }
    }

    void OnSaifuriForQinEnd()
    {
        logicOwner.SetRandomChiicha();

        // set jikazes.
        logicOwner.PrepareKyokuYama();

        EventManager.Get().SendEvent(UIEventType.Init_PlayerInfoUI);
        EventManager.Get().SendEvent(UIEventType.SetYama_BeforeHaipai);

        owner.ChangeState<HaiPaiState>();
    }

}
