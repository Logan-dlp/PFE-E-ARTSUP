using MoonlitMixes.Health;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonlitMixes.Respawn
{
    public class PlayerRespawnInOtherScene : MonoBehaviour
    {
        [SerializeField] private RespawnPointData _respawnPointData;

        private PlayerHealth _playerHealth;

        private static bool shouldReposition = false;

        private void Awake()
        {
            _playerHealth = GetComponent<PlayerHealth>();
        }

        private void OnEnable()
        {
            _playerHealth.OnPlayerRespawnInOtherScene += PrepareRespawnInOtherScene;
        }

        private void OnDisable()
        {
            _playerHealth.OnPlayerRespawnInOtherScene -= PrepareRespawnInOtherScene;
        }

        private void PrepareRespawnInOtherScene()
        {
            shouldReposition = true;
            SceneManager.LoadScene(_respawnPointData.RespawnScene);
        }

        private void Start()
        {
            GameObject respawnPoint = GameObject.FindGameObjectWithTag("Respawn");

            if (respawnPoint != null)
            {
                _respawnPointData.RespawnPosition = respawnPoint.transform.position;
                _respawnPointData.RespawnRotation = respawnPoint.transform.rotation;
                Debug.Log($"RespawnPoint mis à jour: {_respawnPointData.RespawnPosition}");
            }

            if (shouldReposition)
            {
                shouldReposition = false;
                RepositionPlayer();
            }
        }

        private void RepositionPlayer()
        {
            CharacterController player = GameObject.FindAnyObjectByType<CharacterController>();

            if (player != null)
            {
                player.enabled = false;
                player.transform.position = _respawnPointData.RespawnPosition;
                player.transform.rotation = _respawnPointData.RespawnRotation;
                player.enabled = true;
            }
        }
    }
}