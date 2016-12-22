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

    private UIPanel uiPanel;


    void Awake(){
        uiPanel = GetComponent<UIPanel>();
    }

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
        uiPanel.depth = depth + TehaiDepth;
    }


    public void SortTehai(Hai[] hais, float delay, bool setLastNew)
    {
        StartCoroutine(Sort(hais, delay, setLastNew));
    }
    protected IEnumerator Sort(Hai[] hais, float delay, bool setLastNew)
    {
        yield return new WaitForSeconds( delay );

        SetTehai( hais );
    }

    public void SetTehai(Hai[] hais, bool setLastNew = false) 
    {
        Clear();

        for( int i = 0; i < hais.Length; i++ ) 
        {
            AddPai( hais[i], setLastNew, !OwnerPlayer.IsAI );
        }
        //uiPanel.Update();
    }

    public void AddPai( Hai hai, bool newPicked = false, bool isShow = false )
    {
        MahjongPai pai = PlayerUI.CreateMahjongPai( transform, Vector3.zero, hai, isShow );

        AddPai(pai, newPicked, isShow);
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

            return pai;
        }
        return null;
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
        //uiPanel.Update();
    }


    protected readonly static Vector3 SelectStatePosOffset = new Vector3(0f, 20f, 0f);

    protected List<MahjongPai> chiiPaiSelectList = new List<MahjongPai>();



    void OnClickMahjong()
    {
        int index = tehaiList.IndexOf( MahjongPai.current );
        //Debug.Log("OnClick Mahjong " + index.ToString());
        //index = OwnerPlayer.Tehai.getJyunTehaiCount() - 1; // Test: the last one.

        switch(PlayerAction.State)
        {
            //case EActionState.Select_Agari:
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

                SetEnableStateColor(true);
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

                SetEnableStateColor(true);
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
                    Hai[] jyunTehais = OwnerPlayer.Tehai.getJyunTehai();

                    for(int i = 0; i < PlayerAction.AllSarashiHais.Count; i++)
                    {
                        for( int j = 0; j < jyunTehais.Length; j++){
                            if( jyunTehais[j].ID == PlayerAction.AllSarashiHais[i].ID )
                                enableIndexList.Add( j );
                        }
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

                        if( PlayerAction.SarashiHaiRight.Count >= 2 )
                        {
                            PlayerAction.SarashiHaiRight.Sort( Tehai.Compare );
                            if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiRight[0].ID &&
                               chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiRight[1].ID)
                            {
                                PlayerAction.Response = EResponse.Chii_Right;
                                PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Right;
                                Debug.Log("Chii type is Chii_Right");
                            }
                        }

                        if( PlayerAction.SarashiHaiCenter.Count >= 2 )
                        {
                            PlayerAction.SarashiHaiCenter.Sort( Tehai.Compare );
                            if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiCenter[0].ID &&
                               chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiCenter[1].ID)
                            {
                                PlayerAction.Response = EResponse.Chii_Center;
                                PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Center;
                                Debug.Log("Chii type is Chii_Center");
                            }
                        }

                        if( PlayerAction.SarashiHaiLeft.Count >= 2 )
                        {
                            PlayerAction.SarashiHaiLeft.Sort( Tehai.Compare );
                            if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiLeft[0].ID &&
                               chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiLeft[1].ID)
                            {
                                PlayerAction.Response = EResponse.Chii_Left;
                                PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Left;
                                Debug.Log("Chii type is Chii_Left");
                            }
                        }

                        EventManager.Get().SendEvent(UIEventType.HideMenuList);
                        OwnerPlayer.OnPlayerInputFinished();

                        chiiPaiSelectList.Clear();
                    }
                    else // check to disable select other chii type pai.
                    {
                        List<int> enableIndexList = new List<int>();
                        Hai[] jyunTehais = OwnerPlayer.Tehai.getJyunTehai();

                        int curSelectID = chiiPaiSelectList[0].ID;
                        enableIndexList.Add( index );

                        if( PlayerAction.SarashiHaiRight.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiRight.Find(h => h.ID != curSelectID);

                            for( int i = 0; i < jyunTehais.Length; i++ ){
                                if( jyunTehais[i].ID == otherHai.ID && !enableIndexList.Contains(i) )
                                    enableIndexList.Add( i );
                            }
                        }

                        if( PlayerAction.SarashiHaiCenter.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiCenter.Find(h => h.ID != curSelectID);

                            for( int i = 0; i < jyunTehais.Length; i++ ){
                                if( jyunTehais[i].ID == otherHai.ID && !enableIndexList.Contains(i) )
                                    enableIndexList.Add( i );
                            }
                        }

                        if( PlayerAction.SarashiHaiLeft.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiLeft.Find(h => h.ID != curSelectID);

                            for( int i = 0; i < jyunTehais.Length; i++ ){
                                if( jyunTehais[i].ID == otherHai.ID && !enableIndexList.Contains(i) )
                                    enableIndexList.Add( i );
                            }
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

            if( index >= 0 && index < tehaiList.Count )
                tehaiList[index].EnableInput(true);
        }
    }

    public void SetEnableStateColor(bool state)
    {
        for(int i = 0; i < tehaiList.Count; i++)
            tehaiList[i].SetEnableStateColor( state );
    }
}
