using UnityEngine;
using System.Collections;


public class GameInfoUI : UIObject 
{

    private UILabel kyokuLab;
    private UILabel reachCountLab;
    private UISprite reachBan;


    void Start () {
        Init();
    }

    public override void Init() {
        if(isInit == false){
            kyokuLab = transform.FindChild("Kyoku").GetComponent<UILabel>();
            reachCountLab = transform.FindChild("ReachCount").GetComponent<UILabel>();
            reachBan = transform.FindChild("ReachBan").GetComponent<UISprite>();

            isInit = true;
        }
    }

    public override void Clear()
    {
        base.Clear();

        kyokuLab.text = "";
        reachCountLab.text = "";
        reachBan.enabled = false;
    }

    public void SetKyoku( int kaze, int kyoku ) {
        kyokuLab.text = ((EKaze)kaze).ToString() + " " + kyoku.ToString();
    }

    public void SetReachCount(int count) {
        reachCountLab.text = count.ToString();

        if(!reachBan.enabled)
            reachBan.enabled = true;
    }

    public void SetHonba(int honba) {
        Debug.Log( honba + " 本场");
    }
}
