using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Int", menuName = "Values/Int")]
public class IntScriptableValue : ScriptableObject {
    
    /// <summary>
    /// Actual value. Needs to be only a get like the float one, but time is ticking.
    /// </summary>
    public int value;
    
    /// <summary>
    /// What happens when the value changes
    /// </summary>
    public EventHandler<IntValueChangedEventArgs> OnValueChangedEvent;
    
    /// <summary>
    /// Sets the value and invokes the callback
    /// </summary>
    /// <param name="newValue"></param>
    public virtual void SetValue(int newValue) {
        OnValueChangedEvent?.Invoke(this,new IntValueChangedEventArgs(value, newValue));
        value = newValue;
    }
}

public class IntValueChangedEventArgs : ValueChangedArgs<int> {
    public IntValueChangedEventArgs(int oldValue, int newValue) : base(oldValue, newValue) {
    }
}