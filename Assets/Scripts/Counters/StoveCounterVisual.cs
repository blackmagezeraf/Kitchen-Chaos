using System;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
  [SerializeField] private StoveCounter stoveCounter;
  [SerializeField] private GameObject   stoveOnGameObject;
  [SerializeField] private GameObject   particlesGameObject;

  private void Start() {
    stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
  }

  private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
    bool showVisual = e.ChangedState == StoveCounter.State.Frying || e.ChangedState == StoveCounter.State.Fried;
    stoveOnGameObject.SetActive(showVisual);
    particlesGameObject.SetActive(showVisual);
  }
}