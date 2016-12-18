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
        Clear(); // clear then create new.

        if(hais != null)
        {
            for( int i = 0; i < hais.Length; i++ ) 
            {
                AddPai( hais[i] );
            }
        }
    }

    public void AddPai( Hai hai, bool newPicked = false, bool isShow = false )
    {
        Transform parent = transform;

        int index = tehaiList.Count;
        float posX = AlignLeftLocalPos.x + MahjongPai.Width * index + HaiPosOffsetX * index;
        Vector3 localPos = new Vector3( posX, AlignLeftLocalPos.y, 0 );

        MahjongPai pai = PlayerUI.CreateMahjongPai( parent, localPos, hai, isShow );

        if(_ownerPlayer.IsAI == false)
            pai.SetOnClick(OnClickMahjong);

        pai.gameObject.name = hai.ID.ToString();
        tehaiList.Add( pai );

        if( newPicked ) 
            pai.transform.localPosition += new Vector3( NewHaiPosOffsetX, 0, 0 );
    }

    public void AddPai( MahjongPai pai, bool newPicked = false, bool isShow = false )
    {
        int index = tehaiList.Count;
        float posX = AlignLeftLocalPos.x + MahjongPai.Width * index + HaiPosOffsetX * index;

        pai.transform.parent = transform;
        pai.transform.localPosition = new Vector3( posX, AlignLeftLocalPos.y, 0 );

        if( isShow ) {
            pai.Show();
        }
        else {
            pai.Hide();
        }

        if(_ownerPlayer.IsAI == false)
            pai.SetOnClick(OnClickMahjong);

        pai.gameObject.name = pai.ID.ToString();
        tehaiList.Add( pai );

        if( newPicked ) 
            pai.transform.localPosition += new Vector3( NewHaiPosOffsetX, 0, 0 );
    }

    public MahjongPai SuteHai( int index )
    {
        if( index >= 0 && index < tehaiList.Count )
        {
            MahjongPai pai = tehaiList[index];
            tehaiList.RemoveAt(index);

            pai.transform.parent = null;
            Sort();
            return pai;
        }
        return null;
    }

    // TODO
    protected void Sort()
    {
        
    }

    public void SetAllHaisVisiable(bool visiable) 
    {
        for( int i = 0; i < tehaiList.Count; i++ ) 
        {
            if( visiable ) {
                tehaiList[i].Show();
            }
            else {
                tehaiList[i].Hide();
            }
        }
    }

    void OnClickMahjong()
    {
        int index = tehaiList.IndexOf( MahjongPai.current );

        index = _ownerPlayer.Tehai.getJyunTehaiCount() - 1; // the last one.

        if( _ownerPlayer.Action.State == EActionState.Select_Sutehai )
        {
            _ownerPlayer.Action.Response = EResponse.SuteHai;
            _ownerPlayer.Action.SutehaiIndex = index;

            EventManager.Get().SendEvent(UIEventType.HideMenuList);
            _ownerPlayer.OnPlayerInputFinished();
        }
        else{
            
        }
    }

    public void DisableInput()
    {
        for(int i = 0; i < tehaiList.Count; i++)
            tehaiList[i].DisableInput();
    }
    public void EnableInput()
    {
        for(int i = 0; i < tehaiList.Count; i++)
            tehaiList[i].EnableInput();
    }
}
