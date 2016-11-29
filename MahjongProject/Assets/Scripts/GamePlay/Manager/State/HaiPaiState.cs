using UnityEngine;
using System.Collections;


public class HaiPaiState : MahjongState 
{

    public override void Enter() {
        base.Enter();

        StartCoroutine(PrepareYamaUI());
    }

    IEnumerator PrepareYamaUI() {

        yield return new WaitForSeconds(1);

        owner.SetDelegate(OnSaifuriForHaipaiEnd);
        EventManager.Get().SendEvent(EventId.Saifuri_For_Haipai);
    }

    void OnSaifuriForHaipaiEnd() {
        // haipai.
        logicOwner.SetWaremeAndHaipai();

        EventManager.Get().SendEvent(EventId.SetUI_AfterHaipai);
        
        owner.ChangeState<LoopState_Start>();
    }

}
