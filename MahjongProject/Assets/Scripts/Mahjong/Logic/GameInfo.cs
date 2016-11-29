
public class GameInfo : Info 
{
    protected PlayerAction _playerAction;

    public GameInfo(Mahjong game, PlayerAction playerAction) : base(game) {
        this.setPlayerAction(playerAction);
    }

    public Hai[] getUraDoraHais() {
        return _game.getUraDoras();
    }

    public EKaze getManKaze() {
        return _game.getManKaze();
    }

    public void setPlayerAction(PlayerAction playerAction) {
        this._playerAction = playerAction;
    }
    public PlayerAction getPlayerAction() {
        return _playerAction;
    }


    // 起家のプレイヤーインデックスを取得する
    public int getChiichaIndex() {
        return _game.getChiichaIndex();
    }

    public AgariInfo getAgariInfo() {
        return _game.getAgariInfo();
    }

    public bool[] getTenpai() {
        return _game.getTenpai();
    }
}