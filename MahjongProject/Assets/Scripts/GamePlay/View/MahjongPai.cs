using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MahjongPaiComparer : IComparer<MahjongPai> 
{
    public int Compare(MahjongPai x, MahjongPai y)
    {
        return x.ID - y.ID;
    }
}

[RequireComponent(typeof(BoxCollider))]
public class MahjongPai : UIButtonColor 
{
    private Hai _data;
    public int ID
    {
        get { return _data == null? -1 : _data.ID; }
    }

    private static int _width = 58;
    public static int Width 
    {
        get { return _width; }
    }

    private static int _height = 84;
    public static int Height 
    {
        get { return _height; }
    }

    public const int LandHaiPosOffsetY = -15; // 当麻将横着放时，往下移15像素. /


    private Transform front;
    private Transform back;
    private UISprite majSprite;
    private BoxCollider boxCollider;

    EFrontBack curFrontBack = EFrontBack.Front;
    public bool IsShownOut 
    {
        get { return curFrontBack == EFrontBack.Front; }
    }


    public void DisableInput()
    {
        boxCollider.enabled = false;
        ResetDefaultColor();
    }
    public void EnableInput()
    {
        boxCollider.enabled = true;
    }

    public void Init() 
    {
        if( mInitDone == false )
        {
            OnInit();

            front = transform.FindChild("Front");
            back = transform.FindChild("Back");
            majSprite = front.FindChild("info").GetComponent<UISprite>();

            boxCollider = GetComponent<BoxCollider>();
        }

        SetOrientation(EOrientation.Portrait);
        SetRedDora(false);
        Hide();

        DisableInput();
    }

    public void SetInfo(Hai hai)
    {
        this._data = hai;
    }

    public void UpdateImage() {
        majSprite.spriteName = ResManager.getMahjongSpriteName(_data.Kind, _data.Num);
        SetRedDora(_data.IsRed);
    }

    public void SetRedDora(bool isRed) {
        if( isRed ) {
            majSprite.color = Color.red;
        }
        else {
            majSprite.color = Color.white;
        }
    }

    public void SetHighlight(bool isLight){
        UISprite bg = back.FindChild("background").GetComponent<UISprite>();
        if( isLight ) {
            bg.color = Color.magenta;
        }
        else {
            bg.color = Color.white;
        }
    }

    public void Show() {
        SetFrontBack(EFrontBack.Front);
    }
    public void Hide() {
        SetFrontBack(EFrontBack.Back);
    }

    private void SetFrontBack(EFrontBack fb) {
        front.gameObject.SetActive( fb == EFrontBack.Front );
        back.gameObject.SetActive( fb == EFrontBack.Back);

        curFrontBack = fb;
    }

    public void SetOrientation(EOrientation orien) {
        if( orien == EOrientation.Landscape_Left ){
            transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if( orien == EOrientation.Landscape_Right ) {
            transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else if( orien == EOrientation.Portrait_Down ) {
            transform.localEulerAngles = new Vector3(0, 0, -180);
        }
        else // if( orien == EOrientation.Portrait ) 
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }


    public static MahjongPai current = null;

    protected System.Action _onClick;
    protected void OnClick()
    {
        if(current == null && enabled)
        {
            current = this;
            if( _onClick != null ) _onClick();
            current = null;
        }
    }

    public void SetOnClick(System.Action onClick)
    {
        _onClick = onClick;
    }
}
