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
public class MahjongPai : UIObject 
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


    public Color normalColor = Color.white;
    public Color redDoraColor = Color.red;
    public Color nakiColor = new Color(0.7f, 0.7f, 0.7f);
    public Color disableColor = new Color(0.6f, 0.6f, 0.6f);


    protected BoxCollider boxCollider;
    protected UISprite background;
    protected UISprite majSprite;


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
        if(state){
            background.color = normalColor;
        }
        else{
            background.color = disableColor;
        }
    }


    public override void Init() 
    {
        base.Init();

        if( isInit == false )
        {
            background = GetComponent<UISprite>();
            majSprite = transform.Find("sprite").GetComponent<UISprite>();
            boxCollider = GetComponent<BoxCollider>();

            isInit = true;
        }

        SetOrientation(EOrientation.Portrait);
        SetRedDora(false);
        Hide();

        DisableInput();
    }

    public override void Clear()
    {
        base.Clear();

        SetRedDora(false);
        SetTedashi(false);
        SetNaki(false);
        SetReach(false);
        setShining(false);

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
            majSprite.color = redDoraColor;
        }
        else {
            majSprite.color = normalColor;
        }
    }

    public void SetTedashi(bool state)
    {
        this.isTedashi = state;


    }

    public void SetNaki(bool state)
    {
        this.isNaki = state;

        if(isNaki){
            background.color = nakiColor;
        }
        else{
            background.color = normalColor;
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

    // shining on menu list shown out.
    public void setShining(bool isShining)
    {
        if( isShining ) {
            TweenAlpha tweener = TweenAlpha.Begin( background.gameObject, 1f, 0.5f );
            tweener.style = UITweener.Style.PingPong;
            tweener.method = UITweener.Method.EaseInOut;
        }
        else {
            TweenAlpha tweener = GetComponent<TweenAlpha>();
            if( tweener != null )
                TweenAlpha.Begin( background.gameObject, 0f, 1f );
        }
    }

    public void Show() {
        SetFrontBack(EFrontBack.Front);
    }
    public void Hide() {
        SetFrontBack(EFrontBack.Back);
    }

    protected void SetFrontBack(EFrontBack fb)
    {
        curFrontBack = fb;

        if( fb == EFrontBack.Front ){
            background.spriteName = "mj_bg";
            majSprite.gameObject.SetActive(true);
        }
        else{
            background.spriteName = "mj_bg_back";
            majSprite.gameObject.SetActive(false);
        }

        // mark as changed. don't know why UIPanel won't update.
        gameObject.SetActive(false);
        gameObject.SetActive(true);
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
