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
    protected Hai _data;
    public int ID
    {
        get { return _data == null? -1 : _data.ID; }
    }

    public bool isRed
    {
        get; private set;
    }
    public bool isTedashi
    {
        get; private set;
    }
    public bool isNaki
    {
        get; private set;
    }
    public bool isReach
    {
        get; private set;
    }

    public readonly static float Width  = 58f;
    public readonly static float Height = 84f;

    public const int LandHaiPosOffsetY = -15; // 当麻将横着放时，往下移15像素. /


    protected Transform front;
    protected Transform back;
    protected UISprite majSprite;
    protected BoxCollider boxCollider;

    protected EFrontBack curFrontBack = EFrontBack.Front;
    public bool IsShownOut 
    {
        get { return curFrontBack == EFrontBack.Front; }
    }


    public void DisableInput(bool updateColor = false)
    {
        boxCollider.enabled = false;

        if(updateColor) SetEnableStateColor(false);
    }
    public void EnableInput(bool updateColor = false)
    {
        boxCollider.enabled = true;

        if(updateColor) SetEnableStateColor(true);
    }

    public void SetEnableStateColor(bool state)
    {
        SetState(state? State.Disabled : State.Normal, true);
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

    public void Clear()
    {
        SetRedDora(false);
        SetTedashi(false);
        SetNaki(false);
        SetReach(false);

        Hide();

        DisableInput();
        SetEnableStateColor(true);

        SetOnClick(null);
        SetInfo(null);
    }

    public void SetInfo(Hai hai)
    {
        this._data = hai;
    }

    public void UpdateImage() {
        majSprite.spriteName = ResManager.getMahjongSpriteName(_data.Kind, _data.Num);
        SetRedDora(_data.IsRed);
    }

    public void SetRedDora(bool isRed)
    {
        this.isRed = isRed;

        if( isRed ) {
            majSprite.color = Color.red;
        }
        else {
            majSprite.color = Color.white;
        }
    }

    public void SetTedashi(bool state)
    {
        this.isTedashi = state;


    }

    public void SetNaki(bool state)
    {
        this.isNaki = state;

        UISprite fg = front.FindChild("background").GetComponent<UISprite>();
        if(isNaki){
            fg.color = new Color(0.7f, 0.7f, 0.7f);
        }
        else{
            fg.color = defaultColor;
        }
    }

    public void SetReach(bool state)
    {
        this.isReach = state;

        if(isReach == true){
            SetOrientation(EOrientation.Landscape_Left);
            transform.localPosition += new Vector3((Height-Width)*0.5f, MahjongPai.LandHaiPosOffsetY, 0);
        }
        else{
            SetOrientation(EOrientation.Portrait);
        }
    }

    public void SetHighlight(bool isLight)
    {
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

    protected void SetFrontBack(EFrontBack fb) {
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
    public void ClearOnClick()
    {
        _onClick = null;
    }
}
