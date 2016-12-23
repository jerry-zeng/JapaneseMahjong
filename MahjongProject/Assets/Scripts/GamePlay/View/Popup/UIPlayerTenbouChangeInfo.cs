using UnityEngine;
using System.Collections;


public class UIPlayerTenbouChangeInfo : MonoBehaviour
{
    public UILabel lab_kaze;
    public UILabel lab_current;
    public UILabel lab_change;


    public void SetInfo(EKaze kaze, int curTenbou, int changeValue)
    {
        lab_kaze.text = ResManager.getString( "kaze_" + kaze.ToString().ToLower() );

        lab_current.text = curTenbou.ToString();

        if( changeValue > 0 ){
            lab_change.color = Color.blue;
            lab_change.text = "+" + changeValue.ToString();
        }
        else if( changeValue < 0 ){
            lab_change.color = Color.red;
            lab_change.text = "" + changeValue.ToString();
        }
        else{
            lab_change.text = "";
        }
    }

}
