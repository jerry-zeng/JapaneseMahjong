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
            PlayerUI.CollectMahjongPai( tehaiList[i] );
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

        if(OwnerPlayer.IsAI == false)
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

        if(OwnerPlayer.IsAI == false)
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


    protected readonly static Vector3 SelectStatePosOffset = new Vector3(0f, 20f, 0f);

    protected List<MahjongPai> chiiPaiSelectList = new List<MahjongPai>();



    void OnClickMahjong()
    {
        int index = tehaiList.IndexOf( MahjongPai.current );

        index = OwnerPlayer.Tehai.getJyunTehaiCount() - 1; // Test: the last one.

        switch(PlayerAction.State)
        {
            case EActionState.Select_Agari:
            case EActionState.Select_Sutehai:
            {
                PlayerAction.Response = EResponse.SuteHai;
                PlayerAction.SutehaiIndex = index;

                EventManager.Get().SendEvent(UIEventType.HideMenuList);
                OwnerPlayer.OnPlayerInputFinished();
            }
            break;

            case EActionState.Select_Reach:
            {
                PlayerAction.Response = EResponse.Reach;
                PlayerAction.ReachSelectIndex = PlayerAction.ReachHaiIndexList.FindIndex(i => i == index);

                EventManager.Get().SendEvent(UIEventType.HideMenuList);
                OwnerPlayer.OnPlayerInputFinished();
            }
            break;

            case EActionState.Select_Kan:
            {
                Hai kanHai = new Hai( MahjongPai.current.ID );

                if( OwnerPlayer.Tehai.validKaKan( kanHai ) )
                {
                    PlayerAction.Response = EResponse.Kakan;
                }
                else
                {
                    PlayerAction.Response = EResponse.Ankan;
                }

                PlayerAction.KanSelectIndex = PlayerAction.TsumoKanHaiList.FindIndex(h=> h.ID == kanHai.ID);

                EventManager.Get().SendEvent(UIEventType.HideMenuList);
                OwnerPlayer.OnPlayerInputFinished();
            }
            break;

            case EActionState.Select_Chii:
            {
                MahjongPai curSelect = MahjongPai.current;

                if( chiiPaiSelectList.Contains(curSelect) )
                {
                    chiiPaiSelectList.Remove( curSelect );
                    curSelect.transform.localPosition -= SelectStatePosOffset;

                    // check to enable select other chii type pai.
                    List<int> enableIndexList = new List<int>();
                    for(int i = 0; i < PlayerAction.AllSarashiHais.Count; i++)
                    {
                        int chiiHaiIndex = OwnerPlayer.Tehai.getHaiIndex( PlayerAction.AllSarashiHais[i].ID );
                        enableIndexList.Add( chiiHaiIndex );
                    }

                    EnableInput( enableIndexList );
                }
                else
                {
                    chiiPaiSelectList.Add( curSelect );
                    curSelect.transform.localPosition += SelectStatePosOffset;

                    if( chiiPaiSelectList.Count >= 2 ) // confirm Chii.
                    {
                        chiiPaiSelectList.Sort( MahjongPaiCompare );

                        PlayerAction.SarashiHaiLeft.Sort( Tehai.Compare );
                        if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiLeft[0].ID &&
                           chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiLeft[1].ID)
                        {
                            PlayerAction.Response = EResponse.Chii_Left;
                            PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Left;
                            Debug.Log("Chii type is Chii_Left");
                        }

                        PlayerAction.SarashiHaiCenter.Sort( Tehai.Compare );
                        if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiCenter[0].ID &&
                           chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiCenter[1].ID)
                        {
                            PlayerAction.Response = EResponse.Chii_Center;
                            PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Center;
                            Debug.Log("Chii type is Chii_Center");
                        }

                        PlayerAction.SarashiHaiRight.Sort( Tehai.Compare );
                        if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiRight[0].ID &&
                           chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiRight[1].ID)
                        {
                            PlayerAction.Response = EResponse.Chii_Right;
                            PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Right;
                            Debug.Log("Chii type is Chii_Right");
                        }

                        EventManager.Get().SendEvent(UIEventType.HideMenuList);
                        OwnerPlayer.OnPlayerInputFinished();

                        chiiPaiSelectList.Clear();
                    }
                    else // check to disable select other chii type pai.
                    {
                        List<int> enableIndexList = new List<int>();

                        int curSelectID = chiiPaiSelectList[0].ID;
                        enableIndexList.Add( OwnerPlayer.Tehai.getHaiIndex( curSelectID ) );

                        if( PlayerAction.SarashiHaiLeft.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiLeft.Find(h => h.ID != curSelectID);
                            enableIndexList.Add( OwnerPlayer.Tehai.getHaiIndex( otherHai.ID ) );
                        }

                        if( PlayerAction.SarashiHaiCenter.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiCenter.Find(h => h.ID != curSelectID);
                            enableIndexList.Add( OwnerPlayer.Tehai.getHaiIndex( otherHai.ID ) );
                        }

                        if( PlayerAction.SarashiHaiRight.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiRight.Find(h => h.ID != curSelectID);
                            enableIndexList.Add( OwnerPlayer.Tehai.getHaiIndex( otherHai.ID ) );
                        }

                        EnableInput( enableIndexList );
                    }
                }
            }
            break;
        }
    }

    public static int MahjongPaiCompare(MahjongPai x, MahjongPai y)
    {
        return x.ID - y.ID;
    }

    public void DisableInput(bool updateColor = false)
    {
        for(int i = 0; i < tehaiList.Count; i++)
            tehaiList[i].DisableInput(updateColor);
    }
    public void EnableInput(bool updateColor = false)
    {
        for(int i = 0; i < tehaiList.Count; i++)
            tehaiList[i].EnableInput(updateColor);
    }


    public void DisableInput(List<int> indexList)
    {
        if(indexList == null)
            return;

        EnableInput(true);

        int index = 0;
        for( int i = 0; i < indexList.Count; i++ )
        {
            index = indexList[i];

            if( index >= 0 && index < indexList.Count )
                tehaiList[index].DisableInput(true);
        }
    }
    public void EnableInput(List<int> indexList)
    {
        if(indexList == null)
            return;

        DisableInput(true);

        int index = 0;
        for( int i = 0; i < indexList.Count; i++ )
        {
            index = indexList[i];

            if( index >= 0 && index < indexList.Count )
                tehaiList[index].EnableInput(true);
        }
    }
}
