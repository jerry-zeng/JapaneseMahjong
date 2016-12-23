using UnityEngine;
using System.Collections;


public class HaiPaiState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        StartCoroutine(PrepareYamaUI());
    }

    IEnumerator PrepareYamaUI() 
    {
        yield return new WaitForSeconds(1);

        EventManager.Get().SendEvent(UIEventType.Saifuri_For_Haipai);
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        switch(evtID)
        {
            case UIEventType.On_Saifuri_For_Haipai_End:
            {
                OnSaifuriForHaipaiEnd();
            }
            break;
        }
    }

    void OnSaifuriForHaipaiEnd()
    {
        // haipai.
        logicOwner.SetWaremeAndHaipai();

        EventManager.Get().SendEvent(UIEventType.SetUI_AfterHaipai);

        StartCoroutine(StartLoop());
    }

    IEnumerator StartLoop()
    {
        yield return new WaitForSeconds(0.5f);

        logicOwner.PrepareToStart();

        owner.ChangeState<LoopState_PickHai>();
    }
}
