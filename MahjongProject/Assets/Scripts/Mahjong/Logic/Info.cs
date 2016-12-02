
/// <summary>
/// プレイヤー(Player)に提供する情報を管理するクラスです。
/// </summary>

public class Info 
{
    protected Mahjong _game;

    public Info(Mahjong game)
    {
        this._game = game;

        setSutehaiIndex(int.MaxValue);
    }

    private int _sutehaiIdx;

    public void setSutehaiIndex(int mSutehaiIdx) {
        this._sutehaiIdx = mSutehaiIdx;
    }
    public int getSutehaiIdx() {
        return _sutehaiIdx;
    }

    private HaiCombi[] combis = new HaiCombi[10]
    {
        new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi(),
        new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi()
    };


    public Sai[] getSais() {
        return _game.getSais();
    }

    // 表ドラ、槓ドラの配列を取得する
    public Hai[] getDoraHais() {
        return _game.getOmotoDoras();
    }

    // 自風を取得する
    public EKaze getJikaze() {
        return _game.getJiKaze();
    }

    public void copyTehai(Tehai tehai) {
        _game.copyTehai(tehai, _game.getJiKaze());
    }
    public void copyTehai(Tehai tehai, EKaze kaze) {
        _game.copyTehai(tehai, kaze);
    }

    // 河をコピーする
    public void copyHou(Hou hou, EKaze kaze) {
        _game.copyHou(hou, kaze);
    }

    // ツモ牌を取得する
    public Hai getTsumoHai() {
        Hai tsumoHai = _game.getTsumoHai();
        if (tsumoHai != null) {
            return new Hai(tsumoHai);
        }
        return null;
    }

    // 捨牌を取得する
    public Hai getSuteHai() {
        return new Hai(_game.getSuteHai());
    }

    public int getAgariScore() {
        return 0;
    }

    // あがり点を取得する
    public int getAgariScore(Tehai tehai, Hai addHai) {
        return _game.getAgariScore(tehai, addHai);
    }

    public bool isReach() {
        return _game.isReach(_game.getJiKaze());
    }

    // リーチを取得する
    public bool isReach(EKaze kaze) {
        return _game.isReach(kaze);
    }

    // ツモの残り数を取得する
    public int getTsumoRemain() {
        return _game.getTsumoRemain();
    }

    // 局を取得する
    public int getkyoku() {
        return _game.getkyoku();
    }

    // 名前を取得する
    public string getName(EKaze kaze) {
        return _game.getName(kaze);
    }

    // 本場を取得する
    public int getHonba() {
        return _game.getHonba();
    }

    // リーチ棒の数を取得する
    public int getReachbou() {
        return _game.getReachbou();
    }

    // 点棒を取得する
    public int getTenbou(EKaze kaze) {
        return _game.getTenbou(kaze);
    }


    public int getReachIndexs(Tehai a_tehai, Hai tsumoHai, int[] indexs)
    {
        // 鳴いている場合は、リーチできない。
        if (a_tehai.isNaki())
            return 0;

        Tehai tehai = new Tehai();
        Tehai.copy(tehai, a_tehai, true);

        int index = 0;
        Hai[] jyunTehai = tehai.getJyunTehai();
        int jyunTehaiLength = tehai.getJyunTehaiLength();
        Hai haiTemp = new Hai();
        Hai addHai;
        CountFormat countFormat = new CountFormat();

        for (int i = 0; i < jyunTehaiLength; i++)
        {
            Hai.copy(haiTemp, jyunTehai[i]);
            tehai.removeJyunTehai(jyunTehai[i]);

            for (int id = 0; id < Hai.ID_ITEM_MAX; id++)
            {
                addHai = new Hai(id);
                tehai.addJyunTehai(addHai);
                countFormat.setCounterFormat(tehai, tsumoHai);

                if (countFormat.calculateCombisCount(combis) > 0)
                {
                    indexs[index] = i;
                    index++;
                    tehai.removeJyunTehai(addHai);
                    break;
                }
                tehai.removeJyunTehai(addHai);
            }
            tehai.addJyunTehai(haiTemp);
        }

        for (int id = 0; id < Hai.ID_ITEM_MAX; id++)
        {
            addHai = new Hai(id);
            tehai.addJyunTehai(addHai);
            countFormat.setCounterFormat(tehai, null);

            if (countFormat.calculateCombisCount(combis) > 0)
            {
                indexs[index] = 13;
                index++;
                tehai.removeJyunTehai(addHai);
                break;
            }
            tehai.removeJyunTehai(addHai);
        }

        return index;
    }

    public int getMachiIndexs(Tehai a_tehai, Hai[] hais)
    {
        Tehai tehai = new Tehai();
        Tehai.copy(tehai, a_tehai, true);

        int index = 0;
        CountFormat countFormat = new CountFormat();

        for (int id = 0; id < Hai.ID_ITEM_MAX; id++)
        {
            Hai addHai = new Hai(id);
            tehai.addJyunTehai(addHai);
            countFormat.setCounterFormat(tehai, null);

            if (countFormat.calculateCombisCount(combis) > 0) {
                hais[index] = new Hai(id);
                index++;
                tehai.removeJyunTehai(addHai);
            } 
            else {
                tehai.removeJyunTehai(addHai);
            }
        }

        return index;
    }


    public void postUiEvent(EventID eventId, EKaze kazeFrom, EKaze kazeTo) {
        _game.PostUIEvent(eventId, kazeFrom, kazeTo);
    }

    public int getSuteHaisCount() {
        return _game.getSuteHaisCount();
    }

    public SuteHai[] getSuteHais() {
        return _game.getSuteHais();
    }

    public int getPlayerSuteHaisCount() {
        return _game.getPlayerSuteHaisCount(_game.getJiKaze());
    }
}
