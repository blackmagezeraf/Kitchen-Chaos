using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSo kitchenObjectSo;

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            // Player is carrying Something
            return;
        }

        // Player is not carrying anything
        KitchenObject.SpawnKitchenObject(kitchenObjectSo, player);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
