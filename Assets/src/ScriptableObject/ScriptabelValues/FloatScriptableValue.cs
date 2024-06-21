using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Scriptable Float", menuName = "Values/Float")]
public class FloatScriptableValue : ScriptableObject {

    [SerializeField] protected float value;
    public float Value { get => value; }

    /// <summary>
    /// What happens when the value changes
    /// </summary>
    public EventHandler<FloatValueChangedEventArgs> OnValueChangedEvent;

    /// <summary>
    /// Sets the value and invokes the callback
    /// </summary>
    /// <param name="newValue"></param>
    public virtual void SetValue(float newValue) {
        OnValueChangedEvent?.Invoke(this,new FloatValueChangedEventArgs(value, newValue));
        value = newValue;
    }

    /// <summary>
    /// Sets the value and invokes the callback. Used via a slider to simplify things
    /// </summary>
    /// <param name="newValue"></param>
    public virtual void SetValue(Slider slider) {
        OnValueChangedEvent?.Invoke(this,new FloatValueChangedEventArgs(value, slider.value));
        value = slider.value;
    }
}


public class FloatValueChangedEventArgs : ValueChangedArgs<float> {
    public FloatValueChangedEventArgs(float oldValue, float newValue) : base(oldValue, newValue) {
    }
}