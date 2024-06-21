using UnityEngine;

/// <summary>
/// Class to configurate app values
/// </summary>
public class AppManager : MonoBehaviour {
    
    [Header("App configuration")]
    [Range(30,120)]
    [SerializeField] private int fps = 60;
    
    private void Awake() {
        Application.targetFrameRate = fps;
    }
}
