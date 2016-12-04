using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MahjongView : UIObject, IUIObserver 
{
    private Dictionary<int, PlayerUI> playerUIDict = new Dictionary<int, PlayerUI>();
    private Dictionary<EKaze, PlayerUI> playerUIDict_Kaze = new Dictionary<EKaze, PlayerUI>();
    private GameInfoUI gameInfo;


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
        }

        gameInfo = transform.FindChild("Info_Panel/GameInfo").GetComponent<GameInfoUI>();

        isInit = true;       
    }


    private int tsumoHaiStartIndex = 0;

    // handle ui event.
    public void OnHandleEvent(UIEventID evtID, object[] args) 
    {
        switch(evtID)
        {
            case UIEventID.Init_Game: // game init /
            {
                Clear();
                Init();
            }
            break;

            case UIEventID.Saifuri: 
            {
                SetSaisButton( evtID );
            }
            break;

            case UIEventID.Init_PlayerInfoUI: 
            {
                List<Player> players = Model.getPlayers();
                for( int i = 0; i < players.Count; i++ )
                {
                    Player player = players[i];
                    PlayerUI ui = playerUIDict[i];

                    ui.SetKaze( player.JiKaze );
                    ui.SetTenbou( player.Tenbou );
                    ui.Reach( false );

                    ui.SetOyaKaze( i == Model.getChiichaIndex() );
                    ui.BindPlayer(player);
                }
                Debug.Log( "MahjongView: init player info ui end..." );
            }
            break;

            case UIEventID.SetYama_BeforeHaipai: 
            {
                // Yama.
                const int MaxLength = YamaUI.MaxYamaPairInPlayer * 2;

                Hai[] yamaHais = Model.getYama().getYamaHais();
                int PlayerLength = Model.getPlayers().Count;

                for( int i = 0; i < PlayerLength; i++ ) 
                {
                    Dictionary<int, Hai> haiDict = new Dictionary<int, Hai>();

                    for( int h = 1; h <= MaxLength; h++ ) 
                    {
                        int endIndex = h + MaxLength * i - 1;
                        haiDict.Add( endIndex, yamaHais[endIndex] );
                    }

                    if( haiDict.Count > 0 ){
                        int uiIndex = (PlayerLength - i) % PlayerLength; // reverse.
                        int[] indexRange = getStartEndOfYamaUIOfPlayer( uiIndex );

                        playerUIDict[uiIndex].SetYamaHais( haiDict, indexRange[0], indexRange[1] );
                    }
                }
                Debug.Log("MahjongView: SetYama_BeforeHaipai end...");
            }
            break;

            case UIEventID.Saifuri_For_Haipai: 
            {
                SetSaisButton( evtID );
            }
            break;

            case UIEventID.SetUI_AfterHaipai: // 配牌 /
            {
                /// set game info.
                gameInfo.SetKyoku( Model.getBaKaze(), Model.getkyoku() );
                gameInfo.SetReachCount( Model.getReachbou() );
                gameInfo.SetHonba(Model.getHonba());

                /// set tehais.
                for( int i = 0; i < Model.getPlayers().Count; i++ ) 
                {
                    Player player = Model.getPlayers()[i];

                    PlayerUI ui = playerUIDict[i];

                    ui.SetTehai( player.Tehai.getJyunTehai() );
                    ui.SetAllHaisVisiable( true );

                    playerUIDict_Kaze[player.JiKaze] = ui;
                }

                /// set yama.                
                int PlayerLength = Model.getPlayers().Count;
                int waremeIndex = Model.getWareme();

                // count start tsumo index.
                tsumoHaiStartIndex = (waremeIndex+1) + 13 * PlayerLength - 1;

                Debug.Log(string.Format("remove yamahai with range({0},{1})", waremeIndex+1, tsumoHaiStartIndex % Yama.YAMA_HAIS_MAX));

                for( int yamahaiID = waremeIndex + 1; yamahaiID <= tsumoHaiStartIndex; yamahaiID++ ) 
                {                    
                    int id = yamahaiID % Yama.YAMA_HAIS_MAX;
                    int p = findPlayerForYamahaiIndex(id);
                    playerUIDict[p].PickUpYamaHai(id);
                }
                
                /// set init Dora.
                int showIndex = waremeIndex - 5;
                if( showIndex < 0 )
                    showIndex += Yama.YAMA_HAIS_MAX;

                int pi = findPlayerForYamahaiIndex(showIndex);
                playerUIDict[pi].ShowYamaHai(showIndex);

                // TODO: set Wareme.
                showIndex = waremeIndex-13;
                if( showIndex < 0 )
                    showIndex += Yama.YAMA_HAIS_MAX;
                pi = findPlayerForYamahaiIndex(showIndex);
                playerUIDict[pi].HighlightHai(showIndex);

                Debug.Log("MahjongView: SetUI_AfterHaipai end...");
            }
            break;

            case UIEventID.PickHai:
            {
                Player activePlayer = (Player)args[0];
                var jyunHais = activePlayer.Tehai.getJyunTehai();
                Hai newHai = jyunHais[jyunHais.Length-1];
                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.PickHai( newHai, true, true );

                int lastPickIndex = (int)args[1];
                //int pi = findPlayerForYamahaiIndex(lastPickIndex);
                //playerUIDict[pi].PickUpYamaHai(lastPickIndex);
                //playerUIDict[pi].HighlightHai(lastPickIndex);
                Debug.LogWarning("MahjongView: PickHai end..." + lastPickIndex.ToString());

                SetManInputEnable( !activePlayer.IsAI );
            }
            break;

            case UIEventID.SuteHai:
            {
                Player activePlayer = (Player)args[0];
                PlayerUI ui = playerUIDict_Kaze[activePlayer.JiKaze];
                ui.SetTehai( activePlayer.Tehai.getJyunTehai() );
                ui.SetAllHaisVisiable( true );

                Hai sutehai = (Hai)args[1];
                ui.getHouUI().AddHai( sutehai );

                SetManInputEnable(false);
            }
            break;
        }
    }

    void SetManInputEnable(bool isEnable)
    {
        if( isEnable == true ) {
            playerUIDict[0].getTehaiUI().EnableInput();
        }
        else{
            playerUIDict[0].getTehaiUI().DisableInput();
        }
    }

    // sais panel objects.
    public GameObject saifuriPanel;
    public UIButton saisButton;
    public UILabel saiTip;

    UIEventID lastSaifuriTarget = 0;

    void SetSaisButton(UIEventID saiTarget) 
    {
        if( saisButton == null )
            saisButton = saifuriPanel.transform.FindChild("SaisButton").GetComponent<UIButton>();
        
        saisButton.SetOnClick(OnClickSaisButton);

        if( saiTip == null )
            saiTip = saifuriPanel.transform.FindChild("tip").GetComponent<UILabel>();            

        if( saiTarget == UIEventID.Saifuri ) {
            saiTip.text = "Saifuri for deciding Chiicha";
        }
        else {
            saiTip.text = "Saifuri for deciding Wareme";
        }

        lastSaifuriTarget = saiTarget;

        saisButton.isEnabled = true;
        saifuriPanel.SetActive(true);
    }
    void OnClickSaisButton() {
        Sai[] sais = Model.Saifuri();
        saiTip.text = sais[0].Num + ", " + sais[1].Num;

        saisButton.isEnabled = false;

        StartCoroutine(OnSaifuriEnd());
    }
    IEnumerator OnSaifuriEnd()
    {      
        yield return new WaitForSeconds(2);

        saifuriPanel.SetActive(false);

        UIEventID evtID;
        if( lastSaifuriTarget == UIEventID.On_Saifuri_End ) {
            evtID = UIEventID.On_Saifuri_End;
        }
        else {
            evtID = UIEventID.On_Saifuri_For_Haipai_End;
        }

        EventManager.Get().SendUIEvent(evtID);
    }


    /// <summary>
    /// 获取对应玩家的Yama范围.
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    int[] getStartEndOfYamaUIOfPlayer(int playerIndex) {
        if( playerIndex < 0 || playerIndex > 3 )
            return null;

        int MaxLength = 34;
        int MaxPlayer = Model.getPlayers().Count;

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
    int[] getStartEndOfYamahaiIndex( int yamahaiIndex ) {
        int MaxPlayer = Model.getPlayers().Count;

        for( int i = 0; i < MaxPlayer; i++ ) {
            int[] index = getStartEndOfYamaUIOfPlayer( i );
            if( yamahaiIndex >= index[0] && yamahaiIndex <= index[1] ) {
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
    int findPlayerForYamahaiIndex(int yamahaiIndex) {
        int MaxPlayer = Model.getPlayers().Count;

        for( int i = 0; i < MaxPlayer; i++ ) 
        {
            int[] index = getStartEndOfYamaUIOfPlayer(i);
            if( yamahaiIndex >= index[0] && yamahaiIndex <= index[1] ){
                return i;
            }
        }
        return -1;
    }

    void Test() {
        for( int i = 0; i < 4; i++ ) {
            int[] index = getStartEndOfYamaUIOfPlayer( i );
            Debug.LogWarning( string.Format( "~~yamahai index range of player {0} is ({1}, {2})", i, index[0], index[1] ) );
        }

        int yamahaiIndex = 77;
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
        if(btn == null || onClick == null) 
            return;

        btn.onClick.Clear();
        btn.onClick.Add( new EventDelegate(onClick) );
    }
}
