using UnityEngine;
using System.Collections;

namespace TMPro.Examples {
  public class ShaderPropAnimator : MonoBehaviour {
    private Renderer _renderer;
    private Material _material;

    public AnimationCurve glowCurve;

    public float frame;

    void Awake() {
      // Cache a reference to object's renderer
      _renderer = GetComponent<Renderer>();

      // Cache a reference to object's material and create an instance by doing so.
      _material = _renderer.sharedMaterial;
    }

    void Start() {
      StartCoroutine(AnimateProperties());
    }

    IEnumerator AnimateProperties() {
      //float lightAngle;
      float glowPower;
      frame = Random.Range(0f, 1f);

      while (true) {
        //lightAngle = (m_Material.GetFloat(ShaderPropertyIDs.ID_LightAngle) + Time.deltaTime) % 6.2831853f;
        //m_Material.SetFloat(ShaderPropertyIDs.ID_LightAngle, lightAngle);

        glowPower = glowCurve.Evaluate(frame);
        _material.SetFloat(ShaderUtilities.ID_GlowPower, glowPower);

        frame += Time.deltaTime * Random.Range(0.2f, 0.3f);
        yield return new WaitForEndOfFrame();
      }
    }
  }
}