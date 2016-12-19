using UnityEngine;
using System.Collections;


public class PlayerInputButton : UIButton
{
    public int index;

    protected UILabel lab_tag;
    public UILabel TagLabel
    {
        get{
            if(lab_tag == null){
                lab_tag = GetComponentInChildren<UILabel>();
                cacheTagName = TagLabel.text;
            }
            return lab_tag;
        }
    }

    protected string cacheTagName;


    public void SetTag( string tag )
    {
        TagLabel.text = tag;
    }

    public void ResetTag()
    {
        TagLabel.text = cacheTagName;
    }

    public void SetEnable(bool state)
    {
        isEnabled = state;

        TagLabel.color = state? Color.red : Color.gray;
    }

}
