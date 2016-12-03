using System;

/// <summary>
/// Player.
/// </summary>

public class Player
{
    private string _name;
    private IPlayer _iplayer;

    public Player(string name, IPlayer player)
    {
        this._name = name;
        this._iplayer = player;
    }

    #region proxy.
    public bool IsAI
    {
        get{ return _iplayer.IsAI; }
    }
    public PlayerAction getAction()
    {
        return _iplayer.getAction();
    }
    public int getSutehaiIndex()
    {
        return _iplayer.getAction().SutehaiIndex;
    }
    public EventID HandleEvent(EventID evtID, EKaze kazeFrom, EKaze kazeTo, Action<EventID> onAction)
    {
        _iplayer.HandleEvent(evtID, kazeFrom, kazeTo, onAction);
        return EventID.None;
    }
    #endregion proxy.


    public string Name
    {
        get{ return _name; }
    }

    // 手牌
    private Tehai _tehai = new Tehai();
    public Tehai Tehai
    {
        get{ return _tehai; }
    }

    // 河
    private Hou _hou = new Hou();
    public Hou Hou
    {
        get{ return _hou; }
    }

    // 自風
    private EKaze _jikaze;
    public EKaze JiKaze
    {
        get{ return _jikaze; }
        set{ _jikaze = value; }
    }

    // 点棒
    private int _tenbou;
    public int Tenbou 
    {
        get{ return _tenbou; }
        set{ _tenbou = value; }
    }

    // リーチ
    private bool _reach;
    public bool IsReach
    {
        get{ return _reach; }
        set{ _reach = value; }
    }

    // ダブルリーチ
    private bool _doubleReach;
    public bool IsDoubleReach
    {
        get{ return _doubleReach; }
        set{ _doubleReach = value; }
    }

    // 一発
    private bool _ippatsu;
    public bool IsIppatsu
    {
        get{ return _ippatsu; }
        set{ _ippatsu = value; }
    }

    // 捨牌数
    private int _suteHaisCount;
    public int SuteHaisCount
    {
        get{ return _suteHaisCount; }
        set{ _suteHaisCount = value; }
    }

    private CountFormat _countFormat = new CountFormat();
    public CountFormat FormatWorker
    {
        get{ return _countFormat; }
    }


    // 点棒を増やします
    public void increaseTenbou(int value)
    {
        Tenbou += value;
    }

    // 点棒を減らします
    public void reduceTenbou(int value)
    {
        Tenbou -= value;
    }

    // 听牌
    public bool isTenpai()
    {
        if( _reach == true )
            return true;

        for( int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++ )
        {
            Hai addHai = new Hai(id);
            FormatWorker.setCounterFormat(_tehai, addHai);

            if( FormatWorker.calculateCombisCount(null) > 0 )
                return true;
        }

        return false;
    }


    public void Init() 
    {
        // 手牌を初期化します。
        _tehai.initialize();

        // 河を初期化します。
        _hou.initialize();

        // リーチを初期化します。
        _reach = false;
        _doubleReach = false;
        _ippatsu = false;

        _suteHaisCount = 0;

        AttachAI();
    }

    public void AttachAI()
    {
        _iplayer.AttachToPlayer(this);
    }

}
