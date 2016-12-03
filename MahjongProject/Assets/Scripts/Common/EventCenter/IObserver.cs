
public interface IUIObserver : IEventListener<UIEventID, object[]>{
    
}

public interface IObserver : IEventListener<EventID, object[]> {
}

public interface IEventListener<TEventType, TArgument>{
    void OnHandleEvent(TEventType eventType, TArgument args);
}
