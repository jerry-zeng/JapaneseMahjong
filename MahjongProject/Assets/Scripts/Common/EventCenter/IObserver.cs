
public interface IObserver : IEventListener<EventId, object[]> {
}

public interface IEventListener<TEventType, TArgument>{
    void OnHandleEvent(TEventType eventType, TArgument args);
}
