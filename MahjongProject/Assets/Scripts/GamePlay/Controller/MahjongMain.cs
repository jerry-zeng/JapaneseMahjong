using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
/// <summary>
/// Mahjong main.
/// The game logic manager
/// </summary>

public class MahjongMain : Mahjong 
{
    protected bool testHaipai = false;


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

        m_honba = 0;
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

        // イベントを発行した風を初期化する。
        m_kazeFrom = m_playerList[m_oyaIndex].JiKaze;

        // イベントの対象となった風を初期化する。
        m_kazeTo = m_playerList[m_oyaIndex].JiKaze;

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
    }


    public bool IsLastKyoku()
    {
        return (int)m_kyoku >= GameSettings.Kyoku_Max;
    }
    public void GoToNextKyoku()
    {
        m_kyoku++;
    }
    public bool IsLastHai()
    {
        // ツモ牌がない場合、流局する。
        return m_tsumoHai == null;
    }
    public void GameOver()
    {
        Debug.LogWarning("Game Over!!!");
    }


    public void OnPlayerInput(EPlayerInputType inputType, EKaze playerKaze, object[] args)
    {
        
    }


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
                m_kazeFrom = m_kazeTo = m_playerList[j].JiKaze;

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
                PostUIEvent( UIEventID.Tsumo_Agari, m_kazeFrom, m_kazeTo );

                // 親を更新する。
                if( m_oyaIndex != getPlayerIndex( m_kazeFrom ) )
                {
                    m_oyaIndex++;
                    if( m_oyaIndex >= m_playerList.Count )
                        m_oyaIndex = 0;

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
        PostUIEvent( UIEventID.RyuuKyoku );

        // 親を更新する,上がり連荘とする
        m_oyaIndex++;
        if( m_oyaIndex >= m_playerList.Count )
            m_oyaIndex = 0;

        // 本場を増やす。
        m_honba++;
    }
    #endregion


    // Step: 7
    public void PickNewTsumoHai()
    {
        // ツモ牌を取得する。
        m_tsumoHai = m_yama.PickTsumoHai();

        if( IsLastHai() )
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

            AskTsumoOrSutehai();
        }
    }

    // pick up Rinshan hai.
    public void PickRinshanHai()
    {
        // 1. active player add a new hai.
        // 2. yama remove a rinshan hai
        // 3. yama open a new omote dora hai.

        bool has4Kan = false;
        bool allKanCountOver4 = false;

        if( has4Kan == true ){
            HandleTsumo();
        }
        else if( allKanCountOver4 == true ){
            HandleInvalidKyoku();
        }
        else{
            AskTsumoOrSutehai();
        }
    }


    public void AskTsumoOrSutehai()
    {
        // wait for response.
    }
    public void OnResponse_TsumoOrSutehai()
    {
        EventID response = EventID.Nagashi;
        if( response == EventID.Tsumo_Agari )
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
        else if(response == EventID.Ankan || response == EventID.Kakan)
        {
            // set kan.

            PickRinshanHai();
        }
        else if(response == EventID.SuteHai || response == EventID.Reach)
        {
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;

            // update UI...


            // cache.
            m_kazeFrom = m_activePlayer.JiKaze;

            ResetAskPurpose();
            AskHandleSuteHai();
        }
    }



    protected ERequestType m_request = ERequestType.Ron_OrNot;

    protected void ResetAskPurpose()
    {
        m_request = ERequestType.Ron_OrNot;
    }


    /// <summary>
    /// Asks other players to handle the sute hai.
    /// 
    /// As only one player can do ron/pon/kan/chii at one time, this check flow is allowed. 
    /// 由于不存在2个或以上的人同时 荣胡、碰、槓、吃，所以用这种方法可行,
    /// 如果支持多人同时荣胡，该方法需要修改!
    /// </summary>

    public void AskHandleSuteHai()
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

            if( m_request == ERequestType.Ron_OrNot )
            {
                bool enableRon = false;
                if( enableRon == true ){
                    AskRonOrNot();
                    return;
                }
            }
            else if( m_request == ERequestType.PonKan_OrNot )
            {
                bool enablePonOrKan = false;
                if( enablePonOrKan == true ){
                    AskPonOrKan();
                    return;
                }
            }
            else if( m_request == ERequestType.Chii_OrNot )
            {
                int relation = getRelation(m_kazeFrom, m_activePlayer.JiKaze);
                if( relation == (int)ERelation.KaMiCha )
                {
                    bool enableChii = false;
                    if( enableChii == true ){
                        AskChii();
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

    public void AskRonOrNot()
    {
        // wait for response.
    }
    public void OnResponse_RonOrNot()
    {
        EventID response = EventID.Nagashi;
        if( response == EventID.Ron_Agari ){
            HandleRon();
        }
        else{
            AskHandleSuteHai();
        }
    }

    public void AskPonOrKan()
    {
        
    }
    public void OnResponse_PonOrKan()
    {
        EventID response = EventID.Nagashi;
        if( response == EventID.DaiMinKan ){
            // set kan.
            PickRinshanHai();
        }
        else if(response == EventID.Pon){
            // set pon.
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;
            m_kazeFrom = m_activePlayer.JiKaze;

            ResetAskPurpose();
            AskHandleSuteHai();
        }
        else{
            AskHandleSuteHai();
        }
    }

    public void AskChii()
    {
        // wait for response
    }
    public void OnResponse_Chii()
    {
        EventID response = EventID.Nagashi;
        if( response == EventID.Chii_Left ||
           response == EventID.Chii_Center ||
           response == EventID.Chii_Right)
        {
            // set chii.
            // 1. active player sute a hai.
            // 2. active player's hou add a hai.

            //Hai suteHai = m_suteHai;
            m_kazeFrom = m_activePlayer.JiKaze;

            ResetAskPurpose();
            AskHandleSuteHai();
        }
        else{
            GoToNextLoop();
        }
    }


    public void GoToNextLoop()
    {
        // イベントを発行した風を更新する。
        m_kazeFrom = m_kazeFrom.Next();
        m_activePlayer = getPlayer( m_kazeFrom );

        PickNewTsumoHai();
    }

    // some one has tsumo.
    protected void HandleTsumo()
    {
        
    }

    // some one has ron.
    protected void HandleRon()
    {
        
    }

    // kan count over 4 but no one agari.
    protected void HandleInvalidKyoku()
    {
        
    }


    #region Codes to refactor
    public bool HasTsumo()
    {
        int tsumoNokori = m_yama.getTsumoNokori();
        if( tsumoNokori <= 0 ) {
            m_isLast = true;
        }
        else if( tsumoNokori < 66 ) {
            m_isChiihou = false;
        }

        // イベント（ツモ）を発行する。
        EventID retEventID = tsumoEvent();

        AgariParam param = new AgariParam(this);
        int score = 0;
        int iPlayer = 0;

        // イベントを処理する。
        switch( retEventID ) 
        {
            case EventID.Tsumo_Agari:// ツモあがり.
            {                
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
                PostUIEvent( UIEventID.Tsumo_Agari, m_kazeFrom, m_kazeTo );

                // 親を更新する。
                if( m_oyaIndex != getPlayerIndex( m_kazeFrom ) ) {
                    m_oyaIndex++;
                    if( m_oyaIndex >= m_playerList.Count ) {
                        m_oyaIndex = 0;
                    }
                    m_honba = 0;
                }
                else {
                    m_renchan = true;
                    m_honba++;
                }
            }
            return true;

            case EventID.Ron_Agari:// ロン
            {                
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

                getPlayer( m_kazeFrom ).increaseTenbou( score );
                getPlayer( m_kazeTo ).reduceTenbou( score );

                m_agariInfo.agariScore = score - (m_honba * 300);

                // 点数を清算する。
                m_activePlayer.increaseTenbou( m_reachbou * 1000 );

                // リーチ棒の数を初期化する。
                m_reachbou = 0;

                // UIイベント（ロン）を発行する。
                PostUIEvent( UIEventID.Ron_Agari, m_kazeFrom, m_kazeTo );

                // 親を更新する。
                if( m_oyaIndex != getPlayerIndex( m_kazeFrom ) ) {
                    m_oyaIndex++;
                    if( m_oyaIndex >= m_playerList.Count ) {
                        m_oyaIndex = 0;
                    }
                    m_honba = 0;
                }
                else {
                    m_renchan = true;
                    m_honba++;
                }
            }
            return true;
        }

        return false;
    }


    public override void PostUIEvent(UIEventID eventId, EKaze kazeFrom = EKaze.Ton, EKaze kazeTo = EKaze.Ton)
    {
        EventManager.Get().SendUIEvent(eventId, kazeFrom, kazeTo);
    }

    // 自摸
    public EventID tsumoEvent() 
    {
        // アクティブプレイヤーを設定する。
        m_activePlayer = getPlayer(m_kazeFrom);

        m_isTsumo = true;

        // UIイベント（ツモ）を発行する。
        PostUIEvent(UIEventID.PickHai, m_kazeFrom, m_kazeFrom);

        // イベント（ツモ）を発行する。
        EventID result = EventID.Nagashi;
            m_activePlayer.HandleEvent(EventID.PickHai, m_kazeFrom, m_kazeFrom, null);

        m_isTenhou = false;
        m_isTsumo = false;

        // UIイベント（進行待ち）を発行する。
        PostUIEvent(UIEventID.UI_Wait_Progress, m_kazeFrom, m_kazeFrom);

        int sutehaiIndex;
        Hai[] kanHais;

        if( result != EventID.Reach )
            m_activePlayer.IsIppatsu = false;

        // イベントを処理する。
        switch( result ) 
        {
            case EventID.Ankan: 
            {
                m_isChiihou = false;

                m_activePlayer.Tehai.addJyunTehai( m_tsumoHai );
                sutehaiIndex = m_activePlayer.getSutehaiIndex();
                kanHais = m_activePlayer.getAction().getKanHais();
                m_activePlayer.Tehai.setAnKan( kanHais[sutehaiIndex] );

                // イベントを通知する。
                result = PostGameEvent( EventID.Ankan, m_kazeFrom, m_kazeFrom );

                // UIイベント（進行待ち）を発行する。
                PostUIEvent( UIEventID.UI_Wait_Progress );

                // ツモ牌を取得する。
                m_tsumoHai = m_yama.PickRinshanTsumoHai();

                // イベント（ツモ）を発行する。
                m_isRinshan = true;
                result = tsumoEvent();
                m_isRinshan = false;
            }
            break;

            case EventID.Kakan:
            break;

            case EventID.Tsumo_Agari:// ツモあがり
                break;

            case EventID.SuteHai:// 捨牌
            {
                // 捨牌のインデックスを取得する。
                sutehaiIndex = m_activePlayer.getSutehaiIndex();

                // 理牌の間をとる。
                setSutehaiIndex( sutehaiIndex );

                PostUIEvent( UIEventID.UI_Wait_Rihai, m_kazeFrom, m_kazeFrom );

                if( sutehaiIndex >= m_activePlayer.Tehai.getJyunTehai().Length ) {// ツモ切り
                    Hai.copy( m_suteHai, m_tsumoHai );
                    m_activePlayer.Hou.addHai( m_suteHai );
                }
                else {// 手出し
                    m_activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, sutehaiIndex );
                    m_activePlayer.Tehai.removeJyunTehai( sutehaiIndex );
                    m_activePlayer.Tehai.addJyunTehai( m_tsumoHai );
                    m_activePlayer.Hou.addHai( m_suteHai );
                    m_activePlayer.Hou.setTedashi( true );
                }

                m_suteHaiList.Add( new SuteHai( m_suteHai ) );

                if( !m_activePlayer.IsReach )
                    m_activePlayer.SuteHaisCount = m_suteHaiList.Count;

                // イベントを通知する。
                result = PostGameEvent( EventID.SuteHai, m_kazeFrom, m_kazeFrom );
            }
            break;

            case EventID.Reach: 
            {
                // 捨牌のインデックスを取得する。
                sutehaiIndex = m_activePlayer.getSutehaiIndex();
                m_activePlayer.IsReach = true;

                if( m_isChiihou ) {
                    m_activePlayer.IsDoubleReach = true;
                }

                m_activePlayer.SuteHaisCount = m_suteHaiList.Count;

                PostUIEvent( UIEventID.UI_Wait_Rihai, m_kazeFrom, m_kazeFrom );

                if( sutehaiIndex >= m_activePlayer.Tehai.getJyunTehai().Length ) {// ツモ切り
                    Hai.copy( m_suteHai, m_tsumoHai );
                    m_activePlayer.Hou.addHai( m_suteHai );
                    m_activePlayer.Hou.setReach( true );
                }
                else {// 手出し
                    m_activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, sutehaiIndex );
                    m_activePlayer.Tehai.removeJyunTehai( sutehaiIndex );
                    m_activePlayer.Tehai.addJyunTehai( m_tsumoHai );
                    m_activePlayer.Hou.addHai( m_suteHai );
                    m_activePlayer.Hou.setTedashi( true );
                    m_activePlayer.Hou.setReach( true );
                }

                m_suteHaiList.Add( new SuteHai( m_suteHai ) );

                m_activePlayer.reduceTenbou( 1000 );
                m_activePlayer.IsReach = true;
                m_reachbou++;

                m_activePlayer.IsIppatsu = true;

                // イベントを通知する。
                result = PostGameEvent( EventID.Reach, m_kazeFrom, m_kazeFrom );
            }
            break;
        }

        return result;
    }


    public EventID PostGameEvent(EventID eventId, EKaze kazeFrom = EKaze.Ton, EKaze kazeTo = EKaze.Ton) 
    {
        // UIイベントを発行する。
        PostUIEvent(eventId.ToUIEventID(), kazeFrom, kazeTo);

        EventID ret = EventID.Nagashi;
        int iSuteHai;

        //--------------------------------check if someone will Ron.----------------------------
        EKaze nextKaze = kazeFrom;

        switch( eventId ) 
        {
            case EventID.Pon:
            case EventID.Chii_Center:
            case EventID.Chii_Left:
            case EventID.Chii_Right:
            case EventID.DaiMinKan:
            case EventID.SuteHai:
            case EventID.Reach:
            {
                nextKaze = kazeFrom.Next();

                for( int i = 0; i < m_playerList.Count - 1; i++ ) 
                {
                    // アクティブプレイヤーを設定する。
                    m_activePlayer = getPlayer(nextKaze);

                    ret = EventID.Nagashi;
                        m_activePlayer.HandleEvent(EventID.Ron_Check, kazeFrom, nextKaze, null);

                    if( ret == EventID.Ron_Agari ) {
                        // アクティブプレイヤーを設定する。
                        this.m_kazeFrom = nextKaze;
                        this.m_kazeTo = kazeFrom;
                        m_activePlayer = getPlayer(this.m_kazeFrom); // foucs on Ron player.

                        return ret;
                    }

                    nextKaze = kazeFrom.Next();
                }
            }
            break;
        }

        //----------------- Nobody ron, then notify other players to handle the Sutehai -----------------------------

        nextKaze = kazeFrom;

        // 各プレイヤーにイベントを通知する。       
        for (int i = 0; i < m_playerList.Count; i++) 
        {
            // アクティブプレイヤーを設定する。
            m_activePlayer = getPlayer(nextKaze);

            // イベントを発行する。
            kazeTo = nextKaze;
            ret = EventID.Nagashi;
                m_activePlayer.HandleEvent(eventId, kazeFrom, kazeTo, null);

            if (ret != EventID.Nagashi) 
            {
                for (int k = 0; k < m_playerList.Count; k++) {
                    m_playerList[k].IsIppatsu = false;
                }
            }

            // イベントを処理する。
            switch (ret)
            {
                case EventID.Tsumo_Agari:// ツモあがり
                {
                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = nextKaze;
                    this.m_kazeTo = kazeTo;
                    m_activePlayer = getPlayer( this.m_kazeFrom );
                }
                goto NOTIFYLOOP_End;

                case EventID.Ron_Agari:// ロン
                {
                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = kazeTo;
                    this.m_kazeTo = kazeFrom;
                    m_activePlayer = getPlayer( this.m_kazeFrom );
                }
                goto NOTIFYLOOP_End;

                case EventID.Pon: 
                {
                    m_isChiihou = false;

                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = nextKaze;
                    this.m_kazeTo = kazeFrom;

                    m_activePlayer = getPlayer( this.m_kazeFrom );
                    m_activePlayer.Tehai.setPon( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    PostGameEvent( EventID.Select_SuteHai, this.m_kazeFrom, this.m_kazeTo );

                    // 捨牌のインデックスを取得する。
                    iSuteHai = m_activePlayer.getSutehaiIndex();
                    m_activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, iSuteHai );
                    m_activePlayer.Tehai.removeJyunTehai( iSuteHai );

                    m_activePlayer.Hou.addHai( m_suteHai );
                    //activePlayer.getHou.setNaki(true);
                    m_activePlayer.Hou.setTedashi( true );

                    m_suteHaiList.Add( new SuteHai( m_suteHai ) );

                    // イベントを通知する。
                    ret = PostGameEvent( EventID.Pon, this.m_kazeFrom, this.m_kazeTo );
                }
                goto NOTIFYLOOP_End;

                case EventID.Chii_Left: 
                {
                    m_isChiihou = false;

                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = nextKaze;
                    this.m_kazeTo = kazeFrom;

                    m_activePlayer = getPlayer( this.m_kazeFrom );
                    m_activePlayer.Tehai.setChiiLeft( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    PostGameEvent( EventID.Select_SuteHai, this.m_kazeFrom, this.m_kazeTo );

                    // 捨牌のインデックスを取得する。
                    iSuteHai = m_activePlayer.getSutehaiIndex();
                    m_activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, iSuteHai );
                    m_activePlayer.Tehai.removeJyunTehai( iSuteHai );

                    m_activePlayer.Hou.addHai( m_suteHai );
                    //activePlayer.getHou.setNaki(true);
                    m_activePlayer.Hou.setTedashi( true );

                    m_suteHaiList.Add( new SuteHai( m_suteHai ) );

                    // イベントを通知する。
                    ret = PostGameEvent( EventID.Chii_Left, this.m_kazeFrom, this.m_kazeTo );
                }
                goto NOTIFYLOOP_End;

                case EventID.Chii_Center: 
                {
                    m_isChiihou = false;

                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = nextKaze;
                    this.m_kazeTo = kazeFrom;

                    m_activePlayer = getPlayer( this.m_kazeFrom );
                    m_activePlayer.Tehai.setChiiCenter( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    PostGameEvent( EventID.Select_SuteHai, this.m_kazeFrom, this.m_kazeTo );

                    // 捨牌のインデックスを取得する。
                    iSuteHai = m_activePlayer.getSutehaiIndex();
                    m_activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, iSuteHai );
                    m_activePlayer.Tehai.removeJyunTehai( iSuteHai );

                    m_activePlayer.Hou.addHai( m_suteHai );
                    //activePlayer.getHou.setNaki(true);
                    m_activePlayer.Hou.setTedashi( true );

                    m_suteHaiList.Add( new SuteHai( m_suteHai ) );

                    // イベントを通知する。
                    ret = PostGameEvent( EventID.Chii_Center, this.m_kazeFrom, this.m_kazeTo );
                }
                goto NOTIFYLOOP_End;

                case EventID.Chii_Right: 
                {
                    m_isChiihou = false;

                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = nextKaze;
                    this.m_kazeTo = kazeFrom;

                    m_activePlayer = getPlayer( this.m_kazeFrom );
                    m_activePlayer.Tehai.setChiiRight( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    PostGameEvent( EventID.Select_SuteHai, this.m_kazeFrom, this.m_kazeTo );

                    // 捨牌のインデックスを取得する。
                    iSuteHai = m_activePlayer.getSutehaiIndex();
                    m_activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, iSuteHai );
                    m_activePlayer.Tehai.removeJyunTehai( iSuteHai );

                    m_activePlayer.Hou.addHai( m_suteHai );
                    //activePlayer.getHou.setNaki(true);
                    m_activePlayer.Hou.setTedashi( true );

                    m_suteHaiList.Add( new SuteHai( m_suteHai ) );

                    // イベントを通知する。
                    ret = PostGameEvent( EventID.Chii_Right, this.m_kazeFrom, this.m_kazeTo );
                }
                goto NOTIFYLOOP_End;

                case EventID.DaiMinKan: 
                {
                    m_isChiihou = false;

                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = nextKaze;
                    this.m_kazeTo = kazeFrom;

                    m_activePlayer = getPlayer( this.m_kazeFrom );
                    m_activePlayer.Tehai.setDaiMinKan( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    // イベントを通知する。
                    ret = PostGameEvent( EventID.DaiMinKan, this.m_kazeFrom, this.m_kazeTo );

                    // UIイベント（進行待ち）を発行する。
                    PostUIEvent( UIEventID.UI_Wait_Progress );

                    // ツモ牌を取得する。
                    m_tsumoHai = m_yama.PickRinshanTsumoHai();

                    // イベント（ツモ）を発行する。
                    m_isRinshan = true;
                    ret = tsumoEvent();
                    m_isRinshan = false;
                }
                goto NOTIFYLOOP_End;
            }

            if (eventId == EventID.Select_SuteHai)
                return ret;
        } // end for().
        
        NOTIFYLOOP_End: {
            
        }

        // アクティブプレイヤーを設定する。
        m_activePlayer = getPlayer(kazeFrom);

        return ret;
    }
    #endregion

    #region Other Method
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
        m_players[iPlayer].getTehai().setPon(new Hai(0), getRelation(this.m_kazeFrom, this.m_kazeTo));
        m_players[iPlayer].getTehai().setPon(new Hai(31), getRelation(this.m_kazeFrom, this.m_kazeTo));

        // test ChiiLeft.
        m_players[iPlayer].getTehai().removeJyunTehai(0);
        m_players[iPlayer].getTehai().setChiiLeft(new Hai(0), getRelation(this.m_kazeFrom, this.m_kazeTo));
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
