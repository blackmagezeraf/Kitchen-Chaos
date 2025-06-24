using System;
using UnityEngine;

public class SetCursor : MonoBehaviour {
  private void Awake() {
    DontDestroyOnLoad(gameObject);
    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible   = false;
  }

  private void Start() {
    GameManager.Instance.OnGamePaused   += GameManager_OnGamePaused;
    GameManager.Instance.OnGameUnpaused += GameManager_OnUnpaused;
  }

  private void OnDestroy() {
    GameInput.Instance.OnPauseAction -= GameManager_OnGamePaused;
  }

  private void GameManager_OnUnpaused(object sender, EventArgs e) {
    Cursor.visible = GameManager.Instance.IsGamePaused();
  }

  private static void GameManager_OnGamePaused(object sender, EventArgs e) {
    Cursor.visible = GameManager.Instance.IsGamePaused();
  }
}