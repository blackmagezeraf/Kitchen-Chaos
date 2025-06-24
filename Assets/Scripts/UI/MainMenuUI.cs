using Unity.UI.Shaders.Sample;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
  [SerializeField] private CustomButton playButton;
  [SerializeField] private CustomButton quitButton;

  private void Awake() {
    playButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.GameScene); });
    quitButton.onClick.AddListener(Application.Quit);
    Time.timeScale = 1f;
  }
}