
/**
 * Playerを管理するクラスです。
 */
public class Player
{
    private IPlayer iplayer;

    public Player(IPlayer player) {
        this.iplayer = player;
    }

    private IPlayer getPlayer() {
        return this.iplayer;
    }

    #region proxy.
    public string getName() {
        return this.iplayer.getName();
    }
    public bool isAI() {
        return this.iplayer.isAI();
    }
    public int getSutehaiIndex() {
        return this.iplayer.getSutehaiIndex();
    }
    public EventId HandleEvent(EventId eid, int a_kazeFrom, int a_kazeTo) {
        return this.iplayer.HandleEvent(eid, a_kazeFrom, a_kazeTo);
    }
    #endregion proxy.



    /** 手牌 */
    private Tehai m_tehai = new Tehai();

    /**
     * 手牌を取得します。
     */
    public Tehai getTehai() {
        return m_tehai;
    }

    /** 河 */
    private Hou m_hou = new Hou();

    /**
     * 河を取得します。
     */
    public Hou getHou() {
        return m_hou;
    }

    /** 自風 */
    private int jikaze;

    /**
     * 自風を取得します。
     */
    public int getJikaze() {
        return jikaze;
    }

    /**
     * 自風を設定します。
     */
    public void setJikaze(int jikaze) {
        this.jikaze = jikaze;
    }

    /** 点棒 */
    private int tenbou;

    /**
     * 点棒を取得します。
     */
    public int getTenbou() {
        return tenbou;
    }

    /**
     * 点棒を設定します。
     */
    public void setTenbou(int tenbou) {
        this.tenbou = tenbou;
    }

    /**
     * 点棒を増やします。
     */
    public void increaseTenbou(int ten) {
        tenbou += ten;
    }

    /**
     * 点棒を減らします。
     */
    public void reduceTenbou(int ten) {
        tenbou -= ten;
    }

    /** リーチ */
    private bool reach;
    public bool isReach() {
        return reach;
    }
    public void setReach(bool reach) {
        this.reach = reach;
    }

    /** ダブルリーチ */
    private bool m_doubleReach;
    public bool isDoubleReach() {
        return m_doubleReach;
    }
    public void setDoubleReach(bool a_doubleReach) {
        this.m_doubleReach = a_doubleReach;
    }

    /**
     * 捨牌数。
     */
    private int m_suteHaisCount;
    public void setSuteHaisCount(int a_suteHaisCount) {
        this.m_suteHaisCount = a_suteHaisCount;
    }
    public int getSuteHaisCount() {
        return m_suteHaisCount;
    }

    /**
     *  一発.
     */
    private bool m_ippatsu;
    public void setIppatsu(bool a_ippatsu) {
        this.m_ippatsu = a_ippatsu;
    }

    public bool isIppatsu() {
        return m_ippatsu;
    }


    private CountFormat m_countFormat = new CountFormat();

    // 听牌 //
    public bool isTenpai() {
        if( reach == true ) {
            return true;
        }

        Hai addHai;
        for( int id = 0; id < Hai.ID_ITEM_MAX; id++ ) {
            addHai = new Hai(id);
            m_countFormat.setCountFormat(m_tehai, addHai);

            if( m_countFormat.getCombis(null) > 0 ) {
                return true;
            }
        }

        return false;
    }


    /**
     * プレイヤーを初期化します。
     */
    public void init() {
        // 手牌を初期化します。
        m_tehai.initialize();

        // 河を初期化します。
        m_hou.initialize();

        // リーチを初期化します。
        reach = false;

        m_ippatsu = false;
        m_doubleReach = false;

        m_suteHaisCount = 0;
    }

}
