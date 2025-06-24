using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {
  public static event EventHandler OnAnyCut;

  public new static void ResetStaticData() {
    OnAnyCut = null;
  }

  public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

  public event EventHandler OnCut;

  [SerializeField] private CuttingRecipeSo[] cuttingRecipeSoArray;

  private int _cuttingProgress;

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      // There is no KitchenObject Here
      if (player.HasKitchenObject()) {
        // Player is carrying a KitchenObject
        if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo())) {
          // Player Carrying something that can be cut
          player.GetKitchenObject().SetKitchenObjectParent(this);
          _cuttingProgress = 0;

          CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            ProgressNormalized = (float)_cuttingProgress / cuttingRecipeSo.cuttingProgressMax
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
          }
        }
      }
      else {
        // player is not carrying anything
        GetKitchenObject().SetKitchenObjectParent(player);
      }
    }
  }

  public override void InteractAlternate(Player player) {
    if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo())) {
      // There is a KitchenObject here and It can be cut
      _cuttingProgress++;

      OnCut?.Invoke(this, EventArgs.Empty);
      OnAnyCut?.Invoke(this, EventArgs.Empty);

      CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSo());

      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
        ProgressNormalized = (float)_cuttingProgress / cuttingRecipeSo.cuttingProgressMax
      });

      if (_cuttingProgress < cuttingRecipeSo.cuttingProgressMax) {
        return;
      }

      KitchenObjectSo outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());

      GetKitchenObject().DestroySelf();
      KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
    }
  }

  private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo) {
    CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
    if (cuttingRecipeSo) {
      return cuttingRecipeSo.output;
    }
    else {
      return null;
    }
  }

  private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSo) {
    CuttingRecipeSo cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
    return cuttingRecipeSo is not null;
  }

  private CuttingRecipeSo GetCuttingRecipeSoWithInput(KitchenObjectSo inputKitchenObjectSo) {
    for (int i = 0; i < cuttingRecipeSoArray.Length; i++) {
      CuttingRecipeSo cuttingRecipeSo = cuttingRecipeSoArray[i];
      if (cuttingRecipeSo.input == inputKitchenObjectSo) {
        return cuttingRecipeSo;
      }
    }

    return null;
  }
}