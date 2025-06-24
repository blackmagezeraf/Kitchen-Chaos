using System;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {
  [SerializeField] private Transform container;
  [SerializeField] private Transform recipeTemplate;

  private void Awake() {
    recipeTemplate.gameObject.SetActive(false);
  }

  private void Start() {
    DeliveryManager.Instance.OnRecipeCompleted += OnRecipeCompleted;
    DeliveryManager.Instance.OnRecipeSpawned   += OnRecipeSpawned;

    UpdateVisual();
  }

  private void OnRecipeCompleted(object sender, EventArgs e) {
    UpdateVisual();
  }

  private void OnRecipeSpawned(object sender, EventArgs e) {
    UpdateVisual();
  }

  private void UpdateVisual() {
    foreach (Transform child in container) {
      if (child == recipeTemplate) continue;
      Destroy(child.gameObject);
    }

    foreach (RecipeSo recipeSo in DeliveryManager.Instance.GetWaitingRecipeSoList()) {
      Transform recipeTransform = Instantiate(recipeTemplate, container);
      recipeTransform.gameObject.SetActive(true);
      recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSo(recipeSo);
    }
  }
}