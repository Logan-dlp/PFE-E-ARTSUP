using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ZoneSonoreSphere : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioEventScriptableObject _audioEvent;

    [Header("Paramètres de zone")]
    [Range(0.1f, 50f)] public float _outerDistance = 10f;
    [Range(0.1f, 20f)] public float _innerDistance = 2f;

    [Header("Volume")]
    [Range(0f, 1f)] public float _outerVolume = 0.5f;
    [Range(0f, 1f)] public float _innerVolume = 1f;

    private EventInstance _instance;
    private bool _isPlaying = false;
    private bool _isPlayerInside = false;
    private SphereCollider _sphereCollider;
    private Transform _player;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        _sphereCollider.radius = _outerDistance;

        if (_player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                _player = playerObj.transform;
            else
                Debug.LogWarning("[ZoneSonoreSphere] Aucun objet avec le tag 'Player' trouvé.");
        }
    }

    private void OnValidate()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        if (_sphereCollider != null)
        {
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = _outerDistance;
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

        Vector3 colliderCenterWorld = transform.position + transform.TransformVector(_sphereCollider.center);
        _instance.set3DAttributes(RuntimeUtils.To3DAttributes(colliderCenterWorld));

        float distance = Vector3.Distance(_player.position, colliderCenterWorld);

        if (distance <= _outerDistance)
        {
            if (distance <= _innerDistance)
            {
                _instance.setVolume(_innerVolume);
            }
            else
            {
                float t = Mathf.InverseLerp(_innerDistance, _outerDistance, distance);
                float volume = Mathf.Lerp(_innerVolume, _outerVolume, t);
                _instance.setVolume(volume);
            }
        }
        else
        {
            _instance.setVolume(0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_sphereCollider == null)
            _sphereCollider = GetComponent<SphereCollider>();

        Vector3 colliderCenterWorld = transform.position + transform.TransformVector(_sphereCollider.center);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(colliderCenterWorld, _outerDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(colliderCenterWorld, _innerDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(colliderCenterWorld, 0.2f);
    }
}