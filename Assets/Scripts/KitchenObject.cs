using UnityEngine;
using UnityEngine.Pool;

public class KitchenObject : MonoBehaviour {
  [SerializeField] private KitchenObjectSo kitchenObjectSo;

  private IKitchenObjectParent _kitchenObjectParent;

  public KitchenObjectSo GetKitchenObjectSo() {
    return kitchenObjectSo;
  }

  public void SetKitchenObjectParent(IKitchenObjectParent newKitchenObjectParent) {
    if (_kitchenObjectParent != null) {
      _kitchenObjectParent.ClearKitchenObject();
    }

    _kitchenObjectParent = newKitchenObjectParent;

    if (newKitchenObjectParent.HasKitchenObject()) {
      Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
    }

    newKitchenObjectParent.SetKitchenObject(this);

    transform.parent        = newKitchenObjectParent.GetKitchenObjectFollowTransform();
    transform.localPosition = Vector3.zero;
  }

  public IKitchenObjectParent GetKitchenObjectParent() {
    return _kitchenObjectParent;
  }

  /// <summary>
  /// Check if KitchenObject is a Plate or not.
  /// </summary>
  /// <param name="plateKitchenObject"></param>
  /// <returns></returns>
  public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
    if (this is PlateKitchenObject) {
      plateKitchenObject = this as PlateKitchenObject;
      return true;
    }
    else {
      plateKitchenObject = null;
      return false;
    }
  }

  /// <summary>
  /// Destroy the KitchenObject.
  /// </summary>
  public void DestroySelf() {
    _kitchenObjectParent.ClearKitchenObject();
    Destroy(gameObject);
  }

  /// <summary>
  /// Spawn the KitchenObject. Globally.
  /// </summary>
  /// <param name="kitchenObjectSo"></param>
  /// <param name="kitchenObjectParent"></param>
  /// <returns></returns>
  public static KitchenObject SpawnKitchenObject(
    KitchenObjectSo      kitchenObjectSo,
    IKitchenObjectParent kitchenObjectParent) {
    Transform     kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab);
    KitchenObject kitchenObject          = kitchenObjectTransform.GetComponent<KitchenObject>();
    kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    return kitchenObject;
  }
}