using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {
  public event EventHandler<OnStateChangedEventArgs>                 OnStateChanged;
  public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

  public class OnStateChangedEventArgs : EventArgs {
    public State ChangedState;
  }

  public enum State {
    Idle,
    Frying,
    Fried,
    Burned,
  }

  [SerializeField] private FryingRecipeSo[]  fryingRecipeSoArray;
  [SerializeField] private BurningRecipeSo[] burningRecipeSoArray;

  private State _currentState;

  private float          _fryingTimer;
  private FryingRecipeSo _fryingRecipeSo;

  private float           _burningTimer;
  private BurningRecipeSo _burningRecipeSo;


  private void Start() {
    _currentState = State.Idle;
  }

  private void Update() {
    if (!HasKitchenObject()) {
      return;
    }

    switch (_currentState) {
    case State.Idle:
      break;
    case State.Frying:
      _fryingTimer += Time.deltaTime;
      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
        ProgressNormalized = _fryingTimer / _fryingRecipeSo.fryingTimerMax
      });
      if (_fryingTimer > _fryingRecipeSo.fryingTimerMax) {
        // Fried
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(_fryingRecipeSo.output, this);

        _currentState = State.Fried;
        _burningTimer = 0f;

        _burningRecipeSo = GetBurningRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { ChangedState = _currentState });
      }

      break;
    case State.Fried:
      _burningTimer += Time.deltaTime;
      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
        ProgressNormalized = _burningTimer / _burningRecipeSo.burningTimerMax
      });
      if (_burningTimer > _burningRecipeSo.burningTimerMax) {
        // Burned
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(_burningRecipeSo.output, this);
        _currentState = State.Burned;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { ChangedState = _currentState });
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
          ProgressNormalized = 0f
        });
      }

      break;
    case State.Burned:
      break;
    default:
      throw new ArgumentOutOfRangeException();
    }
  }

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      // There is no KitchenObject Here
      if (player.HasKitchenObject()) {
        // Player is carrying a KitchenObject
        if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo())) {
          // Player Carrying something that can be fried
          player.GetKitchenObject().SetKitchenObjectParent(this);


          _fryingRecipeSo = GetFryingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());
          _currentState   = State.Frying;
          _fryingTimer    = 0f;

          OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { ChangedState = _currentState });
          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            ProgressNormalized = _fryingTimer / _fryingRecipeSo.fryingTimerMax
          });
        }
      }
      else {
        // Player is not carrying anything
      }
    }
    else {
      // There is a KitchenObject Here
      if (player.HasKitchenObject()) {
        // Player is carrying something
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
          // The Player is holding a plate
          if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo())) {
            GetKitchenObject().DestroySelf();

            _currentState = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { ChangedState = _currentState });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = 0f });
          }
        }
        else {
          // player is not carrying anything
          GetKitchenObject().SetKitchenObjectParent(player);

          _currentState = State.Idle;
          OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { ChangedState = _currentState });
          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = 0f });
        }
      }
    }
  }

  private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo) {
    FryingRecipeSo fryingRecipeSo = GetFryingRecipeSoWithInput(inputKitchenObjectSo);
    if (fryingRecipeSo) {
      return fryingRecipeSo.output;
    }
    else {
      return null;
    }
  }

  private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSo) {
    FryingRecipeSo fryingRecipeSo = GetFryingRecipeSoWithInput(inputKitchenObjectSo);
    return fryingRecipeSo is not null;
  }

  private FryingRecipeSo GetFryingRecipeSoWithInput(KitchenObjectSo inputKitchenObjectSo) {
    foreach (FryingRecipeSo fryingRecipeSo in fryingRecipeSoArray) {
      if (fryingRecipeSo.input == inputKitchenObjectSo) {
        return fryingRecipeSo;
      }
    }

    return null;
  }

  private BurningRecipeSo GetBurningRecipeSoWithInput(KitchenObjectSo inputKitchenObjectSo) {
    foreach (BurningRecipeSo burningRecipeSo in burningRecipeSoArray) {
      if (burningRecipeSo.input == inputKitchenObjectSo) {
        return burningRecipeSo;
      }
    }

    return null;
  }
}