using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Scriptable Clamped Float", menuName = "Values/Clamped Float")]
public class ClampedFloatScriptableValue : ScriptableObject {
    
    [SerializeField] protected float value;
    public float Value { get => value; }

    public float minValue;
    
    public float maxValue;
    
    public EventHandler<ClampedFloatValueChangedEventArgs> OnMaxValueReachedEvent;
    
    public EventHandler<ClampedFloatValueChangedEventArgs> OnMinValueReachedEvent;
    
    public EventHandler<ClampedFloatValueChangedEventArgs> OnValueChangedEvent;
    
    public EventHandler<FloatValueChangedEventArgs> OnMaxValueChangedEvent;
    
    /// <summary>
    /// Sets the clamping values for this value
    /// </summary>
    /// <param name="newMinValue"></param>
    /// <param name="newMaxValue"></param>
    /// <param name="autoClamp">If current value should be auto clamped from the new min and max values</param>
    public void SetClampingValues(float newMinValue, float newMaxValue, bool autoClamp = false) {
        minValue = newMinValue;
        OnMaxValueChangedEvent?.Invoke(this, new FloatValueChangedEventArgs(maxValue, newMaxValue));
        maxValue = newMaxValue;
        if (autoClamp) {
            SetValue(Value);
        }
    }

    public void SetValue(float newValue) {
        value = Mathf.Clamp(newValue,minValue, maxValue);
        if (newValue == maxValue) {
            OnMaxValueReachedEvent?.Invoke(this,new ClampedFloatValueChangedEventArgs(value,newValue,minValue, maxValue));
        } else if (newValue == minValue) {
            OnMinValueReachedEvent?.Invoke(this,new ClampedFloatValueChangedEventArgs(value,newValue,minValue,maxValue));
        }
        OnValueChangedEvent?.Invoke(this,new ClampedFloatValueChangedEventArgs(value,newValue,minValue,maxValue));
    }
}

public class ClampedFloatValueChangedEventArgs : FloatValueChangedEventArgs {

    public float minValue;
    
    public float maxValue;

    public ClampedFloatValueChangedEventArgs(float oldValue, float newValue, float minValue, float maxValue) :
    base(oldValue,newValue) {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}