using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_AskHandleSuteHai : GameStateBase
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_SuteHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_SuteHai_Handler = OnHandle_ResponseSuteHai;

        StartCoroutine(AskHandleSuteHai());
    }

    IEnumerator AskHandleSuteHai()
    {
        // wait for sute hai animation time.
        yield return new WaitForSeconds( MahjongView.SuteHaiAnimationTime + 0.1f );

        logicOwner.Ask_Handle_SuteHai();
    }

    void OnHandle_ResponseSuteHai()
    {
        List<EKaze> ronPlayers = logicOwner.GetRonPlayers();
        if( ronPlayers.Count > 0 )
        {
            logicOwner.Handle_SuteHai_Ron();

            // show ron ui.
            EventManager.Get().SendEvent(UIEventType.Ron_Agari, ronPlayers, logicOwner.FromKaze, logicOwner.SuteHai);

            owner.ChangeState<LoopState_AgariRon>();
        }
        else
        {
            // As DaiMinKan and Pon is availabe to one player at the same time, and their priority is bigger than Chii,
            // perform DaiMinKan and Pon firstly.
            List<EKaze> validKaze = new List<EKaze>();

            foreach( var info in logicOwner.PlayerResponseMap )
            {
                if( info.Value == EResponse.Pon || info.Value == EResponse.DaiMinKan )
                    validKaze.Add( info.Key );
            }

            if( validKaze.Count > 0 )
            {
                if( validKaze.Count == 1 )
                {
                    EKaze kaze = validKaze[0];
                    EResponse resp = logicOwner.PlayerResponseMap[kaze];

                    logicOwner.ResetActivePlayer(kaze);

                    switch( resp )
                    {
                        case EResponse.Pon:
                        {
                            logicOwner.Handle_Pon();

                            EventManager.Get().SendEvent(UIEventType.Pon, logicOwner.ActivePlayer, logicOwner.FromKaze);

                            owner.ChangeState<LoopState_AskSelectSuteHai>();
                        }
                        break;
                        case EResponse.DaiMinKan:
                        {
                            logicOwner.Handle_DaiMinKan();

                            EventManager.Get().SendEvent(UIEventType.DaiMinKan, logicOwner.ActivePlayer, logicOwner.FromKaze);

                            owner.ChangeState<LoopState_PickRinshanHai>();
                        }
                        break;
                    }
                }
                else{
                    throw new InvalidResponseException("More than one player perform Pon or DaiMinKan!?");
                }
            }
            else // no one Pon or DaiMinKan, perform Chii
            {
                foreach( var info in logicOwner.PlayerResponseMap )
                {
                    if( info.Value == EResponse.Chii_Left || 
                       info.Value == EResponse.Chii_Center || 
                       info.Value == EResponse.Chii_Right )
                    {
                        validKaze.Add( info.Key );
                    }
                }

                if( validKaze.Count > 0 )
                {
                    if( validKaze.Count == 1 )
                    {
                        EKaze kaze = validKaze[0];
                        EResponse resp = logicOwner.PlayerResponseMap[kaze];

                        logicOwner.ResetActivePlayer(kaze);

                        switch( resp )
                        {
                            case EResponse.Chii_Left:
                            {
                                logicOwner.Handle_ChiiLeft();

                                EventManager.Get().SendEvent(UIEventType.Chii_Left, logicOwner.ActivePlayer, logicOwner.FromKaze);
                            }
                            break;
                            case EResponse.Chii_Center:
                            {
                                logicOwner.Handle_ChiiCenter();

                                EventManager.Get().SendEvent(UIEventType.Chii_Center, logicOwner.ActivePlayer, logicOwner.FromKaze);
                            }
                            break;
                            case EResponse.Chii_Right:
                            {
                                logicOwner.Handle_ChiiRight();

                                EventManager.Get().SendEvent(UIEventType.Chii_Right, logicOwner.ActivePlayer, logicOwner.FromKaze);
                            }
                            break;
                        }

                        owner.ChangeState<LoopState_AskSelectSuteHai>();
                    }
                    else{
                        throw new InvalidResponseException("More than one player perform Chii!?");
                    }
                }
                else // Nagashi
                {
                    logicOwner.Handle_SuteHai_Nagashi();

                    owner.ChangeState<LoopState_ToNextLoop>();
                }
            }
        }
    }
}
