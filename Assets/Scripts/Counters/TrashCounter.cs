using System;

public class TrashCounter : BaseCounter {
  public static event EventHandler OnAnyTrashed;

  public new static void ResetStaticData() {
    OnAnyTrashed = null;
  }

  public override void Interact(Player player) {
    if (player.HasKitchenObject()) {
      player.GetKitchenObject().DestroySelf();
      OnAnyTrashed?.Invoke(this, EventArgs.Empty);
    }
  }
}