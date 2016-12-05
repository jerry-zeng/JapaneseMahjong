using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
/// <summary>
/// Mahjong main.
/// The game logic manager
/// </summary>

public class MahjongMain : Mahjong 
{

    // Step: 1
    protected override void initialize()
    {
        // 山を作成する。
        m_yama = new Yama();

        m_sais = new Sai[] { new Sai(), new Sai() };

        // 赤ドラを設定する。
        if( GameSettings.UseRedDora ) {
            m_yama.setRedDora(Hai.ID_PIN_5, 2);
            m_yama.setRedDora(Hai.ID_WAN_5, 1);
            m_yama.setRedDora(Hai.ID_SOU_5, 1);
        }

        // 捨牌を作成する
        m_suteHaiList = new List<SuteHai>();

        // リーチ棒の数を初期化する。
        m_reachbou = 0;

        // 本場を初期化する。
        m_honba = 0;

        // 局を初期化する。
        m_kyoku = (int)EKyoku.Ton_1;

        // プレイヤーの配列を初期化する。
        m_playerList = new List<Player>();
        m_playerList.Add( new Man("A") );
        m_playerList.Add( new AI("B") );
        m_playerList.Add( new AI("C") );
        m_playerList.Add( new AI("D") );

        GameSettings.PlayerCount = m_playerList.Count;

        for(int i = 0; i < m_playerList.Count; i++)
        {
            m_playerList[i].Tenbou = GameSettings.TENBOU_INIT;
        }

        // プレイヤーに提供する情報を作成する。
        GameAgent.Instance.Initialize(this);
    }

    // Step: 2
    public Sai[] Saifuri()
    {
        m_sais[0].SaiFuri();
        m_sais[1].SaiFuri();

        return m_sais;
    }

    // Step: 3
    public void SetRandomChiicha()
    {
        //m_oyaIndex = (m_sais[0].Num + m_sais[1].Num - 1) % m_playerList.Count;
        m_oyaIndex = 0;
        m_chiichaIndex = m_oyaIndex;
    }

    public void SetNextOya()
    {
        m_oyaIndex++;
        if(m_oyaIndex >= m_playerList.Count)
            m_oyaIndex = 0;
    }

    // プレイヤーの自風を設定する
    protected void initPlayerKaze()
    {
        EKaze kaze = (EKaze)m_oyaIndex;

        for( int i = 0; i < m_playerList.Count; i++)
        {
            m_playerList[i].JiKaze = kaze;

            kaze = kaze.Next();
        }
    }

    // Step: 4
    public void PrepareKyokuYama()
    {
        // 連荘を初期化する。
        m_renchan = false;

        m_isTenhou = true;
        m_isChiihou = true;
        m_isTsumo = false;
        m_isRinshan = false;
        m_isLast = false;

        // プレイヤーの自風を設定する。
        initPlayerKaze();

        // プレイヤー配列を初期化する。
        for( int i = 0; i < m_playerList.Count; i++ )
            m_playerList[i].Init();

        m_suteHaiList.Clear();

        // 洗牌する。
        m_yama.XiPai();
    }

    // 山に割れ目を設定する
    protected void setWareme(Sai[] sais)
    {
        int sum = sais[0].Num + sais[1].Num; 

        int waremePlayer = (getChiichaIndex() - sum - 1) % 4;
        if( waremePlayer < 0 )
            waremePlayer += 4;

        int startHaisIndex = ( (4- waremePlayer) % 4 ) * 34 + sum * 2;

        m_wareme = startHaisIndex - 1; // 开始拿牌位置-1.

        getYama().setTsumoHaisStartIndex(startHaisIndex);
    }

    // 配牌する
    protected virtual void Haipai()
    {
        // everyone picks 3x4 hais.
        for( int i = 0, j = m_oyaIndex; i < m_playerList.Count * 12; j++ ) 
        {
            if( j >= m_playerList.Count )
                j = 0;

            // pick 4 hais once.
            Hai[] hais = m_yama.PickHaipai();
            for( int h = 0; h < hais.Length; h++ )
            {
                m_playerList[j].Tehai.addJyunTehai( hais[h] );

                i++;
            }
        }

        // then everyone picks 1 hai.
        for( int i = 0, j = m_oyaIndex; i < m_playerList.Count * 1; i++,j++ )
        {
            if( j >= m_playerList.Count )
                j = 0;

            Hai hai = m_yama.PickTsumoHai();
            m_playerList[j].Tehai.addJyunTehai( hai );
        }

        // sort hais
        for( int i = 0; i < m_playerList.Count; i++ )
        {
            m_playerList[i].Tehai.Sort();
        }


        if(testHaipai == true)
            StartTest();
    }

    // Step: 5
    public void SetWaremeAndHaipai() 
    {
        // 山に割れ目を設定する。
        setWareme(m_sais);

        // 配牌する。
        Haipai();
    }

    // Step: 6
    public void PrepareToStart()
    {
        int i = m_oyaIndex >= m_playerList.Count ? 0 : m_oyaIndex;
        m_activePlayer = m_playerList[i];

        // イベントを発行した風を初期化する。
        m_kazeFrom = m_activePlayer.JiKaze;
    }


    public bool IsLastKyoku()
    {
        return (int)m_kyoku >= GameSettings.Kyoku_Max;
    }
    public void GoToNextKyoku()
    {
        m_kyoku++;
    }

    // ツモ牌がない場合、流局する
    public bool IsRyuukyoku()
    {
        return m_tsumoHai == null;
    }
    public void GameOver()
    {
        Debug.LogWarning("Game Over!!!");
        PostUIEvent(UIEventType.End_Game);
    }

    public void CacheActivePlayer()
    {
        m_kazeFrom = m_activePlayer.JiKaze;
    }
    public void SetNextPlayer()
    {
        // イベントを発行した風を更新する。
        m_kazeFrom = m_kazeFrom.Next();
        m_activePlayer = getPlayer( m_kazeFrom );
    }
    public void GoToNextLoop()
    {
        SetNextPlayer();

        PickNewTsumoHai();
    }


    #region Request & Response

    protected ERequest m_request = ERequest.Ron_OrNot;

    protected void ResetAskPurpose()
    {
        m_request = ERequest.Ron_OrNot;
    }


    public void SendRequest(ERequest request, EKaze fromKaze, Hai haiToHandle)
    {
        m_activePlayer.HandleRequest( request, fromKaze, haiToHandle, OnActivePlayerResponse );
    }

    public void OnActivePlayerResponse(EResponse response)
    {
        switch( m_request )
        {
            case ERequest.Tsumo_OrNot:
            {
                if( response != EResponse.Tsumo_Agari || 
                    response != EResponse.Ankan ||
                    response != EResponse.Kakan ||
                    response != EResponse.Reach || 
                    response != EResponse.SuteHai )
                {
                    throw new InvalidResponseException("Invalid response to ERequest.Tsumo_OrNot");
                }
                else
                {
                    OnResponse_TsumoOrNot();
                }
            }
            break;
            case ERequest.Ron_OrNot:
            {
                if( response != EResponse.Ron_Agari || 
                    response != EResponse.Nagashi )
                {
                    throw new InvalidResponseException("Invalid response to ERequest.Ron_OrNot");
                }
                else
                {
                    OnResponse_RonOrNot();
                }
            }
            break;
            case ERequest.PonKan_OrNot:
            {
                if( response != EResponse.Pon || 
                    response != EResponse.DaiMinKan ||
                    response != EResponse.Nagashi )
                {
                    throw new InvalidResponseException("Invalid response to ERequest.PonKan_OrNot");
                }
                else
                {
                    OnResponse_PonKanOrNot();
                }
            }
            break;
            case ERequest.Chii_OrNot:
            {
                if( response != EResponse.Chii_Left ||
                    response != EResponse.Chii_Center ||
                    response != EResponse.Chii_Right ||
                    response != EResponse.Nagashi )
                {
                    throw new InvalidResponseException("Invalid response to ERequest.Chii_OrNot");
                }
                else
                {
                    OnResponse_ChiiOrNot();
                }
            }
            break;

            default:
            {
                throw new InvalidResponseException("Unhandled request: " + m_request.ToString());
            }
        }
    }
    #endregion

    #region Pick Hais

    // Step: 7
    public void PickNewTsumoHai()
    {
        // ツモ牌を取得する。
        m_tsumoHai = m_yama.PickTsumoHai();

        int tsumoNokori = m_yama.getTsumoNokori();
        if( tsumoNokori <= 0 ) {
            m_isLast = true;
        }
        else if( tsumoNokori < 66 ) {
            m_isChiihou = false;
        }


        if( IsRyuukyoku() )
        {
            if( HasRyuukyokuMan() ){
                HandleRyuukyokuMan();
            }
            else{
                if( HasRyuukyokuTenpai() ){
                    HandleRyuukyokuTenpai();
                }
                else{
                    if( IsLastKyoku() ){
                        GameOver();
                    }
                    else{
                        GoToNextKyoku();
                    }
                }
            }
        }
        else
        {
            // 1. active player pick up a new hai.
            // 2. yama remove a hai.

            //m_activePlayer.PickNewHai( m_tsumoHai );
            //int lastPickIndex = getYama().getLastTsumoHaiIndex();

            // update UI...

            Ask_TsumoOrNot();
        }
    }

    // pick up Rinshan hai.
    public void PickRinshanHai()
    {
        m_tsumoHai = m_yama.PickRinshanTsumoHai();

        // 1. active player add a new hai.
        // 2. yama remove a rinshan hai
        // 3. yama open a new omote dora hai.

        bool has4Kan = false;
        bool allKanCountOver4 = (m_tsumoHai == null);

        if( has4Kan == true ){
            HandleTsumo();
        }
        else if( allKanCountOver4 == true ){
            HandleInvalidKyoku();
        }
        else{
            Ask_TsumoOrNot();
        }
    }

    #endregion

    #region Tsumo Or Not

    public void Ask_TsumoOrNot()
    {
        SendRequest(ERequest.Tsumo_OrNot, m_kazeFrom, m_tsumoHai);
    }

    public void OnResponse_TsumoOrNot()
    {
        PlayerAction action = getPlayerAction();
        EResponse response = action.Response;

        if( response == EResponse.Tsumo_Agari )
        {
            bool enableTsumoCheck = false;
            if( enableTsumoCheck == true )
            {
                HandleTsumo();
            }
            else{
                Debug.LogError("The active player Can't tsumo!!!");
            }
        }
        else if(response == EResponse.Ankan || response == EResponse.Kakan)
        {
            // set kan.
            m_isChiihou = false;

            PickRinshanHai();
            m_isRinshan = true;

        }
        else if( response == EResponse.Reach )
        {
            // 0. active player set reach.
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;

            // update UI...

            // 捨牌のインデックスを取得する。
            m_sutehaiIndex = m_activePlayer.getSutehaiIndex();
            m_activePlayer.IsReach = true;

            if( m_isChiihou ) {
                m_activePlayer.IsDoubleReach = true;
            }

            m_activePlayer.SuteHaisCount = m_suteHaiList.Count;

            m_activePlayer.reduceTenbou( 1000 );
            m_activePlayer.IsReach = true;
            m_reachbou++;

            m_activePlayer.IsIppatsu = true;


            // cache.
            CacheActivePlayer();

            ResetAskPurpose();
            Ask_HandleSuteHai();
        }
        else if(response == EResponse.SuteHai)
        {
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;

            // update UI...

            // 捨牌のインデックスを取得する。
            m_sutehaiIndex = m_activePlayer.getSutehaiIndex();

            if( m_sutehaiIndex >= m_activePlayer.Tehai.getJyunTehai().Length ) {// ツモ切り
                Hai.copy( m_suteHai, m_tsumoHai );
                m_activePlayer.Hou.addHai( m_suteHai );
            }
            else {// 手出し
                m_activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, m_sutehaiIndex );
                m_activePlayer.Tehai.removeJyunTehai( m_sutehaiIndex );
                m_activePlayer.Tehai.addJyunTehai( m_tsumoHai );
                m_activePlayer.Hou.addHai( m_suteHai );
                m_activePlayer.Hou.setTedashi( true );
            }

            m_suteHaiList.Add( new SuteHai( m_suteHai ) );

            if( !m_activePlayer.IsReach )
                m_activePlayer.SuteHaisCount = m_suteHaiList.Count;

            // cache.
            CacheActivePlayer();

            ResetAskPurpose();
            Ask_HandleSuteHai();
        }
    }

    #endregion

    #region Request response to Non-Tsumo

    /// <summary>
    /// Asks other players to handle the sute hai.
    /// 
    /// As only one player can do ron/pon/kan/chii at one time, this check flow is allowed. 
    /// 由于不存在2个或以上的人同时 荣胡、碰、槓、吃，所以用这种方法可行,
    /// 如果支持多人同时荣胡，该方法需要修改!
    /// </summary>

    public void Ask_HandleSuteHai()
    {
        EKaze nextKaze = m_activePlayer.JiKaze.Next();
        if( nextKaze == m_kazeFrom )
        {
            m_request++;
            nextKaze = m_activePlayer.JiKaze.Next();
        }

        // ask other 3 players.
        for(int i = 0; i < m_playerList.Count-1; i++)
        {
            m_activePlayer = getPlayer( nextKaze );

            if( m_request == ERequest.Ron_OrNot )
            {
                bool enableRon = false;
                if( enableRon == true ){
                    Ask_RonOrNot();
                    return;
                }
            }
            else if( m_request == ERequest.PonKan_OrNot )
            {
                bool enablePonOrKan = false;
                if( enablePonOrKan == true ){
                    Ask_PonKanOrNot();
                    return;
                }
            }
            else if( m_request == ERequest.Chii_OrNot )
            {
                int relation = getRelation(m_kazeFrom, m_activePlayer.JiKaze);
                if( relation == (int)ERelation.KaMiCha )
                {
                    bool enableChii = false;
                    if( enableChii == true ){
                        Ask_ChiiOrNot();
                        return;
                    }
                }
            }
            else
            {
                GoToNextLoop();
                return;
            }
        }
    
        GoToNextLoop();
    }

    #endregion

    #region Ron Or Not

    public void Ask_RonOrNot()
    {
        // wait for response.
        SendRequest(ERequest.Ron_OrNot, m_kazeFrom, m_suteHai);
    }

    public void OnResponse_RonOrNot()
    {
        PlayerAction action = getPlayerAction();
        EResponse response = action.Response;

        if( response == EResponse.Ron_Agari )
        {
            HandleRon();
        }
        else
        {
            Ask_HandleSuteHai();
        }
    }

    #endregion

    #region Pon Kan Or Not

    public void Ask_PonKanOrNot()
    {
        SendRequest(ERequest.PonKan_OrNot, m_kazeFrom, m_suteHai);
    }

    public void OnResponse_PonKanOrNot()
    {
        PlayerAction action = getPlayerAction();
        EResponse response = action.Response;

        if( response == EResponse.DaiMinKan )
        {
            // set kan.
            m_isChiihou = false;
            getPlayer( m_kazeFrom ).Hou.setNaki( true );

            PickRinshanHai();
        }
        else if(response == EResponse.Pon)
        {
            // set pon.
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;

            m_isChiihou = false;
            getPlayer( m_kazeFrom ).Hou.setNaki( true );

            CacheActivePlayer();
            ResetAskPurpose();
            Ask_HandleSuteHai();
        }
        else
        {
            Ask_HandleSuteHai();
        }
    }

    #endregion

    #region Chii Or Not

    public void Ask_ChiiOrNot()
    {
        SendRequest(ERequest.Chii_OrNot, m_kazeFrom, m_suteHai);
    }

    public void OnResponse_ChiiOrNot()
    {
        PlayerAction action = getPlayerAction();
        EResponse response = action.Response;

        if( response == EResponse.Chii_Left )
        {
            // set chii.
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;

            CacheActivePlayer();
            ResetAskPurpose();
            Ask_HandleSuteHai();
        }
        else if( response == EResponse.Chii_Center )
        {
            // set chii.
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;

            CacheActivePlayer();
            ResetAskPurpose();
            Ask_HandleSuteHai();
        }
        else if( response == EResponse.Chii_Right )
        {
            // set chii.
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;

            CacheActivePlayer();
            ResetAskPurpose();
            Ask_HandleSuteHai();
        }
        else{
            GoToNextLoop();
        }
    }

    #endregion

    #region Handle Game Result

    #region RyuuKyoku

    public bool HasRyuukyokuMan() 
    { 
        // 流し満貫の確認をする。
        for( int i = 0, j = m_oyaIndex; i < m_playerList.Count; i++, j++ ) 
        {
            if( j >= m_playerList.Count )
                j = 0;

            bool agari = true;

            Hou hou = m_playerList[j].Hou;
            SuteHai[] suteHais = hou.getSuteHais();
            int suteHaisLength = suteHais.Length;

            // check 1,9,字./
            for( int k = 0; k < suteHaisLength; k++ )
            {
                if( suteHais[k].IsNaki || !suteHais[k].IsYaochuu )
                {
                    agari = false;
                    break;
                }
            }
            if( agari == true )
                return true;
        }

        return false;
    }

    public void HandleRyuukyokuMan() 
    {
        int score = 0;
        int iPlayer = 0;

        // 流し満貫の確認をする。
        for( int i = 0, j = m_oyaIndex; i < m_playerList.Count; i++, j++ ) 
        {
            if( j >= m_playerList.Count )
                j = 0;

            bool agari = true;

            SuteHai[] suteHais = m_playerList[j].Hou.getSuteHais();
            for( int k = 0; k < suteHais.Length; k++ )
            {
                // check 1,9,字./
                if( suteHais[k].IsNaki || !suteHais[k].IsYaochuu ) {
                    agari = false;
                    break;
                }
            }

            if( agari == true ) // count score.
            {
                m_kazeFrom = m_playerList[j].JiKaze;

                AgariScoreManager.SetNagashiMangan( m_agariInfo ); // visitor.

                iPlayer = getPlayerIndex( m_kazeFrom );
                if( m_oyaIndex == iPlayer ) // count chii cha score.
                {
                    score = m_agariInfo.scoreInfo.oyaRon + (m_honba * 300);

                    for( int l = 0; l < 3; l++ )
                    {
                        iPlayer = (iPlayer + 1) % GameSettings.PlayerCount;
                        m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.oyaTsumo + (m_honba * 100) );
                    }
                }
                else 
                {
                    score = m_agariInfo.scoreInfo.koRon + (m_honba * 300);

                    for( int l = 0; l < 3; l++ )
                    {
                        iPlayer = (iPlayer + 1) % GameSettings.PlayerCount;
                        if( m_oyaIndex == iPlayer ) {
                            m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.oyaTsumo + (m_honba * 100) );
                        }
                        else {
                            m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.koTsumo + (m_honba * 100) );
                        }
                    }
                }

                //1. add NagashiMangan score.
                m_activePlayer.increaseTenbou( score );

                m_agariInfo.agariScore = score - (m_honba * 300);

                //2. add reach bou score.
                // 点数を清算する。
                m_activePlayer.increaseTenbou( m_reachbou * 1000 );

                // リーチ棒の数を初期化する。
                m_reachbou = 0;

                // UIイベント（ツモあがり）を発行する。
                PostUIEvent( UIEventType.Tsumo_Agari, m_kazeFrom, m_kazeFrom );

                // 親を更新する。
                if( m_oyaIndex != getPlayerIndex( m_kazeFrom ) )
                {
                    SetNextOya();

                    m_honba = 0;
                }
                else // 连庄. /
                {
                    m_renchan = true;
                    m_honba++;
                }
            } // end if (agari == true) .

        } // end if (m_tsumoHai == null) //流局./

    }

    // テンパイの確認をする
    public bool HasRyuukyokuTenpai()
    {
        for( int i = 0; i < m_playerList.Count; i++ )
        {
            if( m_playerList[i].isTenpai() )
                return true;
        }
        return false;
    }

    public void HandleRyuukyokuTenpai() 
    {
        bool[] tenpaiFlags = new bool[m_playerList.Count];

        int tenpaiCount = 0;
        for( int i = 0; i < m_playerList.Count; i++ )
        {
            tenpaiFlags[i] = m_playerList[i].isTenpai();

            if( tenpaiFlags[i] == true ) 
                tenpaiCount++;
        }

        int increasedScore = 0;
        int reducedScore = 0;

        switch( tenpaiCount )
        {
            case 0:
            break;
            case 1:
            increasedScore = 3000;
            reducedScore = 1000;
            break;
            case 2:
            increasedScore = 1500;
            reducedScore = 1500;
            break;
            case 3:
            increasedScore = 1000;
            reducedScore = 3000;
            break;
        }

        for( int i = 0; i < tenpaiFlags.Length; i++ )
        {
            if( tenpaiFlags[i] == true ){
                getPlayer(i).increaseTenbou( increasedScore );
            }
            else {
                getPlayer(i).reduceTenbou( reducedScore );
            }
        }

        // UIイベント（流局）を発行する。
        PostUIEvent( UIEventType.RyuuKyoku );

        // 親を更新する,上がり連荘とする
        SetNextOya();

        // 本場を増やす。
        m_honba++;
    }

    #endregion


    // some one has tsumo.
    protected void HandleTsumo()
    {
        AgariParam param = new AgariParam(this);
        int score = 0;
        int iPlayer = 0;

        param.setOmoteDoraHais( getOmotoDoras() );
        if( m_activePlayer.IsReach )                    
            param.setUraDoraHais( getUraDoras() );

        AgariScoreManager.GetAgariScore( m_activePlayer.Tehai, m_tsumoHai, param, ref m_combis, ref m_agariInfo );

        iPlayer = getPlayerIndex( m_kazeFrom );
        if( m_oyaIndex == iPlayer ) {
            score = m_agariInfo.scoreInfo.oyaRon + (m_honba * 300);
            for( int i = 0; i < 3; i++ )
            {
                iPlayer = (iPlayer + 1) % GameSettings.PlayerCount;
                m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.oyaTsumo + (m_honba * 100) );
            }
        }
        else 
        {
            score = m_agariInfo.scoreInfo.koRon + (m_honba * 300);
            for( int i = 0; i < 3; i++ )
            {
                iPlayer = (iPlayer + 1) % GameSettings.PlayerCount;
                if( m_oyaIndex == iPlayer ) {
                    m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.oyaTsumo + (m_honba * 100) );
                }
                else {
                    m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.koTsumo + (m_honba * 100) );
                }
            }
        }

        m_activePlayer.increaseTenbou( score ); //1. add Tsumo score.

        m_agariInfo.agariScore = score - (m_honba * 300);

        // 点数を清算する。//2. add reach bou score.
        m_activePlayer.increaseTenbou( m_reachbou * 1000 );

        // リーチ棒の数を初期化する。
        m_reachbou = 0;

        // UIイベント（ツモあがり）を発行する。
        PostUIEvent( UIEventType.Tsumo_Agari, m_kazeFrom, m_kazeFrom );

        // 親を更新する。
        if( m_oyaIndex != getPlayerIndex( m_kazeFrom ) ) 
        {
            SetNextOya();
            m_honba = 0;
        }
        else {
            m_renchan = true;
            m_honba++;
        }
    }

    // some one has ron.
    protected void HandleRon()
    {
        AgariParam param = new AgariParam(this);
        int score = 0;

        param.setOmoteDoraHais( getOmotoDoras() );
        if( m_activePlayer.IsReach )                    
            param.setUraDoraHais( getUraDoras() );

        AgariScoreManager.GetAgariScore( m_activePlayer.Tehai, m_suteHai, param, ref m_combis, ref m_agariInfo );

        if( m_oyaIndex == getPlayerIndex( m_kazeFrom ) ) {
            score = m_agariInfo.scoreInfo.oyaRon + (m_honba * 300);
        }
        else {
            score = m_agariInfo.scoreInfo.koRon + (m_honba * 300);
        }

        getPlayer( m_kazeFrom ).reduceTenbou( score );

        m_activePlayer.increaseTenbou( score );
        m_agariInfo.agariScore = score - (m_honba * 300);

        // 点数を清算する。
        m_activePlayer.increaseTenbou( m_reachbou * 1000 );

        // リーチ棒の数を初期化する。
        m_reachbou = 0;

        // UIイベント（ロン）を発行する。
        PostUIEvent( UIEventType.Ron_Agari, m_kazeFrom, m_kazeFrom );

        // 親を更新する。
        if( m_oyaIndex != getPlayerIndex( m_kazeFrom ) )
        {
            SetNextOya();
            m_honba = 0;
        }
        else {
            m_renchan = true;
            m_honba++;
        }
    }

    // kan count over 4 but no one agari.
    protected void HandleInvalidKyoku()
    {

    }

    #endregion


    public override void PostUIEvent(UIEventType eventType, params object[] args)
    {
        EventManager.Get().SendEvent(eventType, args);
    }



    #region Other Method

    protected bool testHaipai = false;

    protected void StartTest()
    {
        if(testHaipai == false)
            return;

        // remove all the hais of player 0.
        int iPlayer = 0;
        while( m_playerList[iPlayer].Tehai.getJyunTehai().Length > 0 )
            m_playerList[iPlayer].Tehai.removeJyunTehai(0);

        // add the test hais.
        int[] haiIds = getTestHaiIds();
        for( int i = 0; i < haiIds.Length - 1; i++ )
            m_playerList[iPlayer].Tehai.addJyunTehai( new Hai(haiIds[i]) );

        /*
        // test Pon.
        m_players[iPlayer].getTehai().removeJyunTehai(0);
        m_players[iPlayer].getTehai().setPon(new Hai(0), getRelation(m_kazeFrom, m_kazeTo));
        m_players[iPlayer].getTehai().setPon(new Hai(31), getRelation(m_kazeFrom, m_kazeTo));

        // test ChiiLeft.
        m_players[iPlayer].getTehai().removeJyunTehai(0);
        m_players[iPlayer].getTehai().setChiiLeft(new Hai(0), getRelation(m_kazeFrom, m_kazeTo));
        m_players[iPlayer].getKawa().add(new Hai(0));
        */
    }

    protected int[] getTestHaiIds() 
    {
        int[] haiIds = { 27, 27, 27, 28, 28, 28, 0, 0, 1, 2, 3, 4, 5, 6 };
        //int[] haiIds = {0, 1, 2, 3, 4, 5, 6, 7, 8, 33, 33, 33, 31, 31};
        //int[] haiIds = {29, 29, 29, 30, 30, 30, 31, 31, 31, 32, 32, 33, 33, 33};
        //int[] haiIds = {0, 1, 2, 3, 4, 5, 6, 7, 31, 31, 33, 33, 33};
        //int[] haiIds = {0, 1, 2, 10, 11, 12, 13, 14, 15, 31, 31, 33, 33, 33};
        //int[] haiIds = {0, 1, 2, 3, 4, 5, 6, 7, 31, 31, 32, 32, 32};
        //int[] haiIds = {0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8};
        //int[] haiIds = {1, 1, 3, 3, 5, 5, 7, 7, 30, 30, 31, 31, 32, 32};
        //int[] haiIds = {1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        //int[] haiIds = {0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8};
        //int[] haiIds = {27, 27, 28, 28, 29, 29, 30, 30, 31, 31, 32, 32, 33, 33};
        //int[] haiIds = {0, 0, 0, 0, 8, 8, 8, 8, 9, 9, 9, 9, 18, 18};
        //int[] haiIds = {0, 8, 9, 17, 18, 26, 27, 28, 29, 30, 31, 32, 33, 34};
        //int[] haiIds = {0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8};
        //int[] haiIds = {1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        //int[] haiIds = {19, 19, 20, 20, 21, 21, 23, 23, 23, 23, 25, 25, 25, 25};
        //int[] haiIds = {0, 0, 0, 8, 8, 8, 9, 9, 9, 17, 17, 17, 18, 18};
        //int[] haiIds = {0, 0, 0, 0, 8, 8, 8, 8, 9, 9, 9, 9, 18, 18};
        //int[] haiIds = {0, 0, 0, 8, 8, 8, 9, 9, 9, 18, 18, 18, 26, 26};
        //int[] haiIds = {27, 27, 27, 28, 28, 28, 29, 29, 29, 30, 30, 31, 31, 31};
        //int[] haiIds = {31, 31, 31, 32, 32, 32, 33, 33, 33, 30, 30, 30, 29, 29};
        //int[] haiIds = {0, 0, 1, 1, 2, 2, 6, 6, 7, 7, 8, 8, 9, 9};
        //int[] haiIds = {31, 31, 31, 32, 32, 32, 33, 33, 3, 4, 5, 6, 7, 8};
        //int[] haiIds = {1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4};
        //int[] haiIds = {0, 0, 0, 9, 9, 9, 18, 18, 18, 27, 27, 29, 28, 28};
        //int[] haiIds = {0, 0, 0, 9, 9, 9, 18, 18, 18, 27, 27, 28, 28, 28};
        //int[] haiIds = {0, 0, 0, 9, 9, 9, 18, 18, 18, 5, 6, 7, 27, 27};
        //int[] haiIds = {0, 0, 0, 2, 2, 2, 3, 3, 3, 5, 6, 7, 27, 27};
        //int[] haiIds = {0, 0, 0, 2, 2, 2, 3, 3, 3, 4, 4, 4, 10, 10};
        //int[] haiIds = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 9, 10, 10}; // イッツー
        //int[] haiIds = {0, 1, 2, 9, 10, 11, 18, 19, 20, 33, 33, 33, 27, 27};
        //int[] haiIds = {1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4}; // リーチタンピンイーペーコー
        //int[] haiIds = {1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7}; // リーチタンピンイーペーコー
        //int[] haiIds = {1, 1, 2, 2, 3, 3, 4, 5, 6, 10, 10, 10, 11, 12}; // リーチタンピンイーペーコー

        return haiIds;
    }

    #endregion

}
