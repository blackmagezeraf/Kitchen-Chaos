using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
[AddComponentMenu("UI/ShaderGraph Samples/Graphic Color")]
[RequireComponent(typeof(Graphic))]
public class GraphicColor : UIBehaviour {
  [SerializeField] private Color _normal      = Color.white,
                                 _highlighted = Color.white,
                                 _pressed     = Color.white,
                                 _selected    = Color.white,
                                 _disabled    = Color.gray;

  private Graphic _graphic;

  private Graphic Graphic {
    get {
      if (_graphic == null)
        _graphic = GetComponent<Graphic>();
      return _graphic;
    }
  }

  public void SetColor(int state) {
    Graphic.color = state switch {
      0 => _normal,
      1 => _highlighted,
      2 => _pressed,
      3 => _selected,
      4 => _disabled,
      _ => _normal,
    };
  }
}