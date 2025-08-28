using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Trigger _trigger;

    private Renderer _renderer;
    private Color _baseColor;

    private bool _isColorChanged;

    private void OnEnable()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
            _baseColor = _renderer.material.color;
        }

        _renderer.material.color = _baseColor;
        _trigger.Triggered += ChangeColor;
        _isColorChanged = false;
    }

    private void OnDisable()
    {
        _trigger.Triggered -= ChangeColor;
    }

    private void ChangeColor()
    {
        if (_isColorChanged == false)
        {
            _renderer.material.color = Random.ColorHSV();
            _isColorChanged = true;
        }
    }
}