using UnityEngine;
using UnityEngine.Events;

public class GameObjectExtensions : MonoBehaviour {

    [SerializeField] private UnityEvent OnEnableCallback;
    
    [SerializeField] private  UnityEvent OnDisableCallback;
    
    [SerializeField] private  UnityEvent OnAwakeCallback;

    private void Awake() {
        OnAwakeCallback.Invoke();
    }

    private void OnEnable() {
        OnEnableCallback.Invoke();
    }

    private void OnDisable() {
        OnDisableCallback.Invoke();
    }
    
    public void DisableGameObject(){
        gameObject.SetActive(false);
    }
    

    /// <summary>
    /// Toggles the gameObject to active or inactive
    /// </summary>
    public void Toggle() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ClearParent() {
        transform.parent = null;
    }
}