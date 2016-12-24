using UnityEngine;
using System.Collections;


public class SaifuriPanel : MonoBehaviour
{
    public UILabel lab_tip;
    public UILabel lab_num;

    private float EachAnimTime = 0.5f;


    private int num1;
    private int num2;

    private bool AnimEnd = true;

    bool startAnim1 = false;
    bool startAnim2 = false;
    float saifuriTime = 0f;
    int updateTick = 0;


    void Start(){
        
    }


    public void Hide()
    {
        AnimEnd = true;

        startAnim1 = false;
        startAnim2 = false;
        saifuriTime = 0f;
        updateTick = 0;

        gameObject.SetActive(false);
    }

    public void Show(int left, int right )
    {
        this.num1 = left;
        this.num2 = right;

        lab_num.text = "";

        gameObject.SetActive(true);

        AnimEnd = false;
        startAnim1 = true;

    }


    void Update()
    {
        if( AnimEnd == true ) return;

        if( startAnim1 == true ){
            saifuriTime += Time.deltaTime;
            updateTick++;

            if( lab_num.alpha < 1f )
                lab_num.alpha += Time.deltaTime * 3f;

            if( saifuriTime < EachAnimTime ){
                if( updateTick % 2 == 0 )
                    SetSaiString( GetRandomNum(), GetRandomNum() );                
            }
            else{
                startAnim1 = false;
                startAnim2 = true;
                saifuriTime = 0f;
            }
        }
        else if( startAnim2 == true ){
            saifuriTime += Time.deltaTime;
            updateTick++;

            if( saifuriTime < EachAnimTime ){
                if( updateTick % 2 == 0 )
                    SetSaiString( num1, GetRandomNum() );
            }
            else{
                startAnim2 = false;
                saifuriTime = 0f;

                SetSaiString( num1, num2 );
            }
        }
        else{
            saifuriTime += Time.deltaTime;

            if( saifuriTime >= 1f )
                OnEnd();
        }
    }

    void SetSaiString( int n1, int n2 )
    {
        lab_num.text = n1.ToString() + " , " + n2.ToString();
    }

    int GetRandomNum()
    {
        return Random.Range(1, 7);
    }

    void OnEnd()
    {
        AnimEnd = true;

        Hide();

        EventManager.Get().SendEvent(UIEventType.On_Select_Wareme_End);
    }
}
