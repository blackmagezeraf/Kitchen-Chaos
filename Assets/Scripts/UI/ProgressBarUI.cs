using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
  [SerializeField] private GameObject hasProgressGameObject;
  [SerializeField] private Image      barImage;

  private IHasProgress _hasProgress;

  private void Start() {
    _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
    if (!hasProgressGameObject) {
      Debug.LogError($"Game object: {hasProgressGameObject} does not implement IHasProgress interface.");
    }

    _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
    barImage.fillAmount            =  0f;
    Hide();
  }

  private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
    barImage.fillAmount = e.ProgressNormalized;
    if (e.ProgressNormalized is 0f or 1f) {
      Hide();
    }
    else {
      Show();
    }
  }

  private void Show() {
    gameObject.SetActive(true);
  }

  private void Hide() {
    gameObject.SetActive(false);
  }
}