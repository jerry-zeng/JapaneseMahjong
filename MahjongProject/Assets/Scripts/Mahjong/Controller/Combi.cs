
/// <summary>
/// 上がりの組み合わせのクラスです。
/// 胡牌的组合。
/// </summary>

public class Combi 
{
    // 頭のNK
    public int _atamaNumKind = 0;

    // 順子のNKの配列
    public int[] _shunNumKinds = new int[4];

    // 順子のNKの配列の有効な個数
    public int _shunNum = 0;

    // 刻子のNKの配列
    public int[] _kouNumKinds = new int[4];

    // 刻子のNKの配列の有効な個数
    public int _kouNum = 0;


    public static void copy(Combi dest, Combi src)
    {
        dest._atamaNumKind = src._atamaNumKind;

        dest._shunNum = src._shunNum;
        for( int i = 0; i < dest._shunNum; i++ ) {
            dest._shunNumKinds[i] = src._shunNumKinds[i];
        }

        dest._kouNum = src._kouNum;
        for( int i = 0; i < dest._kouNum; i++ ) {
            dest._kouNumKinds[i] = src._kouNumKinds[i];
        }
    }
}
