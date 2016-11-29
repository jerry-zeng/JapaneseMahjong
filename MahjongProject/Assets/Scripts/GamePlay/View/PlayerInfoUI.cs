using UnityEngine;
using System.Collections;


public class PlayerInfoUI : UIObject 
{

    private UILabel kazeLab;
    private UILabel pointLab;
    private UISprite reachBan;
    private GameObject oyaObj;

    Color initColor;

    // Use this for initialization
    void Start () {
        Init();
    }

    public override void Init() {
        if(isInit == false){
            kazeLab = transform.Find("Kaze").GetComponent<UILabel>();
            pointLab = transform.Find("Point").GetComponent<UILabel>();
            reachBan = transform.Find("ReachBan").GetComponent<UISprite>();
            initColor = kazeLab.color;

            oyaObj = transform.Find( "Oya" ).gameObject;

            isInit = true;
        }
    }

    public void SetKaze(EKaze kaze) {
        string str = kaze.ToString();

        kazeLab.text = str.Substring(0,1);
    }
    public void SetOyaKaze(bool isOya) {
        if( isOya ) {
            kazeLab.color = Color.red;
        }
        else {
            kazeLab.color = initColor;
        }
        oyaObj.SetActive(isOya);
    }

    public void SetTenbou(int point) {
        pointLab.text = point.ToString();
    }

    public void SetReach(bool isReach) {
        reachBan.enabled = isReach;
    }

    public override void Clear() {
        kazeLab.text = "";
        pointLab.text = "";
        reachBan.enabled = false;

        oyaObj.SetActive(false);
    }
}
