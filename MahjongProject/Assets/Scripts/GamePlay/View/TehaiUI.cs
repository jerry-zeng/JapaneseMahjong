using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TehaiUI : UIObject 
{

    public Vector2 AlignLeftLocalPos = new Vector2(-380, 0);
    public int HaiPosOffsetX = 2;
    public int NewHaiPosOffsetX = 10;
    public int TehaiDepth = 6;

    private List<MahjongPai> tehaiList = new List<MahjongPai>();


    void Start () {

    }

    public override void Init() {

    }

    public override void Clear() {
        base.Clear();

        for( int i = 0; i < tehaiList.Count; i++ ) {
            ResManager.collectMahjongObject( tehaiList[i] );
        }
        tehaiList.Clear();
    }

    public override void SetParentPanelDepth( int depth ) {
        GetComponent<UIPanel>().depth = depth + TehaiDepth;
    }

    public void SetTehai(Hai[] hais) 
    {
        if(hais != null)
        {
            Clear(); // clear then create new.

            for( int i = 0; i < hais.Length; i++ ) 
            {
                AddHai( hais[i] );
            }
        }
    }

    public MahjongPai AddHai( Hai hai, int newIndex = -1, bool isShow = false ) {
        if( !Hai.IsValidHai( hai ) )
            return null;

        Transform parent = transform;

        int index = tehaiList.Count;
        float posX = AlignLeftLocalPos.x + MahjongPai.Width * index + HaiPosOffsetX * index;
        Vector3 localPos = new Vector3( posX, AlignLeftLocalPos.y, 0 );

        MahjongPai pai = PlayerUI.CreateMahjongPai( parent, localPos, hai, isShow );
        tehaiList.Add( pai );

        if( newIndex == index ) {
            pai.transform.localPosition += new Vector3( NewHaiPosOffsetX, 0, 0 );
        }

        return pai;
    }

    public void SetAllHaisVisiable(bool visiable) {
        for( int i = 0; i < tehaiList.Count; i++ ) {
            if( visiable ) {
                tehaiList[i].Show();
            }
            else {
                tehaiList[i].Hide();
            }
        }
    }

}
