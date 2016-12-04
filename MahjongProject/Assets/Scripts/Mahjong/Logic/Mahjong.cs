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


    #region Fields.
    // 山
    protected Yama m_yama;

    // 本場
    protected int m_honba;

    // 局
    protected int m_kyoku;

    // リーチ棒の数
    protected int m_reachbou;

    // 連荘
    protected bool m_renchan;

    // プレイヤーの配列
    protected List<Player> m_playerList;

    // サイコロの配列
    protected Sai[] m_sais;

    // 割れ目
    protected int m_wareme;

    // 親のプレイヤーインデックス
    protected int m_oyaIndex;

    // 起家のプレイヤーインデックス
    protected int m_chiichaIndex;

    // 捨牌
    protected List<SuteHai> m_suteHaiList;
    private int m_sutehaiIndex = 13;


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

    public Yama getYama()
    {
        return m_yama;
    }

    public int getWareme()
    {
        return m_wareme;
    }

    public int getkyoku()
    {
        return m_kyoku;
    }

    public int getHonba()
    {
        return m_honba;
    }

    public Hai getTsumoHai()
    {
        return m_tsumoHai;
    }

    public Hai getSuTehai()
    {
        return m_suteHai;
    }

    public int getReachbou() {
        return m_reachbou;
    }
    protected void setReachbou(int reachbou) {
        m_reachbou = reachbou;
    }

    // 親のプレイヤーインデックスを取得する
    public int getOyaIndex()
    {
        return m_oyaIndex;
    }

    // 起家のプレイヤーインデックスを取得する
    public int getChiichaIndex()
    {
        return m_chiichaIndex;
    }

    public Sai[] getSais()
    {
        return m_sais;
    }

    public AgariInfo getAgariInfo()
    {
        return m_agariInfo;
    }

    public List<Player> getPlayers()
    {
        return m_playerList;
    }

    public Player getActivePlayer()
    {
        return activePlayer;
    }

    public PlayerAction getPlayerAction()
    {
        return getActivePlayer().getAction();
    }

    public SuteHai[] getSuteHaiList()
    {
        return m_suteHaiList.ToArray();
    }

    protected void setSutehaiIndex(int mSutehaiIndex) {
        m_sutehaiIndex = mSutehaiIndex;
    }
    public int getSutehaiIndex() {
        return m_sutehaiIndex;
    }
    #endregion

    // -----------------------virtual methods start---------------------------
    #region virtual methods.

    // get player from index.
    public Player getPlayer( int index )
    { 
        if(index >= 0 && index < m_playerList.Count)
            return m_playerList[index];
        return null;
    }

    // get player from kaze.
    public Player getPlayer( EKaze kaze )
    { 
        return m_playerList.Find((p) => p.JiKaze == kaze);
    }

    // get player index from kaze.
    public int getPlayerIndex( EKaze kaze )
    { 
        return m_playerList.FindIndex( (p) => p.JiKaze == kaze );
    }

    // 表ドラ、槓ドラの配列を取得する
    public Hai[] getOmotoDoras()
    {
        return getYama().getOmoteDoraHais();
    }

    // 里ドラ、槓ドラの配列を取得する
    public Hai[] getUraDoras()
    {
        return getYama().getUraDoraHais();
    }

    public Hai[] getAllDoras()
    {
        return getYama().getAllDoraHais();
    }

    // ツモの残り数を取得する
    public int getTsumoRemain()
    {
        return getYama().getTsumoNokori();
    }

    public EKaze getManKaze()
    {
        return m_playerList[0].JiKaze;
    }

    // 自風を取得する
    public EKaze getJiKaze() 
    {
        return m_playerList[m_oyaIndex].JiKaze;
    }

    // 場風を取得する
    public EKaze getBaKaze() 
    {
        return m_kyoku <= (int)EKyoku.Ton_4 ? EKaze.Ton : EKaze.Nan;
    }


    // 手牌をコピーする
    public void copyTehai(Tehai tehai, EKaze kaze)
    {
        if( activePlayer.JiKaze == kaze ) {
            Tehai.copy(tehai, activePlayer.Tehai, true);
        }
        else {
            Tehai.copy(tehai, getPlayer(kaze).Tehai, false);
        }
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
    #endregion virtual methods.


    // -----------------------static methods start---------------------------
    public static int getRelation(EKaze from, EKaze to)
    {
        int fromKaze = (int)from;
        int toKaze = (int)to;

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


    public Mahjong()
    {
        initialize();
    }

    // abstract methods.
    protected abstract void initialize();
    public abstract void PostUIEvent(UIEventID eventId, EKaze kazeFrom = EKaze.Ton, EKaze kazeTo = EKaze.Ton);
}

