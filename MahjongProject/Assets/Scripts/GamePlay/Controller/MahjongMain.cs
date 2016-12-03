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

    protected EventID retEventID = EventID.None;
    protected int score = 0;
    protected int iPlayer = 0;

    public MahjongMain() : base()
    {
        current = this;
    }

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
        m_playerList.Add( new Player("A", new Man()) );
        m_playerList.Add( new Player("B", new AI()) );
        m_playerList.Add( new Player("C", new AI()) );
        m_playerList.Add( new Player("D", new AI()) );

        m_tenpaiFlags = new bool[m_playerList.Count];
        for(int i = 0; i < m_playerList.Count; i++)
        {
            m_playerList[i].Tenbou = GameSettings.TENBOU_INIT;
            m_tenpaiFlags[i] = false;
        }

        // プレイヤーに提供する情報を作成する。
        m_info = new Info(this);

        // UIに提供する情報を作成する。
        m_infoUi = new GameInfo(this);
    }

    public void SetChiicha() {
        m_oyaIndex = (m_sais[0].Num + m_sais[1].Num - 1) % 4;
        m_chiichaIndex = m_oyaIndex;
    }


    /// <summary>
    /// 准备牌山.
    /// </summary>

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
        setPlayerKaze();

        // イベントを発行した風を初期化する。
        m_kazeFrom = m_playerList[m_oyaIndex].JiKaze;

        // イベントの対象となった風を初期化する。
        m_kazeTo = m_playerList[m_oyaIndex].JiKaze;

        // プレイヤー配列を初期化する。
        for( int i = 0; i < m_playerList.Count; i++ ) {
            m_playerList[i].Init();
        }

        m_suteHaiList.Clear();

        // 洗牌する。
        m_yama.XiPai();
    }

    /// <summary>
    /// 发牌.
    /// </summary>

    public void SetWaremeAndHaipai() 
    {
        // 山に割れ目を設定する。
        setWareme(m_sais);

        // 配牌する。
        Haipai();
    }

    public void PrepareToStart()
    {
        
    }

    public bool IsLastKyoku() {
        return (int)m_kyoku >= GameSettings.Kyoku_Max;
    }
    public void GoToNextKyoku() {
        m_kyoku++;
    }

    /// <summary>
    /// 牌发好，游戏开始循环.
    /// </summary>
    /*
    public void StartGameLoop() {
        PickNewTsumoHai();

        if( IsLastHai() ) // ツモ牌がない場合、流局する。
        {
            if( IsLiujuManGuan() == true ) {
                Debug.Log("do ryuu kyo ku anim.");
            }
            else {
                HasTenpai();
            }
        }
        else {
            if( HasTsumo() == true ) {
                Debug.Log( "do tsumo anim." );
            }
            else {
                PrepareToNextLoop();
            }
        }
    }
    */

    public void PickNewTsumoHai() {
        // ツモ牌を取得する。
        m_tsumoHai = m_yama.PickTsumoHai();
    }

    public bool IsLastHai() {
        // ツモ牌がない場合、流局する。
        return m_tsumoHai == null;
    }

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
                        iPlayer = (iPlayer + 1) % 4;
                        m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.oyaTsumo + (m_honba * 100) );
                    }
                }
                else 
                {
                    score = m_agariInfo.scoreInfo.koRon + (m_honba * 300);

                    for( int l = 0; l < 3; l++ )
                    {
                        iPlayer = (iPlayer + 1) % 4;
                        if( m_oyaIndex == iPlayer ) {
                            m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.oyaTsumo + (m_honba * 100) );
                        }
                        else {
                            m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.koTsumo + (m_honba * 100) );
                        }
                    }
                }

                //1. add NagashiMangan score.
                activePlayer.increaseTenbou( score );

                m_agariInfo.agariScore = score - (m_honba * 300);

                // 点数を清算する。//2. add reach bou score.
                activePlayer.increaseTenbou( m_reachbou * 1000 );

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
        int tenpaiCount = 0;
        for( int i = 0; i < m_playerList.Count; i++ )
        {
            m_tenpaiFlags[i] = m_playerList[i].isTenpai();
            if( m_tenpaiFlags[i] )
                tenpaiCount++;
        }

        int increasedScore = 0;
        int reducedScore = 0;

        switch( tenpaiCount ) {
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

        for( int i = 0; i < m_tenpaiFlags.Length; i++ )
        {
            if( m_tenpaiFlags[i] == true ){
                getPlayer(i).increaseTenbou( increasedScore );
            }
            else {
                getPlayer(i).reduceTenbou( reducedScore );
            }
        }

        // UIイベント（流局）を発行する。
        PostUIEvent( UIEventID.RyuuKyoku );

        // フラグを落としておく。
        for( int i = 0; i < m_tenpaiFlags.Length; i++ )
            m_tenpaiFlags[i] = false;

        // 親を更新する。上がり連荘とする。
        m_oyaIndex++;
        if( m_oyaIndex >= m_playerList.Count )
            m_oyaIndex = 0;

        // 本場を増やす。
        m_honba++;
    }


    public bool HasTsumo()
    {
        int tsumoNokori = m_yama.getTsumoNokori();
        if( tsumoNokori == 0 ) {
            m_isLast = true;
        }
        else if( tsumoNokori < 66 ) {
            m_isChiihou = false;
        }

        // イベント（ツモ）を発行する。
        retEventID = tsumoEvent();

        AgariParam param = new AgariParam(this);

        // イベントを処理する。
        switch( retEventID ) 
        {
            case EventID.Tsumo_Agari:// ツモあがり.
            {                
                param.setOmoteDoraHais( getOmotoDoras() );
                if( activePlayer.IsReach )                    
                    param.setUraDoraHais( getUraDoras() );

                AgariScoreManager.GetAgariScore( activePlayer.Tehai, m_tsumoHai, param, ref m_combis, ref m_agariInfo );

                iPlayer = getPlayerIndex( m_kazeFrom );
                if( m_oyaIndex == iPlayer ) {
                    score = m_agariInfo.scoreInfo.oyaRon + (m_honba * 300);
                    for( int i = 0; i < 3; i++ )
                    {
                        iPlayer = (iPlayer + 1) % 4;
                        m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.oyaTsumo + (m_honba * 100) );
                    }
                }
                else {
                    score = m_agariInfo.scoreInfo.koRon + (m_honba * 300);
                    for( int i = 0; i < 3; i++ )
                    {
                        iPlayer = (iPlayer + 1) % 4;
                        if( m_oyaIndex == iPlayer ) {
                            m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.oyaTsumo + (m_honba * 100) );
                        }
                        else {
                            m_playerList[iPlayer].reduceTenbou( m_agariInfo.scoreInfo.koTsumo + (m_honba * 100) );
                        }
                    }
                }

                activePlayer.increaseTenbou( score ); //1. add Tsumo score.

                m_agariInfo.agariScore = score - (m_honba * 300);

                // 点数を清算する。//2. add reach bou score.
                activePlayer.increaseTenbou( m_reachbou * 1000 );

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
                if( activePlayer.IsReach )                    
                    param.setUraDoraHais( getUraDoras() );

                AgariScoreManager.GetAgariScore( activePlayer.Tehai, m_suteHai, param, ref m_combis, ref m_agariInfo );

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
                activePlayer.increaseTenbou( m_reachbou * 1000 );

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

    public void GoToNextLoop()
    {
        // イベントを発行した風を更新する。
        int tmp = (int)m_kazeFrom;
        tmp++;
        if( tmp >= m_playerList.Count ) {
            tmp = 0;
        }
        m_kazeFrom = (EKaze)tmp;
    }


    // 自摸
    public EventID tsumoEvent() 
    {
        // アクティブプレイヤーを設定する。
        activePlayer = getPlayer(m_kazeFrom);

        m_isTsumo = true;

        // UIイベント（ツモ）を発行する。
        PostUIEvent(UIEventID.PickHai, m_kazeFrom, m_kazeFrom);

        // イベント（ツモ）を発行する。
        EventID result = activePlayer.HandleEvent(EventID.PickHai, m_kazeFrom, m_kazeFrom, null);

        m_isTenhou = false;
        m_isTsumo = false;

        // UIイベント（進行待ち）を発行する。
        PostUIEvent(UIEventID.UI_Wait_Progress, m_kazeFrom, m_kazeFrom);

        int sutehaiIndex;
        Hai[] kanHais;

        if( result != EventID.Reach ) {
            activePlayer.IsIppatsu = false;
        }

        // イベントを処理する。
        switch( result ) 
        {
            case EventID.Ankan: 
            {
                m_isChiihou = false;

                activePlayer.Tehai.addJyunTehai( m_tsumoHai );
                sutehaiIndex = activePlayer.getSutehaiIndex();
                kanHais = activePlayer.getAction().getKanHais();
                activePlayer.Tehai.setAnKan( kanHais[sutehaiIndex] );

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

            case EventID.Tsumo_Agari:// ツモあがり
                break;

            case EventID.SuteHai:// 捨牌
            {
                // 捨牌のインデックスを取得する。
                sutehaiIndex = activePlayer.getSutehaiIndex();

                // 理牌の間をとる。
                m_infoUi.setSutehaiIndex( sutehaiIndex );

                PostUIEvent( UIEventID.UI_Wait_Rihai, m_kazeFrom, m_kazeFrom );

                if( sutehaiIndex >= activePlayer.Tehai.getJyunTehai().Length ) {// ツモ切り
                    Hai.copy( m_suteHai, m_tsumoHai );
                    activePlayer.Hou.addHai( m_suteHai );
                }
                else {// 手出し
                    activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, sutehaiIndex );
                    activePlayer.Tehai.removeJyunTehai( sutehaiIndex );
                    activePlayer.Tehai.addJyunTehai( m_tsumoHai );
                    activePlayer.Hou.addHai( m_suteHai );
                    activePlayer.Hou.setTedashi( true );
                }

                m_suteHaiList.Add( new SuteHai( m_suteHai ) );

                if( !activePlayer.IsReach )
                    activePlayer.SuteHaisCount = m_suteHaiList.Count;

                // イベントを通知する。
                result = PostGameEvent( EventID.SuteHai, m_kazeFrom, m_kazeFrom );
            }
            break;

            case EventID.Reach: 
            {
                // 捨牌のインデックスを取得する。
                sutehaiIndex = activePlayer.getSutehaiIndex();
                activePlayer.IsReach = true;

                if( m_isChiihou ) {
                    activePlayer.IsDoubleReach = true;
                }

                activePlayer.SuteHaisCount = m_suteHaiList.Count;

                PostUIEvent( UIEventID.UI_Wait_Rihai, m_kazeFrom, m_kazeFrom );

                if( sutehaiIndex >= activePlayer.Tehai.getJyunTehai().Length ) {// ツモ切り
                    Hai.copy( m_suteHai, m_tsumoHai );
                    activePlayer.Hou.addHai( m_suteHai );
                    activePlayer.Hou.setReach( true );
                }
                else {// 手出し
                    activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, sutehaiIndex );
                    activePlayer.Tehai.removeJyunTehai( sutehaiIndex );
                    activePlayer.Tehai.addJyunTehai( m_tsumoHai );
                    activePlayer.Hou.addHai( m_suteHai );
                    activePlayer.Hou.setTedashi( true );
                    activePlayer.Hou.setReach( true );
                }

                m_suteHaiList.Add( new SuteHai( m_suteHai ) );

                activePlayer.reduceTenbou( 1000 );
                activePlayer.IsReach = true;
                m_reachbou++;

                activePlayer.IsIppatsu = true;

                // イベントを通知する。
                result = PostGameEvent( EventID.Reach, m_kazeFrom, m_kazeFrom );
            }
            break;
        }

        return result;
    }




    public override void PostUIEvent(UIEventID eventId, EKaze kazeFrom = EKaze.Ton, EKaze kazeTo = EKaze.Ton)
    {
        EventManager.Get().SendUIEvent(eventId, kazeFrom, kazeTo);
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
                    activePlayer = getPlayer(nextKaze);

                    ret = activePlayer.HandleEvent(EventID.Ron_Check, kazeFrom, nextKaze, null);

                    if( ret == EventID.Ron_Agari ) {
                        // アクティブプレイヤーを設定する。
                        this.m_kazeFrom = nextKaze;
                        this.m_kazeTo = kazeFrom;
                        activePlayer = getPlayer(this.m_kazeFrom); // foucs on Ron player.

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
            activePlayer = getPlayer(nextKaze);

            // イベントを発行する。
            kazeTo = nextKaze;
            ret = activePlayer.HandleEvent(eventId, kazeFrom, kazeTo, null);

            if (ret != EventID.Nagashi) 
            {
                for (int k = 0; k < 4; k++) {
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
                    activePlayer = getPlayer( this.m_kazeFrom );
                }
                goto NOTIFYLOOP_End;

                case EventID.Ron_Agari:// ロン
                {
                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = kazeTo;
                    this.m_kazeTo = kazeFrom;
                    activePlayer = getPlayer( this.m_kazeFrom );
                }
                goto NOTIFYLOOP_End;

                case EventID.Pon: 
                {
                    m_isChiihou = false;

                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = nextKaze;
                    this.m_kazeTo = kazeFrom;

                    activePlayer = getPlayer( this.m_kazeFrom );
                    activePlayer.Tehai.setPon( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    PostGameEvent( EventID.Select_SuteHai, this.m_kazeFrom, this.m_kazeTo );

                    // 捨牌のインデックスを取得する。
                    iSuteHai = activePlayer.getSutehaiIndex();
                    activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, iSuteHai );
                    activePlayer.Tehai.removeJyunTehai( iSuteHai );

                    activePlayer.Hou.addHai( m_suteHai );
                    //activePlayer.getHou.setNaki(true);
                    activePlayer.Hou.setTedashi( true );

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

                    activePlayer = getPlayer( this.m_kazeFrom );
                    activePlayer.Tehai.setChiiLeft( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    PostGameEvent( EventID.Select_SuteHai, this.m_kazeFrom, this.m_kazeTo );

                    // 捨牌のインデックスを取得する。
                    iSuteHai = activePlayer.getSutehaiIndex();
                    activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, iSuteHai );
                    activePlayer.Tehai.removeJyunTehai( iSuteHai );

                    activePlayer.Hou.addHai( m_suteHai );
                    //activePlayer.getHou.setNaki(true);
                    activePlayer.Hou.setTedashi( true );

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

                    activePlayer = getPlayer( this.m_kazeFrom );
                    activePlayer.Tehai.setChiiCenter( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    PostGameEvent( EventID.Select_SuteHai, this.m_kazeFrom, this.m_kazeTo );

                    // 捨牌のインデックスを取得する。
                    iSuteHai = activePlayer.getSutehaiIndex();
                    activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, iSuteHai );
                    activePlayer.Tehai.removeJyunTehai( iSuteHai );

                    activePlayer.Hou.addHai( m_suteHai );
                    //activePlayer.getHou.setNaki(true);
                    activePlayer.Hou.setTedashi( true );

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

                    activePlayer = getPlayer( this.m_kazeFrom );
                    activePlayer.Tehai.setChiiRight( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                    getPlayer( this.m_kazeTo ).Hou.setNaki( true );

                    PostGameEvent( EventID.Select_SuteHai, this.m_kazeFrom, this.m_kazeTo );

                    // 捨牌のインデックスを取得する。
                    iSuteHai = activePlayer.getSutehaiIndex();
                    activePlayer.Tehai.copyJyunTehaiIndex( m_suteHai, iSuteHai );
                    activePlayer.Tehai.removeJyunTehai( iSuteHai );

                    activePlayer.Hou.addHai( m_suteHai );
                    //activePlayer.getHou.setNaki(true);
                    activePlayer.Hou.setTedashi( true );

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

                    activePlayer = getPlayer( this.m_kazeFrom );
                    activePlayer.Tehai.setDaiMinKan( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
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

            if (eventId == EventID.Select_SuteHai) {
                return ret;
            }
        } // end for().
        
        NOTIFYLOOP_End: {
            
        }

        // アクティブプレイヤーを設定する。
        activePlayer = getPlayer(kazeFrom);

        return ret;
    }


    #region Other Method
    protected override void Haipai()
    {
        base.Haipai();

        if( testHaipai == true ) 
        {
            int[] haiIds = getTestHaiIds();
            
            // remove all the hais of player 0.
            int iPlayer = 0;
            while( m_playerList[iPlayer].Tehai.getJyunTehai().Length > 0 ) {
                m_playerList[iPlayer].Tehai.removeJyunTehai(0);
            }

            // add the test hais.
            for( int i = 0; i < haiIds.Length - 1; i++ ) {
                m_playerList[iPlayer].Tehai.addJyunTehai( new Hai(haiIds[i]) );
            }

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
