using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RyuuKyokuPanel : MonoBehaviour
{
    public GameObject btn_Confirm;
    public UILabel lab_msg;

    public List<UIPlayerTenbouChangeInfo> playerTenbouList = new List<UIPlayerTenbouChangeInfo>();

    private ERyuuKyokuReason ryuuKyokuReason;
    private AgariUpdateInfo currentAgari;


    void Start(){
        UIEventListener.Get(btn_Confirm).onClick = OnConfirm;
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


    void OnConfirm( GameObject go )
    {
        EventManager.Get().SendEvent(UIEventType.End_RyuuKyoku);
    }
}
