using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneSonoreBox : MonoBehaviour
{
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
    private Transform _player;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = _outerSize;

        if (_player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                _player = playerObj.transform;
            else
                Debug.LogWarning("[ZoneSonoreBox] Aucun objet avec le tag 'Player' trouvé.");
        }
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
        if (!_isPlayerInside || !_isPlaying || _player == null) return;

        float volume = CalculateBoxVolume(_player.position);
        _instance.setVolume(volume);
    }

    private float CalculateBoxVolume(Vector3 playerPos)
    {
        Vector3 localPos = transform.InverseTransformPoint(playerPos) - _boxCollider.center;

        Vector3 outerHalf = _outerSize * 0.5f;
        Vector3 innerHalf = _innerSize * 0.5f;

        if (Mathf.Abs(localPos.x) > outerHalf.x ||
            Mathf.Abs(localPos.y) > outerHalf.y ||
            Mathf.Abs(localPos.z) > outerHalf.z)
        {
            return 0f;
        }

        float ratioX = Mathf.InverseLerp(innerHalf.x, outerHalf.x, Mathf.Abs(localPos.x));
        float ratioY = Mathf.InverseLerp(innerHalf.y, outerHalf.y, Mathf.Abs(localPos.y));
        float ratioZ = Mathf.InverseLerp(innerHalf.z, outerHalf.z, Mathf.Abs(localPos.z));

        float ratio = Mathf.Max(ratioX, ratioY, ratioZ);
        return Mathf.Lerp(_innerVolume, _outerVolume, Mathf.Clamp01(ratio));
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