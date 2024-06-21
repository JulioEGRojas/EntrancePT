using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Scriptable Vector2", menuName = "Values/Vector2")]
public class Vector2ScriptableValue : ScriptableObject {

    [SerializeField] protected Vector2 value;
    public Vector2 Value { get => value; }

    public EventHandler<Vector2ValueChangedEventArgs> OnValueChangedEvent;

    public virtual void SetValue(Vector2 newValue) {
        OnValueChangedEvent?.Invoke(this,new Vector2ValueChangedEventArgs(value, newValue));
        value = newValue;
    }
}


public class Vector2ValueChangedEventArgs : EventArgs {
    
    public Vector2 oldValue;
    
    public Vector2 newValue;

    public Vector2ValueChangedEventArgs(Vector2 oldValue, Vector2 newValue) {
        this.oldValue = oldValue;
        this.newValue = newValue;
    }
}