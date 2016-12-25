using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AgariPanel : MonoBehaviour
{
    public UILabel lab_player_kaze;
    public UILabel lab_kyoku;
    public TehaiUI tehai;
    public FuuroUI fuuro;

    public Transform omoteDoraRoot;
    public Transform uraDoraRoot;

    public Transform yakuRoot;
    public GameObject yakuItemPrefab;
    public Vector2 yakuItemPosOffset = new Vector2( 60f, -40f);
    public float yakuDisplayTime = 0.5f;

    public UILabel lab_han;
    public UILabel lab_point;
    public UILabel lab_level;

    public Transform tenbouInfoRoot;
    public List<UIPlayerTenbouChangeInfo> playerTenbouList = new List<UIPlayerTenbouChangeInfo>();
    public UILabel lab_reachbou;

    public GameObject btn_Continue;


    private const float haiOffset = 2f;
    private const int DoraHaisColumn = 5;

    private List<MahjongPai> _omoteDoraHais = new List<MahjongPai>();
    private List<MahjongPai> _uraDoraHais = new List<MahjongPai>();
    private List<UIYakuItem> _yakuItems = new List<UIYakuItem>();


    private List<AgariUpdateInfo> agariInfoList;
    private AgariUpdateInfo currentAgari;

    #region Init
    void Start()
    {
        yakuItemPrefab.SetActive(false);

        InitYakuInfo();

        UIEventListener.Get(btn_Continue).onClick = OnClickContinue;

        UILabel btnTag = btn_Continue.GetComponentInChildren<UILabel>(true);
        btnTag.text = ResManager.getString("continue");

        HideButtons();
    }


    void HideButtons()
    {
        btn_Continue.GetComponent<BoxCollider>().enabled = false;
        btn_Continue.gameObject.SetActive(false);
    }

    void InitYakuInfo()
    {
        lab_player_kaze.text = "";
        lab_kyoku.text = "";
        lab_reachbou.text = "";

        ClearYakuItemList( _yakuItems );

        lab_han.text = "";
        lab_point.text = "";
        lab_level.text = "";
        lab_level.alpha = 0f;
    }


    void InitDoraHais(Hai[] allOmoteDoras, Hai[] allUraDoras)
    {
        if( _omoteDoraHais != null ){
            ClearMahjongList( _omoteDoraHais );

            for( int i = 0; i < DoraHaisColumn; i++ )
            {
                Vector3 pos = new Vector3( -i * (MahjongPai.Width+haiOffset), 0f, 0f );
                Hai hai = allOmoteDoras[i];

                MahjongPai pai = PlayerUI.CreateMahjongPai(omoteDoraRoot, pos, hai, false);
                _omoteDoraHais.Add( pai );
            }
        }

        if( _uraDoraHais != null ){
            ClearMahjongList( _uraDoraHais );

            for( int i = 0; i < DoraHaisColumn; i++ )
            {
                Vector3 pos = new Vector3( -i * (MahjongPai.Width+haiOffset), 0f, 0f );
                Hai hai = allUraDoras[i];

                MahjongPai pai = PlayerUI.CreateMahjongPai(uraDoraRoot, pos, hai, false);
                _uraDoraHais.Add( pai );
            }
        }
    }

    void ClearMahjongList( List<MahjongPai> list )
    {
        if( list == null )
            return;

        for( int i = 0; i < list.Count; i++ )
        {
            PlayerUI.CollectMahjongPai(list[i]);
        }
        list.Clear();
    }

    void ClearYakuItemList( List<UIYakuItem> list )
    {
        if( list == null )
            return;

        for( int i = 0; i < list.Count; i++ )
        {
            GameObject.Destroy( list[i].gameObject );
        }
        list.Clear();
    }

    void SetTenbouInfo( bool state )
    {
        tenbouInfoRoot.gameObject.SetActive( state );
    }


    public void ShowOmoteDora(int count)
    {
        for( int i = 0; i < _omoteDoraHais.Count; i++ )
        {
            if( i < count )
                _omoteDoraHais[i].Show();
            else
                _omoteDoraHais[i].Hide();
        }
    }

    public void ShowUraDora(int count)
    {
        for( int i = 0; i < _uraDoraHais.Count; i++ )
        {
            if( i < count )
                _uraDoraHais[i].Show();
            else
                _uraDoraHais[i].Hide();
        }
            
    }
    #endregion


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show( List<AgariUpdateInfo> agariList )
    {
        HideButtons();
        gameObject.SetActive(true);

        agariInfoList = agariList;

        StartCoroutine( Show_Internel() );
    }

    private IEnumerator Show_Internel()
    {
        for( int i = 0; i < agariInfoList.Count; i++ )
        {
            currentAgari = agariInfoList[i];

            InitYakuInfo();
            SetTenbouInfo(false);

            InitDoraHais( currentAgari.allOmoteDoraHais, currentAgari.allUraDoraHais );
            ShowOmoteDora( currentAgari.openedOmoteDoraCount );
            ShowUraDora( currentAgari.openedUraDoraCount );

            Player player = currentAgari.agariPlayer;
            tehai.BindPlayer(player);
            fuuro.BindPlayer(player);

            UpdateKyokuInfo();

            Fuuro[] fuuros = player.Tehai.getFuuros();
            fuuro.UpdateFuuro( fuuros );

            Hai addHai = currentAgari.agariHai;

            Hai[] hais = player.Tehai.getJyunTehai();
            tehai.SetTehai( hais, false );
            tehai.AddPai( addHai, true, true );
            tehai.SetAllHaisVisiable( true );

            float tehaiPosOffsetX = 0; // move to left if Fuuro has too many DaiMinKan or AnKan.

            // TODO: it's not fit the UI on 4 fuuros and over 3 KaKan or DaiMinKan
            // move tehai position or scale down fuuros
            for( int f = 0; f < fuuros.Length; f++ )
            {
                switch( fuuros[i].Type )
                {
                    case EFuuroType.MinKou:
                    case EFuuroType.MinShun:
                        tehaiPosOffsetX += (MahjongPai.Height - MahjongPai.Width);
                    break;
                    case EFuuroType.AnKan:
                        tehaiPosOffsetX += MahjongPai.Width * 0.5f;
                    break;
                    case EFuuroType.DaiMinKan:
                    case EFuuroType.KaKan:
                        tehaiPosOffsetX += (MahjongPai.Height - MahjongPai.Width) + MahjongPai.Width * 0.5f;
                    break;
                }
            }

            Vector3 pos = tehai.transform.localPosition;
            pos.x = -tehaiPosOffsetX;
            tehai.transform.localPosition = pos;

            yield return StartCoroutine( ShowYakuOneByOne() );

            yield return new WaitForSeconds(3f);
        }

        ShowSkipButton();
        currentAgari = null;
    }


    void UpdateKyokuInfo()
    {
        lab_player_kaze.text = "Player: " + ResManager.getString( "kaze_" + currentAgari.agariPlayer.JiKaze.ToString().ToLower() );

        string kyokuStr = "";
        string honbaStr = "";

        if( currentAgari.isLastKyoku )
        {
            kyokuStr = ResManager.getString("info_end");
        }
        else
        {
            string kazeStr = ResManager.getString( "kaze_" + currentAgari.bakaze.ToString().ToLower() );
            kyokuStr = kazeStr + currentAgari.kyoku.ToString() + ResManager.getString("kyoku");
        }

        if( currentAgari.honba > 0 )
            honbaStr = currentAgari.honba.ToString() + ResManager.getString("honba");

        lab_kyoku.text = kyokuStr + "  " + honbaStr;
    }

    IEnumerator ShowYakuOneByOne()
    {
        yield return new WaitForSeconds(1.0f);

        var yakuArr = currentAgari.hanteiYakus;

        for( int i = 0; i < yakuArr.Length; i++ )
        {
            var yaku = yakuArr[i];

            string yakuName = yaku.getYakuNameKey();

            UIYakuItem item;

            if( yaku.isYakuman() ){
                item = CreateYakuItem_Yakuman( yakuName, yaku.isDoubleYakuman() );
            }
            else{
                item = CreateYakuItem( yakuName, yaku.getHanSuu() );
            }

            item.transform.parent = yakuRoot;
            item.transform.localScale = yakuItemPrefab.transform.localScale;
            item.transform.localPosition = new Vector3( yakuItemPosOffset.x, yakuItemPosOffset.y * (i+1), 0f );

            _yakuItems.Add( item );

            AudioManager.Get().PlaySFX( AudioConfig.GetSEPath(ESeType.Yaku) );

            yield return new WaitForSeconds( yakuDisplayTime );
        }

        yield return new WaitForSeconds( yakuDisplayTime * 0.5f );

        ShowTotalScrote();
    }

    void ShowTotalScrote()
    {
        int yakumanCount = 0;

        var yakuArr = currentAgari.hanteiYakus;

        for( int i = 0; i < yakuArr.Length; i++ )
        {
            var yaku = yakuArr[i];

            if( yaku.isDoubleYakuman() ){
                yakumanCount += 2;
            }
            else if( yaku.isYakuman() ){
                yakumanCount += 1;
            }
        }


        bool isOya = currentAgari.agariPlayerIsOya;
        int point = currentAgari.agariScore;

        if( yakumanCount > 0 ){
            SetYakuman();

            //int yakumanScore = isOya? currentAgari.scoreInfo.oyaRon : currentAgari.scoreInfo.oyaRon;

            SetPoint( point );
        }
        else
        {
            int han = currentAgari.han;
            int fu = currentAgari.fu;

            int level = 0;

            if( han < 5 ){
                if( point >= 12000 )
                    level = 1;
                else if( !isOya && point >= 8000 )
                    level = 1;
                else
                    level = 0;
            }
            else if( han < 6 ){ //5     满贯.
                level = 1;
            }
            else if( han < 8 ){ //6-7   跳满
                level = 2;
            }
            else if( han < 11 ){ //9-10 倍满.
                level = 3;
            }
            else if( han < 13 ){ //11-12 三倍满.
                level = 4;
            }
            else{                     //13 役满.
                level = 5;
            }

            SetHan( han, fu, level );
            SetPoint( point );
        }

        StartCoroutine( ShowTenbouInfo() );
    }

    void SetHan( int han, int fu, int level )
    {
        if( level != 0 ){
            lab_level.alpha = 1f;
            lab_level.text = ResManager.getString( GetYakuLevelNameKey(level) );
        }
        else{
            lab_level.text = "";
            lab_level.alpha = 0f;
        }

        string oyaStr = ResManager.getString( currentAgari.agariPlayerIsOya ? "parent" : "child" );
        lab_han.text = oyaStr + "    " + string.Format("{0}{1}    {2}{3}", 
                                     fu, ResManager.getString("fu"),
                                     han, ResManager.getString("han"));

        PlayLevelVoice(level);
    }

    void SetYakuman()
    {
        lab_han.text = "";

        lab_level.alpha = 1f;
        lab_level.text = ResManager.getString( GetYakuLevelNameKey(5) );

        PlayLevelVoice(5);
    }

    void SetPoint( int point )
    {
        lab_point.text = point.ToString() + ResManager.getString("ten");
    }

    string GetYakuLevelNameKey(int level)
    {
        switch( level )
        {
            default:
            case 0: return "";

            case 1: return "mangan";
            case 2: return "haneman";
            case 3: return "baiman";
            case 4: return "sanbaiman";
            case 5: return "yakuman";
        }
    }

    void PlayLevelVoice(int level)
    {
        ECvType cv = ECvType.ManGan;

        switch( level )
        {
            default:
            case 0: return;

            case 1: cv = ECvType.ManGan; break;
            case 2: cv = ECvType.HaReMan; break;
            case 3: cv = ECvType.BaiMan; break;
            case 4: cv = ECvType.SanBaiMan; break;
            case 5: cv = ECvType.YakuMan; break;
        }

        string cvPath = AudioConfig.GetCVPath(currentAgari.agariPlayer.VoiceType, cv);
        AudioManager.Get().PlaySFX( cvPath );
    }

    UIYakuItem CreateYakuItem( string yakuNameKey, int han )
    {
        GameObject item = Instantiate( yakuItemPrefab ) as GameObject;
        item.SetActive( true );
        UIYakuItem comp = item.GetComponent<UIYakuItem>();
        comp.SetYaku( yakuNameKey, han );

        return comp;
    }
    UIYakuItem CreateYakuItem_Yakuman( string yakuNameKey, bool doubleYakuman )
    {
        GameObject item = Instantiate( yakuItemPrefab ) as GameObject;
        item.SetActive( true );
        UIYakuItem comp = item.GetComponent<UIYakuItem>();
        comp.SetYakuMan( yakuNameKey, doubleYakuman );

        return comp;
    }


    IEnumerator ShowTenbouInfo()
    {
        yield return new WaitForSeconds(1f);

        lab_reachbou.text = "x" + currentAgari.reachBou.ToString();

        SetTenbouInfo(true);


        var tenbouInfos = currentAgari.tenbouChangeInfoList;
        EKaze nextKaze = currentAgari.manKaze;

        for( int i = 0; i < playerTenbouList.Count; i++ )
        {
            PlayerTenbouChangeInfo info = tenbouInfos.Find( ptci=> ptci.playerKaze == nextKaze );
            playerTenbouList[i].SetInfo( info.playerKaze, info.current, info.changed );
            nextKaze = nextKaze.Next();
        }

    }

    void ShowSkipButton()
    {
        btn_Continue.SetActive(true);
        btn_Continue.GetComponent<UIWidget>().alpha = 0f;
        TweenAlpha.Begin( btn_Continue.gameObject, 0.5f, 1f ).SetOnFinished( () =>
        {
            btn_Continue.GetComponent<BoxCollider>().enabled = true;
        } );
    }


    void OnClickContinue(GameObject go)
    {
        Hide();

        EventManager.Get().SendEvent(UIEventType.End_Kyoku);
    }

}
