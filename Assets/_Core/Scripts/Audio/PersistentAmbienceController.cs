using UnityEngine;

public class PersistentAmbienceController : MonoBehaviour
{
    [SerializeField] private AudioEventScriptableObject _ambienceEvent;

    void Start()
    {
        if (_ambienceEvent != null)
        {
            AudioManager.Instance?.PlayPersistentAmbience(_ambienceEvent);
        }
    }
}