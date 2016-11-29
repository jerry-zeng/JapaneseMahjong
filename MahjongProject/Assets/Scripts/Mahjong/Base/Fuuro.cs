
/**
 * 副露を管理する。
 * 副露包括吃牌(チー),碰牌(ポン)和杠，也就是放桌角的那些牌.
 */
public class Fuuro 
{
    /** 種別 明順 */
    public const int TYPE_MINSHUN = 0;
    /** 種別 明刻 */
    public const int TYPE_MINKOU = 1;
    /** 種別 大明槓 */
    public const int TYPE_DAIMINKAN = 2;
    /** 種別 加槓 */
    public const int TYPE_KAKAN = 3;
    /** 種別 暗槓 */
    public const int TYPE_ANKAN = 4;


    /** 種別 */
    private int m_type = -1;

    /** 他家との関係(和其他人的关系) */
    private int m_relation = -1;

    /// <summary>
    /// index of the new hai in m_hais that is newly picked by player or from others(AI). 
    /// </summary>
    private int m_newPickIndex = -1;

    /** 構成牌 */
    private Hai[] m_hais;

    public Fuuro(){
        m_hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        for (int i = 0; i < m_hais.Length; i++) {
            m_hais[i] = new Hai();
        }
    }

    /**
     * 種別を設定する。
     */
    public void setType(int a_type) {
        this.m_type = a_type;
    }

    /**
     * 種別を取得する。
     */
    public int getType() {
        return m_type;
    }

    /**
     * 他家との関係を設定する。
     */
    public void setRelation(int a_relation) {
        this.m_relation = a_relation;
    }

    /**
     * 他家との関係を取得する。
     */
    public int getRelation() {
        return m_relation;
    }

    /**
     * 構成牌を設定する。
     */
    public void setHais(Hai[] m_hais) {
        this.m_hais = m_hais;
    }

    /**
     * 構成牌を取得する。
     */
    public Hai[] getHais() {
        return m_hais;
    }

    public void setNewPickIndex(int index) {
        this.m_newPickIndex = index;
    }
    public int getNewPickIndex() {
        return m_newPickIndex;
    }


    /**
     * 副露をコピーする。
     *
     */
    public static void copy(Fuuro a_dest, Fuuro a_src) {
        a_dest.m_type = a_src.m_type;
        a_dest.m_relation = a_src.m_relation;
        a_dest.m_newPickIndex = a_src.m_newPickIndex;

        for (int i = 0; i < Mahjong.MENTSU_HAI_MEMBERS_4; i++) {
            Hai.copy(a_dest.m_hais[i], a_src.m_hais[i]);
        }
    }
}
