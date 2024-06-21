using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Runtime GameObject Reference", menuName = "Values/Runtime GameObject")]
public class RuntimeGameObjectScriptableValue : ScriptableObject {
    
    private GameObject value;
    
    public EventHandler<ValueChangedArgs<GameObject>> OnValueChangedEvent;

    public void SetValue(GameObject newValue) {
        OnValueChangedEvent?.Invoke(this, new ValueChangedArgs<GameObject>(value, newValue));
        value = newValue;
    }
    
    public GameObject GetValue() {
        return value;
    }

    public T GetComponent<T>() {
        if (value) {
            return value.GetComponent<T>(); 
        }
        object nullValue = null;
        return (T) nullValue;
    }
}

public class RuntimeGameObjectValueChangedEventArgs : ValueChangedArgs<GameObject> {
    public RuntimeGameObjectValueChangedEventArgs(GameObject oldValue, GameObject newValue) : base(oldValue, newValue) {
    }
}
