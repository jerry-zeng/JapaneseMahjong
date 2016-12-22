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
    public const float SuteHaiAnimationTime = 0.3f;
    public const float NakiAnimationTime = 0.3f;
    public const float AgariAnimationTime = 0.3f;

    private Dictionary<int, PlayerUI> playerUIDict = new Dictionary<int, PlayerUI>();
    private Dictionary<EKaze, PlayerUI> playerUIDict_Kaze = new Dictionary<EKaze, PlayerUI>();
    private GameInfoUI gameInfo;

    public PlayerInputPanel playerInputPanel;
    public SaifuriPanel saifuriPanel;
    public RyuuKyokuPanel ryuuKyokuPanel;
    public Transform mahjongPoolRoot;


    private MahjongMain Model
    {
        get { return GameManager.Instance.MahjongMain; }
    }


    void OnEnable() {
        EventManager.Get().addObserver(this);
    }
    void OnDisable() {
        EventManager.Get().removeObserver(this);
    }


    public override void Clear() {
        base.Clear();

        if(gameInfo != null)
            gameInfo.Clear();

        foreach( var kv in playerUIDict )
            kv.Value.Clear();

        playerUIDict.Clear();
        playerUIDict_Kaze.Clear();
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

            Transform dirParent = transform.FindChild(dir);
            Transform uiTran = dirParent.FindChild("PlayerUI");
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

        gameInfo = transform.FindChild("Info_Panel/GameInfo").GetComponent<GameInfoUI>();

        ResManager.SetPoolRoot( mahjongPoolRoot );

        isInit = true;

        ResManager.LoadStringTable();
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
            }
            break;

            case UIEventType.Saifuri: 
            {
                SetSaisPanel( UIEventType.Saifuri );
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

                    ui.SetOyaKaze( i == Model.ChiiChaIndex );
                    ui.BindPlayer(player);

                    if(player.JiKaze == Model.getManKaze()){
                        playerInputPanel.BindPlayer( player );
                        playerInputPanel.SetOwnerPlayerUI(ui);
                    }
                }
                Debug.Log( "MahjongView: init player info ui end..." );
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
                Debug.Log("MahjongView: SetYama_BeforeHaipai end...");

                //TestYama();
            }
            break;

            case UIEventType.Saifuri_For_Haipai: 
            {
                SetSaisPanel( UIEventType.Saifuri_For_Haipai );
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

                Debug.Log(string.Format("remove yamahai in range[{0},{1}]", waremeIndex+1, tsumoHaiStartIndex % Yama.YAMA_HAIS_MAX));

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

                Debug.Log("MahjongView: SetUI_AfterHaipai end...");
            }
            break;


            case UIEventType.DisplayMenuList:
            {
                playerInputPanel.Show();
            }
            break;
            case UIEventType.HideMenuList:
            {
                playerInputPanel.HideMenu();
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
            }
            break;

            case UIEventType.Reach:
            {
                Player activePlayer = (Player)args[0];
                int sutehaiIndex = (int)args[1];
                //Hai suteHai = (Hai)args[2];
                bool isTedashi = (bool)args[3];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.SuteHai(sutehaiIndex);
                ui.SetTedashi(isTedashi);
                ui.Reach();

                // needn't sort hais if sute the last one.
                if( sutehaiIndex < activePlayer.Tehai.getJyunTehaiCount() )
                    ui.SortTehai( activePlayer.Tehai.getJyunTehai(), SuteHaiAnimationTime );

                ui.Info.SetReach(true);
                ui.Info.SetTenbou( activePlayer.Tenbou );

                SetManInputEnable(false);
            }
            break;

            case UIEventType.Kakan:
            {
                Player activePlayer = (Player)args[0];
                //Hai kakanHai = (Hai)args[1];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.Ankan:
            {
                Player activePlayer = (Player)args[0];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.DaiMinKan:
            {
                Player activePlayer = (Player)args[0];
                EKaze fromKaze = (EKaze)args[1];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                PlayerUI fromUI = playerUIDict_Kaze[fromKaze];
                fromUI.SetNaki();

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.Pon:
            {
                Player activePlayer = (Player)args[0];
                EKaze fromKaze = (EKaze)args[1];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                PlayerUI fromUI = playerUIDict_Kaze[fromKaze];
                fromUI.SetNaki();

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.Chii_Left:
            case UIEventType.Chii_Center:
            case UIEventType.Chii_Right:
            {
                Player activePlayer = (Player)args[0];
                EKaze fromKaze = (EKaze)args[1];

                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.UpdateFuuro( activePlayer.Tehai.getFuuros() );
                ui.SetTehai( activePlayer.Tehai.getJyunTehai(), true );

                PlayerUI fromUI = playerUIDict_Kaze[fromKaze];
                fromUI.SetNaki();

                SetManInputEnable(!activePlayer.IsAI);
            }
            break;

            case UIEventType.Ron_Agari:
            {
                List<EKaze> ronPlayers = (List<EKaze>)args[0];
                //EKaze fromKaze = (EKaze)args[1];
                Hai ronHai = (Hai)args[2];

                for( int i = 0; i < ronPlayers.Count; i++ )
                {
                    Debug.LogWarning("Ron!!!");

                    PlayerUI playerUI = playerUIDict_Kaze[ronPlayers[i]];
                    playerUI.PickHai( ronHai, true, false );
                    playerUI.SetTehaiVisiable(true);
                }

                // show out all players' tehai
                ShowAllPlayerTehai();
            }
            break;

            case UIEventType.Tsumo_Agari:
            {
                Player activePlayer = (Player)args[0];

                Debug.LogWarning("Tsumo!!!");

                PlayerUI playerUI = playerUIDict_Kaze[ activePlayer.JiKaze ];
                playerUI.SetTehaiVisiable(true);

                // show out all players' tehai
                ShowAllPlayerTehai();
            }
            break;

            case UIEventType.RyuuKyoku:
            {
                ERyuuKyokuReason reason = (ERyuuKyokuReason)args[0];

                string msg = "";

                if( reason == ERyuuKyokuReason.NoTsumoHai )
                {
                    List<int> tenpaiPlayers = (List<int>)args[1];

                    string tenpaiIndex = "";
                    for(int i = 0; i < tenpaiPlayers.Count; i++)
                        tenpaiIndex += tenpaiPlayers[i].ToString() + ",";

                    msg = string.Format( "{0}人听牌{1}", tenpaiPlayers.Count.ToString(), 
                                    (tenpaiIndex.Length > 0? (": " + tenpaiIndex.Substring(0, tenpaiIndex.Length-1)) : "" ) );
                    
                }

                ryuuKyokuPanel.Show( msg, null );

                ShowAllPlayerTehai();
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

    // sais panel objects.
    UIEventType lastSaifuriTarget;

    void SetSaisPanel(UIEventType saiTarget) 
    {
        lastSaifuriTarget = saiTarget;

        string tip = "";
        if( saiTarget == UIEventType.Saifuri ) {
            tip = "Saifuri for deciding Chiicha";
        }
        else {
            tip = "Saifuri for deciding Wareme";
        }

        saifuriPanel.Show( tip, OnClickSaisButton );
    }
    void OnClickSaisButton() {
        Sai[] sais = Model.Saifuri();
        saifuriPanel.SetResult( sais[0].Num + ", " + sais[1].Num );

        StartCoroutine(OnSaifuriEnd());
    }
    IEnumerator OnSaifuriEnd()
    {      
        yield return new WaitForSeconds(2);

        saifuriPanel.Hide();

        if( lastSaifuriTarget == UIEventType.Saifuri ) {
            lastSaifuriTarget = UIEventType.On_Saifuri_End;
        }
        else {
            lastSaifuriTarget = UIEventType.On_Saifuri_For_Haipai_End;
        }
        //lastSaifuriTarget = lastSaifuriTarget+1;

        Debug.Log("Response event type: " + lastSaifuriTarget.ToString());

        EventManager.Get().SendEvent( lastSaifuriTarget );
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
