using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Event broadcaster class. Subscribe and unsubscribe to avoid references in scene and reduce spaguetti code.
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventBroadCaster<T> : ScriptableObject where T:EventArgs  {
    
    private HashSet<EventHandler<T>> eventListeners = new HashSet<EventHandler<T>>();

    /// <summary>
    /// Broadcasts the event to all the registered listeners
    /// </summary>
    public void BroadCast(Object requester, T request) {
        foreach (EventHandler<T> eventListener in eventListeners) {
            eventListener.Invoke(requester,request);
        }
    }
    
    /// <summary>
    /// Subscribe a listener to the broadcaster
    /// </summary>
    /// <param name="newListener"></param>
    public void Subscribe(EventHandler<T> newListener) {
        if (!eventListeners.Contains(newListener)) {
            eventListeners.Add(newListener);            
        }
    }
    
    /// <summary>
    /// Unsubscribe a listener to the broadcaster
    /// </summary>
    /// <param name="newListener"></param>
    public void Unsubscribe(EventHandler<T> newListener) {
        if(eventListeners.Contains(newListener)) {
            eventListeners.Remove(newListener);
        }
    }

    public void ClearListeners() {
        eventListeners.Clear();
    }

    private void OnDisable() {
        ClearListeners();
    }
}
