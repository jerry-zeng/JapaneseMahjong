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


    public static Mahjong current;

    #region Fields.
    // 山
    protected Yama m_yama;

    // 本場
    protected int m_honba;

    // 局
    protected int m_kyoku;

    // 連荘
    protected bool m_renchan;

    // リーチ棒の数
    protected int m_reachbou;

    // プレイヤーの配列
    protected List<Player> m_playerList;
    protected bool[] m_tenpaiFlags;

    // サイコロの配列
    protected Sai[] m_sais;

    // 割れ目
    protected int m_wareme;

    // 親のプレイヤーインデックス
    protected int m_oyaIndex;

    // 起家のプレイヤーインデックス
    protected int m_chiichaIndex;

    // 捨牌数量.
    protected List<SuteHai> m_suteHaiList;

    // プレイヤーに提供する情報
    protected Info m_info;

    // UIに提供する情報
    protected GameInfo m_infoUi;


    // イベントを発行した風
    protected EKaze m_kazeFrom;

    // イベントの対象となった風
    protected EKaze m_kazeTo;

    // current player
    protected Player activePlayer;

    // 摸入牌
    protected Hai m_tsumoHai = new Hai();
    // 打出牌
    protected Hai m_suteHai = new Hai();

    protected HaiCombi[] m_combis = new HaiCombi[0];
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

    public PlayerAction getPlayerAction()
    {
        return getActivePlayer().getAction();
    }

    public SuteHai[] getSuteHaiList() {
        return m_suteHaiList.ToArray();
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

    public Hai getSuTehai() {
        return m_suteHai;
    }

    public int getReachbou() {
        return m_reachbou;
    }
    public void setReachbou(int reachbou) {
        m_reachbou = reachbou;
    }

    // 起家のプレイヤーインデックスを取得する
    public int getChiichaIndex() {
        return m_chiichaIndex;
    }

    public Sai[] getSais() {
        return m_sais;
    }

    public bool[] getTenpaiFlags() {
        return m_tenpaiFlags;
    }

    public int getHonba() {
        return m_honba;
    }

    public AgariInfo getAgariInfo() {
        return m_agariInfo;
    }

    public List<Player> getPlayers() {
        return m_playerList;
    }

    public Info getInfo(){
        return m_info;
    }
    public GameInfo getInfoUI(){
        return m_infoUi;
    }

    #endregion get&set-properties.


    // -----------------------static methods start---------------------------
    public static int getRelation(EKaze efromKaze, EKaze etoKaze)
    {
        int fromKaze = (int)efromKaze;
        int toKaze = (int)etoKaze;

        ERelation relation;
        if( fromKaze == toKaze ) {
            relation = ERelation.JiBun; //自家
        }
        else if( (fromKaze + 1) % 4 == toKaze ) {
            relation = ERelation.ShiMoCha; //下家.
        }
        else if( (fromKaze + 2) % 4 == toKaze ) {
            relation = ERelation.ToiMen;  //对家
        }
        else //if( (fromKaze + 3) % 4 == toKaze )
        {
            relation = ERelation.KaMiCha; //上家.
        }

        return (int)relation;
    }


    // -----------------------virtual methods start---------------------------
    #region virtual methods.
    // プレイヤーの自風を設定する
    public void setPlayerKaze()
    {
        EKaze kaze = (EKaze)m_oyaIndex;

        for( int i = 0; i < m_playerList.Count; i++)
        {
            m_playerList[i].JiKaze = kaze;

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
    public Player getPlayer( int index )
    { 
        if(index >= 0 && index < m_playerList.Count)
            return m_playerList[index];
        return null;
    }

    public Player getPlayer( EKaze kaze )
    { 
        return m_playerList.Find((p) => p.JiKaze == kaze);
    }

    /// <summary>
    /// get player index from kaze.
    /// </summary>
    public int getPlayerIndex( EKaze kaze ) { 
        return m_playerList.FindIndex( (p) => p.JiKaze == kaze );
    }

    public int getPlayerSuteHaisCount(EKaze kaze) {
        return getPlayer(kaze).SuteHaisCount;
    }

    public virtual bool isReach(EKaze kaze) {
        return getPlayer(kaze).IsReach;
    }

    public int getTenbou(EKaze kaze) {
        return getPlayer(kaze).Tenbou;
    }

    public string getName(EKaze kaze) {
        return getPlayer(kaze).Name;
    }

    // 表ドラ、槓ドラの配列を取得する
    public Hai[] getOmotoDoras() {
        return getYama().getOmoteDoraHais();
    }

    // 里ドラ、槓ドラの配列を取得する
    public Hai[] getUraDoras() {
        return getYama().getUraDoraHais();
    }

    public Hai[] getAllDoras(){
        return getYama().getAllDoraHais();
    }

    // ツモの残り数を取得する
    public int getTsumoRemain() {
        return getYama().getTsumoNokori();
    }

    public EKaze getManKaze()
    {
        return m_playerList[0].JiKaze;
    }

    // 自風を取得する
    public EKaze getJiKaze() {
        return m_playerList[m_oyaIndex].JiKaze;
    }

    public EKaze getBaKaze() {
        if( m_kyoku <= (int)EKyoku.Ton_4 ) {
            return EKaze.Ton;
        }
        else {
            return EKaze.Nan;
        }
    }

    // 手牌をコピーする
    public void copyTehai(Tehai tehai, EKaze kaze)
    {
        if( activePlayer.JiKaze == (EKaze)kaze ) {
            Tehai.copy(tehai, activePlayer.Tehai, true);
        }
        else {
            Tehai.copy(tehai, getPlayer(kaze).Tehai, false);
        }
    }

    // 手牌をコピーする
    public void copyTehaiUi(Tehai tehai, EKaze kaze)
    {
        Tehai.copy(tehai, getPlayer(kaze).Tehai, true);
    }

    // 河をコピーする
    public void copyHou(Hou hou, EKaze kaze)
    {
        Hou.copy(hou, getPlayer(kaze).Hou);
    }


    public int getAgariScore(Tehai tehai, Hai addHai)
    {
        AgariParam param = new AgariParam(this);

        if( activePlayer.IsReach ) {
            if( activePlayer.IsDoubleReach ) {
                param.setYakuFlag((int)EYakuFlagType.DOUBLE_REACH, true);
            }
            else {
                param.setYakuFlag((int)EYakuFlagType.REACH, true);
            }
        }

        if( m_isTsumo ) {
            param.setYakuFlag((int)EYakuFlagType.TSUMO, true);
            if( m_isTenhou ) {
                param.setYakuFlag((int)EYakuFlagType.TENHOU, true);
            }
            else if( m_isChiihou ) {
                param.setYakuFlag((int)EYakuFlagType.TIHOU, true);
            }
        }

        if( m_isTsumo && m_isRinshan ) {
            param.setYakuFlag((int)EYakuFlagType.RINSYAN, true);
        }

        if( m_isLast ) {
            if( m_isTsumo ) {
                param.setYakuFlag((int)EYakuFlagType.HAITEI, true);
            }
            else {
                param.setYakuFlag((int)EYakuFlagType.HOUTEI, true);
            }
        }

        if( activePlayer.IsIppatsu ) {
            param.setYakuFlag((int)EYakuFlagType.IPPATU, true);
        }

        if( GameSettings.UseKuitan ) {
            param.setYakuFlag((int)EYakuFlagType.KUITAN, true);
        }

        return AgariScoreManager.GetAgariScore(tehai, addHai, param, ref m_combis, ref m_agariInfo);
    }

    public void setSutehaiIndex(int sutehaiIdx)
    {
        m_info.setSutehaiIndex(sutehaiIdx);
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

            // pick 4 hais oncely.
            Hai[] hais = m_yama.PickHaipai();
            for( int h = 0; h < hais.Length; h++ )
            {
                m_playerList[j].Tehai.addJyunTehai( hais[h] );

                i++;
            }
        }

        // then everyone picks 1 hai.
        for( int i = 0, j = m_oyaIndex; i < 4; i++,j++ )
        {
            if( j >= m_playerList.Count )
                j = 0;

            m_playerList[j].Tehai.addJyunTehai( m_yama.PickTsumoHai() );
        }
    }

    #endregion virtual methods.


    public Mahjong()
    {
        current = this;
        initialize();
    }

    // abstract methods.
    protected abstract void initialize();
    public abstract void PostUIEvent(UIEventID eventId, EKaze kazeFrom = EKaze.Ton, EKaze kazeTo = EKaze.Ton);
}

