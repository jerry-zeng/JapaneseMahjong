using UnityEngine;
using System.Collections;


/// <summary>
/// Sai sai furi for deciding Qin jia.
/// </summary>

public class GamePrepareState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        if( logicOwner.needSelectOya() )
            EventManager.Get().SendEvent(UIEventType.Saifuri_For_Oya);
        else
            OnSaifuriForQinEnd();
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        switch(evtID)
        {
            case UIEventType.On_Saifuri_For_Oya_End:
            {
                logicOwner.SetOyaChiicha();

                OnSaifuriForQinEnd();
            }
            break;
        }
    }

    void OnSaifuriForQinEnd()
    {
        logicOwner.PrepareKyoku();

        string kyokuStr = "";
        if( logicOwner.IsLastKyoku() ){
            kyokuStr = "オラス";
        }
        else{
            string kazeStr = ResManager.getString( "kaze_" + logicOwner.getBaKaze().ToString().ToLower() );
            kyokuStr = kazeStr + logicOwner.Kyoku.ToString() + "局";
            if( logicOwner.HonBa > 0 )
                kyokuStr += " " + logicOwner.HonBa.ToString() + "本场";
            kyokuStr += " Start!";
        }
        Debug.LogWarningFormat( kyokuStr );

        EventManager.Get().SendEvent(UIEventType.Init_PlayerInfoUI);
        EventManager.Get().SendEvent(UIEventType.SetYama_BeforeHaipai);

        owner.ChangeState<HaiPaiState>();
    }

}
