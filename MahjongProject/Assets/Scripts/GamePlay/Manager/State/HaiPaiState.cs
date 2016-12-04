using UnityEngine;
using System.Collections;


public class HaiPaiState : MahjongState 
{

    protected override void AddListeners()
    {
        owner.AddInputListener(OnSaifuriForHaipaiEnd);
    }
    protected override void RemoveListeners()
    {
        owner.RemoveInputListener(OnSaifuriForHaipaiEnd);
    }


    public override void Enter() {
        base.Enter();

        StartCoroutine(PrepareYamaUI());
    }

    IEnumerator PrepareYamaUI() 
    {
        yield return new WaitForSeconds(1);

        EventManager.Get().SendUIEvent(UIEventID.Saifuri_For_Haipai);
    }

    void OnSaifuriForHaipaiEnd(EPlayerInputType type, EKaze kaze, object[] args)
    {
        // haipai.
        logicOwner.SetWaremeAndHaipai();

        EventManager.Get().SendUIEvent(UIEventID.SetUI_AfterHaipai);
        
        owner.ChangeState<LoopState_Start>();
    }

}
