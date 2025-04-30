using MoonlitMixes.Respawn;
using System;
using MoonlitMixes.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonlitMixes.Health
{
    public class PlayerHealth : AHealth
    {
        public event Action OnPlayerRespawnInScene;
        public event Action OnPlayerRespawnInOtherScene;

        [SerializeField] private float _timeBeforeGettingOutOfFight;
        [SerializeField] private float _healthRegeneration;

        private bool _isInFight;
        private float _timeBeforeOutOfFight;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            if (_isInFight)
            {
                if (_timeBeforeOutOfFight > 0)
                {
                    _timeBeforeOutOfFight -= Time.fixedDeltaTime;
                }
                else
                {
                    _isInFight = false;
                }
            }

            if (!_isInFight && _currentHealth < _maxHealth)
            {
                _currentHealth += _healthRegeneration * Time.fixedDeltaTime;
                _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
                CheckHealth();
            }
        }

        public override void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            EnterFightMode();
            CheckHealth();
        }

        public void AddDamage(int damage, Vector3 direction, float force, float duration)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            EnterFightMode();
            CheckHealth();
            StartCoroutine(_playerMovement.Knockback(direction, force, duration));
        }

        public void EnterFightMode()
        {
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