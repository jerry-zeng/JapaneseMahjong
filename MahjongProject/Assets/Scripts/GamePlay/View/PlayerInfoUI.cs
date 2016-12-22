using UnityEngine;
using System.Collections;


public class PlayerInfoUI : UIObject 
{
    private UILabel lab_kaze;
    private UILabel lab_point;
    private UISprite reachBan;
    private GameObject oyaObj;

    Color initColor;

    // Use this for initialization
    void Start () {
        Init();
    }

    public override void Init() {
        if(isInit == false){
            lab_kaze = transform.Find("Kaze").GetComponent<UILabel>();
            lab_point = transform.Find("Point").GetComponent<UILabel>();
            reachBan = transform.Find("ReachBan").GetComponent<UISprite>();
            initColor = lab_kaze.color;

            oyaObj = transform.Find( "Oya" ).gameObject;

            isInit = true;
        }
    }

    public void SetKaze(EKaze kaze) {
        lab_kaze.text = ResManager.getString( "kaze_" + kaze.ToString().ToLower() );
    }

    public void SetOyaKaze(bool isOya) {
        if( isOya ) {
            lab_kaze.color = Color.red;
        }
        else {
            lab_kaze.color = initColor;
        }
        oyaObj.SetActive(isOya);
    }

    public void SetTenbou(int point) {
        lab_point.text = point.ToString();
    }

    public void SetReach(bool isReach) {
        reachBan.enabled = isReach;
    }

    public override void Clear() {
        lab_kaze.text = "";
        lab_point.text = "";
        reachBan.enabled = false;

        oyaObj.SetActive(false);
    }
}
