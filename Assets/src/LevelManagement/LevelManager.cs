using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    /// <summary>
    /// How much score we have. Used by the UI manager to update the score with the OnValueChanged event handler.
    /// </summary>
    [Header("Score")]
    [SerializeField] private IntScriptableValue scoreValue;
    
    private int Score => scoreValue.value;
    
    /// <summary>
    /// How much the score base is multiplied each time the score is increases.
    /// </summary>
    [SerializeField] private IntScriptableValue scoreMultiplierValue;
    
    private int ScoreMultiplier => scoreMultiplierValue.value;
    
    /// <summary>
    /// Score gets increased by this value times the score multiplier
    /// </summary>
    [SerializeField] private int scoreBase = 1;

    /// <summary>
    /// WHat the score multiplier starts at
    /// </summary>
    [SerializeField] private int initialScoreMultiplier = 1;
    
    /// <summary>
    /// Flag that allows the score to keep raising.
    /// </summary>
    [SerializeField] private bool gameExecuting = true;
    
    /// <summary>
    /// Everything related to customizing what to do when the game ends.
    /// </summary>
    [Header("Game Ended management")]
    [SerializeField] private GameEndedEventBroadcaster gameEndedEventBroadcaster;
    [SerializeField] private UnityEvent onGameEndedEvent;
    private EventHandler<GameEndedEventArgs> _onGameEnded;

    [Header("Level phases configuration")]
    /// <summary>
    /// Duration and events of each phase
    /// </summary>
    [SerializeField] private List<LevelPhaseInfo> _phasesInfo;

    private void Start() {
        // These operations are needed because if we come from a lost game, those values are still on memory.
        UnpauseGame();
        scoreValue.SetValue(0);
        scoreMultiplierValue.SetValue(initialScoreMultiplier);
        // Start score increase and level progression, as requested.
        StartCoroutine(LevelProgressionCoroutine());
        StartCoroutine(ScoreIncreaseCoroutine());
    }

    /// <summary>
    /// We subscribe an event handler to easily customize what happens when the game ends.
    /// </summary>
    private void OnEnable() {
        _onGameEnded += OnGameEnded;
        gameEndedEventBroadcaster.Subscribe(_onGameEnded);
    }
    
    /// <summary>
    /// Unsubscribe, to avoid memory leaks. Probably better to place it on OnDestroy
    /// </summary>
    private void OnDisable() {
        gameEndedEventBroadcaster.Unsubscribe(_onGameEnded);
        _onGameEnded -= OnGameEnded;
    }
    
    /// <summary>
    /// Pauses the game. Coroutines are scaled of time scale, so it pauses them.
    /// </summary>
    public void PauseGame() {
        Time.timeScale = 0f;
    }
    
    /// <summary>
    /// Unppauses the game. Coroutines are scaled of time scale, so it resumes them.
    /// </summary>
    public void UnpauseGame() {
        Time.timeScale = 1f;
    }

    public void ReloadCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Sets the score multiplier. Useful for level progression.
    /// </summary>
    /// <param name="newScoreMultiplier"></param>
    public void SetScoreMultiplier(int newScoreMultiplier) {    
        scoreMultiplierValue.SetValue(newScoreMultiplier);
    }
    
    /// <summary>
    /// Increases the score each second, based on the score multiplier and score base.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScoreIncreaseCoroutine() {
        // While the game is executing, increase the score each second. Flag could be useful later, but not right now.
        while (gameExecuting) {
            yield return new WaitForSeconds(1);
            scoreValue.SetValue(Score + scoreBase * ScoreMultiplier);
        }
    }

    /// <summary>
    /// Activates level phases and their callbacks upon reaching their respective durations.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LevelProgressionCoroutine() {
        for (var i = 0; i < _phasesInfo.Count; i++) {
            yield return new WaitForSeconds(_phasesInfo[i].duration);
            _phasesInfo[i].onEndedEvent.Invoke();
        }
    }

    #region Game Ended

    /// <summary>
    /// Called when game ends. Added as a listener to the Game ended broadcaster, which is broadcaster by the player
    /// and listened here.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnGameEnded(object sender, GameEndedEventArgs e) {
        onGameEndedEvent.Invoke();
    }

    #endregion
}

/// <summary>
/// Info for the level phases. How much it takes for it to end and what happens when it ends. Fully customizable on the
/// editor.
/// </summary>
[Serializable]
public class LevelPhaseInfo {
    public float duration;

    public UnityEvent onEndedEvent;
}
