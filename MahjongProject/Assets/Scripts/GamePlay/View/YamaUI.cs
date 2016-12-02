using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Yama array is like:
///   ->
///  |  |
///   <-
///  so align right.
///  
///  Wareme is like:
///               Doras     Rinshan
///            -        -    -   -
/// <--top    |8|..x3..|0|  |2| |0|  <--
///            -        -    -   -
///            -        -    -   -
/// <--botom  |9|..x3..|1|  |3| |1|  <--
///            -        -    -   -
///             total x5       x2
/// </summary>

public class YamaUI : UIObject 
{

    public const int MaxYamaPairInPlayer = 17;

    public Vector2 AlignRightLocalPos = new Vector2(520, 0);
    public int TopY = 0;
    public int BottomY = -30;
    public int TopDepth = 5;
    public int BottomDepth = 4;

    public const int MaxLines = 2;

    private int yama_start = -1;
    private int yama_end = -1;

    public int Yama_Start {
        get {
            return yama_start;
        }
    }
    public int Yama_End {
        get {
            return yama_end;
        }
    }

    private Transform top;
    private Transform bottom;

    private Dictionary<int, MahjongPai> mahjongYama = new Dictionary<int, MahjongPai>();


    // Use this for initialization
    void Start () {
        Init();
    }

    public override void Init() {
        base.Init();

        if(isInit == false){
            top = transform.FindChild("Top");
            bottom = transform.FindChild("Bottom");

            isInit = true;
        }
    }

    public override void Clear() {
        base.Clear();

        foreach( var kv in mahjongYama ) {
            ResManager.collectMahjongObject(kv.Value);
        }
        mahjongYama.Clear();
    }

    public override void SetParentPanelDepth( int depth ) {
        top.GetComponent<UIPanel>().depth = depth + TopDepth;
        bottom.GetComponent<UIPanel>().depth = depth + BottomDepth;
    }

    public void SetYamaIndex(int start, int end) {
        this.yama_start = start;
        this.yama_end = end;
    }

    public void SetYamaHais(Dictionary<int, Hai> yamaHais, int start, int end) {
        if( yamaHais == null )
            return;

        Clear();

        SetYamaIndex(start, end);

        foreach(var kv in yamaHais)
        {
            int index = kv.Key;
            Hai hai = kv.Value;

            AddHai(hai, index);
        }
    }

    MahjongPai AddHai( Hai hai, int index ) {
        if( !Hai.IsValidHai(hai) ) {
            return null;
        }

        int line = Mathf.Max( 0, (index - this.yama_start) % MaxLines );
        int indexInLine = Mathf.Max( 0, (index - this.yama_start) / MaxLines );

        Transform parent = top;
        if( line == 0 ) {
            parent = top;
        }
        else {
            parent = bottom;
        }

        // set position. align right.
        float posX = AlignRightLocalPos.x - MahjongPai.Width * indexInLine;
        Vector3 localPos = new Vector3( posX, 0, 0 );

        MahjongPai pai = PlayerUI.CreateMahjongPai( parent, localPos, hai, false );
        mahjongYama.Add( index, pai );

        return pai;
    }


    public void PickUp( int index ) {
        if( mahjongYama.ContainsKey( index ) ) {
            ResManager.collectMahjongObject( mahjongYama[index] );
        }
        else {
            Debug.LogError("No such mahjong with index == " + index);
        }
    }

    public void ShowHai( int index ) {
        if( mahjongYama.ContainsKey(index) ) {
            mahjongYama[index].Show();
        }
    }
    public void HideHai(int index) {
        if( mahjongYama.ContainsKey(index) ) {
            mahjongYama[index].Hide();
        }
    }

    public void SetWareme() { 

    }

    public void ShowAllYamaHais() {
        foreach(var kv in mahjongYama){
            kv.Value.Show();
        }
    }
    public void HideAllYamaHais() {
        foreach( var kv in mahjongYama ) {
            kv.Value.Hide();
        }
    }
}
