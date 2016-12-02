using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MahjongPaiComparer : IComparer<MahjongPai> 
{
    public int Compare(MahjongPai x, MahjongPai y) {
        if(x.ID > y.ID){
            return 1;
        }
        else if(x.ID < y.ID){
            return -1;
        }
        return 0;
    }
}

public class MahjongPai : UIObject 
{

    private int id = 0;
    private int kind = 1;
    private int num = 1;
    private bool isRed = false;

    public int ID {
        get {
            return id;
        }
    }


    private static int majWidth = 58;
    public static int Width {
        get {
            return majWidth;
        }
    }

    private static int majHeight = 84;
    public static int Height {
        get {
            return majHeight;
        }
    }

    public const int LandHaiPosOffsetY = -15; // 当麻将横着放时，往下移15像素. /


    private Transform front;
    private Transform back;
    private UISprite majSprite;

    EFrontBack curFrontBack = EFrontBack.Front;
    public bool IsShownOut {
        get {
            return curFrontBack == EFrontBack.Front;
        }
    }


    void Start () {

    }

    public override void Init() {
        base.Init();

        if(isInit == false){
            front = transform.FindChild("Front");
            back = transform.FindChild("Back");
            majSprite = front.FindChild("info").GetComponent<UISprite>();

            isInit = true;
        }

        SetOrientation(EOrientation.Portrait);
        Hide();
        SetRedDora(false);
    }

    public void SetInfo( int id, int kind, int num, bool isRed = false ) {
        this.id = id;
        this.kind = kind;
        this.num = num;
        this.isRed = isRed;
    }
    public void SetInfo(Hai hai) {
        this.id = hai.ID;
        this.kind = hai.Kind;
        this.num = hai.Num;
        this.isRed = hai.IsRed;
    }

    public void UpdateImage() {
        majSprite.spriteName = ResManager.getMahjongSpriteName(kind, num);
        SetRedDora(this.isRed);
    }

    public void SetRedDora(bool isRed) {
        if( isRed ) {
            majSprite.color = Color.red;
        }
        else {
            majSprite.color = Color.white;
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
}
