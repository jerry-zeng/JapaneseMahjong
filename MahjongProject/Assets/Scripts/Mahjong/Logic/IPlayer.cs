
public interface IPlayer : IEventReceiver 
{
    // name.
    string getName();

    // is ai or player.
    bool isAI();

    //捨牌 /
    int getSutehaiIndex();
}

public interface IEventReceiver 
{
    EventId HandleEvent(EventId a_eventId, int a_kazeFrom, int a_kazeTo);
}
