using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
  public static GameInput Instance { get; private set; }

  public event EventHandler OnInteractAction;
  public event EventHandler OnInteractAlternateAction;
  public event EventHandler OnPauseAction;

  private PlayerInputActions _playerInputActions;

  private void Awake() {
    if (Instance) {
      Destroy(this);
    }
    else {
      Instance = this;
    }

    _playerInputActions = new PlayerInputActions();
    _playerInputActions.Player.Enable();

    _playerInputActions.Player.Interact.performed          += Interact_performed;
    _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    _playerInputActions.Player.Pause.performed             += Pause_Performed;
  }

  private void OnDestroy() {
    _playerInputActions.Player.Interact.performed          -= Interact_performed;
    _playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
    _playerInputActions.Player.Pause.performed             -= Pause_Performed;

    _playerInputActions.Dispose();
  }

  private void Interact_performed(InputAction.CallbackContext obj) {
    OnInteractAction?.Invoke(this, EventArgs.Empty);
  }

  private void InteractAlternate_performed(InputAction.CallbackContext obj) {
    OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
  }

  private void Pause_Performed(InputAction.CallbackContext obj) {
    OnPauseAction?.Invoke(this, EventArgs.Empty);
  }

  public Vector2 GetMovementVectorNormalized() {
    Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

    inputVector = inputVector.normalized;

    return inputVector;
  }
}