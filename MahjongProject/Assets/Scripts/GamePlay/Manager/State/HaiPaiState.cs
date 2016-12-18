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

        EventManager.Get().SendEvent(UIEventType.Saifuri_For_Haipai);
    }

    void OnSaifuriForHaipaiEnd(EPlayerInputType type, EKaze kaze, object[] args)
    {
        // haipai.
        logicOwner.SetWaremeAndHaipai();

        EventManager.Get().SendEvent(UIEventType.SetUI_AfterHaipai);

        StartCoroutine(StartLoop());
    }

    IEnumerator StartLoop()
    {
        yield return new WaitForSeconds(0.5f);

        owner.ChangeState<LoopState_Start>();
    }
}
