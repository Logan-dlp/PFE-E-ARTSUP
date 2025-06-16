using FMOD.Studio;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
public class ZoneSonoreAudioEventTrigger : MonoBehaviour
{
    [Header("Références")]
    public Transform _player;

    [Header("Audio")]
    [SerializeField] private AudioEventScriptableObject _audioEvent;

    [Header("Paramètres de zone")]
    [Range(0.1f, 20f)] public float _innerRadius = 2f;
    [Range(0.1f, 50f)] public float _outerRadius = 10f;

    private EventInstance _instance;
    private bool _isPlaying = false;
    private bool _isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrée détectée avec : " + other.name);

        if (_audioEvent == null) return;
        if (!other.CompareTag("Player")) return;

        _isPlayerInside = true;

        if (!_isPlaying)
        {
            _instance = FMODUnity.RuntimeManager.CreateInstance(_audioEvent.EventReference);
            _instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
            _instance.setVolume(0f);
            _instance.start();
            _isPlaying = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

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

        float distance = Vector3.Distance(_player.position, transform.position);

        _instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

        if (distance <= _outerRadius)
        {
            float t = Mathf.InverseLerp(_outerRadius, _innerRadius, distance);
            float volume = Mathf.Clamp01(1f - t);
            _instance.setVolume(volume);
        }
        else
        {
            _instance.setVolume(0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _innerRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _outerRadius);
    }
}