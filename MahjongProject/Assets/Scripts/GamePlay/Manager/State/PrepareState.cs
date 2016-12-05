using UnityEngine;
using System.Collections;


/// <summary>
/// Sai sai furi for deciding Qin jia.
/// </summary>

public class PrepareState : MahjongState 
{

    protected override void AddListeners()
    {
        owner.AddInputListener(OnSaifuriForQinEnd);
    }
    protected override void RemoveListeners()
    {
        owner.RemoveInputListener(OnSaifuriForQinEnd);
    }

    public override void Enter() {
        base.Enter();

        EventManager.Get().SendEvent(UIEventType.Saifuri);
    }

    void OnSaifuriForQinEnd(EPlayerInputType type, EKaze kaze, object[] args)
    {
        logicOwner.SetRandomChiicha();

        // set jikazes.
        logicOwner.PrepareKyokuYama();

        EventManager.Get().SendEvent(UIEventType.Init_PlayerInfoUI);
        EventManager.Get().SendEvent(UIEventType.SetYama_BeforeHaipai);

        owner.ChangeState<HaiPaiState>();
    }
}
