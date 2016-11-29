
/**
 * 麻将を管理するクラスです。
 */
public abstract class Mahjong 
{
    /** 東一局 */
    public readonly static int KYOKU_TON_1 = 0;
    /** 東二局 */
    public readonly static int KYOKU_TON_2 = 1;
    /** 東三局 */
    public readonly static int KYOKU_TON_3 = 2;
    /** 東四局 */
    public readonly static int KYOKU_TON_4 = 3;
    /** 南一局 */
    public readonly static int KYOKU_NAN_1 = 4;
    /** 南二局 */
    public readonly static int KYOKU_NAN_2 = 5;
    /** 南三局 */
    public readonly static int KYOKU_NAN_3 = 6;
    /** 南四局 */
    public readonly static int KYOKU_NAN_4 = 7;

    /*
     * 共通定義
     */
    /** 他家との関係 自分 */
    public readonly static int RELATION_JIBUN = 0;
    /** 他家との関係 上家 */
    public readonly static int RELATION_KAMICHA = 1;
    /** 他家との関係 対面 */
    public readonly static int RELATION_TOIMEN = 2;
    /** 他家との関係 下家 */
    public readonly static int RELATION_SHIMOCHA = 3;

    /** 面子の構成牌の数(3個) */
    public readonly static int MENTSU_HAI_MEMBERS_3 = 3;
    /** 面子の構成牌の数(4個) */
    public readonly static int MENTSU_HAI_MEMBERS_4 = 4;

    /** 捨牌最大值 */
    public readonly static int SUTE_HAI_MAX = 136;


    // Field.
    #region Fields.

    /** 山 */
    protected Yama m_yama;

    /** 局 */
    protected int m_kyoku;

    /** 摸入牌 */
    protected Hai m_tsumoHai;

    /** 打出牌 */
    protected Hai m_suteHai;

    /** リーチ棒の数 */
    protected int m_reachbou;

    /** 本場 */
    protected int m_honba;

    /** プレイヤーの人数 */
    protected int m_playerNum;

    /** プレイヤーに提供する情報 */
    protected Info m_info;

    /** プレイヤーの配列 */
    protected Player[] m_players;

    /** 風をプレイヤーインデックスに変換する配列 */
    protected int[] m_kazeToPlayerIndex = new int[4];

    /** UIに提供する情報 */
    protected GameInfo m_infoUi;

    /** サイコロの配列 */
    protected Sai[] m_sais = new Sai[] { new Sai(), new Sai() };

    /** 親のプレイヤーインデックス */
    protected int m_iOya;

    /** 起家のプレイヤーインデックス */
    protected int m_iChiicha;

    /** 連荘 */
    protected bool m_renchan;

    /** イベントを発行した風 */
    protected int m_kazeFrom;

    /** イベントの対象となった風 */
    protected int m_kazeTo;

    // 捨牌数量.
    protected int m_suteHaisCount = 0;

    protected SuteHai[] m_suteHais = new SuteHai[SUTE_HAI_MAX];

    /** 割れ目 */
    protected int m_wareme;

    /** アクティブプレイヤー */
    protected Player activePlayer;
    protected PlayerAction m_playerAction = new PlayerAction();

    protected bool[] m_tenpai = new bool[4];

    protected Combi[] combis = new Combi[10]{
        new Combi(),new Combi(),new Combi(),
        new Combi(),new Combi(),new Combi(),
        new Combi(),new Combi(),new Combi(),new Combi()
    };

    protected AgariInfo m_agariInfo = new AgariInfo();
    protected AgariScore m_score;


    protected bool m_isTenhou = false;
    protected bool m_isChiihou = false;
    protected bool m_isTsumo = false;
    protected bool m_isRinshan = false;
    protected bool m_isLast = false;

    #endregion Fields.

    // ------------------------get&set-properties start-------------------------
    #region get&set-properties.
    public Player getActivePlayer() {
        return activePlayer;
    }
    public int getSuteHaisCount() {
        return m_suteHaisCount;
    }

    public SuteHai[] getSuteHais() {
        return m_suteHais;
    }
    /**
     * 山を取得する。
     */
    public Yama getYama() {
        return m_yama;
    }

    /**
     * 割れ目を取得する。
     */
    public int getWareme() {
        return m_wareme;
    }

    /**
     * 局を取得する。
     */
    public int getkyoku() {
        return m_kyoku;
    }

    /**
     * ツモ牌を取得する。
     */
    public Hai getTsumoHai() {
        return m_tsumoHai;
    }

    /**
     * 捨牌を取得する。
     */
    public Hai getSuteHai() {
        return m_suteHai;
    }

    public int getReachbou() {
        return m_reachbou;
    }
    public void setReachbou(int reachbou) {
        m_reachbou = reachbou;
    }

    /**
     * 起家のプレイヤーインデックスを取得する。
     */
    public int getChiichaIndex() {
        return m_iChiicha;
    }

    /**
     * サイコロの配列を取得する。
     */
    public Sai[] getSais() {
        return m_sais;
    }

    public bool[] getTenpai() {
        return m_tenpai;
    }

    /**
     * 本場を取得する。
     */
    public int getHonba() {
        return m_honba;
    }

    public AgariInfo getAgariInfo() {
        return m_agariInfo;
    }

    public Player[] getPlayers() {
        return m_players;
    }

    #endregion get&set-properties.


    // -----------------------static methods start---------------------------
    public static int getRelation(int fromKaze, int toKaze) {
        int relation;
        if( fromKaze == toKaze ) {
            relation = RELATION_JIBUN;
        }
        else if( (fromKaze + 1) % 4 == toKaze ) {
            relation = RELATION_SHIMOCHA; //下家.
        }
        else if( (fromKaze + 2) % 4 == toKaze ) {
            relation = RELATION_TOIMEN;
        }
        else //if( (fromKaze + 3) % 4 == toKaze )
        {
            relation = RELATION_KAMICHA; //上家.
        }
        return relation;
    }


    // -----------------------virtual methods start---------------------------
    #region virtual methods.
    /**
     * プレイヤーの自風を設定する。
     */
    public void setJikaze() {
        for( int i = 0, j = m_iOya; i < m_players.Length; i++, j++ ) {
            if( j >= m_players.Length ) {
                j = 0;
            }

            // プレイヤーの自風を設定する。
            m_players[j].setJikaze(i);

            // 風をプレイヤーインデックスに変換する配列を設定する。
            m_kazeToPlayerIndex[i] = j;
        }
    }

    /// <summary>
    /// 摇色子.
    /// </summary>
    public Sai[] Saifuri() {
        m_sais[0].SaiFuri();
        m_sais[1].SaiFuri();

        return m_sais;
    }

    /// <summary>
    /// get player from kaze.
    /// </summary>
    /// <param name="kaze"></param>
    /// <returns> Player </returns>
    public Player getPlayer( int kaze ) { 
        return m_players[getPlayerIndex(kaze)];
    }

    /// <summary>
    /// get player from index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns> Player </returns>
    public Player getPlayerFromIndex( int index ) {
        return m_players[index];
    }

    /// <summary>
    /// get player index from kaze.
    /// </summary>
    /// <param name="kaze"></param>
    /// <returns> player index </returns>
    public int getPlayerIndex( int kaze ) { 
        return m_kazeToPlayerIndex[kaze];
    }

    public int getPlayerSuteHaisCount(int a_kaze) {
        return m_players[m_kazeToPlayerIndex[a_kaze]].getSuteHaisCount();
    }


    /**
     * リーチを取得する。
     */
    public virtual bool isReach(int kaze) {
        return m_players[m_kazeToPlayerIndex[kaze]].isReach();
    }

    public int getTenbou(int kaze) {
        return m_players[m_kazeToPlayerIndex[kaze]].getTenbou();
    }

    public string getName(int kaze) {
        return m_players[m_kazeToPlayerIndex[kaze]].getName();
    }

    /**
     * 山に割れ目を設定する。
     */
    protected void setWareme(Sai[] sais) {
        int sum = sais[0].getNum() + sais[1].getNum(); 

        int waremePlayer = (getChiichaIndex() - sum - 1) % 4;
        if( waremePlayer < 0 ){
            waremePlayer += 4;
        }

        int startHaisIndex = ( (4- waremePlayer) % 4 ) * 34 + sum * 2;

        m_wareme = startHaisIndex - 1; // 开始拿牌位置-1.

        getYama().setTsumoHaisStartIndex(startHaisIndex);
    }

    /**
     * 配牌する。
     */
    protected virtual void Haipai() {

        // everyone picks 3x4 hais.
        int max = m_players.Length * 12;

        for( int i = 0, j = m_iOya; i < max; j++ ) 
        {
            if( j >= m_players.Length ) {
                j = 0;
            }

            // pick 4 hais oncely.
            Hai[] hais = m_yama.PickHaipai();
            for( int h = 0; h < hais.Length; h++ ) {
                m_players[j].getTehai().addJyunTehai( hais[h] );

                i++;
            }
        }

        // then everyone picks 1 hai.
        max = 4;
        for( int i = 0, j = m_iOya; i < max; i++,j++ ) {
            if( j >= m_players.Length ) {
                j = 0;
            }
            m_players[j].getTehai().addJyunTehai( m_yama.PickTsumoHai() );
        }
    }

    /**
     * 表ドラ、槓ドラの配列を取得する。
     */
    public Hai[] getOmotoDoras() {
        return getYama().getOmoteDoraHais();
    }

    /**
     * 表ドラ、槓ドラの配列を取得する。
     */
    public Hai[] getUraDoras() {
        return getYama().getUraDoraHais();
    }

    /**
     * ツモの残り数を取得する。
     */
    public int getTsumoRemain() {
        return getYama().getTsumoNokori();
    }

    public int getManKaze()
    {
        return m_players[0].getJikaze();
    }

    /**
     * 自風を取得する。
     */
    public int getJiKaze() {
        return m_players[m_iOya].getJikaze();
    }

    public int getBaKaze() {
        if( m_kyoku <= KYOKU_TON_4 ) {
            return Kaze.KAZE_TON;
        }
        else {
            return Kaze.KAZE_NAN;
        }
    }

    /**
     * 手牌をコピーする。
     */
    public void copyTehai(Tehai tehai, int kaze) {
        if( activePlayer.getJikaze() == kaze ) {
            Tehai.copy(tehai, activePlayer.getTehai(), true);
        }
        else {
            Tehai.copy(tehai, m_players[m_kazeToPlayerIndex[kaze]].getTehai(), false);
        }
    }

    /**
     * 手牌をコピーする。
     */
    public void copyTehaiUi(Tehai tehai, int kaze) {
        Tehai.copy(tehai, m_players[m_kazeToPlayerIndex[kaze]].getTehai(), true);
    }

    /**
     * 河をコピーする。
     */
    public void copyHou(Hou hou, int kaze) {
        Hou.copy(hou, m_players[m_kazeToPlayerIndex[kaze]].getHou());
    }


    public int getAgariScore(Tehai tehai, Hai addHai) {
        AgariSetting setting = new AgariSetting(this);

        setting.setOmoteDoraHais(getOmotoDoras());
        if( activePlayer.isReach() ) {
            if( activePlayer.isDoubleReach() ) {
                setting.setYakuflg((int)YakuflgName.DOUBLEREACH, true);
            }
            else {
                setting.setYakuflg((int)YakuflgName.REACH, true);
            }
        }

        if( m_isTsumo ) {
            setting.setYakuflg((int)YakuflgName.TUMO, true);
            if( m_isTenhou ) {
                setting.setYakuflg((int)YakuflgName.TENHOU, true);
            }
            else if( m_isChiihou ) {
                setting.setYakuflg((int)YakuflgName.TIHOU, true);
            }
        }

        if( m_isTsumo && m_isRinshan ) {
            setting.setYakuflg((int)YakuflgName.RINSYAN, true);
        }

        if( m_isLast ) {
            if( m_isTsumo ) {
                setting.setYakuflg((int)YakuflgName.HAITEI, true);
            }
            else {
                setting.setYakuflg((int)YakuflgName.HOUTEI, true);
            }
        }

        if( activePlayer.isIppatsu() ) {
            setting.setYakuflg((int)YakuflgName.IPPATU, true);
        }

        if( MahjongSetting.UseKuitan ) {
            setting.setYakuflg((int)YakuflgName.KUITAN, true);
        }

        m_score = new AgariScore();

        return m_score.getAgariScore(tehai, addHai, combis, setting, m_agariInfo);
    }

    public void setSutehaiIndex(int sutehaiIdx) {
        m_info.setSutehaiIndex(sutehaiIdx);
    }

    #endregion virtual methods.


    // abstract methods.
    public abstract void initialize();
    public abstract void PostUIEvent(EventId a_eventId, int a_kazeFrom, int a_kazeTo);
}

