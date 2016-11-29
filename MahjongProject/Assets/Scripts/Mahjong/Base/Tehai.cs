
/**
 * 手牌を管理する。
 * 手牌:拿到手上的牌
 */
public class Tehai 
{
    /** 純手牌の長さの最大値 */
    public readonly static int JYUN_TEHAI_LENGTH_MAX = 14;

    /** 副露の最大値 */
    public readonly static int FUURO_MAX = 4;

    /** 面子の長さ(3) */
    public readonly static int MENTSU_LENGTH_3 = 3;

    /** 面子の長さ(4) */
    public readonly static int MENTSU_LENGTH_4 = 4;

    // Fields.
    /** 純手牌 */
    private Hai[] m_jyunTehai = new Hai[JYUN_TEHAI_LENGTH_MAX];

    /** 純手牌の長さ */
    private int m_jyunTehaiLength;

    /** 副露の配列 */
    private Fuuro[] m_fuuros = new Fuuro[FUURO_MAX];

    /** 副露の個数 */
    private int m_fuuroNums;


    /**
     * 手牌を作成する。
     */
    public Tehai() {
        initialize();

        for( int i = 0; i < JYUN_TEHAI_LENGTH_MAX; i++ ) {
            m_jyunTehai[i] = new Hai();
        }

        for( int i = 0; i < FUURO_MAX; i++ ) {
            m_fuuros[i] = new Fuuro();
        }
    }

    /**
     * 手牌を初期化する。
     */
    public void initialize() {
        m_jyunTehaiLength = 0;
        m_fuuroNums = 0;
    }

    /**
     * 手牌をコピーする。
     */
    public static void copy(Tehai a_dest, Tehai a_src, bool jyunTehaiCopy) {
        if (jyunTehaiCopy == true) {
            a_dest.m_jyunTehaiLength = a_src.m_jyunTehaiLength;
            Tehai.copyJyunTehai(a_dest.m_jyunTehai, a_src.m_jyunTehai, a_dest.m_jyunTehaiLength);
        }

        a_dest.m_fuuroNums = a_src.m_fuuroNums;
        copyFuuros(a_dest.m_fuuros, a_src.m_fuuros, a_dest.m_fuuroNums);
    }

    /**
     * 純手牌を取得する。
     */
    public Hai[] getJyunTehai() {
        return m_jyunTehai;
    }

    /**
     * 純手牌の長さを取得する。
     */
    public int getJyunTehaiLength() {
        return m_jyunTehaiLength;
    }

    /**
     * 純手牌に牌を追加する。
     */
    public bool addJyunTehai(Hai a_hai) {
        if (m_jyunTehaiLength >= JYUN_TEHAI_LENGTH_MAX) {
            return false;
        }

        int i;
        for (i = m_jyunTehaiLength; i > 0; i--) {
            if (m_jyunTehai[i - 1].getID() <= a_hai.getID()) {
                break;
            }

            Hai.copy(m_jyunTehai[i], m_jyunTehai[i - 1]);
        }

        Hai.copy(m_jyunTehai[i], a_hai);
        m_jyunTehaiLength++;

        return true;
    }

    /**
     * 純手牌から指定位置の牌を削除する。
     */
    public bool removeJyunTehai(int a_index) {
        if (a_index >= JYUN_TEHAI_LENGTH_MAX) {
            return false;
        }

        for (int i = a_index; i < m_jyunTehaiLength - 1; i++) {
            Hai.copy(m_jyunTehai[i], m_jyunTehai[i + 1]);
        }

        m_jyunTehaiLength--;

        return true;
    }

    /**
     * 純手牌から指定の牌を削除する。
     */
    public bool removeJyunTehai(Hai a_hai) {
        int id = a_hai.getID();

        for (int i = 0; i < m_jyunTehaiLength; i++) {
            if (id == m_jyunTehai[i].getID()) {
                return removeJyunTehai(i);
            }
        }

        return false;
    }

    /**
     * 純手牌をコピーする。
     */
    public static bool copyJyunTehai(Hai[] a_dest, Hai[] a_src, int a_length) {
        if (a_length > JYUN_TEHAI_LENGTH_MAX) {
            return false;
        }

        for (int i = 0; i < a_length; i++) {
            Hai.copy(a_dest[i], a_src[i]);
        }

        return true;
    }

    /**
     * 純手牌の指定位置の牌をコピーする。
     */
    public bool copyJyunTehaiIndex(Hai a_hai, int a_index) {
        if (a_index >= m_jyunTehaiLength) {
            return false;
        }

        Hai.copy(a_hai, m_jyunTehai[a_index]);

        return true;
    }

    /**
     * チー(左)の可否をチェックする。
     */
    public bool validChiiLeft(Hai a_suteHai, Hai[] a_sarashiHais) {
        if (a_suteHai.isTsuu()) {
            return false;
        }

        if (a_suteHai.getNum() == Hai.NUM_8 || a_suteHai.getNum() == Hai.NUM_9) {
            return false;
        }

        if (m_fuuroNums >= FUURO_MAX) {
            return false;
        }

        int noKindLeft = a_suteHai.getNumKind();
        int noKindCenter = noKindLeft + 1;
        int noKindRight = noKindLeft + 2;

        for (int i = 0; i < m_jyunTehaiLength; i++)
        {
            if (m_jyunTehai[i].getNumKind() == noKindCenter) {
                for (int j = i + 1; j < m_jyunTehaiLength; j++) 
                {
                    if (m_jyunTehai[j].getNumKind() == noKindRight) {
                        a_sarashiHais[0] = new Hai(m_jyunTehai[i]);
                        a_sarashiHais[1] = new Hai(m_jyunTehai[j]);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    /**
     * チー(左)を設定する。
     */
    public bool setChiiLeft(Hai a_suteHai, int a_relation) {
        Hai[] sarashiHais = new Hai[2];

        if (!validChiiLeft(a_suteHai, sarashiHais)) {
            return false;
        }

        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        hais[0] = new Hai(a_suteHai);
        int newPickIndex = 0;

        int noKindLeft = a_suteHai.getNumKind();
        int noKindCenter = noKindLeft + 1;
        int noKindRight = noKindLeft + 2;

        for (int i = 0; i < m_jyunTehaiLength; i++) {
            if (m_jyunTehai[i].getNumKind() == noKindCenter) {
                hais[1] = new Hai(m_jyunTehai[i]);

                removeJyunTehai(i);

                for (int j = i; j < m_jyunTehaiLength; j++) {
                    if (m_jyunTehai[j].getNumKind() == noKindRight) {
                        hais[2] = new Hai(m_jyunTehai[j]);

                        removeJyunTehai(j);

                        hais[3] = new Hai();

                        m_fuuros[m_fuuroNums].setType(Fuuro.TYPE_MINSHUN);
                        m_fuuros[m_fuuroNums].setRelation(a_relation);
                        m_fuuros[m_fuuroNums].setHais(hais);
                        m_fuuros[m_fuuroNums].setNewPickIndex(newPickIndex);
                        m_fuuroNums++;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    /**
     * チー(中央)の可否をチェックする。
     */
    public bool validChiiCenter(Hai a_suteHai, Hai[] a_sarashiHais) {
        if (a_suteHai.isTsuu()) {
            return false;
        }

        if (a_suteHai.getNum() == Hai.NUM_1 || a_suteHai.getNum() == Hai.NUM_9) {
            return false;
        }

        if (m_fuuroNums >= FUURO_MAX) {
            return false;
        }

        int noKindCenter = a_suteHai.getNumKind();
        int noKindLeft = noKindCenter - 1;
        int noKindRight = noKindCenter + 1;

        for (int i = 0; i < m_jyunTehaiLength; i++) 
        {
            if (m_jyunTehai[i].getNumKind() == noKindLeft) {
                for (int j = i + 1; j < m_jyunTehaiLength; j++) 
                {
                    if (m_jyunTehai[j].getNumKind() == noKindRight) {
                        a_sarashiHais[0] = new Hai(m_jyunTehai[i]);
                        a_sarashiHais[1] = new Hai(m_jyunTehai[j]);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    /**
     * チー(中央)を設定する。
     */
    public bool setChiiCenter(Hai a_suteHai, int a_relation) {
        Hai[] sarashiHais = new Hai[2];

        if (!validChiiCenter(a_suteHai, sarashiHais)) {
            return false;
        }

        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        hais[1] = new Hai(a_suteHai);
        int newPickIndex = 1;

        int noKindCenter = a_suteHai.getNumKind();
        int noKindLeft = noKindCenter - 1;
        int noKindRight = noKindCenter + 1;

        for (int i = 0; i < m_jyunTehaiLength; i++) {
            if (m_jyunTehai[i].getNumKind() == noKindLeft) {
                hais[0] = new Hai(m_jyunTehai[i]);

                removeJyunTehai(i);

                for (int j = i; j < m_jyunTehaiLength; j++) {
                    if (m_jyunTehai[j].getNumKind() == noKindRight) {
                        hais[2] = new Hai(m_jyunTehai[j]);

                        removeJyunTehai(j);

                        hais[3] = new Hai();

                        m_fuuros[m_fuuroNums].setType(Fuuro.TYPE_MINSHUN);
                        m_fuuros[m_fuuroNums].setRelation(a_relation);
                        m_fuuros[m_fuuroNums].setHais(hais);
                        m_fuuros[m_fuuroNums].setNewPickIndex(newPickIndex);
                        m_fuuroNums++;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    /**
     * チー(右)の可否をチェックする。
     */
    public bool validChiiRight(Hai a_suteHai, Hai[] a_sarashiHais) {
        if (a_suteHai.isTsuu()) {
            return false;
        }

        if (a_suteHai.getNum() == Hai.NUM_1 || a_suteHai.getNum() == Hai.NUM_2) {
            return false;
        }

        if (m_fuuroNums >= FUURO_MAX) {
            return false;
        }

        int noKindRight = a_suteHai.getNumKind();
        int noKindLeft = noKindRight - 2;
        int noKindCenter = noKindRight - 1;

        for (int i = 0; i < m_jyunTehaiLength; i++) 
        {
            if (m_jyunTehai[i].getNumKind() == noKindLeft) {
                for (int j = i + 1; j < m_jyunTehaiLength; j++) 
                {
                    if (m_jyunTehai[j].getNumKind() == noKindCenter) {
                        a_sarashiHais[0] = new Hai(m_jyunTehai[i]);
                        a_sarashiHais[1] = new Hai(m_jyunTehai[j]);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    /**
     * チー(右)を設定する。
     */
    public bool setChiiRight(Hai a_suteHai, int a_relation) {
        Hai[] sarashiHais = new Hai[2];

        if (!validChiiRight(a_suteHai, sarashiHais)) {
            return false;
        }

        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        hais[2] = new Hai(a_suteHai);
        int newPickIndex = 2;

        int noKindRight = a_suteHai.getNumKind();
        int noKindLeft = noKindRight - 2;
        int noKindCenter = noKindRight - 1;

        for (int i = 0; i < m_jyunTehaiLength; i++) {
            if (m_jyunTehai[i].getNumKind() == noKindLeft) {
                hais[0] = new Hai(m_jyunTehai[i]);

                removeJyunTehai(i);

                for (int j = i; j < m_jyunTehaiLength; j++) {
                    if (m_jyunTehai[j].getNumKind() == noKindCenter) {
                        hais[1] = new Hai(m_jyunTehai[j]);

                        removeJyunTehai(j);

                        hais[3] = new Hai();

                        m_fuuros[m_fuuroNums].setType(Fuuro.TYPE_MINSHUN);
                        m_fuuros[m_fuuroNums].setRelation(a_relation);
                        m_fuuros[m_fuuroNums].setHais(hais);
                        m_fuuros[m_fuuroNums].setNewPickIndex(newPickIndex);
                        m_fuuroNums++;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    /**
     * ポン(碰)の可否をチェックする。
     */
    public bool validPon(Hai a_suteHai) {
        if (m_fuuroNums >= FUURO_MAX) {
            return false;
        }

        int id = a_suteHai.getID();
        for (int i = 0, count = 1; i < m_jyunTehaiLength; i++) {
            if (id == m_jyunTehai[i].getID()) {
                count++;
                if (count >= MENTSU_LENGTH_3) {
                    return true;
                }
            }
        }

        return false;
    }

    /**
     * ポンを設定する。
     */
    public bool setPon(Hai a_suteHai, int a_relation) {
        if (!validPon(a_suteHai)) {
            return false;
        }

        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        int iHais = 0;
        hais[iHais] = new Hai(a_suteHai);
        int newPickIndex = iHais;

        iHais++;

        int id = a_suteHai.getID();
        for (int i = 0; i < m_jyunTehaiLength; i++) {
            if (id == m_jyunTehai[i].getID()) {
                hais[iHais] = new Hai(m_jyunTehai[i]);
                iHais++;

                removeJyunTehai(i);
                i--;

                if (iHais >= MENTSU_LENGTH_3) {
                    break;
                }
            }
        }

        hais[iHais] = new Hai();

        m_fuuros[m_fuuroNums].setType(Fuuro.TYPE_MINKOU);
        m_fuuros[m_fuuroNums].setRelation(a_relation);
        m_fuuros[m_fuuroNums].setHais(hais);
        m_fuuros[m_fuuroNums].setNewPickIndex(newPickIndex);
        m_fuuroNums++;

        return true;
    }

    /**
     * 槓の可否をチェックする。
     */
    public int validKan(Hai a_addHai, Hai[] a_kanHais) {
        if (m_fuuroNums >= FUURO_MAX) {
            return 0;
        }

        int kanCount = 0;
        int id;

        addJyunTehai(a_addHai);

        // 加カンのチェック
        for (int i = 0; i < m_fuuroNums; i++) 
        {
            if (m_fuuros[i].getType() == Fuuro.TYPE_MINKOU) {
                for (int j = 0; j < m_jyunTehaiLength; j++) 
                {
                    if (m_fuuros[i].getHais()[0].getID() == m_jyunTehai[j].getID()) {
                        a_kanHais[kanCount] = new Hai(m_jyunTehai[j]);
                        kanCount++;
                    }
                }
            }
        }

        id = m_jyunTehai[0].getID();
        for (int i = 1, count = 1; i < m_jyunTehaiLength; i++) 
        {
            if (id == m_jyunTehai[i].getID()) {
                count++;
                if (count >= MENTSU_LENGTH_4) {
                    a_kanHais[kanCount] = new Hai(m_jyunTehai[i]);
                    kanCount++;
                }
            } 
            else {
                id = m_jyunTehai[i].getID();
                count = 1;
            }
        }

        removeJyunTehai(a_addHai);

        return kanCount;
    }

    public bool validDaiMinKan(Hai a_suteHai) {
        if (m_fuuroNums >= FUURO_MAX) {
            return false;
        }

        int id = a_suteHai.getID();
        for (int i = 0, count = 1; i < m_jyunTehaiLength; i++) {
            if (id == m_jyunTehai[i].getID()) {
                count++;
                if (count >= MENTSU_LENGTH_4) {
                    return true;
                }
            }
        }

        return false;
    }

    /**
     * 大明槓を設定する。
     */
    public bool setDaiMinKan(Hai a_suteHai, int a_relation) {
        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        int iHais = 0;
        hais[iHais] = new Hai(a_suteHai);
        int newPickIndex = iHais;

        iHais++;

        int suteHaiId = a_suteHai.getID();
        for (int i = 0; i < m_jyunTehaiLength; i++) {
            if (suteHaiId == m_jyunTehai[i].getID()) {
                hais[iHais] = new Hai(m_jyunTehai[i]);
                iHais++;

                removeJyunTehai(i);
                i--;

                if (iHais >= MENTSU_LENGTH_4) {
                    break;
                }
            }
        }

        m_fuuros[m_fuuroNums].setType(Fuuro.TYPE_DAIMINKAN);
        m_fuuros[m_fuuroNums].setRelation(a_relation);
        m_fuuros[m_fuuroNums].setHais(hais);
        m_fuuros[m_fuuroNums].setNewPickIndex(newPickIndex);
        m_fuuroNums++;

        return true;
    }

    /**
     * 加槓の可否をチェックする。
     */
    public bool validKaKan(Hai a_tsumoHai) {
        if (m_fuuroNums >= FUURO_MAX) {
            return false;
        }

        int id = a_tsumoHai.getID();
        for (int i = 0; i < m_fuuroNums; i++) {
            if (m_fuuros[i].getType() == Fuuro.TYPE_MINKOU) {
                if (id == (m_fuuros[i].getHais())[0].getID()) {
                    return true;
                }
            }
        }

        return false;
    }

    /**
     * 加槓を設定する。
     */
    public bool setKaKan(Hai a_tsumoHai) {
        if (!validKaKan(a_tsumoHai)) {
            return false;
        }

        Hai[] hais = null;

        int id = a_tsumoHai.getID();

        for (int i = 0; i < m_fuuroNums; i++) 
        {
            if (m_fuuros[i].getType() == Fuuro.TYPE_MINKOU) 
            {
                hais = m_fuuros[i].getHais();

                if (id == hais[0].getID()) {
                    Hai.copy(hais[3], a_tsumoHai);
                    m_fuuros[i].setType(Fuuro.TYPE_KAKAN);
                    m_fuuros[i].setHais(hais);
                    m_fuuros[i].setNewPickIndex(3);
                }
            }
        }

        return true;
    }

    /**
     * 暗槓を設定する。
     */
    public bool setAnKan(Hai a_tsumoHai, int a_relation) {
        Hai[] fuuroHais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        int id = a_tsumoHai.getID();
        for (int i = 0; i < m_fuuroNums; i++) 
        {
            if (m_fuuros[i].getType() == Fuuro.TYPE_MINKOU) 
            {
                fuuroHais = m_fuuros[i].getHais();

                if (id == fuuroHais[0].getID()) {
                    Hai.copy(fuuroHais[3], a_tsumoHai);
                    removeJyunTehai(a_tsumoHai);

                    m_fuuros[i].setType(Fuuro.TYPE_KAKAN);
                    m_fuuros[i].setHais(fuuroHais);
                    m_fuuros[i].setNewPickIndex(3);
                    //m_fuuroNums++;

                    return true;
                }
            }
        }

        int iHais = 0;
        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        for (int i = 0; i < m_jyunTehaiLength; i++) {
            if (id == m_jyunTehai[i].getID()) {
                hais[iHais] = new Hai(m_jyunTehai[i]);
                iHais++;

                removeJyunTehai(i);
                i--;

                if (iHais >= MENTSU_LENGTH_4) {
                    break;
                }
            }
        }

        m_fuuros[m_fuuroNums].setType(Fuuro.TYPE_ANKAN);
        m_fuuros[m_fuuroNums].setRelation(a_relation);
        m_fuuros[m_fuuroNums].setHais(hais);
        m_fuuros[m_fuuroNums].setNewPickIndex(MENTSU_LENGTH_4 - 1);
        m_fuuroNums++;

        return true;
    }

    /**
     * 副露の配列を取得する。
     */
    public Fuuro[] getFuuros() {
        return m_fuuros;
    }

    /**
     * 副露の個数を取得する。
     */
    public int getFuuroNum() {
        return m_fuuroNums;
    }

    /**
     * 鸣听Flag. 
     **/
    public bool isNaki() {
        for (int i = 0; i < m_fuuroNums; i++) {
            if (m_fuuros[i].getType() != Fuuro.TYPE_ANKAN) {
                return true;
            }
        }

        return false;
    }

    /**
     * 副露の配列をコピーする。
     */
    public static bool copyFuuros(Fuuro[] a_dest, Fuuro[] a_src, int a_length) {
        if (a_length > FUURO_MAX) {
            return false;
        }

        for (int i = 0; i < a_length; i++) {
            Fuuro.copy(a_dest[i], a_src[i]);
        }

        return true;
    }
}
