using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 * Mahjong Table:
 *     P2
 * P3      P1
 *     P0
 * 
 * Yama Hai array is like:
 *    ->
 *  |    |
 *    <- 
 * 
 * index = 0 is in P0'right, index = Yama.YAMA_HAIS_MAX-1 is in P3'bottom
 */

public class MahjongView : UIObject, IObserver 
{
    // all yield trigger should be replaced with UIEventType.On_UIAnim_End callback.
    public const float NormalWaitTime = 0.5f;

    private const float KyokuInfoAnimationTime = 2.5f;
    private const float SuteHaiAnimationTime = 0.3f;
    private const float ReachAnimationTime = 0.4f;
    private const float NakiAnimationTime = 0.3f;
    private const float AgariAnimationTime = 0.5f;


    private Dictionary<int, PlayerUI> playerUIDict = new Dictionary<int, PlayerUI>();
    private Dictionary<EKaze, PlayerUI> playerUIDict_Kaze = new Dictionary<EKaze, PlayerUI>();
    private GameInfoUI gameInfo;

    public Transform mahjongPoolRoot;

    public PlayerInputPanel playerInputPanel;
    public SelectChiiChaPanel selectChiiChanPanel;
    public KyokuInfoPanel kyokuInfoPanel;
    public SaifuriPanel saifuriPanel;
    public RyuuKyokuPanel ryuuKyokuPanel;
    public AgariPanel agariPanel;
    public GameOverPanel gameOverPanel;

    protected EKaze shiningKaze = EKaze.Ton;
    protected bool hasShining = false;

    private MahjongMain Model
    {
        get { return GameManager.Get().LogicMain; }
    }


    void OnEnable() {
        EventManager.Get().addObserver(this);
    }
    void OnDisable() {
        EventManager.Get().removeObserver(this);
    }


    public void UIAnimWillEndAfter(float time)
    {
        Invoke( "OnUIAnimEnd", time ); // use StartCoroutine() instead if any errors appear.
    }
    void OnUIAnimEnd()
    {
        EventManager.Get().SendEvent(UIEventType.On_UIAnim_End);
    }


    public void HideAllHudPanel()
    {
        if(playerInputPanel != null)
            playerInputPanel.Hide();
        if(selectChiiChanPanel != null)
            selectChiiChanPanel.Hide();
        if(kyokuInfoPanel != null)
            kyokuInfoPanel.Hide();
        if(saifuriPanel != null)
            saifuriPanel.Hide();
        if(ryuuKyokuPanel != null)
            ryuuKyokuPanel.Hide();
        if(agariPanel != null)
            agariPanel.Hide();
        if(gameOverPanel != null)
            gameOverPanel.Hide();
    }

    public override void Clear() {
        base.Clear();

        if(gameInfo != null)
            gameInfo.Clear();

        foreach( var kv in playerUIDict )
            kv.Value.Clear();

        playerUIDict.Clear();
        playerUIDict_Kaze.Clear();

        isInit = false;
    }


    public override void Init() 
    {
        if( isInit == true )  return;

        string[] panelNames = new string[]{
            "South_Panel", "East_Panel", "North_Panel", "West_Panel"
        };
        float[] eulerAngles = new float[]{
            0f, 90f, 180f, -90f
        };

        // init player ui.
        for( int i = 0; i < GameSettings.PlayerCount; i++ ) 
        {
            string dir = panelNames[i];
            float eulerAngleZ = eulerAngles[i];

            Transform dirParent = transform.Find(dir);
            Transform uiTran = dirParent.Find("PlayerUI");
            if( uiTran == null ){
                uiTran = ResManager.CreatePlayerUIObject().transform;
                uiTran.parent = dirParent;
                uiTran.localScale = Vector3.one;
                uiTran.localEulerAngles = new Vector3(0, 0, eulerAngleZ);
            }

            PlayerUI ui = uiTran.GetComponent<PlayerUI>();
            ui.Init();

            UIPanel parentPanel = dirParent.GetComponent<UIPanel>();
            ui.SetParentPanelDepth( parentPanel.depth );

            playerUIDict.Add(i, ui);
            //Debug.LogWarningFormat("PlayerUI: {0} {1}", i, dir);
        }

        gameInfo = transform.Find("Info_Panel/GameInfo").GetComponent<GameInfoUI>();

        ResManager.SetPoolRoot( mahjongPoolRoot );

        isInit = true;

        hasShining = false;

        ResManager.LoadStringTable(true);
    }


    // handle ui event.
    public void OnHandleEvent(UIEventType evtID, object[] args) 
    {
        switch(evtID)
        {
            case UIEventType.Init_Game: // game init /
            {
                Clear();
                Init();
                HideAllHudPanel();
            }
            break;

            case UIEventType.Select_ChiiCha: 
            {
                selectChiiChanPanel.Show();
            }
            break;

            case UIEventType.Init_PlayerInfoUI: 
            {
                List<Player> players = Model.PlayerList;
                for( int i = 0; i < players.Count; i++ )
                {
                    Player player = players[i];
                    PlayerUI ui = playerUIDict[i];

                    ui.SetKaze( player.JiKaze );
                    ui.SetTenbou( player.Tenbou );
                    ui.Reach( false );

                    ui.SetOyaKaze( i == Model.OyaIndex );
                    ui.BindPlayer(player);

                    if(player.JiKaze == Model.getManKaze()){
                        playerInputPanel.BindPlayer( player );
                        playerInputPanel.SetOwnerPlayerUI(ui);
                    }
                }

            }
            break;

            case UIEventType.SetYama_BeforeHaipai: 
            {
                // Yama.
                Hai[] yamaHais = Model.Yama.getYamaHais();

                for( int i = 0; i < 4; i++ ) 
                {
                    Dictionary<int, Hai> haiDict = new Dictionary<int, Hai>();

                    int[] indexRange = getStartEndOfYamaUIOfPlayer( i );
                    for( int h = indexRange[0]; h <= indexRange[1]; h++ ) 
                    {
                        haiDict.Add( h, yamaHais[h] );
                    }

                    playerUIDict[i].SetYamaHais( haiDict, indexRange[0], indexRange[1] );
                }

                //TestYama();
            }
            break;

            case UIEventType.DisplayKyokuInfo:
            {
                string kyokuStr = (string)args[0];
                string honbaStr = (string)args[1];

                kyokuInfoPanel.Show( kyokuStr, honbaStr );
            }
            break;

            case UIEventType.Select_Wareme: 
            {
                Sai[] sais = Model.Saifuri();

                saifuriPanel.Show( sais[0].Num, sais[1].Num );
            }
            break;

            case UIEventType.SetUI_AfterHaipai: // 配牌 /
            {
                /// set game info.
                gameInfo.SetKyoku( Model.getBaKaze(), Model.Kyoku );
                gameInfo.SetReachCount( Model.ReachBou );
                gameInfo.SetHonba( Model.HonBa );
                gameInfo.SetRemain( Model.getTsumoRemainCount() );

                int totalPickHaiCount = (4*3 + 1) * Model.PlayerList.Count;

                /// set yama.
                int waremeIndex = Model.Wareme;
                int tsumoHaiStartIndex = waremeIndex + totalPickHaiCount;

                //Debug.Log(string.Format("remove yamahai in range[{0},{1}]", waremeIndex+1, tsumoHaiStartIndex % Yama.YAMA_HAIS_MAX));

                for( int i = waremeIndex+1; i <= tsumoHaiStartIndex; i++ ) 
                {
                    int index = i % Yama.YAMA_HAIS_MAX;
                    int p = findPlayerForYamahaiIndex(index);
                    MahjongPai pai = playerUIDict[p].PickUpYamaHai(index);
                    PlayerUI.CollectMahjongPai(pai);
                }

                /// set tehais.
                int PlayerCount = Model.PlayerList.Count;
                for( int i = 0; i < PlayerCount; i++ ) 
                {
                    Player player = Model.PlayerList[i];

                    PlayerUI ui = playerUIDict[i];

                    ui.SetTehai( player.Tehai.getJyunTehai() );
                    ui.SetTehaiVisiable( !player.IsAI );

                    playerUIDict_Kaze[player.JiKaze] = ui;
                }


                /// set init Dora.
                int showIndex = waremeIndex - 5;
                if( showIndex < 0 )
                    showIndex += Yama.YAMA_HAIS_MAX;

                int pi = findPlayerForYamahaiIndex(showIndex);
                playerUIDict[pi].ShowYamaHai(showIndex);

                // set Wareme.
                showIndex = waremeIndex-13;
                if( showIndex < 0 )
                    showIndex += Yama.YAMA_HAIS_MAX;
                pi = findPlayerForYamahaiIndex(showIndex);
                playerUIDict[pi].SetWareme(showIndex);
            }
            break;


            case UIEventType.DisplayMenuList:
            {
                // TODO: shouldn't use Model too frequently

                // if menu is for HandleSuteHai, set sute hai shining.
                if( Model.CurrentRequest == ERequest.Handle_SuteHai &&
                   Model.ActivePlayer.Action.MenuList.Count > 0 )
                {
                    if( hasShining == false ){
                        shiningKaze = Model.FromKaze;
                        playerUIDict_Kaze[shiningKaze].SetShining(true);
                        hasShining = true;
                    }
                }
                playerInputPanel.Show();
            }
            break;
            case UIEventType.HideMenuList:
            {
                playerInputPanel.Hide();

                // if menu is for HandleSuteHai, set sute hai not shining.
                if( hasShining ){
                    playerUIDict_Kaze[shiningKaze].SetShining(false);
                    hasShining = false;
                }
            }
            break;


            case UIEventType.PickTsumoHai:
            {
                gameInfo.SetRemain( Model.getTsumoRemainCount() );

                Player activePlayer = (Player)args[0];
                int lastPickIndex = (int)args[1];
                Hai newHai = (Hai)args[2];

                int yamaPlayerIndex = findPlayerForYamahaiIndex(lastPickIndex);
                MahjongPai pai = playerUIDict[yamaPlayerIndex].PickUpYamaHai(lastPickIndex);
                PlayerUI.CollectMahjongPai(pai);

                PlayerUI playerUI = playerUIDict_Kaze[activePlayer.JiKaze];
                playerUI.PickHai( newHai, true, !activePlayer.IsAI );

                SetManInputEnable( !activePlayer.IsAI && !activePlayer.IsReach );
            }
            break;

            case UIEventType.PickRinshanHai:
            {
                Player activePlayer = (Player)args[0];
                int lastPickRinshanIndex = (int)args[1];
                Hai newHai = (Hai)args[2];
                int newDoraHaiIndex = (int)args[3];

                int yamaPlayerIndex = findPlayerForYamahaiIndex(lastPickRinshanIndex);
                MahjongPai pai = playerUIDict[yamaPlayerIndex].PickUpYamaHai(lastPickRinshanIndex);
                PlayerUI.CollectMahjongPai(pai);

                PlayerUI playerUI = playerUIDict_Kaze[activePlayer.JiKaze];
                playerUI.PickHai( newHai, true, !activePlayer.IsAI );

                // open a omote dora hai
                int omoteDoraPlayerIndex = findPlayerForYamahaiIndex(newDoraHaiIndex);
                playerUIDict[omoteDoraPlayerIndex].ShowYamaHai(newDoraHaiIndex);

                SetManInputEnable( !activePlayer.IsAI && !activePlayer.IsReach );
            }
            break;

            case UIEventType.SuteHai:
            {
                UIAnimWillEndAfter( SuteHaiAnimationTime );

                Player activePlayer = (Player)args[0];
                int sutehaiIndex = (int)args[1];
                //Hai suteHai = (Hai)args[2];
                bool isTedashi = (bool)args[3];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.SuteHai(sutehaiIndex);
                ui.SetTedashi(isTedashi);

                // needn't sort hais if sute the last one.
                if( sutehaiIndex < activePlayer.Tehai.getJyunTehaiCount() )
                    ui.SortTehai( activePlayer.Tehai.getJyunTehai(), SuteHaiAnimationTime );

                SetManInputEnable(false);

                // play a sute hai sound
                AudioManager.Get().PlaySFX( AudioConfig.GetSEPath(ESeType.SuteHai) );
            }
            break;

            case UIEventType.Reach:
            {
                UIAnimWillEndAfter( ReachAnimationTime );

                Player activePlayer = (Player)args[0];
                int sutehaiIndex = (int)args[1];
                //Hai suteHai = (Hai)args[2];
                bool isTedashi = (bool)args[3];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.SuteHai(sutehaiIndex);
                ui.SetTedashi(isTedashi);
                ui.Reach();

                ui.Speak( ECvType.Reach );

                // needn't sort hais if sute the last one.
                if( sutehaiIndex < activePlayer.Tehai.getJyunTehaiCount() )
                    ui.SortTehai( activePlayer.Tehai.getJyunTehai(), SuteHaiAnimationTime );

                ui.Info.SetReach(true);
                ui.Info.SetTenbou( activePlayer.Tenbou );
                gameInfo.SetReachCount( Model.ReachBou );

                SetManInputEnable(false);
            }
            break;

            case UIEventType.Kakan:
            {
                UIAnimWillEndAfter( NakiAnimationTime );

                Player activePlayer = (Player)args[0];
                //Hai kakanHai = (Hai)args[1];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                ui.Speak( ECvType.Kan );

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.Ankan:
            {
                UIAnimWillEndAfter( NakiAnimationTime );

                Player activePlayer = (Player)args[0];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                ui.Speak( ECvType.Kan );

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.DaiMinKan:
            {
                UIAnimWillEndAfter( NakiAnimationTime );

                Player activePlayer = (Player)args[0];
                EKaze fromKaze = (EKaze)args[1];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                ui.Speak( ECvType.Kan );

                PlayerUI fromUI = playerUIDict_Kaze[fromKaze];
                fromUI.SetNaki();

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.Pon:
            {
                UIAnimWillEndAfter( NakiAnimationTime );

                Player activePlayer = (Player)args[0];
                EKaze fromKaze = (EKaze)args[1];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                ui.Speak( ECvType.Pon );

                PlayerUI fromUI = playerUIDict_Kaze[fromKaze];
                fromUI.SetNaki();

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.Chii_Left:
            case UIEventType.Chii_Center:
            case UIEventType.Chii_Right:
            {
                UIAnimWillEndAfter( NakiAnimationTime );

                Player activePlayer = (Player)args[0];
                EKaze fromKaze = (EKaze)args[1];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                ui.Speak( ECvType.Chii );

                PlayerUI fromUI = playerUIDict_Kaze[fromKaze];
                fromUI.SetNaki();

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.Ron_Agari:
            {
                UIAnimWillEndAfter( AgariAnimationTime );

                List<EKaze> ronPlayers = (List<EKaze>)args[0];
                EKaze fromKaze = (EKaze)args[1];
                Hai ronHai = (Hai)args[2];

                for( int i = 0; i < ronPlayers.Count; i++ )
                {
                    PlayerUI ui = playerUIDict_Kaze[ronPlayers[i]];
                    ui.PickHai( ronHai, true, false );
                    ui.SetTehaiVisiable(true);

                    ui.Speak( ECvType.Ron );
                }

                PlayerUI fromUI = playerUIDict_Kaze[fromKaze];
                fromUI.SetNaki();

                // show out all players' tehai
                ShowAllPlayerTehai();
            }
            break;

            case UIEventType.Tsumo_Agari:
            {
                UIAnimWillEndAfter( AgariAnimationTime );

                Player activePlayer = (Player)args[0];

                PlayerUI ui = playerUIDict_Kaze[ activePlayer.JiKaze ];
                ui.SetTehaiVisiable(true);

                ui.Speak( ECvType.Tsumo );

                // show out all players' tehai
                ShowAllPlayerTehai();
            }
            break;

            case UIEventType.Display_Agari_Panel:
            {
                List<AgariUpdateInfo> agariList = (List<AgariUpdateInfo>)args[0];
                agariPanel.Show( agariList );
            }
            break;

            case UIEventType.RyuuKyoku:
            {
                ERyuuKyokuReason reason = (ERyuuKyokuReason)args[0];
                List<AgariUpdateInfo> agariList = (List<AgariUpdateInfo>)args[1];

                ShowAllPlayerTehai();

                ryuuKyokuPanel.Show( reason, agariList );
            }
            break;

            case UIEventType.End_Game:
            {
                List<AgariUpdateInfo> agariList = (List<AgariUpdateInfo>)args[0];
                gameOverPanel.Show( agariList );
            }
            break;
        }
    }

    void SetManInputEnable(bool isEnable)
    {
        playerUIDict_Kaze[Model.getManKaze()].EnableInput(isEnable);
    }


    void ShowAllPlayerTehai()
    {
        foreach( var ui in playerUIDict )
        {
            ui.Value.SetTehaiVisiable(true);
        }
    }



    /// <summary>
    /// 获取对应玩家的Yama范围.
    ///          P2(68~101)
    /// P3(34~67)          P1(102~135)
    ///          P0(0~33)
    /// </summary>
    int[] getStartEndOfYamaUIOfPlayer(int playerIndex)
    {
        if( playerIndex < 0 || playerIndex > 3 )
            return null;

        int MaxLength = 34;
        int MaxPlayer = 4;

        int[] index = new int[2];
        index[0] = MaxLength * ((MaxPlayer - playerIndex) % MaxPlayer);
        index[1] = MaxLength * ((MaxPlayer - playerIndex) % MaxPlayer + 1) - 1;

        return index;
    }

    /// <summary>
    /// 寻找yamahai下标所在玩家Yama的范围.
    /// </summary>
    /// <param name="yamahaiIndex"></param>
    /// <returns></returns>
    int[] getStartEndOfYamahaiIndex( int yamahaiIndex ) 
    {
        for( int i = 0; i < 4; i++ ) 
        {
            int[] index = getStartEndOfYamaUIOfPlayer( i );
            if( yamahaiIndex >= index[0] && yamahaiIndex <= index[1] )
            {
                return index;
            }
        }
        return null;
    }

    /// <summary>
    /// 寻找yamahai下标所在的玩家.
    /// </summary>
    /// <param name="yamahaiIndex"></param>
    /// <returns></returns>
    int findPlayerForYamahaiIndex(int yamahaiIndex)
    {
        for( int i = 0; i < 4; i++ ) 
        {
            int[] index = getStartEndOfYamaUIOfPlayer(i);
            if( yamahaiIndex >= index[0] && yamahaiIndex <= index[1] ){
                return i;
            }
        }
        return -1;
    }

    void TestYama() {
        for( int i = 0; i < 4; i++ ) {
            int[] index = getStartEndOfYamaUIOfPlayer( i );
            Debug.LogWarning( string.Format( "~~yamahai index range of player {0} is ({1}, {2})", i, index[0], index[1] ) );
        }

        int yamahaiIndex = Random.Range(0, Yama.YAMA_HAIS_MAX);
        Debug.LogWarning( "-------------------------------------------" );
        Debug.LogWarning( string.Format( "~player index of yamahai({0}) is {1}", yamahaiIndex, findPlayerForYamahaiIndex( yamahaiIndex ) ) );

        Debug.LogWarning( "-------------------------------------------" );
        int[] index2 = getStartEndOfYamahaiIndex( yamahaiIndex );
        Debug.LogWarning( string.Format( "~index range of yamahai({0}) is ({1}, {2})", yamahaiIndex, index2[0], index2[1] ) );
    }

}


public static class UIHelper
{
    public static void SetOnClick(this UIButton btn, EventDelegate.Callback onClick)
    {
        if(btn == null) 
            return;

        btn.onClick.Clear();
        btn.onClick.Add( new EventDelegate(onClick) );
    }
}
