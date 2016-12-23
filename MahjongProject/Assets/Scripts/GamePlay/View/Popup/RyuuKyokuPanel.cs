using UnityEngine;
using System.Collections;


public class RyuuKyokuPanel : MonoBehaviour
{
    public UIButton btn_Confirm;
    public UILabel lab_msg;


    void Start(){
        btn_Confirm.SetOnClick(OnConfirm);
    }


    public void Show(string msg)
    {
        gameObject.SetActive(true);

        lab_msg.text = msg;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnConfirm()
    {
        EventManager.Get().SendEvent(UIEventType.End_RyuuKyoku);
    }
}
