using UnityEngine;

public class ClearCounter : BaseCounter {
  [SerializeField] private KitchenObjectSo kitchenObjectSo;

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      // There is no KitchenObject Here
      if (player.HasKitchenObject()) {
        // Player is carrying a KitchenObject
        player.GetKitchenObject().SetKitchenObjectParent(this);
      }
      else {
        // Player is not carrying anything
        // Nothing will happen
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
        else {
          // Player is not carrying plate but something else
          if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
            // The Counter is holding a plate
            if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo())) {
              player.GetKitchenObject().DestroySelf();
            }
          }
        }
      }
      else {
        // player is not carrying anything
        GetKitchenObject().SetKitchenObjectParent(player);
      }
    }
  }
}