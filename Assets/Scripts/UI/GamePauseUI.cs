using System;
using Unity.UI.Shaders.Sample;
using UnityEngine;

public class GamePauseUI : MonoBehaviour {
  [SerializeField] private CustomButton resumeButton;
  [SerializeField] private CustomButton optionsButton;
  [SerializeField] private CustomButton mainMenuButton;

  private void Awake() {
    resumeButton.onClick.AddListener(() => { GameManager.Instance.TogglePauseGame(); });
    optionsButton.onClick.AddListener(() => { OptionsUI.Instance.Show(); });
    mainMenuButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.MainMenuScene); });
  }

  private void Start() {
    GameManager.Instance.OnGamePaused   += GameManager_OnGamePaused;
    GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    Hide();
  }

  private void OnDestroy() {
    GameManager.Instance.OnGamePaused   -= GameManager_OnGamePaused;
    GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
  }

  private void GameManager_OnGameUnpaused(object sender, EventArgs e) {
    Hide();
  }

  private void GameManager_OnGamePaused(object sender, EventArgs e) {
    Show();
  }

  private void Show() {
    gameObject.SetActive(true);
  }

  private void Hide() {
    gameObject.SetActive(false);
  }
}