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

        EventManager.Get().SendUIEvent(UIEventID.Saifuri);
    }

    void OnSaifuriForQinEnd(EPlayerInputType type, EKaze kaze, object[] args)
    {
        logicOwner.SetChiicha();

        // set jikazes.
        logicOwner.PrepareKyokuYama();

        EventManager.Get().SendUIEvent(UIEventID.Init_PlayerInfoUI);
        EventManager.Get().SendUIEvent(UIEventID.SetYama_BeforeHaipai);

        owner.ChangeState<HaiPaiState>();
    }
}
