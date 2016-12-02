using System.Collections.Generic;

/// <summary>
/// 河を管理する。
/// 日本麻将，玩家打出去的牌放置的那块区域叫河。
/// </summary>

public class Hou 
{
    // 捨牌の配列の長さの最大値.
    public readonly static int SUTE_HAIS_LENGTH_MAX = 24;

    // 捨牌の配列.
    protected List<SuteHai> _suteHais = new List<SuteHai>(SUTE_HAIS_LENGTH_MAX);


    public void initialize()
    {
        _suteHais.Clear();
    }

    // 河をコピーする
    public static void copy(Hou dest, Hou src) 
    {
        dest._suteHais.Clear();

        for (int i = 0; i < src._suteHais.Count; i++)
        {
            SuteHai suteHai = new SuteHai();
            SuteHai.copy(suteHai, src._suteHais[i]);
            dest._suteHais.Add(suteHai);
        }            
    }


    // 捨牌の配列を取得する
    public SuteHai[] getSuteHais()
    {
        return _suteHais.ToArray();
    }

    // 捨牌の配列に牌を追加する
    public bool addHai(Hai hai)
    {
        if (_suteHais.Count >= SUTE_HAIS_LENGTH_MAX)
            return false;

        SuteHai suteHai = new SuteHai();
        SuteHai.copy(suteHai, hai);
        _suteHais.Add(suteHai);

        return true;
    }

    // 捨牌の配列の最後の牌に、鳴きフラグを設定する
    public bool setNaki(bool isNaki)
    {
        if (_suteHais.Count <= 0)
            return false;
        
        _suteHais[_suteHais.Count-1].IsNaki = isNaki;

        return true;
    }

    // 捨牌の配列の最後の牌に、リーチフラグを設定する
    public bool setReach(bool isReach)
    {
        if (_suteHais.Count <= 0)
            return false;

        _suteHais[_suteHais.Count-1].IsReach = isReach;

        return true;
    }

    // 捨牌の配列の最後の牌に、手出しフラグを設定する
    public bool setTedashi(bool isTedashi)
    {
        if (_suteHais.Count <= 0)
            return false;

        _suteHais[_suteHais.Count-1].IsTedashi = isTedashi;

        return true;
    }

}
