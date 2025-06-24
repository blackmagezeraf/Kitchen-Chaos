using System;
using UnityEngine;

public class PlatesCounter : BaseCounter {
  public event EventHandler OnPlateSpawned;
  public event EventHandler OnPlateRemoved;

  [Header("Plates")]

  [SerializeField] private KitchenObjectSo plateKitchenObjectSo;

  [SerializeField, Range(0.01f, 10f)] private float spawnPlateTimerMax = 5f;
  [SerializeField, Range(1,     10)]  private int   platesSpawnedMax   = 4;

  private float _spawnPlateTimer;
  private int   _platesSpawnedAmount;

  private void Update() {
    _spawnPlateTimer += Time.deltaTime;
    if (!(_spawnPlateTimer > spawnPlateTimerMax)) {
      // If spawn timer is not at the required threshold to do anything, then simply return.
      return;
    }

    // Reset the Spawn Timer to recheck.
    _spawnPlateTimer = 0f;

    if (_platesSpawnedAmount >= platesSpawnedMax) {
      // Plates on the counter are the maximum numbers of plates it can hold.
      return;
    }

    // Spawn a plate since the counter can still hold more plates.
    ++_platesSpawnedAmount;
    OnPlateSpawned?.Invoke(this, EventArgs.Empty);
  }

  public override void Interact(Player player) {
    if (player.HasKitchenObject()) {
      // Player has something in his hand
      return;
    }

    if (_platesSpawnedAmount > 0) {
      // There is at least one plate on the counter
      --_platesSpawnedAmount;
      KitchenObject.SpawnKitchenObject(plateKitchenObjectSo, player);
      OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
  }
}