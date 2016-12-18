using UnityEngine;
using System.Collections;


public class LoopState_AskHandleTsumoHai : MahjongState 
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_TsumoHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_TsumoHai_Handler = OnHandle_ResponseTsumoHai;

        logicOwner.Ask_Handle_TsumoHai();
        //StartCoroutine(AskHandleTsumoHai());
    }

    IEnumerator AskHandleTsumoHai() {
        yield return new WaitForSeconds(0.3f);

        logicOwner.Ask_Handle_TsumoHai();
    }


    void OnHandle_ResponseTsumoHai()
    {
        Player activePlayer = logicOwner.ActivePlayer;

        switch( activePlayer.Action.Response )
        {
            case EResponse.Tsumo_Agari:
            {
                
            }
            break;

            case EResponse.Ankan:
            {

            }
            break;

            case EResponse.Kakan:
            {

                //Ask_Handle_KaKanHai();
            }
            break;

            case EResponse.Reach:
            {

            }
            break;

            case EResponse.SuteHai:
            {
                // 捨牌のインデックスを取得する。
                logicOwner.SuteHaiIndex = activePlayer.getSutehaiIndex();

                int suteHaiIndex = logicOwner.SuteHaiIndex;
                Hai suteHai = logicOwner.SuteHai;

                if( suteHaiIndex >= activePlayer.Tehai.getJyunTehaiCount() ) {// ツモ切り
                    Hai.copy( suteHai, logicOwner.TsumoHai );
                    activePlayer.Hou.addHai( suteHai );
                }
                else {// 手出し
                    activePlayer.Tehai.copyJyunTehaiIndex( suteHai, suteHaiIndex );
                    activePlayer.Tehai.removeJyunTehai( suteHaiIndex );
                    activePlayer.Tehai.Sort();

                    activePlayer.Hou.addHai( suteHai );
                    activePlayer.Hou.setTedashi( true );
                }

                logicOwner.AllSuteHaiList.Add( new SuteHai( suteHai ) );

                if( !activePlayer.IsReach )
                    activePlayer.SuteHaisCount = logicOwner.AllSuteHaiList.Count;

                EventManager.Get().SendEvent(UIEventType.SuteHai, activePlayer, suteHaiIndex, suteHai);

                owner.ChangeState<LoopState_AskHandleSuteHai>();
            }
            break;
        }
    }
}
