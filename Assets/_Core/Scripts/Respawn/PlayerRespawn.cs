using UnityEngine;
using MoonlitMixes.Health;
using System.Collections;

namespace MoonlitMixes.Respawn
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint;
        
        private CharacterController characterController;
        private PlayerHealth playerHealth;

        private void Awake()
        {
            playerHealth = GetComponent<PlayerHealth>();
            characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            playerHealth.OnPlayerDeath += HandlePlayerDeath;
        }

        private void OnDisable()
        {
            playerHealth.OnPlayerDeath -= HandlePlayerDeath;
        }

        private void HandlePlayerDeath()
        {
            RespawnPlayer();
        }

        private void RespawnPlayer()
        {
            transform.position = respawnPoint.position;
            playerHealth.ResetHealth();

            if (characterController != null)
            {
                characterController.enabled = false;
                characterController.enabled = true;
            }

            StartCoroutine(EnableMovementAfterDelay(1f));
        }

        private IEnumerator EnableMovementAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}