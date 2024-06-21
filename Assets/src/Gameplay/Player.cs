using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamageable {
    
    [Header("Movement")]
    /// <summary>
    /// Value of the xy input axis. Used to move the character
    /// </summary>
    [SerializeField] private Vector2ScriptableValue movementInputValue;

    /// <summary>
    /// How fast the entity moves.
    /// </summary>
    [SerializeField] private float speed;
    
    private Rigidbody _rb;
    
    /// <summary>
    /// Health to gain at start. Will broke if greater than 3
    /// </summary>
    [Header("Health")] [Range(1,3)]
    [SerializeField] private int initialHealthValue = 3;
    
    /// <summary>
    /// Broadcaster so we can notify when our health reaches 0.
    /// </summary>
    [SerializeField] private GameEndedEventBroadcaster gameEndedEventBroadcaster;

    /// <summary>
    /// Our actual health
    /// </summary>
    [SerializeField] private IntScriptableValue healthValue;

    private int Health => healthValue.value;

    [Header("Damage and immunity")]
    [SerializeField] private bool isDamageable = true;
    
    /// <summary>
    /// How many times the immunity has been used. We don't use the actual value. Just interested on when it changes.
    /// </summary>
    [SerializeField] private IntScriptableValue manualImmunityUsesValue;
    
    /// <summary>
    /// Unused. May be useful if we want to gain immunity upon getting hit.
    /// </summary>
    [SerializeField] private float hitImmunityDuration = 3f;

    /// <summary>
    /// Called when Immunity is gained
    /// </summary>
    [SerializeField] private UnityEvent onImmunityGain;
    
    /// <summary>
    /// Called when Immunity is lost
    /// </summary>
    [SerializeField] private UnityEvent onImmunityLost;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        // We have to set the value to zero, since scriptable object isn't reset on scene changes.
        healthValue.SetValue(initialHealthValue);
    }

    private void Start() {
        healthValue.SetValue(initialHealthValue);
    }

    private void OnEnable() {
        manualImmunityUsesValue.OnValueChangedEvent += OnGainImmunityUsesChanged;
    }

    private void OnDisable() {
        manualImmunityUsesValue.OnValueChangedEvent -= OnGainImmunityUsesChanged;
    }

    private void Update() {
        _rb.velocity = speed * Vector3.Normalize( new Vector3(movementInputValue.Value.x, 0, movementInputValue.Value.y));
    }

    public void TakeDamage(int damage) {
        // Set health. The UI will detect the health change using the OnValueChangedEvent of the health scriptable value, so
        // no cross references will be needed, reducing spagueti code.
        healthValue.SetValue(Health - damage);
        // Broadcast the game ended event if health is 0.
        if (Health <= 0) {
            gameEndedEventBroadcaster.BroadCast(this, new GameEndedEventArgs());
            Destroy(this);
        }
    }
    
    private void OnGainImmunityUsesChanged(object sender, IntValueChangedEventArgs e) {
        // All coroutines are stopped on manual immunity, so current immunity coroutine doesn't stop the new immunity coroutine.
        StopAllCoroutines();
        StartCoroutine(GainImmunity(3));
    }

    /// <summary>
    /// Gain immunity for the given duration.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator GainImmunity(float duration) {
        onImmunityGain.Invoke();
        isDamageable = false;
        yield return new WaitForSeconds(duration);
        isDamageable = true;
        onImmunityLost.Invoke();
    }

    /// <summary>
    /// Determines if the entity is damageable
    /// </summary>
    /// <returns></returns>
    public bool IsDamageable() {
        return isDamageable;
    }
}