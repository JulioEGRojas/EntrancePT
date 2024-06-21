using System;

public class ValueChangedArgs<T> : EventArgs {
    
    public T oldValue;
    
    public T newValue;

    public ValueChangedArgs(T oldValue, T newValue) {
        this.oldValue = oldValue;
        this.newValue = newValue;
    }
}