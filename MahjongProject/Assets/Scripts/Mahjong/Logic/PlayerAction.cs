
public enum EActionState
{
    None           = 0,
    Action_Wait    = 1,
    Sutehai_Select = 2,
    Chii_Select    = 3,
    Kan_Select     = 4,
    Reach_Select   = 5,
    Ron_Select     = 6,
    Tsumo_Select   = 7,
}


public class PlayerAction 
{
    private EActionState _state = EActionState.None;

    // 捨牌のインデックス
    private int _sutehaiIndex;

    // ロンが可能
    private bool _validRon;

    // ツモが可能
    private bool _validTsumo;

    // ポンが可能
    private bool _validPon;

    private bool _validReach;

    // メニュー選択
    private int _menuSelect;

    private int _menuNum;

    public int[] m_indexs;
    public int m_indexNum;

    private bool _dispMenu;


    public PlayerAction() {
        Init();
    }

    public void Init()
    {
        _state = EActionState.None;
        _sutehaiIndex = int.MaxValue;
        _validRon = false;
        _validTsumo = false;
        _validPon = false;
        _validReach = false;

        _validChiiLeft = false;
        _validChiiCenter = false;
        _validChiiRight = false;

        _validKan = false;
        _kanNum = 0;
        _kanSelect = 0;
        _dispMenu = false;
        _validDaiMinKan = false;

        _menuNum = 0;

        setMenuSelect(5);
    }


    public void setMenuNum(int menuNum) {
        this._menuNum = menuNum;
    }
    public int getMenuNum() {
        return _menuNum;
    }


    public void setState(EActionState state) {
        this._state = state;
    }
    public EActionState getState() {
        return _state;
    }


    public void actionWait() {

    }


    public void setSutehaiIndex(int sutehaiIdx) {
        this._sutehaiIndex = sutehaiIdx;
    }
    public int getSutehaiIndex() {
        return _sutehaiIndex;
    }


    public bool isActionRequest() 
    {
        return _validRon || _validTsumo || _validPon || _validReach
            || _validChiiLeft || _validChiiCenter || _validChiiRight 
            || _validKan || _validDaiMinKan;
    }


    public void setValidRon(bool validRon) {
        this._validRon = validRon;
    }
    public bool isValidRon() {
        return _validRon;
    }


    public void setValidTsumo(bool validTsumo) {
        this._validTsumo = validTsumo;
    }
    public bool isValidTsumo() {
        return _validTsumo;
    }


    public void setValidPon(bool validPon) {
        this._validPon = validPon;
    }
    public bool isValidPon() {
        return _validPon;
    }


    public void setValidReach(bool validReach) {
        this._validReach = validReach;
    }
    public bool isValidReach() {
        return _validReach;
    }


    public void setMenuSelect(int menuSelect) {
        this._menuSelect = menuSelect;
    }
    public int getMenuSelect() {
        return _menuSelect;
    }


    private EventID _chiiEventId;
    public EventID getChiiEventId() {
        return _chiiEventId;
    }
    public void setChiiEventId(EventID a_chiiEventId) {
        _chiiEventId = a_chiiEventId;
    }

    private Hai[] _sarashiHaiLeft = new Hai[2];
    private Hai[] _sarashiHaiCenter = new Hai[2];
    private Hai[] _sarashiHaiRight = new Hai[2];
    private bool _validChiiLeft = false;
    private bool _validChiiCenter = false;
    private bool _validChiiRight = false;

    public void setValidChiiLeft(bool validChii, Hai[] sarashiHai) {
        this._validChiiLeft = validChii;
        this._sarashiHaiLeft = sarashiHai;
    }
    public bool isValidChiiLeft() {
        return _validChiiLeft;
    }
    public Hai[] getSarachiHaiLeft() {
        return _sarashiHaiLeft;
    }

    public void setValidChiiCenter(bool validChii, Hai[] sarashiHai) {
        this._validChiiCenter = validChii;
        this._sarashiHaiCenter = sarashiHai;
    }
    public bool isValidChiiCenter() {
        return _validChiiCenter;
    }
    public Hai[] getSarachiHaiCenter() {
        return _sarashiHaiCenter;
    }

    public void setValidChiiRight(bool validChii, Hai[] sarashiHai) {
        this._validChiiRight = validChii;
        this._sarashiHaiRight = sarashiHai;
    }
    public bool isValidChiiRight() {
        return _validChiiRight;
    }
    public Hai[] getSarachiHaiRight() {
        return _sarashiHaiRight;
    }


    private bool _validKan = false;
    private Hai[] _kanHais = new Hai[3];
    private int _kanNum = 0;


    public void setValidKan(bool validKan, Hai[] kanHais, int kanNum) {
        this._validKan = validKan;
        this._kanHais = kanHais;
        this._kanNum = kanNum;
    }

    public Hai[] getKanHais() {
        return _kanHais;
    }

    public int getKanNum() {
        return _kanNum;
    }


    private bool _validDaiMinKan = false;
    public void setValidDaiMinKan(bool validDaiMinKan) {
        this._validDaiMinKan = validDaiMinKan;
    }
    public bool isValidDaiMinKan() {
        return _validDaiMinKan;
    }


    private int _kanSelect = 0;
    public void setKanSelect(int kanSelect) {
        this._kanSelect = kanSelect;
    }
    public int getKanSelect() {
        return _kanSelect;
    }

    public bool isValidKan() {
        return _validKan;
    }

    public void setDispMenu(bool dispMenu) {
        this._dispMenu = dispMenu;
    }
    public bool isDispMenu() {
        return _dispMenu;
    }

    private int _reachSelect;
    public void setReachSelect(int reachSelect) {
        this._reachSelect = reachSelect;
    }
    public int getReachSelect() {
        return _reachSelect;
    }

}