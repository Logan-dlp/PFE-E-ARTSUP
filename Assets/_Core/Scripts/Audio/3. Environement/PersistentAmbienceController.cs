using UnityEngine;

public class PersistentAudioController : MonoBehaviour
{
    [Header("Ambience")]
    [SerializeField] private AudioEventScriptableObject _ambienceEvent;
    [Range(0f, 1f)][SerializeField] private float _ambienceVolume = 1f;

    [Header("Music")]
    [SerializeField] private AudioEventScriptableObject _musicEvent;
    [Range(0f, 1f)][SerializeField] private float _musicVolume = 1f;

    private void Start()
    {
        if (_ambienceEvent != null)
        {
            AudioManager.Instance?.PlayPersistentAmbience(_ambienceEvent, _ambienceVolume);
        }

        if (_musicEvent != null)
        {
            AudioManager.Instance?.PlayPersistentMusic(_musicEvent, _musicVolume);
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;

        if (_ambienceEvent != null)
        {
            AudioManager.Instance?.SetAmbienceVolume(_ambienceVolume);
        }

        if (_musicEvent != null)
        {
            AudioManager.Instance?.SetMusicVolume(_musicVolume);
        }
    }
}