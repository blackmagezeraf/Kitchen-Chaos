using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class Loader {
  public enum Scene {
    MainMenuScene,
    GameScene,
    LoadingScene,
  }

  private static Scene _targetScene;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void InitializeLoader() {
    _targetScene = Scene.MainMenuScene;
  }

  public static void Load(Scene targetScene) {
    _targetScene = targetScene;
    SceneManager.LoadScene(nameof(Scene.LoadingScene));
  }

  public static void LoaderCallback() {
    SceneManager.LoadScene(_targetScene.ToString());
  }
}