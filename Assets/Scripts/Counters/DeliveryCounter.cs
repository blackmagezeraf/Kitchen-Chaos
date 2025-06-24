using System;

public class DeliveryCounter : BaseCounter {
  public static DeliveryCounter Instance { get; private set; }

  private void Awake() {
    if (Instance) {
      Destroy(this);
    }
    else {
      Instance = this;
    }
  }

  public override void Interact(Player player) {
    if (!player.HasKitchenObject()) {
      return;
    }

    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
      DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
      player.GetKitchenObject().DestroySelf();
    }
  }
}