using UnityEngine;

public class Obstructable : MonoBehaviour
{
    [SerializeField] private Material _transparentMaterialTemplate;
    [Range(0f, 1f)] public float _transparencyRangeX = 0.3f;
    [Range(0f, 1f)] public float _transparencyRangeY = 0.6f;

    private Material _originalMaterial;
    private Material _instanceMaterial;
    private Renderer _objectRenderer;

    private bool _isTransparent = false;

    void Awake()
    {
        _objectRenderer = GetComponent<Renderer>();
        _originalMaterial = _objectRenderer.material;
    }

    public void ApplyTransparency()
    {
        if (_isTransparent) return;

        if (_instanceMaterial == null)
        {
            _instanceMaterial = new Material(_transparentMaterialTemplate);
            _instanceMaterial.mainTexture = _originalMaterial.mainTexture;
        }

        _instanceMaterial.SetFloat("_RangeX", _transparencyRangeX);
        _instanceMaterial.SetFloat("_RangeY", _transparencyRangeY);

        _objectRenderer.material = _instanceMaterial;
        _isTransparent = true;
    }

    public void ResetMaterial()
    {
        if (!_isTransparent) return;
        _objectRenderer.material = _originalMaterial;
        _isTransparent = false;
    }
}