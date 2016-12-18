using UnityEngine;
using System.Collections;


public class PlayerInputButton : UIButton
{
    public int index;

    protected UILabel lab_tag;

    protected override void OnInit()
    {
        base.OnInit();

        if(lab_tag == null)
            lab_tag = GetComponentInChildren<UILabel>();
    }

    public void SetTag( string tag )
    {
        lab_tag.text = tag;
    }

    public void SetEnable(bool state)
    {
        isEnabled = state;

        lab_tag.color = state? Color.red : Color.gray;
    }

}
