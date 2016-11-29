
public interface IPlayer : IEventReceiver 
{
    // name.
    string getName();

    // is ai or man.
    bool isAI();

    // 捨牌
    int getSutehaiIndex();
}

public interface IEventReceiver 
{
    EventId HandleEvent(EventId a_eventId, EKaze kazeFrom, EKaze kazeTo);
}
