
public class GameInfo : Info 
{
    public GameInfo(Mahjong game) : base(game) {
        
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


    // 起家のプレイヤーインデックスを取得する
    public int getChiichaIndex() {
        return _game.getChiichaIndex();
    }

    public AgariInfo getAgariInfo() {
        return _game.getAgariInfo();
    }

    public bool[] getTenpaiFlags() {
        return _game.getTenpaiFlags();
    }
}