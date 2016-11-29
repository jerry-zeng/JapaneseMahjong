
/**
 * 捨牌を管理する。
 * 捨牌 = 打出去的牌。
 */
public class SuteHai : Hai
{
    /** 鳴きフラグ (吃碰以及明槓) */
    private bool m_naki = false;

    /** リーチフラグ(立直flag) */
    private bool m_reach = false;

    /** 手出し(正常打出去？)フラグ */
    private bool m_tedashi = false;

    /**
     * 捨牌を作成する。
     */
    public SuteHai() : base() {

    }

    /**
     * IDから捨牌を作成する。
     */
    public SuteHai(int a_id)
        : base(a_id) {
    }

    /**
     * 牌から捨牌を作成する。
     */
    public SuteHai(Hai a_hai)
        : base(a_hai) {
    }

    /**
     * 捨牌をコピーする。
     */
    public static void copy(SuteHai a_dest, SuteHai a_src) {
        Hai.copy(a_dest, a_src);
        a_dest.m_naki = a_src.m_naki;
        a_dest.m_reach = a_src.m_reach;
        a_dest.m_tedashi = a_src.m_tedashi;
    }

    /**
     * 捨牌をコピーする。
     */
    public static void copy(SuteHai a_dest, Hai a_src) {
        Hai.copy(a_dest, a_src);
        a_dest.m_naki = false;
        a_dest.m_reach = false;
        a_dest.m_tedashi = false;
    }

    /**
     * 鳴きフラグを設定する。
     */
    public void setNaki(bool a_naki) {
        this.m_naki = a_naki;
    }
    public bool isNaki() {
        return m_naki;
    }

    /**
     * リーチフラグを設定する。
     */
    public void setReach(bool a_reach) {
        this.m_reach = a_reach;
    }
    public bool isReach() {
        return m_reach;
    }

    /**
     * 手出しフラグを設定する。
     */
    public void setTedashi(bool a_tedashi) {
        this.m_tedashi = a_tedashi;
    }
    public bool isTedashi() {
        return m_tedashi;
    }
}
