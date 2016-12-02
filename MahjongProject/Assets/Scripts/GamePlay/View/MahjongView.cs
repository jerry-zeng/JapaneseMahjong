using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 麻将界面。
 */

public class MahjongView : UIObject, IObserver 
{

    private Dictionary<int, PlayerUI> playerUIDic = new Dictionary<int, PlayerUI>( 4 );
    private GameInfoUI gameInfo;

    private MahjongMain Model {
        get {
            return GameMain.Instance.MahjongMain;
        }
    }

    void OnEnable() {
        EventManager.Get().addObserver(this);
    }
    void OnDisable() {
        EventManager.Get().removeObserver(this);
    }


    public override void Clear() {
        base.Clear();

        gameInfo.Clear();

        foreach( var kv in playerUIDic ) {
            kv.Value.Clear();
        }
    }


    public override void Init() 
    {
        if( isInit == false ) {
            gameInfo = transform.FindChild("Info_Panel/GameInfo").GetComponent<GameInfoUI>();

            // init player uis.
            for( int i = 0; i < GameSettings.PlayerCount; i++ ) 
            {
                string dir = "South_Panel";
                float eulerAngleZ = 0;

                if( i == 0 ) {
                    dir = "South_Panel";
                    eulerAngleZ = 0;
                }
                else if( i == 1 ) {
                    dir = "East_Panel";
                    eulerAngleZ = 90;
                }
                else if( i == 2 ) {
                    dir = "North_Panel";
                    eulerAngleZ = 180;
                }
                else if( i == 3 ) {
                    dir = "West_Panel";
                    eulerAngleZ = -90;
                }
                else {
                    Debug.LogError("MahjongView: Unknown kaze...");
                    continue;
                }

                Transform dirParent = transform.FindChild(dir);
                Transform uiTran = dirParent.FindChild("PlayerUI");
                if( uiTran == null ){
                    uiTran = ResManager.CreatePlayerUIObject().transform;
                    uiTran.parent = dirParent;
                    uiTran.localScale = Vector3.one;
                    uiTran.localEulerAngles = new Vector3(0, 0, eulerAngleZ);
                }

                PlayerUI p = uiTran.GetComponent<PlayerUI>();
                p.Init();
                
                UIPanel parentPanel = dirParent.GetComponent<UIPanel>();
                p.SetParentPanelDepth( parentPanel .depth);

                playerUIDic.Add(i, p);
            }

            isInit = true;
        }
        
    }


    private int tsumoHaiStartIndex = 0;

    // sais panel objects.
    public GameObject saifuriPanel;
    private UIButton saisButton;
    private UILabel saiTip;



    // handle ui event.
    public void OnHandleEvent(EventID evtID, object[] args) 
    {
        switch(evtID)
        {
        case EventID.Init_Game: // game init /
            {
                Init();
                Clear();
            }
            break;

        case EventID.Saifuri: 
            {
                SetSaisButton( evtID );
            }
            break;

        case EventID.Init_PlayerInfoUI: 
            {
                List<Player> players = Model.getPlayers();
                for( int i = 0; i < players.Count; i++ ) {
                    Player player = players[i];
                    PlayerUI ui = playerUIDic[i];

                    ui.SetKaze( player.getJikaze() );
                    ui.SetTenbou( player.getTenbou() );
                    ui.Reach( false );

                    ui.SetOyaKaze( i == Model.getChiichaIndex() );
                }
                Debug.Log( "MahjongView: init player info ui end..." );
            }
            break;

        case EventID.SetYama_BeforeHaipai: 
            {
                // Yama.
                const int MaxLength = YamaUI.MaxYamaPairInPlayer * 2;

                Hai[] yamaHais = Model.getYama().getYamaHais();
                int PlayerLength = Model.getPlayers().Count;

                for( int i = 0; i < PlayerLength; i++ ) 
                {
                    //int startIndex = MaxLength * i;
                    Dictionary<int, Hai> haiDic = new Dictionary<int, Hai>();

                    for( int h = 1; h <= MaxLength; h++ ) 
                    {
                        int endIndex = h + MaxLength * i - 1;
                        haiDic.Add( endIndex, yamaHais[endIndex] );
                    }

                    if( haiDic.Count > 0 ){
                        int uiIndex = (PlayerLength - i) % PlayerLength; // reverse.
                        int[] indexRange = getStartEndOfYamaUIOfPlayer( uiIndex );

                        playerUIDic[uiIndex].SetYamaHais( haiDic, indexRange[0], indexRange[1] );
                    }
                }
                Debug.Log("MahjongView: SetYama_BeforeHaipai end...");
            }
            break;

        case EventID.Saifuri_For_Haipai: 
            {
                SetSaisButton( evtID );
            }
            break;

        case EventID.SetUI_AfterHaipai: // 配牌 /
            {
                /// set game info.
                gameInfo.SetKyoku( Model.getBaKaze(), Model.getkyoku() );
                gameInfo.SetReachCount( Model.getReachbou() );
                gameInfo.SetHonba(Model.getHonba());

                /// set tehais.
                for( int i = 0; i < Model.getPlayers().Count; i++ ) 
                {
                    Player player = Model.getPlayers()[i];

                    PlayerUI ui = playerUIDic[i];

                    ui.SetTehai( player.getTehai().getJyunTehai() );
                    ui.SetAllHaisVisiable( true );
                }

                /// set yama.                
                int PlayerLength = Model.getPlayers().Count;
                int waremeIndex = Model.getWareme();

                // count start tsumo index.
                tsumoHaiStartIndex = (waremeIndex+1) + 13 * PlayerLength - 1;

                Debug.LogWarning(string.Format("remove yamahai with range({0},{1})", waremeIndex+1, tsumoHaiStartIndex % Yama.YAMA_HAIS_MAX));

                for( int yamahaiID = waremeIndex + 1; yamahaiID <= tsumoHaiStartIndex; yamahaiID++ ) 
                {                    
                    int id = yamahaiID % Yama.YAMA_HAIS_MAX;
                    int p = findPlayerForYamahaiIndex(id);
                    playerUIDic[p].PickUpYamaHai(id);
                }
                
                /// set init Dora.
                int showIndex = waremeIndex - 5;
                if( showIndex < 0 )
                    showIndex += Yama.YAMA_HAIS_MAX;

                int pi = findPlayerForYamahaiIndex(showIndex);
                playerUIDic[pi].ShowYamaHai(showIndex);

                /// TODO: set Wareme.

                Debug.Log("MahjongView: SetUI_AfterHaipai end...");
            }
            break;

            
        
        default:
            break;
        }
    }


    EventID lastSaifuriTarget = 0;
    void SetSaisButton(EventID saiTarget) {
        if( saisButton == null ){
            saisButton = saifuriPanel.transform.FindChild("SaisButton").GetComponent<UIButton>();
            saisButton.onClick.Clear();
            saisButton.onClick.Add(new EventDelegate(OnClickSaisButton));
        }
        if( saiTip == null ){
            saiTip = saifuriPanel.transform.FindChild("tip").GetComponent<UILabel>();            
        }

        if( saiTarget == EventID.Saifuri ) {
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
    IEnumerator OnSaifuriEnd() {      
        yield return new WaitForSeconds(2);

        saifuriPanel.SetActive(false);


        EventID evtID = EventID.None;
        if( lastSaifuriTarget == EventID.On_Saifuri_End ) {
            evtID = EventID.On_Saifuri_End;
        }
        else {
            evtID = EventID.On_Saifuri_For_Haipai_End;
        }

        EventManager.Get().SendEvent(evtID);
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
