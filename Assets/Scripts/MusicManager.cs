using UnityEngine;

public class MusicManager : MonoBehaviour {
  public string sPlayerPrefsMusicVolumeKey = "music_volume";

  public static MusicManager Instance { get; private set; }

  [SerializeField] private AudioSource musicAudioSource;

  private float _volume;

  private void Awake() {
    if (Instance) {
      Destroy(gameObject);
    }
    else {
      Instance = this;
    }

    if (!musicAudioSource) {
      musicAudioSource = GetComponent<AudioSource>();
    }

    _volume                 = PlayerPrefs.GetFloat(sPlayerPrefsMusicVolumeKey, 1f);
    musicAudioSource.volume = _volume;
  }

  private void Start() {
    if (!musicAudioSource) {
      Debug.LogError($"Music Manager audio source is null {musicAudioSource.name} in the gameObject {gameObject.name}");
    }
  }

  public void ChangeVolume() {
    _volume += .1f;
    if (_volume > 1f) {
      _volume = 0f;
    }

    musicAudioSource.volume = _volume;

    PlayerPrefs.SetFloat(sPlayerPrefsMusicVolumeKey, _volume);
    PlayerPrefs.Save();
  }

  public float GetVolume() {
    return _volume;
  }
}