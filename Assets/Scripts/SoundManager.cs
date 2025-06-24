using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour {
  public static SoundManager Instance { get; private set; }

  public string sPlayerPrefsSoundEffectsVolumeKey = "sfx_volume";

  [SerializeField] private AudioClipRefSo audioClipRefSo;

  private float _volume;

  private void Awake() {
    if (Instance) {
      Destroy(this);
    }
    else {
      Instance = this;
    }

    _volume = PlayerPrefs.GetFloat(sPlayerPrefsSoundEffectsVolumeKey, 1f);
  }

  private void Start() {
    DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
    DeliveryManager.Instance.OnRecipeFailed  += DeliveryManager_OnRecipeFailed;
    Player.Instance.OnPickedSomething        += Player_OnPickedSomething;
    CuttingCounter.OnAnyCut                  += CuttingCounter_OnAnyCut;
    BaseCounter.OnAnyObjectPlacedHere        += BaseCounter_OnAnyObjectPlacedHere;
    TrashCounter.OnAnyTrashed                += TrashCounter_OnAnyObjectTrashed;
  }

  private void OnDestroy() {
    DeliveryManager.Instance.OnRecipeSuccess -= DeliveryManager_OnRecipeSuccess;
    DeliveryManager.Instance.OnRecipeFailed  -= DeliveryManager_OnRecipeFailed;
    Player.Instance.OnPickedSomething        -= Player_OnPickedSomething;
    CuttingCounter.OnAnyCut                  -= CuttingCounter_OnAnyCut;
    BaseCounter.OnAnyObjectPlacedHere        -= BaseCounter_OnAnyObjectPlacedHere;
    TrashCounter.OnAnyTrashed                -= TrashCounter_OnAnyObjectTrashed;
  }

  private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e) {
    TrashCounter trashCounter = sender as TrashCounter;
    if (trashCounter) {
      PlaySound(audioClipRefSo.trash, trashCounter!.transform.position);
    }
  }

  private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e) {
    BaseCounter baseCounter = sender as BaseCounter;
    if (baseCounter) {
      PlaySound(audioClipRefSo.objectDropped, baseCounter!.transform.position);
    }
  }

  private void Player_OnPickedSomething(object sender, EventArgs e) {
    PlaySound(audioClipRefSo.objectPickup, Player.Instance.transform.position);
  }

  private void CuttingCounter_OnAnyCut(object sender, EventArgs e) {
    CuttingCounter cuttingCounter = sender as CuttingCounter;
    if (cuttingCounter) {
      PlaySound(audioClipRefSo.chop, cuttingCounter!.transform.position);
    }
  }

  private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e) {
    DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
    PlaySound(audioClipRefSo.deliveryFailed, deliveryCounter.transform.position);
  }

  private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e) {
    DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
    PlaySound(audioClipRefSo.deliverySuccess, deliveryCounter.transform.position);
  }

  private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f) {
    AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position,
                                volumeMultiplier * _volume);
  }

  public void PlayFootStepsSound(Vector3 position, float fVolume) {
    PlaySound(audioClipRefSo.footSteps, position, fVolume);
  }

  public void ChangeVolume() {
    _volume += .1f;
    if (_volume > 1f) {
      _volume = 0f;
    }

    PlayerPrefs.SetFloat(sPlayerPrefsSoundEffectsVolumeKey, _volume);
    PlayerPrefs.Save();
  }

  public float GetVolume() {
    return _volume;
  }
}