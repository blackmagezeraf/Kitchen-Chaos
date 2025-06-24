using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {
  public event EventHandler OnRecipeSpawned;
  public event EventHandler OnRecipeCompleted;
  public event EventHandler OnRecipeSuccess;
  public event EventHandler OnRecipeFailed;

  public static DeliveryManager Instance { get; private set; }

  [SerializeField] private RecipeListSo recipeListSo;

  private List<RecipeSo> _waitingRecipeSoList;
  private float          _spawnRecipeTimer;
  private float          _spawnRecipeTimerMax;
  private int            _successfulRecipeCount;

  [SerializeField, Range(1, 10)] private int waitingRecipesMax = 4;

  public void Awake() {
    if (!Instance) {
      Instance = this;
    }
    else {
      Destroy(this);
    }

    _waitingRecipeSoList = new List<RecipeSo>();
    _spawnRecipeTimerMax = 4f;
    _spawnRecipeTimer    = _spawnRecipeTimerMax;
  }

  private void Update() {
    _spawnRecipeTimer -= Time.deltaTime;
    if (_spawnRecipeTimer <= 0f) {
      _spawnRecipeTimer = _spawnRecipeTimerMax;

      if (_waitingRecipeSoList.Count < waitingRecipesMax) {
        RecipeSo waitingRecipeSo = recipeListSo.recipeList[UnityEngine.Random.Range(0, recipeListSo.recipeList.Count)];
        _waitingRecipeSoList.Add(waitingRecipeSo);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
      }
    }
  }

  public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
    for (int i = 0; i < _waitingRecipeSoList.Count; i++) {
      RecipeSo waitingRecipeSo = _waitingRecipeSoList[i];

      if (waitingRecipeSo.kitchenObjectSoList.Count == plateKitchenObject.GetKitchenObjectSoList().Count) {
        // Has the same number of ingredients
        bool bPlateContentsMatchesRecipe = true;
        foreach (KitchenObjectSo recipeKitchenObjectSo in waitingRecipeSo.kitchenObjectSoList) {
          // Cycling through all ingredients on recipe
          bool ingredientFound = false;
          foreach (KitchenObjectSo plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSoList()) {
            // Cycling through all ingredients on plate
            if (plateKitchenObjectSo == recipeKitchenObjectSo) {
              // Ingredient does match
              ingredientFound = true;
              break;
            }
          }

          if (!ingredientFound) {
            // This Recipe ingredient was not found on the plate
            bPlateContentsMatchesRecipe = false;
          }
        }

        if (bPlateContentsMatchesRecipe) {
          // Player delivered the correct recipe
          _successfulRecipeCount++;
          _waitingRecipeSoList.RemoveAt(i);
          OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
          OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
          return;
        }
      }
    }

    // No matches found!
    // Player did not deliver a correct recipe
    OnRecipeFailed?.Invoke(this, EventArgs.Empty);
  }

  public List<RecipeSo> GetWaitingRecipeSoList() {
    return _waitingRecipeSoList;
  }

  public int GetSuccessfulRecipeCount() {
    return _successfulRecipeCount;
  }
}