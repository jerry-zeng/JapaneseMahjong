using UnityEngine;
using System.Collections;


/// <summary>
/// Sai sai furi for deciding Qin jia.
/// </summary>

public class GamePrepareState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        if( logicOwner.needSelectChiiCha() )
            EventManager.Get().SendEvent(UIEventType.Select_ChiiCha);
        else
            OnSaifuriForOyaEnd();
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        switch(evtID)
        {
            case UIEventType.On_Select_ChiiCha_End:
            {
                int index = (int)args[0];

                logicOwner.SetOyaChiicha(index);

                OnSaifuriForOyaEnd();
            }
            break;
        }
    }

    void OnSaifuriForOyaEnd()
    {
        logicOwner.PrepareKyoku();

        string kyokuStr = "";
        if( logicOwner.IsLastKyoku() )
        {
            kyokuStr = ResManager.getString("info_end");

            if( logicOwner.HonBa == 0 )
                owner.Speak(ECvType.ORaSu);
        }
        else
        {
            if( logicOwner.getBaKaze() == EKaze.Nan ){
                if(logicOwner.HonBa == 0 )
                    owner.Speak(ECvType.NanBa_Start);
            }
            else if(logicOwner.getBaKaze() == EKaze.Ton){
                if( logicOwner.Kyoku == (int)EKyoku.Ton_1 && logicOwner.HonBa == 0 )
                    owner.Speak(ECvType.Kyoku_Start);
            }

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
