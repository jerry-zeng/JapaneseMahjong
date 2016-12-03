using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EventManager 
{
    private List<IObserver> observerList = null;
    private List<IUIObserver> uiObserverList = null;

    private EventManager() 
    {
        observerList = new List<IObserver>();
        uiObserverList = new List<IUIObserver>();
    }

    private static EventManager instance = null;
    public static EventManager Get()
    {
        if(instance == null)
            instance = new EventManager();
        return instance;
    }


    public static void CleanUp()
    {
        instance.observerList.Clear();
        instance.uiObserverList.Clear();
        instance = null;
    }

    // add
    public void addObserver(IObserver observer) 
    {
        if(observer == null)
            return;
        
        if(!observerList.Contains(observer))
            observerList.Add(observer);        
    }
    // remove
    public void removeObserver(IObserver observer) 
    {
        if(observer == null)
            return;
        
        if(observerList.Contains(observer))
            observerList.Remove(observer);
    }

    public void addObserver(IUIObserver observer) 
    {
        if(observer == null)
            return;
        
        if(!uiObserverList.Contains(observer))
            uiObserverList.Add(observer);
    }
    public void removeObserver(IUIObserver observer) 
    {
        if(observer == null)
            return;
        
        if(uiObserverList.Contains(observer))
            uiObserverList.Remove(observer);
    }


    // send event.
    public void SendGameEvent(EventID evtID, params object[] args) 
    {
        for( int i = 0; i < observerList.Count; i++ ) 
        {
            IObserver observer = (IObserver)observerList[i];
            if( observer != null )
                observer.OnHandleEvent(evtID, args);
        }
    }

    // send ui event.
    public void SendUIEvent(UIEventID evtID, params object[] args) 
    {
        for( int i = 0; i < uiObserverList.Count; i++ ) 
        {
            IUIObserver observer = (IUIObserver)uiObserverList[i];
            if( observer != null )
                observer.OnHandleEvent(evtID, args);
        }
    }
}
