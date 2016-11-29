
public class GameInfo : Info 
{
    protected PlayerAction mPlayerAction;

    public GameInfo(Mahjong game, PlayerAction playerAction) : base(game) {
        this.setPlayerAction(playerAction);
    }

    public Hai[] getUraDoraHais() {
        return game.getUraDoras();
    }

    public int getManKaze() {
        return game.getManKaze();
    }

    public void setPlayerAction(PlayerAction playerAction) {
        this.mPlayerAction = playerAction;
    }
    public PlayerAction getPlayerAction() {
        return mPlayerAction;
    }

    /**
     * 起家のプレイヤーインデックスを取得する。
     */
    public int getChiichaIndex() {
        return game.getChiichaIndex();
    }

    public AgariInfo getAgariInfo() {
        return game.getAgariInfo();
    }

    public bool[] getTenpai() {
        return game.getTenpai();
    }
}