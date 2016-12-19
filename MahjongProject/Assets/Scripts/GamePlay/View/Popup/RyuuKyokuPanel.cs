using UnityEngine;
using System.Collections;


public class RyuuKyokuPanel : MonoBehaviour
{
    public UIButton btn_Confirm;
    public UILabel lab_msg;


    void Start(){

    }


    public void Show(string msg, EventDelegate.Callback onClick)
    {
        gameObject.SetActive(true);

        lab_msg.text = msg;

        btn_Confirm.SetOnClick( onClick );
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
