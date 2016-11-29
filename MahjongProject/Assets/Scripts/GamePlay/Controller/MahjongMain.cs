using UnityEngine;
using System.Collections;


/**
 *  game logic manage.
 */ 

public class MahjongMain : Mahjong 
{
    bool testHaipai = false;

    AgariSetting arg_setting;
    EventId retEid = EventId.None;
    int score = 0;
    int iPlayer = 0;


    /**
     * 初期化する。 数据初始化.
     */
    public override void initialize() {
        // 山を作成する。
        m_yama = new Yama();

        // 赤ドラを設定する。
        if( MahjongSetting.UseRedDora ) {
            m_yama.setRedDora(Hai.ID_PIN_5, 2);
            m_yama.setRedDora(Hai.ID_WAN_5, 1);
            m_yama.setRedDora(Hai.ID_SOU_5, 1);
        }

        // ツモ牌を作成する。
        m_tsumoHai = new Hai();

        // 捨牌を作成する。
        m_suteHai = new Hai();
        m_suteHaisCount = 0;

        // リーチ棒の数を初期化する。
        m_reachbou = 0;

        // 本場を初期化する。
        m_honba = 0;

        // 局を初期化する。
        m_kyoku = KYOKU_TON_1;


        // プレイヤーの人数を設定する。
        m_playerNum = MahjongSetting.PlayerNum;
        
        // プレイヤーの配列を初期化する。
        m_players = new Player[m_playerNum];
        //m_players[0] = new Player((IPlayer)new Man(m_info, "A", m_playerAction));
        m_players[0] = new Player((IPlayer)new AI(m_info, "A"));
        m_players[1] = new Player((IPlayer)new AI(m_info, "B"));
        m_players[2] = new Player((IPlayer)new AI(m_info, "C"));
        m_players[3] = new Player((IPlayer)new AI(m_info, "D"));

        for( int i = 0; i < m_playerNum; i++ ) {
            m_players[i].setTenbou(MahjongSetting.TENBOU_INIT);
        }

        // 風をプレイヤーインデックスに変換する配列を初期化する。
        m_kazeToPlayerIndex = new int[m_players.Length];


        // プレイヤーに提供する情報を作成する。
        m_info = new Info(this);

        // UIに提供する情報を作成する。
        m_infoUi = new GameInfo(this, m_playerAction);
    }

    public void SetChiicha() {
        m_iOya = (m_sais[0].getNum() + m_sais[1].getNum() - 1) % 4;
        m_iChiicha = m_iOya;
    }


    /// <summary>
    /// 准备牌山.
    /// </summary>
    public void PrepareKyokuYama() {
        // 連荘を初期化する。
        m_renchan = false;

        m_isTenhou = true;
        m_isChiihou = true;
        m_isTsumo = false;
        m_isRinshan = false;
        m_isLast = false;

        // プレイヤーの自風を設定する。
        setJikaze();

        // イベントを発行した風を初期化する。
        m_kazeFrom = m_players[m_iOya].getJikaze();

        // イベントの対象となった風を初期化する。
        m_kazeTo = m_players[m_iOya].getJikaze();

        // プレイヤー配列を初期化する。
        for( int i = 0; i < m_players.Length; i++ ) {
            m_players[i].init();
        }

        m_suteHaisCount = 0;

        // 洗牌する。
        m_yama.XiPai();
    }

    /// <summary>
    /// 发牌.
    /// </summary>
    public void SetWaremeAndHaipai() {
        // 山に割れ目を設定する。
        setWareme(m_sais);

        // 配牌する。
        Haipai();
    }

    public void PrepareToStart() {
        arg_setting = new AgariSetting( this );
    }

    public bool IsLastKyoku() {
        return m_kyoku >= MahjongSetting.Kyoku_Max;
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
        for( int i = 0, j = m_iOya; i < m_players.Length; i++, j++ ) 
        {
            if( j >= m_players.Length ) {
                j = 0;
            }

            bool agari = true;

            Hou hou = m_players[j].getHou();
            SuteHai[] suteHais = hou.getSuteHais();
            int suteHaisLength = hou.getSuteHaisLength();

            // check 1,9,字./
            for( int k = 0; k < suteHaisLength; k++ ) {
                if( suteHais[k].isNaki() || !suteHais[k].isYaochuu() ) {
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
        for( int i = 0, j = m_iOya; i < m_players.Length; i++, j++ ) 
        {
            if( j >= m_players.Length ) {
                j = 0;
            }

            bool agari = true;
            Hou hou = m_players[j].getHou();
            SuteHai[] suteHais = hou.getSuteHais();
            int suteHaisLength = hou.getSuteHaisLength();

            // check 1,9,字./
            for( int k = 0; k < suteHaisLength; k++ ) {
                if( suteHais[k].isNaki() || !suteHais[k].isYaochuu() ) {
                    agari = false;
                    break;
                }
            }

            if( agari == true ) // count score.
            {
                m_kazeFrom = m_kazeTo = m_players[j].getJikaze();

                m_score = new AgariScore();
                m_score.setNagashiMangan( m_agariInfo ); // visitor.

                iPlayer = getPlayerIndex( m_kazeFrom );
                if( m_iOya == iPlayer ) // count chii cha score.
                {
                    score = m_agariInfo.m_score.m_oyaRon + (m_honba * 300);

                    for( int l = 0; l < 3; l++ ) {
                        iPlayer = (iPlayer + 1) % 4;
                        m_players[iPlayer].reduceTenbou( m_agariInfo.m_score.m_oyaTsumo + (m_honba * 100) );
                    }
                }
                else 
                {
                    score = m_agariInfo.m_score.m_koRon + (m_honba * 300);

                    for( int l = 0; l < 3; l++ ) {
                        iPlayer = (iPlayer + 1) % 4;
                        if( m_iOya == iPlayer ) {
                            m_players[iPlayer].reduceTenbou( m_agariInfo.m_score.m_oyaTsumo + (m_honba * 100) );
                        }
                        else {
                            m_players[iPlayer].reduceTenbou( m_agariInfo.m_score.m_koTsumo + (m_honba * 100) );
                        }
                    }
                }

                //1. add NagashiMangan score.
                activePlayer.increaseTenbou( score );

                m_agariInfo.m_agariScore = score - (m_honba * 300);

                // 点数を清算する。//2. add reach bou score.
                activePlayer.increaseTenbou( m_reachbou * 1000 );

                // リーチ棒の数を初期化する。
                m_reachbou = 0;

                // UIイベント（ツモあがり）を発行する。
                PostUIEvent( EventId.TSUMO_AGARI, m_kazeFrom, m_kazeTo );

                // 親を更新する。
                if( m_iOya != getPlayerIndex( m_kazeFrom ) ) {
                    m_iOya++;
                    if( m_iOya >= m_players.Length ) {
                        m_iOya = 0;
                    }
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


    public bool HasRyuukyokuTenpai() {
        // テンパイの確認をする。
        for( int i = 0; i < m_tenpai.Length; i++ ) {
            m_tenpai[i] = getPlayer( i ).isTenpai();
            if( m_tenpai[i] ) {
                return true;
            }
        }
        return false;
    }
    public void HandleRyuukyokuTenpai() 
    {
        int tenpaiCount = 0;
        for( int i = 0; i < m_tenpai.Length; i++ ) {
            m_tenpai[i] = getPlayer( i ).isTenpai();
            if( m_tenpai[i] ) {
                tenpaiCount++;
            }
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

        for( int i = 0; i < m_tenpai.Length; i++ ) {
            if( m_tenpai[i] ) {
                getPlayer( i ).increaseTenbou( increasedScore );
            }
            else {
                getPlayer( i ).reduceTenbou( reducedScore );
            }
        }

        // UIイベント（流局）を発行する。
        PostUIEvent( EventId.RYUUKYOKU, Kaze.KAZE_NONE, Kaze.KAZE_NONE );

        // フラグを落としておく。
        for( int i = 0; i < m_tenpai.Length; i++ ) {
            m_tenpai[i] = false;
        }

        // 親を更新する。上がり連荘とする。
        m_iOya++;
        if( m_iOya >= m_players.Length ) {
            m_iOya = 0;
        }

        // 本場を増やす。
        m_honba++;
    }


    public bool HasTsumo() {
        int tsumoNokori = m_yama.getTsumoNokori();
        if( tsumoNokori == 0 ) {
            m_isLast = true;
        }
        else if( tsumoNokori < 66 ) {
            m_isChiihou = false;
        }

        // イベント（ツモ）を発行する。
        retEid = tsumoEvent();

        // イベントを処理する。
        switch( retEid ) 
        {
        case EventId.TSUMO_AGARI:// ツモあがり.
        {
            if( activePlayer.isReach() ) {
                arg_setting.setOmoteDoraHais( m_yama.getAllDoraHais() );
            }

            m_score = new AgariScore();
            m_score.getAgariScore( activePlayer.getTehai(), m_tsumoHai, combis, arg_setting, m_agariInfo );

            iPlayer = getPlayerIndex( m_kazeFrom );
            if( m_iOya == iPlayer ) {
                score = m_agariInfo.m_score.m_oyaRon + (m_honba * 300);
                for( int i = 0; i < 3; i++ ) {
                    iPlayer = (iPlayer + 1) % 4;
                    m_players[iPlayer].reduceTenbou( m_agariInfo.m_score.m_oyaTsumo + (m_honba * 100) );
                }
            }
            else {
                score = m_agariInfo.m_score.m_koRon + (m_honba * 300);
                for( int i = 0; i < 3; i++ ) {
                    iPlayer = (iPlayer + 1) % 4;
                    if( m_iOya == iPlayer ) {
                        m_players[iPlayer].reduceTenbou( m_agariInfo.m_score.m_oyaTsumo + (m_honba * 100) );
                    }
                    else {
                        m_players[iPlayer].reduceTenbou( m_agariInfo.m_score.m_koTsumo + (m_honba * 100) );
                    }
                }
            }

            activePlayer.increaseTenbou( score ); //1. add Tsumo score.

            m_agariInfo.m_agariScore = score - (m_honba * 300);

            // 点数を清算する。//2. add reach bou score.
            activePlayer.increaseTenbou( m_reachbou * 1000 );

            // リーチ棒の数を初期化する。
            m_reachbou = 0;

            // UIイベント（ツモあがり）を発行する。
            PostUIEvent( EventId.TSUMO_AGARI, m_kazeFrom, m_kazeTo );

            // 親を更新する。
            if( m_iOya != getPlayerIndex( m_kazeFrom ) ) {
                m_iOya++;
                if( m_iOya >= m_players.Length ) {
                    m_iOya = 0;
                }
                m_honba = 0;
            }
            else {
                m_renchan = true;
                m_honba++;
            }
        }
        return true;

        case EventId.RON_AGARI:// ロン
        {
            if( activePlayer.isReach() ) {
                arg_setting.setOmoteDoraHais( m_yama.getAllDoraHais() );
            }

            m_score = new AgariScore();
            m_score.getAgariScore( activePlayer.getTehai(), m_suteHai, combis, arg_setting, m_agariInfo );

            if( m_iOya == getPlayerIndex( m_kazeFrom ) ) {
                score = m_agariInfo.m_score.m_oyaRon + (m_honba * 300);
            }
            else {
                score = m_agariInfo.m_score.m_koRon + (m_honba * 300);
            }

            getPlayer( m_kazeFrom ).increaseTenbou( score );
            getPlayer( m_kazeTo ).reduceTenbou( score );

            m_agariInfo.m_agariScore = score - (m_honba * 300);

            // 点数を清算する。
            activePlayer.increaseTenbou( m_reachbou * 1000 );

            // リーチ棒の数を初期化する。
            m_reachbou = 0;

            // UIイベント（ロン）を発行する。
            PostUIEvent( EventId.RON_AGARI, m_kazeFrom, m_kazeTo );

            // 親を更新する。
            if( m_iOya != getPlayerIndex( m_kazeFrom ) ) {
                m_iOya++;
                if( m_iOya >= m_players.Length ) {
                    m_iOya = 0;
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

    public void GoToNextLoop() {
        // イベントを発行した風を更新する。
        m_kazeFrom++;
        if( m_kazeFrom >= m_players.Length ) {
            m_kazeFrom = 0;
        }
    }



    /** 自摸 */
    public EventId tsumoEvent() 
    {
        // アクティブプレイヤーを設定する。
        activePlayer = getPlayer(m_kazeFrom);

        m_isTsumo = true;

        // UIイベント（ツモ）を発行する。
        PostUIEvent(EventId.TSUMO, m_kazeFrom, m_kazeFrom);

        // イベント（ツモ）を発行する。
        EventId retEid = activePlayer.HandleEvent(EventId.TSUMO, m_kazeFrom, m_kazeFrom);

        m_isTenhou = false;
        m_isTsumo = false;

        // UIイベント（進行待ち）を発行する。
        PostUIEvent(EventId.UI_WAIT_PROGRESS, m_kazeFrom, m_kazeFrom);

        int sutehaiIndex;
        Hai[] kanHais;

        if( retEid != EventId.REACH ) {
            activePlayer.setIppatsu(false);
        }

        // イベントを処理する。
        switch( retEid ) 
        {
        case EventId.ANKAN: {
            m_isChiihou = false;

            activePlayer.getTehai().addJyunTehai( m_tsumoHai );
            sutehaiIndex = activePlayer.getSutehaiIndex();
            kanHais = m_playerAction.getKanHais();
            activePlayer.getTehai().setAnKan( kanHais[sutehaiIndex], getRelation( this.m_kazeFrom, this.m_kazeTo ) );

            // イベントを通知する。
            retEid = PostGameEvent( EventId.ANKAN, m_kazeFrom, m_kazeFrom );

            // UIイベント（進行待ち）を発行する。
            PostUIEvent( EventId.UI_WAIT_PROGRESS, Kaze.KAZE_NONE, Kaze.KAZE_NONE );

            // ツモ牌を取得する。
            m_tsumoHai = m_yama.PickRinshanTsumoHai();

            // イベント（ツモ）を発行する。
            m_isRinshan = true;
            retEid = tsumoEvent();
            m_isRinshan = false;
        }
        break;

        case EventId.TSUMO_AGARI:// ツモあがり
            break;

        case EventId.SUTEHAI:// 捨牌
        {
            // 捨牌のインデックスを取得する。
            sutehaiIndex = activePlayer.getSutehaiIndex();

            // 理牌の間をとる。
            m_infoUi.setSutehaiIndex( sutehaiIndex );

            PostUIEvent( EventId.UI_WAIT_RIHAI, m_kazeFrom, m_kazeFrom );

            if( sutehaiIndex >= activePlayer.getTehai().getJyunTehaiLength() ) {// ツモ切り
                Hai.copy( m_suteHai, m_tsumoHai );
                activePlayer.getHou().add( m_suteHai );
            }
            else {// 手出し
                activePlayer.getTehai().copyJyunTehaiIndex( m_suteHai, sutehaiIndex );
                activePlayer.getTehai().removeJyunTehai( sutehaiIndex );
                activePlayer.getTehai().addJyunTehai( m_tsumoHai );
                activePlayer.getHou().add( m_suteHai );
                activePlayer.getHou().setTedashi( true );
            }

            m_suteHais[m_suteHaisCount] = new SuteHai( m_suteHai );
            m_suteHaisCount++;

            if( !activePlayer.isReach() ) {
                activePlayer.setSuteHaisCount( m_suteHaisCount );
            }

            // イベントを通知する。
            retEid = PostGameEvent( EventId.SUTEHAI, m_kazeFrom, m_kazeFrom );
        }
        break;

        case EventId.REACH: 
        {
            // 捨牌のインデックスを取得する。
            sutehaiIndex = activePlayer.getSutehaiIndex();
            activePlayer.setReach( true );

            if( m_isChiihou ) {
                activePlayer.setDoubleReach( true );
            }

            activePlayer.setSuteHaisCount( m_suteHaisCount );

            PostUIEvent( EventId.UI_WAIT_RIHAI, m_kazeFrom, m_kazeFrom );

            if( sutehaiIndex >= activePlayer.getTehai().getJyunTehaiLength() ) {// ツモ切り
                Hai.copy( m_suteHai, m_tsumoHai );
                activePlayer.getHou().add( m_suteHai );
                activePlayer.getHou().setReach( true );
            }
            else {// 手出し
                activePlayer.getTehai().copyJyunTehaiIndex( m_suteHai, sutehaiIndex );
                activePlayer.getTehai().removeJyunTehai( sutehaiIndex );
                activePlayer.getTehai().addJyunTehai( m_tsumoHai );
                activePlayer.getHou().add( m_suteHai );
                activePlayer.getHou().setTedashi( true );
                activePlayer.getHou().setReach( true );
            }

            m_suteHais[m_suteHaisCount] = new SuteHai( m_suteHai );
            m_suteHaisCount++;

            activePlayer.reduceTenbou( 1000 );
            activePlayer.setReach( true );
            m_reachbou++;

            activePlayer.setIppatsu( true );

            // イベントを通知する。
            retEid = PostGameEvent( EventId.REACH, m_kazeFrom, m_kazeFrom );
        }
        break;
        }

        return retEid;
    }


    public override void PostUIEvent(EventId a_eventId, int a_kazeFrom, int a_kazeTo) {
        EventManager.Get().SendEvent(a_eventId, a_kazeFrom, a_kazeTo);
    }


    public EventId PostGameEvent(EventId a_eventId, int a_kazeFrom, int a_kazeTo) 
    {
        // UIイベントを発行する。
        PostUIEvent(a_eventId, a_kazeFrom, a_kazeTo);

        EventId ret = EventId.NAGASHI;
        int iSuteHai;

        //--------------------------------check if someone will Ron.----------------------------
        switch( a_eventId ) 
        {
        case EventId.PON:
        case EventId.CHII_CENTER:
        case EventId.CHII_LEFT:
        case EventId.CHII_RIGHT:
        case EventId.DAIMINKAN:
        case EventId.SUTEHAI:
        case EventId.REACH:
            for( int i = 0, j = a_kazeFrom + 1; i < m_players.Length - 1; i++, j++ ) {
                if( j >= m_players.Length ) {
                    j = 0;
                }

                // アクティブプレイヤーを設定する。
                activePlayer = getPlayer(j);

                ret = activePlayer.HandleEvent(EventId.RON_CHECK, a_kazeFrom, j);

                if( ret == EventId.RON_AGARI ) {
                    // アクティブプレイヤーを設定する。
                    this.m_kazeFrom = j;
                    this.m_kazeTo = a_kazeFrom;
                    activePlayer = getPlayer(this.m_kazeFrom); // foucs on Ron player.

                    return ret;
                }
            }
            break;
        }

        //----------------- Nobody ron, then notify other players to handle the Sutehai -----------------------------

        // 各プレイヤーにイベントを通知する。       
        for (int i = 0, j = a_kazeFrom; i < m_players.Length; i++, j++) 
        {
            if (j >= m_players.Length) {
                j = 0;
            }

            // アクティブプレイヤーを設定する。
            activePlayer = getPlayer(j);

            // イベントを発行する。
            a_kazeTo = j;
            ret = activePlayer.HandleEvent(a_eventId, a_kazeFrom, a_kazeTo);

            if (ret != EventId.NAGASHI) 
            {
                for (int k = 0; k < 4; k++) {
                    m_players[k].setIppatsu(false);
                }
            }

            // イベントを処理する。
            switch (ret) {
            case EventId.TSUMO_AGARI:// ツモあがり
            {
                // アクティブプレイヤーを設定する。
                this.m_kazeFrom = j;
                this.m_kazeTo = a_kazeTo;
                activePlayer = getPlayer( this.m_kazeFrom );
            }
            goto NOTIFYLOOP_End;

            case EventId.RON_AGARI:// ロン
            {
                // アクティブプレイヤーを設定する。
                this.m_kazeFrom = a_kazeTo;
                this.m_kazeTo = a_kazeFrom;
                activePlayer = getPlayer( this.m_kazeFrom );
            }
            goto NOTIFYLOOP_End;

            case EventId.PON: 
            {
                m_isChiihou = false;

                // アクティブプレイヤーを設定する。
                this.m_kazeFrom = j;
                this.m_kazeTo = a_kazeFrom;

                activePlayer = getPlayer( this.m_kazeFrom );
                activePlayer.getTehai().setPon( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                getPlayer( this.m_kazeTo ).getHou().setNaki( true );

                PostGameEvent( EventId.SELECT_SUTEHAI, this.m_kazeFrom, this.m_kazeTo );

                // 捨牌のインデックスを取得する。
                iSuteHai = activePlayer.getSutehaiIndex();
                activePlayer.getTehai().copyJyunTehaiIndex( m_suteHai, iSuteHai );
                activePlayer.getTehai().removeJyunTehai( iSuteHai );

                activePlayer.getHou().add( m_suteHai );
                //activePlayer.getHou().setNaki(true);
                activePlayer.getHou().setTedashi( true );

                m_suteHais[m_suteHaisCount] = new SuteHai( m_suteHai );
                m_suteHaisCount++;

                // イベントを通知する。
                ret = PostGameEvent( EventId.PON, this.m_kazeFrom, this.m_kazeTo );
            }
            goto NOTIFYLOOP_End;

            case EventId.CHII_LEFT: 
            {
                m_isChiihou = false;

                // アクティブプレイヤーを設定する。
                this.m_kazeFrom = j;
                this.m_kazeTo = a_kazeFrom;

                activePlayer = getPlayer( this.m_kazeFrom );
                activePlayer.getTehai().setChiiLeft( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                getPlayer( this.m_kazeTo ).getHou().setNaki( true );

                PostGameEvent( EventId.SELECT_SUTEHAI, this.m_kazeFrom, this.m_kazeTo );

                // 捨牌のインデックスを取得する。
                iSuteHai = activePlayer.getSutehaiIndex();
                activePlayer.getTehai().copyJyunTehaiIndex( m_suteHai, iSuteHai );
                activePlayer.getTehai().removeJyunTehai( iSuteHai );

                activePlayer.getHou().add( m_suteHai );
                //activePlayer.getHou().setNaki(true);
                activePlayer.getHou().setTedashi( true );

                m_suteHais[m_suteHaisCount] = new SuteHai( m_suteHai );
                m_suteHaisCount++;

                // イベントを通知する。
                ret = PostGameEvent( EventId.CHII_LEFT, this.m_kazeFrom, this.m_kazeTo );
            }
            goto NOTIFYLOOP_End;

            case EventId.CHII_CENTER: 
            {
                m_isChiihou = false;

                // アクティブプレイヤーを設定する。
                this.m_kazeFrom = j;
                this.m_kazeTo = a_kazeFrom;

                activePlayer = getPlayer( this.m_kazeFrom );
                activePlayer.getTehai().setChiiCenter( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                getPlayer( this.m_kazeTo ).getHou().setNaki( true );

                PostGameEvent( EventId.SELECT_SUTEHAI, this.m_kazeFrom, this.m_kazeTo );

                // 捨牌のインデックスを取得する。
                iSuteHai = activePlayer.getSutehaiIndex();
                activePlayer.getTehai().copyJyunTehaiIndex( m_suteHai, iSuteHai );
                activePlayer.getTehai().removeJyunTehai( iSuteHai );

                activePlayer.getHou().add( m_suteHai );
                //activePlayer.getHou().setNaki(true);
                activePlayer.getHou().setTedashi( true );

                m_suteHais[m_suteHaisCount] = new SuteHai( m_suteHai );
                m_suteHaisCount++;

                // イベントを通知する。
                ret = PostGameEvent( EventId.CHII_CENTER, this.m_kazeFrom, this.m_kazeTo );
            }
            goto NOTIFYLOOP_End;

            case EventId.CHII_RIGHT: 
            {
                m_isChiihou = false;

                // アクティブプレイヤーを設定する。
                this.m_kazeFrom = j;
                this.m_kazeTo = a_kazeFrom;

                activePlayer = getPlayer( this.m_kazeFrom );
                activePlayer.getTehai().setChiiRight( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                getPlayer( this.m_kazeTo ).getHou().setNaki( true );

                PostGameEvent( EventId.SELECT_SUTEHAI, this.m_kazeFrom, this.m_kazeTo );

                // 捨牌のインデックスを取得する。
                iSuteHai = activePlayer.getSutehaiIndex();
                activePlayer.getTehai().copyJyunTehaiIndex( m_suteHai, iSuteHai );
                activePlayer.getTehai().removeJyunTehai( iSuteHai );

                activePlayer.getHou().add( m_suteHai );
                //activePlayer.getHou().setNaki(true);
                activePlayer.getHou().setTedashi( true );

                m_suteHais[m_suteHaisCount] = new SuteHai( m_suteHai );
                m_suteHaisCount++;

                // イベントを通知する。
                ret = PostGameEvent( EventId.CHII_RIGHT, this.m_kazeFrom, this.m_kazeTo );
            }
            goto NOTIFYLOOP_End;

            case EventId.DAIMINKAN: 
            {
                m_isChiihou = false;

                // アクティブプレイヤーを設定する。
                this.m_kazeFrom = j;
                this.m_kazeTo = a_kazeFrom;

                activePlayer = getPlayer( this.m_kazeFrom );
                activePlayer.getTehai().setDaiMinKan( m_suteHai, getRelation( this.m_kazeFrom, this.m_kazeTo ) );
                getPlayer( this.m_kazeTo ).getHou().setNaki( true );

                // イベントを通知する。
                ret = PostGameEvent( EventId.DAIMINKAN, this.m_kazeFrom, this.m_kazeTo );

                // UIイベント（進行待ち）を発行する。
                PostUIEvent( EventId.UI_WAIT_PROGRESS, Kaze.KAZE_NONE, Kaze.KAZE_NONE );

                // ツモ牌を取得する。
                m_tsumoHai = m_yama.PickRinshanTsumoHai();

                // イベント（ツモ）を発行する。
                m_isRinshan = true;
                ret = tsumoEvent();
                m_isRinshan = false;
            }
            goto NOTIFYLOOP_End;
            }

            if (a_eventId == EventId.SELECT_SUTEHAI) {
                return ret;
            }
        } // end for().
        
        NOTIFYLOOP_End: {
            
        }

        // アクティブプレイヤーを設定する。
        activePlayer = getPlayer(a_kazeFrom);

        return ret;
    }

    #region Other Method
    protected override void Haipai() {
        base.Haipai();

        if( testHaipai == true ) 
        {
            int[] haiIds = getTestHaiIds();
            
            // remove all the hais of player 0.
            int iPlayer = 0;
            while( m_players[iPlayer].getTehai().getJyunTehaiLength() > 0 ) {
                m_players[iPlayer].getTehai().removeJyunTehai(0);
            }

            // add the test hais.
            for( int i = 0; i < haiIds.Length - 1; i++ ) {
                m_players[iPlayer].getTehai().addJyunTehai( new Hai(haiIds[i]) );
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
        //int haiIds[] = {0, 1, 2, 3, 4, 5, 6, 7, 8, 33, 33, 33, 31, 31};
        //int haiIds[] = {29, 29, 29, 30, 30, 30, 31, 31, 31, 32, 32, 33, 33, 33};
        //int haiIds[] = {0, 1, 2, 3, 4, 5, 6, 7, 31, 31, 33, 33, 33};
        //int haiIds[] = {0, 1, 2, 10, 11, 12, 13, 14, 15, 31, 31, 33, 33, 33};
        //int haiIds[] = {0, 1, 2, 3, 4, 5, 6, 7, 31, 31, 32, 32, 32};
        //int haiIds[] = {0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8};
        //int haiIds[] = {1, 1, 3, 3, 5, 5, 7, 7, 30, 30, 31, 31, 32, 32};
        //int haiIds[] = {1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        //int haiIds[] = {0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8};
        //int haiIds[] = {27, 27, 28, 28, 29, 29, 30, 30, 31, 31, 32, 32, 33, 33};
        //int haiIds[] = {0, 0, 0, 0, 8, 8, 8, 8, 9, 9, 9, 9, 18, 18};
        //int haiIds[] = {0, 8, 9, 17, 18, 26, 27, 28, 29, 30, 31, 32, 33, 34};
        //int haiIds[] = {0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8};
        //int haiIds[] = {1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        //int haiIds[] = {19, 19, 20, 20, 21, 21, 23, 23, 23, 23, 25, 25, 25, 25};
        //int haiIds[] = {0, 0, 0, 8, 8, 8, 9, 9, 9, 17, 17, 17, 18, 18};
        //int haiIds[] = {0, 0, 0, 0, 8, 8, 8, 8, 9, 9, 9, 9, 18, 18};
        //int haiIds[] = {0, 0, 0, 8, 8, 8, 9, 9, 9, 18, 18, 18, 26, 26};
        //int haiIds[] = {27, 27, 27, 28, 28, 28, 29, 29, 29, 30, 30, 31, 31, 31};
        //int haiIds[] = {31, 31, 31, 32, 32, 32, 33, 33, 33, 30, 30, 30, 29, 29};
        //int haiIds[] = {0, 0, 1, 1, 2, 2, 6, 6, 7, 7, 8, 8, 9, 9};
        //int haiIds[] = {31, 31, 31, 32, 32, 32, 33, 33, 3, 4, 5, 6, 7, 8};
        //int haiIds[] = {1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4};
        //int haiIds[] = {0, 0, 0, 9, 9, 9, 18, 18, 18, 27, 27, 29, 28, 28};
        //int haiIds[] = {0, 0, 0, 9, 9, 9, 18, 18, 18, 27, 27, 28, 28, 28};
        //int haiIds[] = {0, 0, 0, 9, 9, 9, 18, 18, 18, 5, 6, 7, 27, 27};
        //int haiIds[] = {0, 0, 0, 2, 2, 2, 3, 3, 3, 5, 6, 7, 27, 27};
        //int haiIds[] = {0, 0, 0, 2, 2, 2, 3, 3, 3, 4, 4, 4, 10, 10};
        //int haiIds[] = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 9, 10, 10}; // イッツー
        //int haiIds[] = {0, 1, 2, 9, 10, 11, 18, 19, 20, 33, 33, 33, 27, 27};
        //int haiIds[] = {1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4}; // リーチタンピンイーペーコー
        //int haiIds[] = {1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7}; // リーチタンピンイーペーコー
        //int haiIds[] = {1, 1, 2, 2, 3, 3, 4, 5, 6, 10, 10, 10, 11, 12}; // リーチタンピンイーペーコー

        return haiIds;
    }
    #endregion
}
