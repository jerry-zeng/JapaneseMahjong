using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EventManager 
{
    private List<IObserver> observerList = null;

    private EventManager() {
        observerList = new List<IObserver>();
    }

    private static EventManager instance = null;
    public static EventManager Get()
    {
        if(instance == null){
            instance = new EventManager();
        }
        return instance;
    }


    public static void CleanUp() {
        instance.observerList.Clear();
        instance = null;
    }

    // add
    public void addObserver(IObserver observer) {
        if(observer == null){
            return;
        }
        if(!observerList.Contains(observer)){
            observerList.Add(observer);
        }
    }

    // remove
    public void removeObserver(IObserver observer) {
        if(observer == null){
            return;
        }
        if(observerList.Contains(observer)){
            observerList.Remove(observer);
        }
    }


    // 
    public void SendEvent(EventId evtId) {
        SendEvent(evtId, new object[]{});
    }
    public void SendEvent(EventId evtId, object arg0) {
        SendEvent(evtId, new object[]{ arg0 });
    }
    public void SendEvent(EventId evtId, object arg0, object arg1) {
        SendEvent(evtId, new object[]{ arg0, arg1 });
    }
    public void SendEvent(EventId evtId, object arg0, object arg1, object arg2) {
        SendEvent(evtId, new object[]{ arg0, arg1, arg2 });
    }

    // send event.
    public void SendEvent(EventId evtId, object[] args) {
        for( int i = 0; i < observerList.Count; i++ ) {
            IObserver observer = (IObserver)observerList[i];
            if( observer != null ) {
                observer.OnHandleEvent(evtId, args);
            }
        }
    }
}
