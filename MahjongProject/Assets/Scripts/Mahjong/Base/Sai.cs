
/**
 * サイコロ(色子)を管理する。
 */
public class Sai 
{
    /** 番号 */
    private int m_num = 1;

    /**
     * 番号を取得する。
     */
    public int getNum() {
        return m_num;
    }

    /**
     * サイコロを振って番号を取得する。
     * 摇色子，结果1-6.
     */
    public int SaiFuri() {
        m_num = Utils.GetRandomNum(1, 7);

        return m_num;
    }
}