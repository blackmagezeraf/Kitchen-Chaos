using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSo : ScriptableObject {
  [Header("Recipe Description")]

  public string recipeName;

  public List<KitchenObjectSo> kitchenObjectSoList;
}