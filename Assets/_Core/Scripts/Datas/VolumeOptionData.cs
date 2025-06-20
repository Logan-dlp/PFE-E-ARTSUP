using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "VolumeOptionData", menuName = "Scriptable Objects/VolumeOptionData")]
    public class VolumeOptionData : ScriptableObject
    {
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
        public float dialogueVolume;
        public float dialogueSpeed;
    }
}
