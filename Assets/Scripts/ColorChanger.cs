using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Trigger _trigger;

    private Renderer _renderer;
    private Material _baseMaterial;

    private bool _isColorChanged;

    private void OnEnable()
    {
        Debug.Log("enable");
        _renderer.material = _baseMaterial;
        _trigger.Triggered += ChangeColor;
        _isColorChanged = false;
    }

    private void OnDisable()
    {
        _trigger.Triggered -= ChangeColor;
    }

    private void Start()
    {
        Debug.Log("start");
        _renderer = GetComponent<Renderer>();
        _baseMaterial = _renderer.material;
    }

    private void ChangeColor()
    {
        if (_isColorChanged == false)
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = Random.ColorHSV();
            _isColorChanged = true;
        }
    }
}