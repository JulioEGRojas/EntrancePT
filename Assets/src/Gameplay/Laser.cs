using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour {
    
    [Header("Generation")]
    /// <summary>
    /// How much time the laser will be in the scene before being destroyed.
    /// </summary>
    [SerializeField] private float lifeTime;
    
    [Header("Damage")]
    // How much damage the laser deals
    [SerializeField] private int damage;

    /// <summary>
    /// Which layers will the laser try to detect to deal damage.
    /// </summary>
    [SerializeField] private LayerMask damageableLayers;

    [Header("Warning")]
    /// <summary>
    /// State of the laser. Helps us to make the laser not deal damage while the warning is on.
    /// </summary>
    private LaserState _state;

    public enum LaserState {
        WARNING,
        DAMAGING
    }
    
    /// <summary>
    /// Time the warning lasts before the laser starts dealing damage
    /// </summary>
    [SerializeField] private float warningTime;
    
    /// <summary>
    /// Object used to warn the player about the laser.
    /// </summary>
    [SerializeField] private GameObject warningObject;

    /// <summary>
    /// Called when the warning starts
    /// </summary>
    [SerializeField] private UnityEvent onWarningStarted;
    
    /// <summary>
    /// Called when the warning ends
    /// </summary>
    [SerializeField] private UnityEvent onWarningEnded;
    
    [Header("Visual")]
    /// <summary>
    /// Line renderer used for display purposes. It's on local space.
    /// </summary>
    [SerializeField] protected LineRenderer lineRenderer;

    /// <summary>
    /// Calculated direction upon target updated.
    /// </summary>
    private Vector3 _directionToTarget;

    private void Start() {
        // Laser is destroyed after it's lifetime ends.
        Destroy(gameObject, lifeTime);
        // Warning is started
        StartCoroutine(WarningCoroutine());
    }

    /// <summary>
    /// Starts the warning, waits until it ends, and then changes the state to damaging.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WarningCoroutine() {
        _state = LaserState.WARNING;
        onWarningStarted.Invoke();
        yield return new WaitForSeconds(warningTime);
        onWarningEnded.Invoke();
        _state = LaserState.DAMAGING;
    }
    
    /// <summary>
    /// Sets the target of the laser, moving the warning gameObject and updating the line renderer to the requested position. 
    /// </summary>
    /// <param name="worldPosition"></param>
    public virtual void SetTarget(Vector3 worldPosition) {
        if (warningObject) {
            warningObject.transform.position = worldPosition;
        }
        // Transform the point to a direction because the raycast needs the direction, not the point.
        _directionToTarget = transform.worldToLocalMatrix.MultiplyPoint(worldPosition);
        // Line renderer second point is set to direction, due to it being on local coordinate system
        lineRenderer.SetPosition(1, _directionToTarget);
    }

    private void FixedUpdate() {
        // Do nothing
        if (_state == LaserState.WARNING) {
            return;
        }
        // Raycast to try and damage things on the damageable layer. It's on fixed update because the player doesn't move
        // until Fixed Update is called.
        if (Physics.Raycast(transform.position, _directionToTarget, out RaycastHit hit, 40f, damageableLayers)
            && hit.collider.TryGetComponent(out IDamageable damageable) && damageable.IsDamageable()) {
            damageable.TakeDamage(damage);
            // Destroy the component so it doesn't damage anything once again. It will sill get destroyed due to the
            // on destroy call at Start.
            Destroy(this);
        }
    }
}
