using UnityEngine;
using MoonlitMixes.Health;
using System.Collections;

namespace MoonlitMixes.Respawn
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] private Transform _respawnPoint;
        
        private CharacterController _characterController;
        private PlayerHealth _playerHealth;

        private void Awake()
        {
            _playerHealth = GetComponent<PlayerHealth>();
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _playerHealth.OnPlayerDeath += HandlePlayerDeath;
        }

        private void OnDisable()
        {
            _playerHealth.OnPlayerDeath -= HandlePlayerDeath;
        }

        private void HandlePlayerDeath()
        {
            RespawnPlayer();
        }

        private void RespawnPlayer()
        {
            transform.position = _respawnPoint.position;
            _playerHealth.ResetHealth();

            if (_characterController != null)
            {
                _characterController.enabled = false;
                _characterController.enabled = true;
            }

            StartCoroutine(EnableMovementAfterDelay(1f));
        }

        private IEnumerator EnableMovementAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}