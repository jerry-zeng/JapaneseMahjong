using UnityEngine;
using System.Collections;


public class GameOverPanel : MonoBehaviour 
{
    public UIButton btn_Confirm;


    void Start(){
        btn_Confirm.SetOnClick( OnClickConfirm );
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnClickConfirm()
    {
        
    }

}
