using System.Collections.Generic;

/// <summary>
/// 麻将を管理するクラスです
/// </summary>

public abstract class Mahjong 
{
    // 面子の構成牌の数(3個)
    public readonly static int MENTSU_HAI_MEMBERS_3 = 3;
    // 面子の構成牌の数(4個)
    public readonly static int MENTSU_HAI_MEMBERS_4 = 4;

    // 捨牌最大值
    public readonly static int SUTE_HAI_MAX = 136;


    #region Fields.
    // 山
    protected Yama m_yama;

    // 局
    protected int m_kyoku;

    // 摸入牌
    protected Hai m_tsumoHai;

    // 打出牌
    protected Hai m_suteHai;

    // リーチ棒の数
    protected int m_reachbou;

    // 本場
    protected int m_honba;

    // プレイヤーに提供する情報
    protected Info m_info;

    // UIに提供する情報
    protected GameInfo m_infoUi;

    // プレイヤーの人数
    protected int m_playerNum;

    // プレイヤーの配列
    protected List<Player> m_players;

    // サイコロの配列
    protected Sai[] m_sais = new Sai[] { new Sai(), new Sai() };

    // 親のプレイヤーインデックス
    protected int m_iOya;

    // 起家のプレイヤーインデックス
    protected int m_iChiicha;

    // 連荘
    protected bool m_renchan;

    // イベントを発行した風
    protected EKaze m_kazeFrom;

    // イベントの対象となった風
    protected EKaze m_kazeTo;

    // 捨牌数量.
    protected int m_suteHaisCount = 0;
    protected SuteHai[] m_suteHais = new SuteHai[SUTE_HAI_MAX];

    // 割れ目
    protected int m_wareme;

    // アクティブプレイヤー
    protected Player activePlayer;
    protected PlayerAction m_playerAction = new PlayerAction();

    protected bool[] m_tenpai = new bool[4];

    protected HaiCombi[] combis = new HaiCombi[10]
    {
        new HaiCombi(),new HaiCombi(),new HaiCombi(),
        new HaiCombi(),new HaiCombi(),new HaiCombi(),
        new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi()
    };

    protected AgariInfo m_agariInfo = new AgariInfo();


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

    public Yama getYama() {
        return m_yama;
    }
        
    public int getWareme() {
        return m_wareme;
    }

    public int getkyoku() {
        return m_kyoku;
    }

    public Hai getTsumoHai() {
        return m_tsumoHai;
    }

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

    public Sai[] getSais() {
        return m_sais;
    }

    public bool[] getTenpai() {
        return m_tenpai;
    }

    public int getHonba() {
        return m_honba;
    }

    public AgariInfo getAgariInfo() {
        return m_agariInfo;
    }

    public List<Player> getPlayers() {
        return m_players;
    }

    #endregion get&set-properties.


    // -----------------------static methods start---------------------------
    public static int getRelation(EKaze efromKaze, EKaze etoKaze)
    {
        int fromKaze = (int)efromKaze;
        int toKaze = (int)etoKaze;

        ERelation relation;
        if( fromKaze == toKaze ) {
            relation = ERelation.JiBun;
        }
        else if( (fromKaze + 1) % 4 == toKaze ) {
            relation = ERelation.ShiMoCha; //下家.
        }
        else if( (fromKaze + 2) % 4 == toKaze ) {
            relation = ERelation.ToiMen;
        }
        else //if( (fromKaze + 3) % 4 == toKaze )
        {
            relation = ERelation.KaMiCha; //上家.
        }

        return (int)relation;
    }


    // -----------------------virtual methods start---------------------------
    #region virtual methods.
    /**
     * プレイヤーの自風を設定する。
     */
    public void setJikaze()
    {
        EKaze kaze = (EKaze)m_iOya;

        for( int i = 0; i < m_players.Count; i++)
        {
            // プレイヤーの自風を設定する。
            m_players[i].setJikaze( kaze );

            kaze = kaze.Next();
        }
    }

    /// <summary>
    /// 摇色子.
    /// </summary>
    public Sai[] Saifuri()
    {
        m_sais[0].SaiFuri();
        m_sais[1].SaiFuri();

        return m_sais;
    }

    /// <summary>
    /// get player from kaze.
    /// </summary>
    public Player getPlayer( int kaze )
    { 
        return getPlayer((EKaze)kaze);
    }

    public Player getPlayer( EKaze kaze )
    { 
        return m_players.Find((p) => p.getJikaze() == kaze);
    }

    /// <summary>
    /// get player index from kaze.
    /// </summary>
    public int getPlayerIndex( EKaze kaze ) { 
        return m_players.FindIndex( (p) => p.getJikaze() == kaze );
    }

    public int getPlayerSuteHaisCount(EKaze kaze) {
        return getPlayer(kaze).getSuteHaisCount();
    }

    public virtual bool isReach(EKaze kaze) {
        return getPlayer(kaze).isReach();
    }

    public int getTenbou(EKaze kaze) {
        return getPlayer(kaze).getTenbou();
    }

    public string getName(EKaze kaze) {
        return getPlayer(kaze).getName();
    }

    /**
     * 山に割れ目を設定する。
     */
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

    /**
     * 配牌する。
     */
    protected virtual void Haipai()
    {
        // everyone picks 3x4 hais.
        for( int i = 0, j = m_iOya; i < m_players.Count * 12; j++ ) 
        {
            if( j >= m_players.Count )
                j = 0;

            // pick 4 hais oncely.
            Hai[] hais = m_yama.PickHaipai();
            for( int h = 0; h < hais.Length; h++ )
            {
                m_players[j].getTehai().addJyunTehai( hais[h] );

                i++;
            }
        }

        // then everyone picks 1 hai.
        for( int i = 0, j = m_iOya; i < 4; i++,j++ )
        {
            if( j >= m_players.Count )
                j = 0;
            
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
     * 里ドラ、槓ドラの配列を取得する。
     */
    public Hai[] getUraDoras() {
        return getYama().getUraDoraHais();
    }

    public Hai[] getAllDoras(){
        return getYama().getAllDoraHais();
    }

    /**
     * ツモの残り数を取得する。
     */
    public int getTsumoRemain() {
        return getYama().getTsumoNokori();
    }

    public EKaze getManKaze()
    {
        return m_players[0].getJikaze();
    }

    /**
     * 自風を取得する。
     */
    public EKaze getJiKaze() {
        return m_players[m_iOya].getJikaze();
    }

    public EKaze getBaKaze() {
        if( m_kyoku <= (int)EKyoku.Ton_4 ) {
            return EKaze.Ton;
        }
        else {
            return EKaze.Nan;
        }
    }

    /**
     * 手牌をコピーする。
     */
    public void copyTehai(Tehai tehai, EKaze kaze)
    {
        if( activePlayer.getJikaze() == (EKaze)kaze ) {
            Tehai.copy(tehai, activePlayer.getTehai(), true);
        }
        else {
            Tehai.copy(tehai, getPlayer(kaze).getTehai(), false);
        }
    }

    /**
     * 手牌をコピーする。
     */
    public void copyTehaiUi(Tehai tehai, EKaze kaze)
    {
        Tehai.copy(tehai, getPlayer(kaze).getTehai(), true);
    }

    /**
     * 河をコピーする。
     */
    public void copyHou(Hou hou, EKaze kaze)
    {
        Hou.copy(hou, getPlayer(kaze).getHou());
    }


    public int getAgariScore(Tehai tehai, Hai addHai)
    {
        AgariSetting.setOmoteDoraHais(getOmotoDoras());

        if( activePlayer.isReach() ) {
            if( activePlayer.isDoubleReach() ) {
                AgariSetting.setYakuFlag((int)EYakuFlagType.DOUBLE_REACH, true);
            }
            else {
                AgariSetting.setYakuFlag((int)EYakuFlagType.REACH, true);
            }
        }

        if( m_isTsumo ) {
            AgariSetting.setYakuFlag((int)EYakuFlagType.TSUMO, true);
            if( m_isTenhou ) {
                AgariSetting.setYakuFlag((int)EYakuFlagType.TENHOU, true);
            }
            else if( m_isChiihou ) {
                AgariSetting.setYakuFlag((int)EYakuFlagType.TIHOU, true);
            }
        }

        if( m_isTsumo && m_isRinshan ) {
            AgariSetting.setYakuFlag((int)EYakuFlagType.RINSYAN, true);
        }

        if( m_isLast ) {
            if( m_isTsumo ) {
                AgariSetting.setYakuFlag((int)EYakuFlagType.HAITEI, true);
            }
            else {
                AgariSetting.setYakuFlag((int)EYakuFlagType.HOUTEI, true);
            }
        }

        if( activePlayer.isIppatsu() ) {
            AgariSetting.setYakuFlag((int)EYakuFlagType.IPPATU, true);
        }

        if( GameSettings.UseKuitan ) {
            AgariSetting.setYakuFlag((int)EYakuFlagType.KUITAN, true);
        }

        return AgariScoreManager.GetAgariScore(tehai, addHai, combis, ref m_agariInfo);
    }

    public void setSutehaiIndex(int sutehaiIdx)
    {
        m_info.setSutehaiIndex(sutehaiIdx);
    }

    #endregion virtual methods.


    // abstract methods.
    public abstract void initialize();
    public abstract void PostUIEvent(EventId eventId, EKaze kazeFrom = EKaze.Ton, EKaze kazeTo = EKaze.Ton);
}

