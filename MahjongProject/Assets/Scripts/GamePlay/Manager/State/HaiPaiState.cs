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
        EventManager.Get().SendEvent(EventID.Saifuri_For_Haipai);
    }

    void OnSaifuriForHaipaiEnd() {
        // haipai.
        logicOwner.SetWaremeAndHaipai();

        EventManager.Get().SendEvent(EventID.SetUI_AfterHaipai);
        
        owner.ChangeState<LoopState_Start>();
    }

}
