using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AmbienceZoneTrigger : MonoBehaviour
{
    [SerializeField] private AudioEventScriptableObject _ambienceEvent;
    [SerializeField] private AudioEventScriptableObject _otherAmbianceEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_ambienceEvent != null)
        {
            AudioManager.Instance?.PlayPersistentAmbience(_ambienceEvent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_ambienceEvent != null)
        {
            AudioManager.Instance?.PlayPersistentAmbience(_ambienceEvent);
        }
    }
}