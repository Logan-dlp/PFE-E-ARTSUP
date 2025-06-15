using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "NewAudioEvent", menuName = "Audio/Audio Event")]
public class AudioEventScriptableObject : ScriptableObject
{
    public EventReference EventReference;
}