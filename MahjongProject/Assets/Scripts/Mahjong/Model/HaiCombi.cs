using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 上がりの組み合わせのクラスです
/// 
/// 胡牌的组合: 包括刻子(槓、碰)、顺子,
/// 頭(atama)是剩余那个牌
/// </summary>

public class HaiCombi 
{
    // 頭のNK
    public int atamaNumKind = 0;

    // 順子のNKの配列
    public int[] shunNumKinds = new int[4];

    // 順子のNKの配列の有効な個数
    public int shunCount = 0;

    // 刻子のNKの配列
    public int[] kouNumKinds = new int[4];

    // 刻子のNKの配列の有効な個数
    public int kouCount = 0;


    public void Clear()
    {
        for(int i = 0; i < shunNumKinds.Length; i++)
            shunNumKinds[i] = 0;
        for(int i = 0; i < kouNumKinds.Length; i++)
            kouNumKinds[i] = 0;
        
        this.atamaNumKind = 0;
        this.shunCount = 0;
        this.kouCount = 0;
    }

    public static void copy(HaiCombi dest, HaiCombi src)
    {
        dest.atamaNumKind = src.atamaNumKind;

        dest.shunCount = src.shunCount;
        for( int i = 0; i < dest.shunCount; i++ )
            dest.shunNumKinds[i] = src.shunNumKinds[i];

        dest.kouCount = src.kouCount;
        for( int i = 0; i < dest.kouCount; i++ )
            dest.kouNumKinds[i] = src.kouNumKinds[i];
    }
}
