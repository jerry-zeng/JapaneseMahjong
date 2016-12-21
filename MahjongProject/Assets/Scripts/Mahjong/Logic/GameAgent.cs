using System.Collections.Generic;

/// <summary>
/// プレイヤー(Player)に提供する情報を管理するクラスです。
/// </summary>

public class GameAgent 
{
    private GameAgent(){
        
    }
    private static GameAgent _instance;
    public static GameAgent Instance
    {
        get{
            if(_instance == null)
                _instance = new GameAgent();
            return _instance;
        }
    }

    private Mahjong _game;
    public void Initialize(Mahjong game)
    {
        this._game = game;
    }


    // リーチを取得する
    public bool isReach(EKaze kaze) {
        return _game.getPlayer(kaze).IsReach;
    }

    // ツモの残り数を取得する
    public int getTsumoRemain() {
        return _game.getTsumoRemainCount();
    }

    // 局を取得する
    public int getkyoku() {
        return _game.Kyoku;
    }

    // 本場を取得する
    public int getHonba() {
        return _game.HonBa;
    }

    // リーチ棒の数を取得する
    public int getReachbou() {
        return _game.ReachBou;
    }

    public Hai getSuteHai(){
        Hai suteHai = _game.SuteHai;
        return suteHai == null? suteHai : new Hai(suteHai);
    }
    public Hai getTsumoHai(){
        Hai suteHai = _game.TsumoHai;
        return suteHai == null? suteHai : new Hai(suteHai);
    }

    public void PostUiEvent(UIEventType eventType, params object[] args)
    {
        EventManager.Get().SendEvent(eventType, args);
    }

    public SuteHai[] getSuteHaiList() {
        return _game.AllSuteHaiList.ToArray();
    }

    public int getPlayerSuteHaisCount(EKaze kaze) {
        return _game.getPlayer(kaze).SuteHaisCount;
    }

    public Hai[] getOmotoDoraHais(){
        return _game.getOmotoDoras();
    }

    public Hai[] getUraDoraHais() {
        return _game.getUraDoras();
    }

    public EKaze getManKaze() {
        return _game.getManKaze();
    }


    // 起(親)家のプレイヤーインデックスを取得する
    public int getChiichaIndex() {
        return _game.ChiiChaIndex;
    }

    public AgariInfo getAgariInfo() {
        return _game.AgariInfo;
    }

    public HaiCombi[] combis
    {
        get{ return _game.Combis; }
    }


    // あがり点を取得する
    public int getAgariScore(Tehai tehai, Hai addHai) {
        return _game.GetAgariScore(tehai, addHai);
    }


    #region Logic
    protected static CountFormat countFormat = new CountFormat();


    // 打哪些牌可以立直.
    public bool tryGetReachHaiIndex(Tehai a_tehai, Hai tsumoHai, out List<int> haiIndexList)
    {
        haiIndexList = new List<int>();

        // 鳴いている場合は、リーチできない。
        if( a_tehai.isNaki() )
            return false;

        Tehai tehai_copy = new Tehai( a_tehai );

        // As _jyunTehais won't sort automatically on new hais added, 
        // so we can add tsumo hai directly to simplify the checks.

        tehai_copy.addJyunTehai( tsumoHai );

        Hai[] jyunTehai = tehai_copy.getJyunTehai();

        for( int i = 0; i < jyunTehai.Length; i++ )
        {
            Hai haiTemp = tehai_copy.removeJyunTehaiAt(i);

            for( int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++ )
            {
                countFormat.setCounterFormat(tehai_copy, new Hai(id));

                if( countFormat.calculateCombisCount( combis ) > 0 )
                {
                    haiIndexList.Add( i );
                    break;
                }
            }

            tehai_copy.insertJyunTehai(i, haiTemp);
        }

        return haiIndexList.Count > 0;
    }

    // hais为听牌列表.
    public bool tryGetMachiHais(Tehai tehai, out List<Hai> hais)
    {
        hais = new List<Hai>();

        for(int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++)
        {
            Hai addHai = new Hai(id);

            countFormat.setCounterFormat(tehai, addHai);

            if( countFormat.calculateCombisCount( combis ) > 0 )
            {
                hais.Add( addHai );
            }
        }

        return hais.Count > 0;
    }

    // 是否可以听牌，只需要检查一个牌.
    public bool canTenpai(Tehai tehai)
    {
        for(int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++)
        {
            countFormat.setCounterFormat(tehai, new Hai(id));

            if( countFormat.calculateCombisCount( combis ) > 0 )
                return true;
        }
        return false;
    }
    #endregion
}
