using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StoveCounterSound : MonoBehaviour {
  [SerializeField] private StoveCounter stoveCounter;

  private AudioSource _audioSource;

  private void Awake() {
    _audioSource = GetComponent<AudioSource>();
  }

  private void Start() {
    stoveCounter.OnStateChanged += StoveCounter_OnsStateChanged;
  }

  private void StoveCounter_OnsStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
    bool playSound = e.ChangedState == StoveCounter.State.Frying || e.ChangedState == StoveCounter.State.Fried;
    if (playSound) {
      _audioSource.Play();
    }
    else {
      _audioSource.Pause();
    }
  }
}