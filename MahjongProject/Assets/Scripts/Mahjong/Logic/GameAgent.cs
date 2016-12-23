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
        return _game.getOpenedOmotoDoras();
    }

    public Hai[] getUraDoraHais() {
        return _game.getOpenedUraDoraHais();
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
    public CountFormat countFormat
    {
        get{ return _game.CountFormater; }
    }


    // あがり点を取得する
    public int getAgariScore(Tehai tehai, Hai addHai, EKaze jikaze)
    {
        return _game.GetAgariScore(tehai, addHai, jikaze);
    }

    #region Logic


    protected List<Hai> _reachHaiList = new List<Hai>( Tehai.JYUN_TEHAI_LENGTH_MAX );


    // 打哪些牌可以立直.
    public bool tryGetReachHaiIndex(Tehai a_tehai, Hai tsumoHai, out List<int> haiIndexList)
    {
        haiIndexList = new List<int>();

        // 鳴いている場合は、リーチできない。
        if( a_tehai.isNaki() )
            return false;

        /// find all reach-enabled hais in a_tehai, also the tsumoHai.
        _reachHaiList.Clear();


        // As _jyunTehais won't sort automatically on new hais added, 
        // so we can add tsumo hai directly to simplify the checks.
        Tehai tehai_copy = new Tehai( a_tehai );
        tehai_copy.addJyunTehai( tsumoHai );
        tehai_copy.Sort();

        Hai[] jyunTehai = tehai_copy.getJyunTehai();

        for( int i = 0; i < jyunTehai.Length; i++ )
        {
            Hai haiTemp = tehai_copy.removeJyunTehaiAt(i);

            for( int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++ )
            {
                countFormat.setCounterFormat(tehai_copy, new Hai(id));

                if( countFormat.calculateCombisCount( combis ) > 0 )
                {
                    _reachHaiList.Add( new Hai(haiTemp) );
                    break;
                }
            }

            tehai_copy.insertJyunTehai(i, haiTemp);
        }


        /// transfer to index list.
        if( _reachHaiList.Count > 0 )
        {
            jyunTehai = a_tehai.getJyunTehai();

            for( int i = 0; i < _reachHaiList.Count; i++ )
            {
                for( int j = 0; j < jyunTehai.Length; j++ )
                {
                    if( jyunTehai[j].ID == _reachHaiList[i].ID && !haiIndexList.Contains(j))
                        haiIndexList.Add( j );
                }

                if( tsumoHai.ID == _reachHaiList[i].ID && !haiIndexList.Contains(jyunTehai.Length) )
                    haiIndexList.Add( jyunTehai.Length );
            }
            haiIndexList.Sort();

            return true;
        }

        return false;
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

    // 是否可以听牌，只需要检查一个成立的牌.
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
