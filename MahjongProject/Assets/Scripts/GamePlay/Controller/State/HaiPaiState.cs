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

        EventManager.Get().SendEvent(UIEventType.Select_Wareme);
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        switch(evtID)
        {
            case UIEventType.On_Select_Wareme_End:
            {
                OnSelectWaremeEnd();
            }
            break;
        }
    }

    void OnSelectWaremeEnd()
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
