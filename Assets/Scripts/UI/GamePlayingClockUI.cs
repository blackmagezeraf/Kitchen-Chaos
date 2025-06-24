using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {
  [SerializeField] private Image timerImage;

  private void Start() {
    GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
  }

  private void Update() {
    timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
  }

  private void GameManager_OnStateChanged(object sender, EventArgs e) {
    if (GameManager.Instance.IsGamePlaying()) {
      Show();
    }
    else {
      Hide();
    }
  }

  private void Show() => gameObject.SetActive(true);

  private void Hide() => gameObject.SetActive(false);
}