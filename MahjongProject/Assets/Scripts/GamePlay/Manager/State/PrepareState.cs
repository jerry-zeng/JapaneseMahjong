using UnityEngine;
using System.Collections;


/// <summary>
/// Sai sai furi for deciding Qin jia.
/// </summary>

public class PrepareState : MahjongState 
{

    public override void Enter() {
        base.Enter();

        owner.SetDelegate(OnSaifuriForQinEnd);

        EventManager.Get().SendUIEvent(UIEventID.Saifuri);
    }

    void OnSaifuriForQinEnd() {
        logicOwner.SetChiicha();

        // set jikazes.
        logicOwner.PrepareKyokuYama();

        EventManager.Get().SendUIEvent(UIEventID.Init_PlayerInfoUI);
        EventManager.Get().SendUIEvent(UIEventID.SetYama_BeforeHaipai);

        owner.ChangeState<HaiPaiState>();
    }
}
