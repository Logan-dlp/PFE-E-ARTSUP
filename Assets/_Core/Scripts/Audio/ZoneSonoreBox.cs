using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneSonoreBox : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Transform _player;

    [Header("Audio")]
    [SerializeField] private AudioEventScriptableObject _audioEvent;

    [Header("Paramètres de zone")]
    [SerializeField] private Vector3 _outerSize = new Vector3(20f, 20f, 20f);
    [SerializeField] private Vector3 _innerSize = new Vector3(2f, 2f, 2f);

    [Header("Volume")]
    [Range(0f, 1f)] public float _outerVolume = 0.5f;
    [Range(0f, 1f)] public float _innerVolume = 1f;

    private EventInstance _instance;
    private bool _isPlaying = false;
    private bool _isPlayerInside = false;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = _outerSize;
    }

    private void OnValidate()
    {
        _boxCollider = GetComponent<BoxCollider>();
        if (_boxCollider != null)
        {
            _boxCollider.isTrigger = true;
            _boxCollider.size = _outerSize;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _audioEvent == null)
            return;

        _isPlayerInside = true;

        _instance = RuntimeManager.CreateInstance(_audioEvent.EventReference);
        _instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        _instance.setVolume(0f);
        _instance.start();
        _isPlaying = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _isPlayerInside = false;

        if (_isPlaying)
        {
            _instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _instance.release();
            _isPlaying = false;
        }
    }

    private void Update()
    {
        if (!_isPlayerInside || !_isPlaying) return;

        float volume = CalculateBoxVolume(_player.position);
        _instance.setVolume(volume);
    }

    private float CalculateBoxVolume(Vector3 playerPos)
    {
        // Convertir position joueur dans espace local collider
        Vector3 localPos = transform.InverseTransformPoint(playerPos) - _boxCollider.center;

        Vector3 outerHalf = _outerSize * 0.5f;
        Vector3 innerHalf = _innerSize * 0.5f;

        // Vérifier si joueur est dans le box extérieur
        if (Mathf.Abs(localPos.x) > outerHalf.x ||
            Mathf.Abs(localPos.y) > outerHalf.y ||
            Mathf.Abs(localPos.z) > outerHalf.z)
        {
            return 0f; // hors du box => volume 0
        }

        // Calculer la distance normalisée entre inner box et outer box sur chaque axe
        // Ratio = 0 au bord intérieur (innerSize), 1 au bord extérieur (outerSize)
        float ratioX = Mathf.InverseLerp(innerHalf.x, outerHalf.x, Mathf.Abs(localPos.x));
        float ratioY = Mathf.InverseLerp(innerHalf.y, outerHalf.y, Mathf.Abs(localPos.y));
        float ratioZ = Mathf.InverseLerp(innerHalf.z, outerHalf.z, Mathf.Abs(localPos.z));

        // On prend la valeur la plus grande pour que volume diminue dès qu'on approche d'un bord extérieur
        float ratio = Mathf.Max(ratioX, ratioY, ratioZ);
        ratio = Mathf.Clamp01(ratio);

        // Interpoler volume entre innerVolume (0 = zone intérieure) et outerVolume (1 = bord extérieur)
        float volume = Mathf.Lerp(_innerVolume, _outerVolume, ratio);
        return volume;
    }

    private void OnDrawGizmosSelected()
    {
        if (_boxCollider == null)
            _boxCollider = GetComponent<BoxCollider>();

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_boxCollider.center, _outerSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCollider.center, _innerSize);

        Gizmos.matrix = Matrix4x4.identity;
    }
}