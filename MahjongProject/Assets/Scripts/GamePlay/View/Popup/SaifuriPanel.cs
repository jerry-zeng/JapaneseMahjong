using UnityEngine;
using System.Collections;


public class SaifuriPanel : MonoBehaviour
{
    public UIButton saisButton;
    public UILabel saiTip;


    void Start(){
        
    }

    public void Show(string tip, EventDelegate.Callback onClick)
    {
        gameObject.SetActive(true);
        saisButton.isEnabled = true;

        saiTip.text = tip;

        saisButton.SetOnClick( onClick );
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetResult(string result)
    {
        saiTip.text = result;

        saisButton.isEnabled = false;
    }
}
