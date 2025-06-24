using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {
  public static Player Instance { get; private set; }

  public event EventHandler OnPickedSomething;

  public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

  public class OnSelectedCounterChangedEventArgs : EventArgs {
    public BaseCounter SelectedCounter;
  }

  [Header("Player Settings")]

  [SerializeField, Range(0f, 10f)] private float playerRadius = .7f;

  [SerializeField, Range(0f, 10f)] private float playerHeight = 2f;
  [SerializeField, Range(0f, 20f)] private float moveSpeed    = 7f;

  [Header("Interaction Settings")]

  [SerializeField] private GameInput gameInput;

  [SerializeField] private LayerMask countersLayerMask;
  [SerializeField] private Transform kitchenObjectHoldPoint;

  private bool          _isWalking;
  private Vector3       _lastInteractDir;
  private BaseCounter   _selectedCounter;
  private KitchenObject _kitchenObject;

  private void Awake() {
    if (Instance) {
      Destroy(this);
    }
    else {
      Instance = this;
    }
  }

  private void Start() {
    gameInput.OnInteractAction          += GameInput_OnInteractAction;
    gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
  }

  private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
    if (!GameManager.Instance.IsGamePlaying()) return;

    if (_selectedCounter) {
      _selectedCounter.InteractAlternate(this);
    }
  }

  private void GameInput_OnInteractAction(object sender, EventArgs e) {
    if (!GameManager.Instance.IsGamePlaying()) return;

    if (_selectedCounter) {
      _selectedCounter.Interact(this);
    }
  }

  private void Update() {
    HandleMovement();
    HandleInteractions();
  }

#if UNITY_EDITOR
  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (playerHeight / 2), gameObject.transform.position.z),
                        new Vector3(playerRadius, playerHeight, playerRadius));
  }
#endif

  public bool IsWalking() {
    return _isWalking;
  }

  private void HandleInteractions() {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    if (moveDir != Vector3.zero) {
      _lastInteractDir = moveDir;
    }

    float interactDistance = 2f;
    if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance,
                        countersLayerMask)) {
      if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
        // Has ClearCounter
        if (baseCounter != _selectedCounter) {
          SetSelectedCounter(baseCounter);
        }
      }
      else {
        SetSelectedCounter(null);
      }
    }
    else {
      SetSelectedCounter(null);
    }
  }

  private void HandleMovement() {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    float moveDistance = moveSpeed * Time.deltaTime;
    bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                                        playerRadius, moveDir, moveDistance);

    if (!canMove) {
      // Cannot move towards moveDir

      // Attempt only X movement
      Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
      canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position,
                                                       transform.position + Vector3.up * playerHeight, playerRadius,
                                                       moveDirX, moveDistance);

      if (canMove) {
        // Can move only on the X
        moveDir = moveDirX;
      }
      else {
        // Cannot move only on the X

        // Attempt only Z movement
        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
        canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position,
                                                         transform.position + Vector3.up * playerHeight, playerRadius,
                                                         moveDirZ, moveDistance);

        if (canMove) {
          // Can move only on the Z
          moveDir = moveDirZ;
        }
        else {
          // Cannot move in any direction
        }
      }
    }

    if (canMove) {
      transform.position += moveDir * moveDistance;
    }

    _isWalking = moveDir != Vector3.zero;

    float rotateSpeed = 10f;
    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
  }

  private void SetSelectedCounter(BaseCounter selectedCounter) {
    this._selectedCounter = selectedCounter;

    OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
      SelectedCounter = selectedCounter
    });
  }

  public Transform GetKitchenObjectFollowTransform() {
    return kitchenObjectHoldPoint;
  }

  public void SetKitchenObject(KitchenObject newKitchenObject) {
    _kitchenObject = newKitchenObject;

    if (_kitchenObject) {
      OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }
  }

  public KitchenObject GetKitchenObject() {
    return _kitchenObject;
  }

  public void ClearKitchenObject() {
    _kitchenObject = null;
  }

  public bool HasKitchenObject() {
    return _kitchenObject != null;
  }
}