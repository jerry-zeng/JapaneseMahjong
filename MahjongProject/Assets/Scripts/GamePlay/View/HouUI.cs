using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 河.
 */

public class HouUI : UIObject 
{

    public Vector2 AlignLeftLocalPos = new Vector2(-150, 0);
    public int HaiPosOffsetX = 2;

    public int Line_PosY_1 = 0;
    public int Line_PosY_2 = -65;
    public int Line_PosY_3 = -130;

    public const int Max_Lines = 3;
    public const int MaxCoutPerLine = 6;

    private List<Transform> lineParents;
    private List<MahjongPai> allHais = new List<MahjongPai>(Hou.SUTE_HAIS_LENGTH_MAX);


    // Use this for initialization
    void Start () {
        Init();
    }

    public override void Init() {
        base.Init();

        if(isInit == false){
            lineParents = new List<Transform>(Max_Lines);
            for( int i = 0; i < Max_Lines; i++ ) {
                Transform line = transform.FindChild("Line_" + (i+1));
                if(line != null){
                    lineParents.Add(line);
                }
            }

            isInit = true;
        }
    }

    public override void Clear() {
        base.Clear();

        // clear all hai.
        for( int i = 0; i < allHais.Count; i++ ) {
            ResManager.collectMahjongObject(allHais[i]);
        }
        allHais.Clear();
    }

    public override void SetParentPanelDepth( int depth ) {
        for( int i = 0; i < lineParents.Count; i++ ) {
            UIPanel panel = lineParents[i].GetComponent<UIPanel>();

            panel.depth = depth + (lineParents.Count - i);
        }
    }

    public void AddHai(Hai hai) 
    {
        if( !Hai.IsValidHai(hai) ){
            Debug.Log("Invalid hai for id == " + hai.ID);
            return;
        }

        int inLine = allHais.Count / MaxCoutPerLine;  //inLine=0,1,2. >2 has a small chance.
        int indexInLine = allHais.Count % MaxCoutPerLine;

        // set parent.
        int EndingLine = Max_Lines - 1;
        if( inLine > EndingLine ){
            indexInLine += (inLine - EndingLine) * 6;

            inLine = EndingLine;
        }

        Transform parent = lineParents[inLine];

        // set position.
        // TODO: didn't consider the reach hai position change.
        float posX = AlignLeftLocalPos.x + MahjongPai.Width * indexInLine + HaiPosOffsetX * indexInLine;
        Vector3 localPos = new Vector3(posX, 0, 0);

        MahjongPai pai = PlayerUI.CreateMahjongPai(parent, localPos, hai);

        allHais.Add(pai);
    }

    public bool SetReach(bool a_reach) {
        if( allHais.Count <= 0 ) {
            return false;
        }

        // set last hai reach.
        MahjongPai lastHai = allHais[allHais.Count - 1];
        if(a_reach == true){
            lastHai.SetOrientation(EOrientation.Landscape_Left);
            lastHai.transform.localPosition += new Vector3(0, MahjongPai.LandHaiPosOffsetY, 0);
        }          
        else
            lastHai.SetOrientation(EOrientation.Portrait);

        return true;
    }

    public bool setTedashi(bool a_tedashi) {
        if( allHais.Count <= 0 ) {
            return false;
        }

        // set last hai tedashi.
        MahjongPai lastHai = allHais[allHais.Count - 1];
        Debug.Log(lastHai.ID + " tedashi");
        //lastHai.SetOrientation(EOrientation.Landscape_Left);

        return true;
    }
}
