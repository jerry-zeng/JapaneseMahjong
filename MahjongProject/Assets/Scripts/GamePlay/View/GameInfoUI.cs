using UnityEngine;
using System.Collections;


public class GameInfoUI : UIObject 
{
    private UILabel kyokuLab;
    private UILabel reachCountLab;
    private UISprite reachBan;
    private UILabel lab_remain;


    void Start () {
        Init();
    }

    public override void Init() {
        if(isInit == false){
            kyokuLab = transform.Find("Kyoku").GetComponent<UILabel>();
            reachCountLab = transform.Find("ReachCount").GetComponent<UILabel>();
            reachBan = transform.Find("ReachBan").GetComponent<UISprite>();
            lab_remain = transform.Find("lab_remain").GetComponent<UILabel>();

            isInit = true;
        }
    }

    public override void Clear()
    {
        base.Clear();

        kyokuLab.text = "";
        reachCountLab.text = "";
        lab_remain.text = "";
        reachBan.enabled = false;
    }

    public void SetKyoku( EKaze kaze, int kyoku ) {
        string kazeStr = ResManager.getString( "kaze_" + kaze.ToString().ToLower() );
        kyokuLab.text = kazeStr + " " + kyoku.ToString() + "局";
    }

    public void SetReachCount(int count) {
        reachCountLab.text = "x" + count.ToString();

        if(!reachBan.enabled)
            reachBan.enabled = true;
    }

    public void SetHonba(int honba) {
        Debug.Log( honba + "本场");
    }

    public void SetRemain(int remain)
    {
        lab_remain.text = "残: " + remain.ToString();
    }
}
