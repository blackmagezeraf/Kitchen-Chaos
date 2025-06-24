using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {
  public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

  public class OnIngredientAddedEventArgs : EventArgs {
    public KitchenObjectSo KitchenObjectSo;
  }

  [SerializeField] private List<KitchenObjectSo> validKitchenObjectSoList;

  private List<KitchenObjectSo> _kitchenObjectList;

  private void Awake() {
    _kitchenObjectList = new List<KitchenObjectSo>();
  }

  public bool TryAddIngredient(KitchenObjectSo kitchenObjectSo) {
    if (_kitchenObjectList.Contains(kitchenObjectSo)) {
      // Already Has this type
      return false;
    }

    if (!validKitchenObjectSoList.Contains(kitchenObjectSo)) {
      // Kitchen Object to Pickup is not a valid pickup object
      return false;
    }

    _kitchenObjectList.Add(kitchenObjectSo);
    OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { KitchenObjectSo = kitchenObjectSo });
    return true;
  }

  public List<KitchenObjectSo> GetKitchenObjectSoList() {
    return _kitchenObjectList;
  }
}