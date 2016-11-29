

/**
 * カウントフォーマット(CountFormat)を管理するクラスです。
 */
public class CountFormat 
{
    #region internal class.
    public class Count 
    {
        /** NK */
        public int m_numKind = 0;

        /** 個数 */
        public int m_num = 0;

        public void initialize() {
            m_numKind = 0;
            m_num = 0;
        }
    }

    /**
     * 上がりの組み合わせの配列を管理するクラスです。
     */
    public class CombiManage 
    {
        /** 上がりの組み合わせの配列の最大値 */
        public const int COMBI_MAX = 10;

        /** 上がりの組み合わせの配列 */
        public Combi[] m_combis;
        /** 上がりの組み合わせの配列の有効な個数 */
        public int m_combiNum = 0;

        /** カウントの配列の残りの個数 */
        public int m_remain = 0;

        public Combi m_work;


        /** 作業領域 */
        public CombiManage(){
            m_work = new Combi();

            m_combis = new Combi[COMBI_MAX];
            for (int i = 0; i < m_combis.Length; i++) {
                m_combis[i] = new Combi();
            }
        }

        /**
         * 作業領域を初期化する。
         *
         * @param カウントの配列の残りの個数
         */
        public void initialize(int a_remain) {
            this.m_combiNum = 0;
            this.m_remain = a_remain;

            m_work.m_atamaNumKind = 0;
            m_work.m_shunNum = 0;
            m_work.m_kouNum = 0;
        }

        /**
         * 上がりの組み合わせを追加する。
         */
        public void add() {
            Combi.copy(m_combis[m_combiNum++], m_work);
        }
    }
    #endregion internal class.


    /** カウント(count)の最大値 */
    public const int COUNT_MAX = 14 + 2;

    /** カウントの配列 */
    public Count[] m_counts;
    /** カウントの配列の有効な個数 */
    public int m_countNum;

    /** 上がりの組み合わせの配列を管理 */
    private CombiManage m_combiManage;

    public CountFormat(){
        m_combiManage = new CombiManage();

        m_counts = new Count[COUNT_MAX];
        for (int i = 0; i < COUNT_MAX; i++) {
            m_counts[i] = new Count();
        }
    }

    /**
     * カウントの配列の長さの合計を取得する。
     *
     * @return カウントの配列の長さの合計
     */
    private int getTotalCountLength() {
        int totalCountLength = 0;

        for (int i = 0; i < m_countNum; i++) {
            totalCountLength += m_counts[i].m_num;
        }

        return totalCountLength;
    }

    /**
     * カウントフォーマットを設定する。
     *
     * @param a_tehai 手牌
     *            
     * @param a_addHai  追加する牌
     */
    public void setCountFormat(Tehai a_tehai, Hai a_addHai) {
        for (int i = 0; i < m_counts.Length; i++) {
            m_counts[i].initialize();
        }
        m_countNum = 0;

        int addHaiNumKind = 0;
        bool set = true;
        if (a_addHai != null) {
            addHaiNumKind = a_addHai.getNumKind();
            set = false;
        }

        int jyunTehaiNumKind;
        int jyunTehaiLength = a_tehai.getJyunTehaiLength();

        for (int i = 0; i < jyunTehaiLength;) {
            jyunTehaiNumKind = (a_tehai.getJyunTehai())[i].getNumKind();

            if (!set && (jyunTehaiNumKind > addHaiNumKind)) {
                set = true;
                m_counts[m_countNum].m_numKind = addHaiNumKind;
                m_counts[m_countNum].m_num = 1;
                m_countNum++;
                continue;
            }

            m_counts[m_countNum].m_numKind = jyunTehaiNumKind;
            m_counts[m_countNum].m_num = 1;

            if (!set && (jyunTehaiNumKind == addHaiNumKind)) {
                set = true;
                m_counts[m_countNum].m_num++;
            }

            while (++i < jyunTehaiLength) 
            {
                if (jyunTehaiNumKind == (a_tehai.getJyunTehai())[i].getNumKind()) {
                    m_counts[m_countNum].m_num++;
                } 
                else {
                    break;
                }
            }

            m_countNum++;
        }

        if (!set) {
            m_counts[m_countNum].m_numKind = addHaiNumKind;
            m_counts[m_countNum].m_num = 1;
            m_countNum++;
        }

        for (int i = 0; i < m_countNum; i++) {
            if (m_counts[i].m_num > 4) {
                // 5つ目の追加牌は無効とする。
                m_counts[i].m_num--;
            }
        }
    }


    public Combi[] getCombis() {
        return m_combiManage.m_combis;
    }

    public int getCombiNum() {
        return m_combiManage.m_combiNum;
    }

    /**
     * 上がりの組み合わせの配列を取得する。
     *
     * @param a_combis  上がりの組み合わせの配列
     *
     */
    public int getCombis(Combi[] a_combis) {
        m_combiManage.initialize( getTotalCountLength() );
        searchCombi(0);

        if( m_combiManage.m_combiNum == 0 ) 
        {
            m_chiitoitsu = checkChiitoitsu();

            if( m_chiitoitsu ) {
                m_combiManage.m_combiNum = 1;
            }
            else 
            {
                m_kokushi = checkKokushi();
                if( m_kokushi ) {
                    m_combiManage.m_combiNum = 1;
                }
            }
        }

        return m_combiManage.m_combiNum;
    }


    // 七对子.
    private bool m_chiitoitsu;
    public bool isChiitoitsu() {
        return m_chiitoitsu;
    }

    private bool checkChiitoitsu() {
        int count = 0;
        for (int i = 0; i < m_countNum; i++) 
        {
            if (m_counts[i].m_num == 2) {
                count++;
            } 
            else {
                return false;
            }
        }

        if (count == 7) {
            return true;
        }
        return false;
    }


    // 国士无双.
    private bool m_kokushi;
    public bool isKokushi() {
        return m_kokushi;
    }

    private bool checkKokushi() {
        //牌の数を調べるための配列 (0番地は使用しない）
        int[] checkId = {Hai.ID_WAN_1, Hai.ID_WAN_9, Hai.ID_PIN_1, Hai.ID_PIN_9, Hai.ID_SOU_1, Hai.ID_SOU_9,
            Hai.ID_TON, Hai.ID_NAN, Hai.ID_SYA, Hai.ID_PE, Hai.ID_HAKU, Hai.ID_HATSU, Hai.ID_CHUN
        };
        int[] countHai = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //手牌のIDを検索する
        for(int i = 0 ; i < m_countNum ; i++){
            for(int j = 0 ; j < checkId.Length ; j++){
                if(Hai.NumKindToID(m_counts[i].m_numKind) == checkId[j]){
                    countHai[j] = m_counts[i].m_num;
                }
            }
        }

        bool atama = false;
        //国士無双が成立しているか調べる(手牌がすべて1.9字牌 すべての１,９字牌を持っている）
        for(int i = 0 ; i < countHai.Length ; i++){
            //0枚の牌があれば不成立
            if(countHai[i] == 0){
                return false;
            }
            if(countHai[i] == 2){
                atama = true;
            }
        }
        //条件を満たしていれば成立
        if (atama) {
            return true;
        } 
        else {
            return false;
        }
    }


    /**
     * 上がりの組み合わせを再帰的に探す。
     *
     * @param a_iSearch  検索位置.
     */
    private void searchCombi(int a_iSearch) {
        // 検索位置を更新する。
        for (; a_iSearch < m_countNum; a_iSearch++) {
            if (m_counts[a_iSearch].m_num > 0) {
                break;
            }
        }

        if (a_iSearch >= m_countNum) {
            return;
        }

        // 頭をチェック(check)する。
        if (m_combiManage.m_work.m_atamaNumKind == 0) 
        {
            if (m_counts[a_iSearch].m_num >= 2) {
                // 頭を確定する。
                m_counts[a_iSearch].m_num -= 2;
                m_combiManage.m_remain -= 2;
                m_combiManage.m_work.m_atamaNumKind = m_counts[a_iSearch].m_numKind;

                // 上がりの組み合わせを見つけたら追加する。
                if (m_combiManage.m_remain <= 0) {
                    m_combiManage.add();
                } 
                else {
                    searchCombi(a_iSearch);
                }

                // 確定した頭を戻す。
                m_counts[a_iSearch].m_num += 2;
                m_combiManage.m_remain += 2;
                m_combiManage.m_work.m_atamaNumKind = 0;
            }
        }

        // 順子をチェックする。
        int left = a_iSearch;
        int center = a_iSearch + 1;
        int right = a_iSearch + 2;

        if (!Hai.isTsuu(m_counts[left].m_numKind)) 
        {
            if ((m_counts[left].m_numKind + 1 == m_counts[center].m_numKind) && (m_counts[center].m_num > 0)) 
            {
                if ((m_counts[left].m_numKind + 2 == m_counts[right].m_numKind) && (m_counts[right].m_num > 0)) 
                {
                    // 順子を確定する。
                    m_counts[left].m_num--;
                    m_counts[center].m_num--;
                    m_counts[right].m_num--;
                    m_combiManage.m_remain -= 3;
                    m_combiManage.m_work.m_shunNumKinds[m_combiManage.m_work.m_shunNum] = m_counts[left].m_numKind;
                    m_combiManage.m_work.m_shunNum++;

                    // 上がりの組み合わせを見つけたら追加する。
                    if (m_combiManage.m_remain <= 0) {
                        m_combiManage.add();
                    } 
                    else {
                        searchCombi(a_iSearch);
                    }

                    // 確定した順子を戻す。
                    m_counts[left].m_num++;
                    m_counts[center].m_num++;
                    m_counts[right].m_num++;
                    m_combiManage.m_remain += 3;
                    m_combiManage.m_work.m_shunNum--;
                }
            }
        }

        // 刻子をチェックする。
        if (m_counts[a_iSearch].m_num >= 3) {
            // 刻子を確定する。
            m_counts[a_iSearch].m_num -= 3;
            m_combiManage.m_remain -= 3;
            m_combiManage.m_work.m_kouNumKinds[m_combiManage.m_work.m_kouNum] = m_counts[a_iSearch].m_numKind;
            m_combiManage.m_work.m_kouNum++;

            // 上がりの組み合わせを見つけたら追加する。
            if (m_combiManage.m_remain <= 0) {
                m_combiManage.add();
            } 
            else {
                searchCombi(a_iSearch);
            }

            // 確定した刻子を戻す。/
            m_combiManage.m_remain += 3;
            m_counts[a_iSearch].m_num += 3;
            m_combiManage.m_work.m_kouNum--;
        }
    }
}
