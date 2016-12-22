using UnityEngine;
using System.Collections;


public class UIYakuItem : MonoBehaviour
{
    public UILabel lab_name;
    public UILabel lab_han;


    public void SetYaku( string key, int han )
    {
        lab_name.text = ResManager.getString(key);
        lab_han.text = han.ToString() + ResManager.getString( "han" );
    }

    public void SetYakuMan( string key, bool doubleYakuman )
    {
        lab_name.text = ResManager.getString(key);

        if( doubleYakuman == true )
            lab_han.text = ResManager.getString("double") + ResManager.getString("yakuman");
        else
            lab_han.text = ResManager.getString("yakuman");
    }

}
