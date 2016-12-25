using UnityEngine;
using System.Collections;


public class KyokuInfoPanel : MonoBehaviour 
{
    public UIPanel uiPanel;
    public UILabel lab_kyoku;
    public UILabel lab_honba;


    void Start()
    {
        if(uiPanel == null)
            uiPanel = GetComponent<UIPanel>();
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show( string kyokuStr, string honbaStr )
    {
        gameObject.SetActive(true);

        uiPanel.alpha = 0f;
        lab_kyoku.text = kyokuStr;
        lab_honba.text = honbaStr;

        if( string.IsNullOrEmpty(honbaStr) ){
            lab_kyoku.pivot = UIWidget.Pivot.Center;
            lab_kyoku.transform.localPosition = new Vector3(0f, -5f, 0f);
        }
        else{
            lab_kyoku.pivot = UIWidget.Pivot.Right;
            lab_kyoku.transform.localPosition = new Vector3(20f, -5f,0f);
        }

        StartCoroutine( Show_Internel() );
    }

    IEnumerator Show_Internel()
    {
        float Duration = 0.4f;

        TweenAlpha.Begin(gameObject, Duration, 1f).method = UITweener.Method.EaseIn;
        yield return new WaitForSeconds(Duration);

        // stay time.
        yield return new WaitForSeconds(1f);

        TweenAlpha.Begin(gameObject, Duration, 0f).method = UITweener.Method.EaseOut;
        yield return new WaitForSeconds(Duration);

        yield return new WaitForSeconds(Duration);
        OnEnd();
    }

    void OnEnd()
    {
        Hide();

        EventManager.Get().SendEvent(UIEventType.On_UIAnim_End);
    }
}
