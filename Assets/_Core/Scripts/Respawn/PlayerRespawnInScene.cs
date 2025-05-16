using UnityEngine;
using MoonlitMixes.Health;
using System.Collections;
using MoonlitMixes.Events;

namespace MoonlitMixes.Respawn
{
    public class PlayerRespawnInScene : MonoBehaviour
    {
        [SerializeField] private Transform _respawnPoint;
        [SerializeField] private ScriptableEvent _scriptableEvent;
        
        private CharacterController _characterController;
        private PlayerHealth _playerHealth;

        private void Awake()
        {
            _playerHealth = GetComponent<PlayerHealth>();
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _playerHealth.OnPlayerRespawnInScene += RespawnPlayer;
        }

        private void OnDisable()
        {
            _playerHealth.OnPlayerRespawnInScene -= RespawnPlayer;
        }


        private void RespawnPlayer()
        {
            _scriptableEvent?.SendEvent();
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