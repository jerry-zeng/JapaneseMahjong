
/**
 * 河を管理する。
 * 日本麻将，玩家打出去的牌放置的那块区域叫河。
 */
public class Hou 
{
    /** 捨牌の配列の長さの最大値 */
    public readonly static int SUTE_HAIS_LENGTH_MAX = 24;

    /** 捨牌の配列 */
    private SuteHai[] m_suteHais = new SuteHai[SUTE_HAIS_LENGTH_MAX];

    /** 捨牌の配列の長さ */
    private int m_suteHaisLength = 0;


    /**
     * 河を作成する。
     */
    public Hou() {
        initialize();

        for (int i = 0; i < SUTE_HAIS_LENGTH_MAX; i++) {
            m_suteHais[i] = new SuteHai();
        }
    }

    /**
     * 河を初期化する。
     */
    public void initialize() {
        m_suteHaisLength = 0;
    }

    /**
     * 河をコピーする。
     */
    public static void copy(Hou a_dest, Hou a_src) {
        a_dest.m_suteHaisLength = a_src.m_suteHaisLength;
        for (int i = 0; i < a_dest.m_suteHaisLength; i++) {
            SuteHai.copy(a_dest.m_suteHais[i], a_src.m_suteHais[i]);
        }
    }

    /**
     * 捨牌の配列を取得する。
     */
    public SuteHai[] getSuteHais() {
        return m_suteHais;
    }

    /**
     * 捨牌の配列の長さを取得する。
     */
    public int getSuteHaisLength() {
        return m_suteHaisLength;
    }

    /**
     * 捨牌の配列に牌を追加する。
     */
    public bool add(Hai a_hai) {
        if (m_suteHaisLength >= SUTE_HAIS_LENGTH_MAX) {
            return false;
        }

        SuteHai.copy(m_suteHais[m_suteHaisLength], a_hai);
        m_suteHaisLength++;

        return true;
    }

    /**
     * 捨牌の配列の最後の牌に、鳴きフラグを設定する。
     */
    public bool setNaki(bool a_naki) {
        if (m_suteHaisLength <= 0) {
            return false;
        }

        m_suteHais[m_suteHaisLength - 1].setNaki(a_naki);

        return true;
    }

    /**
     * 捨牌の配列の最後の牌に、リーチフラグを設定する。
     */
    public bool setReach(bool a_reach) {
        if (m_suteHaisLength <= 0) {
            return false;
        }

        m_suteHais[m_suteHaisLength - 1].setReach(a_reach);

        return true;
    }

    /**
     * 捨牌の配列の最後の牌に、手出しフラグを設定する。
     */
    public bool setTedashi(bool a_tedashi) {
        if (m_suteHaisLength <= 0) {
            return false;
        }

        m_suteHais[m_suteHaisLength - 1].setTedashi(a_tedashi);

        return true;
    }
}
