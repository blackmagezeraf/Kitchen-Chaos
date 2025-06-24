using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {
  [SerializeField] private PlatesCounter platesCounter;
  [SerializeField] private Transform     counterTopPoint;
  [SerializeField] private Transform     plateVisualPrefab;

  private List<GameObject> _plateVisualGameObjectList;

  private void Awake() {
    _plateVisualGameObjectList = new List<GameObject>();
  }

  private void Start() {
    platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
    platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
  }

  private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e) {
    Transform platesVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

    float plateOffsetY = .1f;
    platesVisualTransform.localPosition = new Vector3(0f, plateOffsetY * _plateVisualGameObjectList.Count, 0f);
    _plateVisualGameObjectList.Add(platesVisualTransform.gameObject);
  }

  private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e) {
    GameObject plateGameObject = _plateVisualGameObjectList[^1];
    _plateVisualGameObjectList.Remove(plateGameObject);
    Destroy(plateGameObject);
  }
}