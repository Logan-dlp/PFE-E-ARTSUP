using MoonlitMixes.Respawn;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonlitMixes.Health
{
    public class PlayerHealth : AHealth
    {
        public event Action OnPlayerRespawnInScene;
        public event Action OnPlayerRespawnInOtherScene;

        [SerializeField] private float _timeBeforeGettingOutOfFight;
        [SerializeField] private float _healthRegenetion;
        [SerializeField] private RespawnPointData respawnData;
        [SerializeField] private PlayerHealthData playerHealthData;

        private bool _isInFight;
        private float _timeBeforeOutOfFight;

        private void FixedUpdate()
        {
            if (_timeBeforeOutOfFight >= 0)
            {
                _timeBeforeOutOfFight -= .02f;
            }
            else
            {
                _isInFight = false;
            }

            if (_currentHealth < _maxHealth && !_isInFight)
            {
                _currentHealth += _healthRegenetion * .02f;
                CheckHealth();
            }
        }

        public override void TakeDamage(float damage)
        {
            RemoveHealth(damage);
            _isInFight = true;
            _timeBeforeOutOfFight = _timeBeforeGettingOutOfFight;
        }

        private void Start()
        {
            _maxHealth = playerHealthData.MaxHealth;
            _currentHealth = playerHealthData.CurrentHealth;
        }

        protected override void CheckHealth()
        {
            if (_currentHealth <= 0)
            {
                if (SceneManager.GetActiveScene().name == respawnData.RespawnScene)
                {
                    OnPlayerRespawnInScene?.Invoke();
                }
                else
                {
                    OnPlayerRespawnInOtherScene?.Invoke();
                }
            }

            playerHealthData.CurrentHealth = _currentHealth;
            playerHealthData.MaxHealth = _maxHealth;

            healthBarScriptableInt.SendHealthAmount(_currentHealth / _maxHealth);
        }

        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
            CheckHealth();
        }
    }
}