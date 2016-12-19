using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_AskHandleSuteHai : MahjongState
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_SuteHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_SuteHai_Handler = OnHandle_ResponseSuteHai;

        //logicOwner.Ask_Handle_SuteHai();
        StartCoroutine(AskHandleSuteHai());
    }

    IEnumerator AskHandleSuteHai()
    {
        // wait for sute hai animation time.
        yield return new WaitForSeconds(0.5f);

        logicOwner.Ask_Handle_SuteHai();
    }


    void OnHandle_ResponseSuteHai()
    {
        if( logicOwner.CheckMultiRon() == true )
        {
            logicOwner.HandleMultiRon();
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

                    switch( resp )
                    {
                        case EResponse.Pon:
                        {

                        }
                        break;
                        case EResponse.DaiMinKan:
                        {

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

                        switch( resp )
                        {
                            case EResponse.Chii_Left:
                            {

                            }
                            break;
                            case EResponse.Chii_Center:
                            {

                            }
                            break;
                            case EResponse.Chii_Right:
                            {

                            }
                            break;
                        }
                    }
                    else{
                        throw new InvalidResponseException("More than one player perform Chii!?");
                    }
                }
                else // Nagashi
                {
                    owner.ChangeState<LoopState_ToNextLoop>();
                }
            }
        }
    }
}
