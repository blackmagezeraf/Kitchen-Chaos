using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
  [Header("Footsteps Setting")]

  [SerializeField, Range(0f, 1f)] private float footstepTimerMax = 0.15f;

  private Player _player;
  private float  _footstepTimer;

  private void Awake() {
    _player = GetComponent<Player>();
  }

  private void Update() {
    _footstepTimer -= Time.deltaTime;
    if (!(_footstepTimer <= 0f)) {
      return;
    }

    _footstepTimer = footstepTimerMax;

    if (_player.IsWalking()) {
      SoundManager.Instance.PlayFootStepsSound(_player.transform.position,
                                               PlayerPrefs.GetFloat(SoundManager.Instance
                                                                      .sPlayerPrefsSoundEffectsVolumeKey));
    }
  }
}