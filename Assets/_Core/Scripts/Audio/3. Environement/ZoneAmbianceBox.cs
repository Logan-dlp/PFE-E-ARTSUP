using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(BoxCollider))]
public class ZoneAmbianceBox : MonoBehaviour
{
    [Header("Audio Ambiance")]
    [SerializeField] private AudioEventScriptableObject _enterZoneSound;
    [SerializeField] private AudioEventScriptableObject _ambianceEvent;
    [SerializeField] private AudioEventScriptableObject _exitAmbianceEvent;

    [Header("Zone")]
    [SerializeField] private Vector3 _outerSize = new Vector3(20f, 20f, 20f);
    [SerializeField] private Vector3 _innerSize = new Vector3(2f, 2f, 2f);

    [Header("Volume")]
    [Range(0f, 1f)] public float _outerVolume = 0f;
    [Range(0f, 1f)] public float _innerVolume = 1f;

    private BoxCollider _boxCollider;
    private EventInstance _instance;
    private bool _isPlayerInside = false;
    private bool _isPlaying = false;
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
                Debug.LogWarning("[ZoneAmbianceBox] Aucun objet avec le tag 'Player' trouvé.");
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
        if (!other.CompareTag("Player") || _isPlaying)
            return;

        _isPlayerInside = true;

        if (_enterZoneSound != null)
        {
            PlaySound(_enterZoneSound);
        }

        if (_ambianceEvent != null)
        {
            _instance = RuntimeManager.CreateInstance(_ambianceEvent.EventReference);
            _instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            _instance.setVolume(0f);
            _instance.start();
            _isPlaying = true;
        }
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

        if (_exitAmbianceEvent != null)
        {
            AudioManager.Instance.PlayPersistentAmbience(_exitAmbianceEvent);
        }
    }

    private void Update()
    {
        if (!_isPlayerInside || !_isPlaying || _player == null || !_instance.isValid())
            return;

        float volume = CalculateVolume(_player.position);
        _instance.setVolume(volume);
    }

    private float CalculateVolume(Vector3 playerPos)
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
        return Mathf.Lerp(_innerVolume, _outerVolume, ratio);
    }

    private void PlaySound(AudioEventScriptableObject audioEvent)
    {
        if (audioEvent == null || AudioManager.Instance == null) return;

        EventInstance instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        instance.start();
        instance.release();
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