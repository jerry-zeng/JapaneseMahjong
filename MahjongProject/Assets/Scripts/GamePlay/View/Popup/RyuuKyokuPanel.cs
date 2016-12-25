using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RyuuKyokuPanel : MonoBehaviour
{
    public GameObject btn_Continue;
    public UILabel lab_msg;

    public List<UIPlayerTenbouChangeInfo> playerTenbouList = new List<UIPlayerTenbouChangeInfo>();

    private ERyuuKyokuReason ryuuKyokuReason;
    private AgariUpdateInfo currentAgari;


    void Start(){
        UIEventListener.Get(btn_Continue).onClick = OnConfirm;

        UILabel btnTag = btn_Continue.GetComponentInChildren<UILabel>(true);
        btnTag.text = ResManager.getString("continue");
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(ERyuuKyokuReason reason, List<AgariUpdateInfo> agariList)
    {
        this.ryuuKyokuReason = reason;
        this.currentAgari = agariList[0];

        gameObject.SetActive(true);

        Show_Internel();
    }

    void Show_Internel()
    {
        lab_msg.text = GetRyuuKyokuReasonString();

        PlayRyuuKyokuVoice();

        bool showTenpai = ryuuKyokuReason == ERyuuKyokuReason.NoTsumoHai;

        var tenbouInfos = currentAgari.tenbouChangeInfoList;
        EKaze nextKaze = currentAgari.manKaze;

        for( int i = 0; i < playerTenbouList.Count; i++ )
        {
            PlayerTenbouChangeInfo info = tenbouInfos.Find( ptci=> ptci.playerKaze == nextKaze );
            playerTenbouList[i].SetInfo( info.playerKaze, info.current, info.changed, info.isTenpai, showTenpai );
            nextKaze = nextKaze.Next();
        }
    }

    string GetRyuuKyokuReasonString()
    {
        return ResManager.getString( ryuuKyokuReason.ToString() );
    }

    void PlayRyuuKyokuVoice()
    {
        ECvType cv = ECvType.RyuuKyoku;
        if( ryuuKyokuReason == ERyuuKyokuReason.NoTsumoHai ){
            cv = ECvType.RyuuKyoku;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.HaiTypeOver9 ){
            cv = ECvType.RKK_HaiTypeOver9;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.SuteFonHai4 ){
            cv = ECvType.RKK_SuteFonHai4;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.KanOver4 ){
            cv = ECvType.RKK_KanOver4;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.Reach4 ){
            cv = ECvType.RKK_Reach4;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.Ron3 ){
            cv = ECvType.RKK_Ron3;
        }
        GameManager.Get().Speak(cv);
    }

    void OnConfirm( GameObject go )
    {
        EventManager.Get().SendEvent(UIEventType.End_RyuuKyoku);
    }
}
