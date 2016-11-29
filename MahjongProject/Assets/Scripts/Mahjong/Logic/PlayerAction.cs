/**
 * 
 */
public class PlayerAction 
{
    // states.
    public readonly static int STATE_NONE = 0;
    public readonly static int STATE_SUTEHAI_SELECT = 1;
    public readonly static int STATE_RON_SELECT     = 2;
    public readonly static int STATE_TSUMO_SELECT   = 3;
    public readonly static int STATE_ACTION_WAIT    = 4;
    public readonly static int STATE_CHII_SELECT    = 5;
    public readonly static int STATE_KAN_SELECT     = 6;
    public readonly static int STATE_REACH_SELECT   = 7;


    private EventId m_chiiEventId;
    public EventId getChiiEventId() {
        return m_chiiEventId;
    }
    public void setChiiEventId(EventId a_chiiEventId) {
        m_chiiEventId = a_chiiEventId;
    }


    private int mState = STATE_NONE;

    /** 捨牌のインデックス */
    private int mSutehaiIndex;

    /** ロンが可能 */
    private bool mValidRon;

    /** ツモが可能 */
    private bool mValidTsumo;

    /** ポンが可能 */
    private bool mValidPon;

    /** メニュー選択 */
    private int mMenuSelect;

    private int m_menuNum;

    public int[] m_indexs;
    public int m_indexNum;

    private bool m_dispMenu;
    private bool m_validReach;

    /**
     * コンストラクター
     */
    public PlayerAction() {
        init();
    }

    /**
     * 初期化する。
     */
    public void init() {
        mState = STATE_NONE;
        mSutehaiIndex = int.MaxValue;
        mValidRon = false;
        mValidTsumo = false;
        mValidPon = false;
        m_validReach = false;

        m_validChiiLeft = false;
        m_validChiiCenter = false;
        m_validChiiRight = false;

        m_validKan = false;
        m_kanNum = 0;
        m_kanSelect = 0;
        m_dispMenu = false;
        m_validDaiMinKan = false;

        m_menuNum = 0;

        setMenuSelect(5);
    }

    // menuNum's getter & setter.
    public void setMenuNum(int a_menuNum) {
        this.m_menuNum = a_menuNum;
    }
    public int getMenuNum() {
        return m_menuNum;
    }

    // mState's getter & setter.
    public void setState(int state) {
        this.mState = state;
    }
    public int getState() {
        return mState;
    }

    /**
     * アクションを待つ。
     */
    public void actionWait() {

    }

    /**
     * 捨牌のインデックスを設定する。
     */
    public void setSutehaiIndex(int sutehaiIdx) {
        this.mSutehaiIndex = sutehaiIdx;
    }
    public int getSutehaiIndex() {
        return mSutehaiIndex;
    }


    /**
     * アクション要求を取得する。
     */
    public bool isActionRequest() {
        return mValidRon | mValidTsumo | mValidPon | m_validReach
            | m_validChiiLeft | m_validChiiCenter | m_validChiiRight | m_validKan | m_validDaiMinKan;
    }

    /**
     * ロンが可能かを設定/取得する。
     */
    public void setValidRon(bool validRon) {
        this.mValidRon = validRon;
    }
    public bool isValidRon() {
        return mValidRon;
    }

    /**
     * ツモが可能かを設定/取得する。
     */
    public void setValidTsumo(bool validTsumo) {
        this.mValidTsumo = validTsumo;
    }
    public bool isValidTsumo() {
        return mValidTsumo;
    }


    /**
     * ポンが可能かを設定する。
     */
    public void setValidPon(bool validPon) {
        this.mValidPon = validPon;
    }
    public bool isValidPon() {
        return mValidPon;
    }


    public void setValidReach(bool a_validReach) {
        this.m_validReach = a_validReach;
    }
    public bool isValidReach() {
        return m_validReach;
    }


    public void setMenuSelect(int menuSelect) {
        this.mMenuSelect = menuSelect;
    }
    public int getMenuSelect() {
        return mMenuSelect;
    }


    private Hai[] m_sarashiHaiLeft = new Hai[2];
    private Hai[] m_sarashiHaiCenter = new Hai[2];
    private Hai[] m_sarashiHaiRight = new Hai[2];
    private bool m_validChiiLeft;
    private bool m_validChiiCenter;
    private bool m_validChiiRight;

    public void setValidChiiLeft(bool a_validChii, Hai[] a_sarashiHai) {
        this.m_validChiiLeft = a_validChii;
        this.m_sarashiHaiLeft = a_sarashiHai;
    }
    public bool isValidChiiLeft() {
        return m_validChiiLeft;
    }
    public Hai[] getSarachiHaiLeft() {
        return m_sarashiHaiLeft;
    }

    public void setValidChiiCenter(bool a_validChii, Hai[] a_sarashiHai) {
        this.m_validChiiCenter = a_validChii;
        this.m_sarashiHaiCenter = a_sarashiHai;
    }
    public bool isValidChiiCenter() {
        return m_validChiiCenter;
    }
    public Hai[] getSarachiHaiCenter() {
        return m_sarashiHaiCenter;
    }

    public void setValidChiiRight(bool a_validChii, Hai[] a_sarashiHai) {
        this.m_validChiiRight = a_validChii;
        this.m_sarashiHaiRight = a_sarashiHai;
    }
    public bool isValidChiiRight() {
        return m_validChiiRight;
    }
    public Hai[] getSarachiHaiRight() {
        return m_sarashiHaiRight;
    }


    private bool m_validKan;
    private Hai[] m_kanHais = new Hai[3];
    private int m_kanNum = 0;


    public void setValidKan(bool a_validKan, Hai[] a_kanHais, int a_kanNum) {
        this.m_validKan = a_validKan;
        this.m_kanHais = a_kanHais;
        this.m_kanNum = a_kanNum;
    }

    public Hai[] getKanHais() {
        return m_kanHais;
    }

    public int getKanNum() {
        return m_kanNum;
    }


    private bool m_validDaiMinKan;
    public void setValidDaiMinKan(bool a_validDaiMinKan) {
        this.m_validDaiMinKan = a_validDaiMinKan;
    }
    public bool isValidDaiMinKan() {
        return m_validDaiMinKan;
    }


    private int m_kanSelect = 0;
    public void setKanSelect(int a_kanSelect) {
        this.m_kanSelect = a_kanSelect;
    }
    public int getKanSelect() {
        return m_kanSelect;
    }

    public bool isValidKan() {
        return m_validKan;
    }

    public void setDispMenu(bool a_dispMenu) {
        this.m_dispMenu = a_dispMenu;
    }
    public bool isDispMenu() {
        return m_dispMenu;
    }

    private int m_reachSelect;
    public void setReachSelect(int a_reachSelect) {
        this.m_reachSelect = a_reachSelect;
    }
    public int getReachSelect() {
        return m_reachSelect;
    }

}