
/**
 * 山を管理する。
 * 日本麻将里最后的7幢牌，共14个，叫牌山。
 */
public class Yama 
{
    /** 山牌の配列の最大数 */
    public readonly static int YAMA_HAIS_MAX = 136;

    /** 自摸牌の配列の最大数 */
    public readonly static int TSUMO_HAIS_MAX = 122;

    /** 杠上开花牌の配列の最大数 */
    public readonly static int RINSHAN_HAIS_MAX = 4;

    /** 各宝牌の配列の最大数 */
    public readonly static int DORA_HAIS_MAX = RINSHAN_HAIS_MAX + 1;

    // Fields.

    /** 山牌の配列 */
    private Hai[] m_yamaHais = new Hai[YAMA_HAIS_MAX];

    /** ツモ牌の配列 */
    private Hai[] m_tsumoHais = new Hai[TSUMO_HAIS_MAX];

    /** リンシャン(岭上开花)牌の配列 */
    private Hai[] m_rinshanHais = new Hai[RINSHAN_HAIS_MAX];

    /** リンシャン牌の位置 */
    private int m_RinshanHaisIndex;

    /** ツモ牌のインデックス(index) */
    private int m_TsumoHaisIndex;

    /** 表ドラ牌の配列 */
    private Hai[] m_omoteDoraHais = new Hai[DORA_HAIS_MAX];

    /** 裏ドラ牌の配列 */
    private Hai[] m_uraDoraHais = new Hai[DORA_HAIS_MAX];


    /**
     * 山を作成する。
     */
    public Yama() {
        setTsumoHaisStartIndex(0);

        for (int i = Hai.ID_ITEM_MIN; i < Hai.ID_ITEM_MAX; i++) {
            for (int j = 0; j < 4; j++) {
                m_yamaHais[(i * 4) + j] = new Hai(i);
            }
        }
    }

    // temply implement.
    public Hai[] getYamaHais() {
        return m_yamaHais;
    }

    /**
     * 洗牌する。
     */
    public void XiPai() {
        Hai temp;
        for (int i = 0, j; i < YAMA_HAIS_MAX; i++) 
        {
            // get a random index.
            j = Utils.GetRandomNum(0, YAMA_HAIS_MAX);

            // exchange hais.
            temp = m_yamaHais[i];
            m_yamaHais[i] = m_yamaHais[j];
            m_yamaHais[j] = temp;
        }
    }

    /**
     * ツモ牌を取得する。
     */
    public Hai PickTsumoHai() {
        if (m_TsumoHaisIndex >= (TSUMO_HAIS_MAX - m_RinshanHaisIndex)) {
            return null;
        }

        Hai tsumoHai = new Hai(m_tsumoHais[m_TsumoHaisIndex]);
        m_TsumoHaisIndex++;

        return tsumoHai;
    }

    /// <summary>
    /// 初始拿牌.
    /// </summary>
    public Hai[] PickHaipai() {
        if( m_TsumoHaisIndex >= (TSUMO_HAIS_MAX - m_RinshanHaisIndex) ) {
            return null;
        }

        Hai[] hais = new Hai[4];
        for( int i = 0; i < hais.Length; i++ ) 
        {
            hais[i] = new Hai( m_tsumoHais[m_TsumoHaisIndex] );

            m_TsumoHaisIndex++;

            if( m_TsumoHaisIndex >= (TSUMO_HAIS_MAX - m_RinshanHaisIndex) ) {
                //break;
            }
        }

        return hais;
    }

    /**
     * リンシャン(岭上开花)牌を取得する。
     */
    public Hai PickRinshanTsumoHai() {
        if (m_RinshanHaisIndex >= RINSHAN_HAIS_MAX) {
            return null;
        }

        Hai rinshanHai = new Hai(m_rinshanHais[m_RinshanHaisIndex]);
        m_RinshanHaisIndex++;

        return rinshanHai;
    }

    /**
     * 表ドラの配列を取得する。
     */
    public Hai[] getOmoteDoraHais() {

        int omoteDoraHaisLength = m_RinshanHaisIndex + 1;
        Hai[] omoteDoraHais = new Hai[omoteDoraHaisLength];

        for (int i = 0; i < omoteDoraHaisLength; i++) {
            omoteDoraHais[i] = new Hai(this.m_omoteDoraHais[i]);
        }

        return omoteDoraHais;
    }

    /**
     * 裏ドラの配列を取得する。
     */
    public Hai[] getUraDoraHais() {

        int uraDoraHaisLength = m_RinshanHaisIndex + 1;
        Hai[] uraDoraHais = new Hai[uraDoraHaisLength];

        for (int i = 0; i < uraDoraHaisLength; i++) {
            uraDoraHais[i] = new Hai(this.m_uraDoraHais[i]);
        }

        return uraDoraHais;
    }

    public Hai[] getAllDoraHais() {
        int omoteDoraHaisLength = m_RinshanHaisIndex + 1;
        int uraDoraHaisLength = m_RinshanHaisIndex + 1;
        int allDoraHaisLength = omoteDoraHaisLength + uraDoraHaisLength;

        Hai[] allDoraHais = new Hai[allDoraHaisLength];

        for (int i = 0; i < omoteDoraHaisLength; i++) {
            allDoraHais[i] = new Hai(this.m_omoteDoraHais[i]);
        }

        for (int i = 0; i < uraDoraHaisLength; i++) {
            allDoraHais[omoteDoraHaisLength + i] = new Hai(this.m_uraDoraHais[i]);
        }

        return allDoraHais;
    }

    /**
     * ツモ牌の開始位置を設定する。
     * 
     * TODO: currently tsumo hais array like:
     * 
     *  Wareme<-- Doras <-- Rinshan <-- Tsumo <--.
     *  
     *  the correct array should be:
     *  
     *  Wareme<-- Rinshan <-- Doras <-- Tsumo <--.
     */
    public bool setTsumoHaisStartIndex(int a_tsumoHaiStartIndex) {
        if (a_tsumoHaiStartIndex >= YAMA_HAIS_MAX) {
            return false;
        }

        int yamaHaisIndex = a_tsumoHaiStartIndex;

        // tsumo hais. 122.
        for (int i = 0; i < TSUMO_HAIS_MAX; i++) {
            m_tsumoHais[i] = m_yamaHais[yamaHaisIndex];

            yamaHaisIndex++;
            if (yamaHaisIndex >= YAMA_HAIS_MAX) {
                yamaHaisIndex = 0;
            }
        }
        m_TsumoHaisIndex = 0;


        // dora hais. 1+4=5. 
        //for( int i = 0; i < DORA_HAIS_MAX; i++ ) 
        for( int i = DORA_HAIS_MAX - 1; i >= 0; i-- ) // reverse.
        {
            // 表dora.
            m_omoteDoraHais[i] = m_yamaHais[yamaHaisIndex];

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX ) {
                yamaHaisIndex = 0;
            }

            // 里dora.
            m_uraDoraHais[i] = m_yamaHais[yamaHaisIndex];

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX ) {
                yamaHaisIndex = 0;
            }
        }

        // rinshan hais. 4.
        for( int i = 0; i < RINSHAN_HAIS_MAX; i++ )      
        {
            m_rinshanHais[i] = m_yamaHais[yamaHaisIndex];

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX ) {
                yamaHaisIndex = 0;
            }    
        }

        /// reverse.
        ///   2 0  ->  0 2
        ///   3 1      1 3
        Hai temp = m_rinshanHais[0];
        m_rinshanHais[0] = m_rinshanHais[2];
        m_rinshanHais[2] = temp;

        temp = m_rinshanHais[1];
        m_rinshanHais[1] = m_rinshanHais[3];
        m_rinshanHais[3] = temp;

        m_RinshanHaisIndex = 0;

        return true;
    }

    /**
     * ツモ牌の残り数を取得する。
     */
    public int getTsumoNokori() {
        return TSUMO_HAIS_MAX - m_TsumoHaisIndex - m_RinshanHaisIndex;
    }

    /**
     * 红ドラ牌。
     */
    public void setRedDora(int a_id, int a_num) {
        if (a_num <= 0) {
            return;
        }

        for (int i = 0; i < m_yamaHais.Length; i++) 
        {
            if (m_yamaHais[i].getID() == a_id) {
                m_yamaHais[i].setRed(true);

                a_num--;
                if (a_num <= 0) {
                    break;
                }
            }
        } 
    }

}