using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour {

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [SerializeField] private TextMeshProUGUI multiplierText;
    
    [SerializeField] private List<GameObject> healthImages;
    
    [SerializeField] private TextMeshProUGUI scoreTextOnEndGameScreen;
    
    [Header("Data")]
    /// <summary>
    /// Score value. We listen to when the value changes and act accordingly.
    /// </summary>
    [SerializeField] private IntScriptableValue scoreValue;
    
    /// <summary>
    /// Health value. We listen to when the value changes and act accordingly.
    /// </summary>
    [SerializeField] private IntScriptableValue healthValue;
    
    /// <summary>
    /// Multiplier value. We listen to when the value changes and act accordingly.
    /// </summary>
    [SerializeField] private IntScriptableValue multiplierValue;
    
    /// <summary>
    /// Everything related to customizing what to do when the game ends.
    /// </summary>
    [Header("Game Ended management")]
    [SerializeField] private GameEndedEventBroadcaster gameEndedEventBroadcaster;
    [SerializeField] private UnityEvent onGameEndedEvent;
    private EventHandler<GameEndedEventArgs> _onGameEnded;
    
    private void OnEnable() {
        // Subscribe our callbacks on what to do when these values change.
        scoreValue.OnValueChangedEvent += OnScoreChanged;
        healthValue.OnValueChangedEvent += OnHealthChanged;
        multiplierValue.OnValueChangedEvent += OnMultiplierChanged;
        // Game ended
        _onGameEnded += OnGameEnded;
        gameEndedEventBroadcaster.Subscribe(_onGameEnded);
    }

    private void OnDisable() {
        scoreValue.OnValueChangedEvent -= OnScoreChanged;
        healthValue.OnValueChangedEvent -= OnHealthChanged;
        multiplierValue.OnValueChangedEvent -= OnMultiplierChanged;
        // Game ended
        gameEndedEventBroadcaster.Unsubscribe(_onGameEnded);
        _onGameEnded -= OnGameEnded;
    }
    
    private void OnGameEnded(object sender, GameEndedEventArgs e) {
        onGameEndedEvent.Invoke();
    }
    
    public void UpdateScoreTextOnEndGame(IntScriptableValue scoreValue) {
        scoreTextOnEndGameScreen.text = "" + scoreValue.value;
    }

    #region Value Change Callbacks
    
    /// <summary>
    /// Callback when health changes. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="healthValue"></param>
    private void OnHealthChanged(object sender, IntValueChangedEventArgs healthValue) {
        // Deactivate all health images. Re activate them with the amount of health we have.
        healthImages.ForEach(x=>x.gameObject.SetActive(false));
        for (int i = 0; i < healthValue.newValue; i++) {
            healthImages[i].SetActive(true);
        }
    }

    private void OnScoreChanged(object sender, IntValueChangedEventArgs scoreValue) {
        scoreText.text = "" + scoreValue.newValue;
    }
    
    private void OnMultiplierChanged(object sender, IntValueChangedEventArgs multiplierValue) {
        multiplierText.text = "x" + multiplierValue.newValue;
    }

    #endregion
    
}
