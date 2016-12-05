using System.Collections.Generic;

public enum EActionState
{
    None           = 0,
    Select_Sutehai = 1,
    Select_Chii    = 2,
    Select_Kan     = 3,
    Select_Reach   = 4,
    Select_Agari   = 5, // ron or tsumo.
}

public enum EActionType
{
    Chii   = 0,
    Pon    = 1,
    Kan    = 2,
    Reach  = 3,
    Agari  = 4,
    Nagashi= 5,
}

public enum EKanType
{
    DaiMinKan,
    KaKan,
    AnKan
}

public enum EAgariType
{
    Tsumo,
    Ron
}

public enum EChiiType
{
    Left,
    Center,
    Right
}


public class PlayerAction 
{
    public PlayerAction()
    {
        Reset();
    }

    public void Reset()
    {
        _response = EResponse.Nagashi;
        _state = EActionState.None;
        _sutehaiIndex = 13;

        _validRon = false;
        _validTsumo = false;
        _validPon = false;
        _validReach = false;

        _validChiiLeft = false;
        _validChiiCenter = false;
        _validChiiRight = false;

        _validKan = false;
        _kanCount = 0;
        _validDaiMinKan = false;

        _displayMenu = false;
        ClearMenus();
        _menuSelectIndex = -1;

        _reachSelectIndex = -1;
        _ponSelectIndex = -1;
        _kanSelectIndex = -1;
        _chiiSelectType = 0;

        _indexList.Clear();
    }


    public void Waiting() {

    }

    public bool isActionRequest() 
    {
        return _validRon || _validTsumo || _validPon || _validReach
            || _validChiiLeft || _validChiiCenter || _validChiiRight 
            || _validKan || _validDaiMinKan;
    }


    private EResponse _response = EResponse.Nagashi;
    public EResponse Response
    {
        get{ return _response; }
        set{ _response = value; }
    }

    private EActionState _state = EActionState.None;
    public EActionState State
    {
        get{ return _state; }
        set{ _state = value; }
    }


    public const int Sutehai_Index_Max = 13;

    // 捨牌のインデックス
    private int _sutehaiIndex = Sutehai_Index_Max;
    /// <summary>
    /// Gets or sets the index of the sutehai (0~13).
    /// </summary>
    public int SutehaiIndex
    {
        get{ return _sutehaiIndex; }
        set{ _sutehaiIndex = value; }
    }


    // ロンが可能
    private bool _validRon;
    public bool IsValidRon
    {
        get{ return _validRon; }
        set{ _validRon = value; }
    }

    // ツモが可能
    private bool _validTsumo;
    public bool IsValidTsumo
    {
        get{ return _validTsumo; }
        set{ _validTsumo = value; }
    }

    // ポンが可能
    private bool _validPon;
    public bool IsValidPon
    {
        get{ return _validPon; }
        set{ _validPon = value; }
    }

    // Reachが可能
    private bool _validReach;
    public bool IsValidReach
    {
        get{ return _validReach; }
        set{ _validReach = value; }
    }

    #region Chii
    private Hai[] _sarashiHaiLeft = new Hai[2];
    private bool _validChiiLeft = false;

    public void setValidChiiLeft(bool validChii, Hai[] sarashiHai)
    {
        this._validChiiLeft = validChii;
        this._sarashiHaiLeft = sarashiHai;
    }
    public bool isValidChiiLeft() {
        return _validChiiLeft;
    }
    public Hai[] getSarachiHaiLeft() {
        return _sarashiHaiLeft;
    }


    private Hai[] _sarashiHaiCenter = new Hai[2];
    private bool _validChiiCenter = false;

    public void setValidChiiCenter(bool validChii, Hai[] sarashiHai)
    {
        this._validChiiCenter = validChii;
        this._sarashiHaiCenter = sarashiHai;
    }
    public bool isValidChiiCenter() {
        return _validChiiCenter;
    }
    public Hai[] getSarachiHaiCenter() {
        return _sarashiHaiCenter;
    }


    private Hai[] _sarashiHaiRight = new Hai[2];
    private bool _validChiiRight = false;

    public void setValidChiiRight(bool validChii, Hai[] sarashiHai)
    {
        this._validChiiRight = validChii;
        this._sarashiHaiRight = sarashiHai;
    }
    public bool isValidChiiRight() {
        return _validChiiRight;
    }
    public Hai[] getSarachiHaiRight() {
        return _sarashiHaiRight;
    }
    #endregion

    private bool _validKan = false;
    private Hai[] _kanHais = new Hai[3];
    private int _kanCount = 0;

    // Kanが可能
    public void setValidKan(bool validKan, Hai[] kanHais, int kanCount) 
    {
        this._validKan = validKan;
        this._kanHais = kanHais;
        this._kanCount = kanCount;
    }
    public bool isValidKan() {
        return _validKan;
    }
    public Hai[] getKanHais() {
        return _kanHais;
    }
    public int getKanCount() {
        return _kanCount;
    }

    // DaiMinKanが可能
    private bool _validDaiMinKan = false;
    public bool IsValidDaiMinKan
    {
        get{ return _validDaiMinKan; }
        set{ _validDaiMinKan = value; }
    }

    private List<int> _indexList = new List<int>();
    public List<int> HaiIndexList
    {
        get{ return _indexList; }
        set{ _indexList = value; }
    }

    #region Menu
    private List<EResponse> _menuList = new List<EResponse>();
    public List<EResponse> MenuList
    {
        get{ return _menuList; }
    }
    public void ClearMenus()
    {
        _menuList.Clear();
    }


    private bool _displayMenu;
    public bool IsDisplayingMenu
    {
        get{ return _displayMenu; }
        set{ _displayMenu = value; }
    }

    private int _menuSelectIndex;
    public int MenuSelectIndex
    {
        get{ return _menuSelectIndex; }
        set{ _menuSelectIndex = value; }
    }


    private int _reachSelectIndex;
    public int ReachSelectIndex
    {
        get{ return _reachSelectIndex; }
        set{ _reachSelectIndex = value; }
    }

    private int _ponSelectIndex = 0;
    public int PonSelectIndex
    {
        get{ return _ponSelectIndex; }
        set{ _ponSelectIndex = value; }
    }

    private int _kanSelectIndex = 0;
    public int KanSelectIndex
    {
        get{ return _kanSelectIndex; }
        set{ _kanSelectIndex = value; }
    }

    private int _chiiSelectType;
    public int ChiiSelectType
    {
        get{ return _chiiSelectType; }
        set{ _chiiSelectType = value; }
    }
    #endregion
}