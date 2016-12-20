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


    public Sai[] getSais() {
        return _game.Sais;
    }

    // 表ドラ、槓ドラの配列を取得する
    public Hai[] getDoraHais() {
        return _game.getOmotoDoras();
    }

    public void copyTehai(Tehai tehai, EKaze kaze) {
        _game.copyTehai(tehai, kaze);
    }

    // 河をコピーする
    public void copyHou(Hou hou, EKaze kaze) {
        _game.copyHou(hou, kaze);
    }

    // ツモ牌を取得する
    protected Hai getTsumoHai() {
        Hai tsumoHai = _game.TsumoHai;
        return tsumoHai == null? null : new Hai(tsumoHai);
    }

    // 捨牌を取得する
    protected Hai getSuteHai() {
        return new Hai(_game.SuteHai);
    }

    // あがり点を取得する
    public int getAgariScore(Tehai tehai, Hai addHai) {
        return _game.getAgariScore(tehai, addHai);
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

    public PlayerAction getPlayerAction() {
        return _game.getPlayerAction();
    }

    // 起(親)家のプレイヤーインデックスを取得する
    public int getChiichaIndex() {
        return _game.ChiiChaIndex;
    }

    public AgariInfo getAgariInfo() {
        return _game.AgariInfo;
    }

    #region Logic
    private static HaiCombi[] combis = new HaiCombi[10]
    {
        new HaiCombi(),new HaiCombi(),new HaiCombi(),
        new HaiCombi(),new HaiCombi(),new HaiCombi(),
        new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi(),
    };
    private static CountFormat countFormat = new CountFormat();

    // 打哪些牌(indexs)可以立直.
    public bool tryGetReachIndexs(Tehai a_tehai, Hai tsumoHai, out List<int> haiIndexList)
    {
        haiIndexList = new List<int>();

        // 鳴いている場合は、リーチできない。
        if( a_tehai.isNaki() )
            return false;

        Tehai tehai = new Tehai( a_tehai );

        // As _jyunTehais won't sort automatically on new hais added, 
        // so we can add tsumo hai directly to simplify the checks.
        tehai.addJyunTehai(tsumoHai);
        Hai[] jyunTehai = tehai.getJyunTehai();

        for( int i = 0; i < jyunTehai.Length; i++ )
        {
            Hai haiTemp = new Hai( jyunTehai[i] );

            tehai.removeJyunTehai(jyunTehai[i]);

            for (int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++)
            {
                Hai addHai = new Hai(id);
                tehai.addJyunTehai(addHai);

                countFormat.setCounterFormat(tehai, null);
                if( countFormat.calculateCombisCount(combis) > 0 )
                {
                    haiIndexList.Add(i);
                    tehai.removeJyunTehai(addHai);
                    break;
                }

                tehai.removeJyunTehai(addHai);
            }

            tehai.addJyunTehai(haiTemp);
        }

        return haiIndexList.Count > 0;
    }

    // hais为听牌列表.
    public bool tryGetMachiHais(Tehai a_tehai, out List<Hai> hais)
    {
        hais = new List<Hai>();

        Tehai tehai = new Tehai(a_tehai);

        for (int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++)
        {
            Hai addHai = new Hai(id);

            countFormat.setCounterFormat(tehai, addHai);
            if (countFormat.calculateCombisCount(combis) > 0)
            {
                hais.Add( addHai );
            }
        }

        return hais.Count > 0;
    }
    #endregion
}
