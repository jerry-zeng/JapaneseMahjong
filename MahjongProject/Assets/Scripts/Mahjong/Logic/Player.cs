
/// <summary>
/// Player.
/// </summary>

public class Player
{
    private IPlayer _iplayer;

    public Player(IPlayer player)
    {
        this._iplayer = player;
    }

    private IPlayer getPlayer() {
        return this._iplayer;
    }

    #region proxy.
    public string getName() {
        return this._iplayer.getName();
    }
    public bool isAI() {
        return this._iplayer.isAI();
    }
    public int getSutehaiIndex() {
        return this._iplayer.getSutehaiIndex();
    }
    public EventId HandleEvent(EventId evtID, EKaze kazeFrom, EKaze kazeTo) {
        return this._iplayer.HandleEvent(evtID, kazeFrom, kazeTo);
    }
    #endregion proxy.


    // 手牌
    private Tehai _tehai = new Tehai();
    public Tehai getTehai() {
        return _tehai;
    }

    // 河
    private Hou _hou = new Hou();
    public Hou getHou() {
        return _hou;
    }

    // 自風
    private EKaze _jikaze;
    public EKaze getJikaze() {
        return _jikaze;
    }
    public void setJikaze(EKaze jikaze) {
        this._jikaze = jikaze;
    }

    // 点棒
    private int _tenbou;
    public int getTenbou() {
        return _tenbou;
    }
    public void setTenbou(int tenbou) {
        this._tenbou = tenbou;
    }

    // 点棒を増やします
    public void increaseTenbou(int ten) {
        _tenbou += ten;
    }

    // 点棒を減らします
    public void reduceTenbou(int ten) {
        _tenbou -= ten;
    }

    // リーチ
    private bool _reach;
    public bool isReach() {
        return _reach;
    }
    public void setReach(bool reach) {
        this._reach = reach;
    }

    // ダブルリーチ
    private bool _doubleReach;
    public bool isDoubleReach() {
        return _doubleReach;
    }
    public void setDoubleReach(bool a_doubleReach) {
        this._doubleReach = a_doubleReach;
    }

    // 捨牌数
    private int _suteHaisCount;
    public void setSuteHaisCount(int a_suteHaisCount) {
        this._suteHaisCount = a_suteHaisCount;
    }
    public int getSuteHaisCount() {
        return _suteHaisCount;
    }

    // 一発
    private bool _ippatsu;
    public void setIppatsu(bool a_ippatsu) {
        this._ippatsu = a_ippatsu;
    }
    public bool isIppatsu() {
        return _ippatsu;
    }


    private CountFormat _countFormat = new CountFormat();

    // 听牌
    public bool isTenpai()
    {
        if( _reach == true )
            return true;

        for( int id = 0; id < Hai.ID_ITEM_MAX; id++ )
        {
            Hai addHai = new Hai(id);
            _countFormat.setCounterFormat(_tehai, addHai);

            if( _countFormat.calculateCombisCount(null) > 0 )
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

        _ippatsu = false;
        _doubleReach = false;

        _suteHaisCount = 0;
    }

}
