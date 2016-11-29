
/**
 * 上がりの組み合わせのクラスです。
 * 牌的组合。
 */

public class Combi 
{
    /** 頭のNK */
    public int m_atamaNumKind = 0;

    /** 順子のNKの配列 */
    public int[] m_shunNumKinds = new int[4];
    /** 順子のNKの配列の有効な個数 */
    public int m_shunNum = 0;

    /** 刻子のNKの配列 */
    public int[] m_kouNumKinds = new int[4];
    /** 刻子のNKの配列の有効な個数 */
    public int m_kouNum = 0;

    /**
     * Combiをコピーする。
     */
    public static void copy(Combi a_dest, Combi a_src)
    {
        a_dest.m_atamaNumKind = a_src.m_atamaNumKind;

        a_dest.m_shunNum = a_src.m_shunNum;
        for( int i = 0; i < a_dest.m_shunNum; i++ ) {
            a_dest.m_shunNumKinds[i] = a_src.m_shunNumKinds[i];
        }

        a_dest.m_kouNum = a_src.m_kouNum;
        for( int i = 0; i < a_dest.m_kouNum; i++ ) {
            a_dest.m_kouNumKinds[i] = a_src.m_kouNumKinds[i];
        }
    }
}
