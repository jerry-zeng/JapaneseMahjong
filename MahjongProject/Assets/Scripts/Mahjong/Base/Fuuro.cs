
/// <summary>
/// 副露を管理する。
/// 副露包括吃牌(チー),碰牌(ポン)和杠，也就是放桌角的那些牌.
/// </summary>

public class Fuuro 
{
    // 種別
    private EFuuroType _type = EFuuroType.None;

    // 構成牌
    private Hai[] _hais;

    // 他家との関係(和其他人的关系)
    private int _relation = -1;

    // index of the new hai in m_hais that is newly picked by player or from others(AI). 
    private int _newPickIndex = -1;


    public Fuuro()
    {
        _hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        for (int i = 0; i < _hais.Length; i++) {
            _hais[i] = new Hai();
        }
    }


    public EFuuroType Type
    {
        get{ return _type; }
        set{ _type = value; }
    }

    public Hai[] Hais
    {
        get{ return _hais; }
        set{ _hais = value; }
    }

    public int Relation
    {
        get{ return _relation; }
        set{ _relation = value; }
    }

    public int NewPickIndex
    {
        get{ return _newPickIndex; }
        set{ _newPickIndex = value; }
    }


    public void Update(EFuuroType newType, Hai[] newHais, int newRelation, int newPick)
    {
        Type = newType;
        Hais = newHais;
        Relation = newRelation;
        NewPickIndex = newPick;
    }
    public void Update(EFuuroType newType, Hai[] newHais, int newPick)
    {
        Type = newType;
        Hais = newHais;
        NewPickIndex = newPick;
    }

    /// <summary>
    /// Copy the specified src furro to dest.
    /// 副露をコピーする
    /// </summary>

    public static void copy(Fuuro dest, Fuuro src)
    {
        dest._type = src._type;
        dest._relation = src._relation;
        dest._newPickIndex = src._newPickIndex;

        for (int i = 0; i < Mahjong.MENTSU_HAI_MEMBERS_4; i++) {
            Hai.copy(dest._hais[i], src._hais[i]);
        }
    }
}
