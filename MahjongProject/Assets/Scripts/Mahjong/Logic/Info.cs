/**
 * プレイヤー(Player)に提供する情報を管理するクラスです。
 */
public class Info 
{
    /** Game */
    protected Mahjong game;

    public Info(Mahjong game) {
        this.game = game;

        setSutehaiIndex(int.MaxValue);
    }

    /**
     * サイコロ(色子)の配列を取得する。
     */
    public Sai[] getSais() {
        return game.getSais();
    }

    /**
     * 表ドラ、槓ドラの配列を取得する。
     */
    public Hai[] getDoraHais() {
        return game.getOmotoDoras();
    }

    /**
     * 自風を取得する。
     */
    public int getJikaze() {
        return game.getJiKaze();
    }

    public void copyTehai(Tehai tehai) {
        game.copyTehai(tehai, game.getJiKaze());
    }
    public void copyTehai(Tehai tehai, int kaze) {
        game.copyTehai(tehai, kaze);
    }

    /**
     * 河をコピーする。
     */
    public void copyHou(Hou hou, int kaze) {
        game.copyHou(hou, kaze);
    }

    /**
     * ツモ牌を取得する。
     */
    public Hai getTsumoHai() {
        Hai tsumoHai = game.getTsumoHai();
        if (tsumoHai != null) {
            return new Hai(tsumoHai);
        }
        return null;
    }

    /**
     * 捨牌を取得する。
     */
    public Hai getSuteHai() {
        return new Hai(game.getSuteHai());
    }

    public int getAgariScore() {
        return 0;
    }

    /**
     * あがり点を取得する。
     */
    public int getAgariScore(Tehai tehai, Hai addHai) {
        return game.getAgariScore(tehai, addHai);
    }

    public bool isReach() {
        return game.isReach(game.getJiKaze());
    }

    /**
     * リーチを取得する。
     */
    public bool isReach(int kaze) {
        return game.isReach(kaze);
    }

    /**
     * ツモの残り数を取得する。
     */
    public int getTsumoRemain() {
        return game.getTsumoRemain();
    }

    /**
     * 局を取得する。
     */
    public int getkyoku() {
        return game.getkyoku();
    }

    /**
     * 名前を取得する。
     */
    public string getName(int kaze) {
        return game.getName(kaze);
    }

    /**
     * 本場を取得する。
     */
    public int getHonba() {
        return game.getHonba();
    }

    /**
     * リーチ棒の数を取得する。
     */
    public int getReachbou() {
        return game.getReachbou();
    }

    /**
     * 点棒を取得する。
     */
    public int getTenbou(int kaze) {
        return game.getTenbou(kaze);
    }


    private int mSutehaiIdx;
    public void setSutehaiIndex(int mSutehaiIdx) {
        this.mSutehaiIdx = mSutehaiIdx;
    }
    public int getSutehaiIdx() {
        return mSutehaiIdx;
    }


    private Combi[] combis = new Combi[10]
    {
        new Combi(),new Combi(),new Combi(),new Combi(),new Combi(),
        new Combi(),new Combi(),new Combi(),new Combi(),new Combi()
    };

    public int getReachIndexs(Tehai a_tehai, Hai a_tsumoHai, int[] a_indexs) {
        // 鳴いている場合は、リーチできない。
        if (a_tehai.isNaki()) {
            return 0;
        }

        Tehai tehai = new Tehai();
        Tehai.copy(tehai, a_tehai, true);

        int index = 0;
        Hai[] jyunTehai = tehai.getJyunTehai();
        int jyunTehaiLength = tehai.getJyunTehaiLength();
        Hai haiTemp = new Hai();
        Hai addHai;
        CountFormat countFormat = new CountFormat();

        for (int i = 0; i < jyunTehaiLength; i++) {
            Hai.copy(haiTemp, jyunTehai[i]);
            tehai.removeJyunTehai(jyunTehai[i]);

            for (int id = 0; id < Hai.ID_ITEM_MAX; id++) {
                addHai = new Hai(id);
                tehai.addJyunTehai(addHai);
                countFormat.setCountFormat(tehai, a_tsumoHai);

                if (countFormat.getCombis(combis) > 0) {
                    a_indexs[index] = i;
                    index++;
                    tehai.removeJyunTehai(addHai);
                    break;
                }
                tehai.removeJyunTehai(addHai);
            }
            tehai.addJyunTehai(haiTemp);
        }

        for (int id = 0; id < Hai.ID_ITEM_MAX; id++) {
            addHai = new Hai(id);
            tehai.addJyunTehai(addHai);
            countFormat.setCountFormat(tehai, null);

            if (countFormat.getCombis(combis) > 0) {
                a_indexs[index] = 13;
                index++;
                tehai.removeJyunTehai(addHai);
                break;
            }
            tehai.removeJyunTehai(addHai);
        }

        return index;
    }

    public int getMachiIndexs(Tehai a_tehai, Hai[] a_hais) {
        Tehai tehai = new Tehai();
        Tehai.copy(tehai, a_tehai, true);

        int index = 0;
        Hai addHai;
        CountFormat countFormat = new CountFormat();

        for (int id = 0; id < Hai.ID_ITEM_MAX; id++) {
            addHai = new Hai(id);
            tehai.addJyunTehai(addHai);
            countFormat.setCountFormat(tehai, null);

            if (countFormat.getCombis(combis) > 0) {
                a_hais[index] = new Hai(id);
                index++;
                tehai.removeJyunTehai(addHai);
            } 
            else {
                tehai.removeJyunTehai(addHai);
            }
        }

        return index;
    }


    public void postUiEvent(EventId a_eventId, int a_kazeFrom, int a_kazeTo) {
        game.PostUIEvent(a_eventId, a_kazeFrom, a_kazeTo);
    }

    public int getSuteHaisCount() {
        return game.getSuteHaisCount();
    }

    public SuteHai[] getSuteHais() {
        return game.getSuteHais();
    }

    public int getPlayerSuteHaisCount() {
        return game.getPlayerSuteHaisCount(game.getJiKaze());
    }
}
