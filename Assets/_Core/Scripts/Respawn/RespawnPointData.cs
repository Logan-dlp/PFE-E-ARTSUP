using UnityEngine;

namespace MoonlitMixes.Respawn
{
    [CreateAssetMenu(fileName = "RespawnPointData", menuName = "Scriptable Objects/RespawnPointData")]
    public class RespawnPointData : ScriptableObject
    {
        [SerializeField] private string _respawnScene;
        [SerializeField] private Vector3 _respawnPosition;
        [SerializeField] private Quaternion _respawnRotation;

        public string RespawnScene
        {
            get => _respawnScene;
            set => _respawnScene = value;
        }
        public Vector3 RespawnPosition
        {
            get => _respawnPosition;
            set => _respawnPosition = value;
        }

        public Quaternion RespawnRotation
        {
            get => _respawnRotation;
            set => _respawnRotation = value;
        }
    }
}