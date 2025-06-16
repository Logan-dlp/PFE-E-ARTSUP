using UnityEngine;

public class PersistentAudioController : MonoBehaviour
{
    [SerializeField] private AudioEventScriptableObject _ambienceEvent;
    [SerializeField] private AudioEventScriptableObject _musicEvent;

    void Start()
    {
        if (_ambienceEvent != null)
        {
            AudioManager.Instance?.PlayPersistentAmbience(_ambienceEvent);
        }

        if (_musicEvent != null)
        {
            AudioManager.Instance?.PlayPersistentMusic(_musicEvent);
        }
    }
}