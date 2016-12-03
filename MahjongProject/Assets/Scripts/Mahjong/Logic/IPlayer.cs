
public interface IPlayer 
{
    bool IsAI { get; }

    void AttachToPlayer( Player owner );

    PlayerAction getAction();
    void HandleEvent(EventID eventID, EKaze kazeFrom, EKaze kazeTo, System.Action<EventID> onAction);
    EventID DoAction(EventID result);
}

