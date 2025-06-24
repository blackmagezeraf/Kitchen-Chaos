using System;
using TMPro;
using Unity.UI.Shaders.Sample;
using UnityEngine;

public class OptionsUI : MonoBehaviour {
  public static OptionsUI Instance;

  [SerializeField] private CustomButton closeButton;

  [Header("Music")]

  [SerializeField] private CustomButton musicButton;

  [SerializeField] private TextMeshProUGUI musicEffectText;

  [Header("SFX")]

  [SerializeField] private CustomButton soundEffectButton;

  [SerializeField] private TextMeshProUGUI soundEffectText;

  public OptionsUI() {
    if (Instance) {
      Destroy(this);
    }
    else {
      Instance = this;
    }
  }

  private void Awake() {
    musicButton.onClick.AddListener(() => {
                                      MusicManager.Instance.ChangeVolume();
                                      UpdateVisual();
                                    });
    soundEffectButton.onClick.AddListener(() => {
                                            SoundManager.Instance.ChangeVolume();
                                            UpdateVisual();
                                          });
    closeButton.onClick.AddListener(() => { Hide(); });
  }

  private void Start() {
    GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

    UpdateVisual();
    Hide();
  }

  private void OnDestroy() {
    GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
  }

  private void GameManager_OnGameUnpaused(object sender, EventArgs e) {
    Hide();
  }

  private void UpdateVisual() {
    soundEffectText.text = $"Sfx: {MathF.Round(SoundManager.Instance.GetVolume()   * 10f)}";
    musicEffectText.text = $"Music: {MathF.Round(MusicManager.Instance.GetVolume() * 10f)}";
  }

  public void Show() {
    gameObject.SetActive(true);
  }

  private void Hide() {
    gameObject.SetActive(false);
  }
}