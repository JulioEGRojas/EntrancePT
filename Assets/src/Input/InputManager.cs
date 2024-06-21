using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    [Header("Movement Input")]
    [SerializeField] private InputActionReference movementAction;

    [SerializeField] private Vector2ScriptableValue movementInputValue;
    
    [Header("Manual immunity")]
    [SerializeField] private InputActionReference manualImmunityAction;

    [SerializeField] private IntScriptableValue gainImmunityUses;

    private bool _immunityOnCooldown;

    private void Start() {
        // This is needed because if we loose while moving and restart, value starts as not zero. 
        movementInputValue.SetValue(Vector2.zero);
    }

    private void OnEnable() {
        // Register input callbacks
        movementAction.action.performed += OnMovementActionPerformed;
        manualImmunityAction.action.performed += OnManualImmunityPerformed;
    }

    private void OnDisable() {
        // Unregister input callbacks
        movementAction.action.performed -= OnMovementActionPerformed;
        manualImmunityAction.action.performed -= OnManualImmunityPerformed;
    }
    
    /// <summary>
    /// Called when manual immunity is performed.
    /// </summary>
    /// <param name="obj"></param>
    private void OnManualImmunityPerformed(InputAction.CallbackContext obj) {
        if (!_immunityOnCooldown) {
            StartCoroutine(GainImmunityManual());
        }
    }

    private IEnumerator GainImmunityManual() {
        _immunityOnCooldown = true;
        gainImmunityUses.SetValue(gainImmunityUses.value+1);
        // The cooldown is 3 + number of times the immunity has been activated. Minus 1, because we increased it before 
        // using the value for the cooldown.
        yield return new WaitForSeconds(3 + gainImmunityUses.value - 1);
        _immunityOnCooldown = false;
    }

    /// <summary>
    /// Movement action only sets the value of the movement vector. Player script then uses it to move.
    /// </summary>
    /// <param name="obj"></param>
    private void OnMovementActionPerformed(InputAction.CallbackContext obj) {
        Vector2 axisMovementInput = obj.ReadValue<Vector2>();
        movementInputValue.SetValue(axisMovementInput);
    }
}
