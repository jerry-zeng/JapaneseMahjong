using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameOverPanel : MonoBehaviour 
{
    public UILabel lab_reachbou;
    public GameObject btn_Confirm;
    public List<UIPlayerTenbouChangeInfo> playerTenbouList = new List<UIPlayerTenbouChangeInfo>();

    private AgariUpdateInfo currentAgari;


    void Start(){
        UIEventListener.Get(btn_Confirm).onClick = OnClickConfirm;
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show( List<AgariUpdateInfo> agariList )
    {
        currentAgari = agariList[0];

        gameObject.SetActive(true);

        Show_Internel();
    }

    void Show_Internel()
    {
        lab_reachbou.text = "x" + currentAgari.reachBou.ToString();

        var tenbouInfos = currentAgari.tenbouChangeInfoList;
        EKaze nextKaze = currentAgari.manKaze;

        for( int i = 0; i < playerTenbouList.Count; i++ )
        {
            PlayerTenbouChangeInfo info = tenbouInfos.Find( ptci=> ptci.playerKaze == nextKaze );
            playerTenbouList[i].SetPointInfo( info.playerKaze, info.changed );
            nextKaze = nextKaze.Next();
        }
    }


    void OnClickConfirm(GameObject go)
    {
        
    }

}
