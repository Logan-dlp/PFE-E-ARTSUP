using MoonlitMixes.Animation;
using MoonlitMixes.Events;
using MoonlitMixes.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Health
{
    public class PlayerHealth : AHealth
    {
        public event Action OnPlayerRespawnInScene;
        public event Action OnPlayerRespawnInOtherScene;

        public event Action OnLowHealth;
        public event Action OnHealthRecovered;

        [SerializeField] private float _timeBeforeGettingOutOfFight;
        [SerializeField] private float _healthRegeneration;
        [SerializeField] private PlayerHealthData _playerHealthData;

        [SerializeField, Range(0f, 1f)]
        private float _lowHealthThreshold = 20f;
        private bool _lowHealthTriggered;

        private bool _isInFight;
        private float _timeBeforeOutOfFight;
        private PlayerMovement _playerMovement;
        private AnimationExplorationManager _animationExplorationManager;
        private bool _isDead;
        private bool _lowHealthAlreadyNotified;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _animationExplorationManager = GetComponent<AnimationExplorationManager>();
        }

        private void Start()
        {
            _maxHealth = _playerHealthData.MaxHealth;
            _currentHealth = _playerHealthData.CurrentHealth;
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
            _animationExplorationManager.Hit();

            EnterFightMode();
            CheckHealth();
            StartCoroutine(_playerMovement.Knockback(direction, force, duration));
        }

        public void EnterFightMode()
        {
            _isInFight = true;
            _timeBeforeOutOfFight = _timeBeforeGettingOutOfFight;
        }

        protected override void CheckHealth()
        {
            if (_currentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                _animationExplorationManager.Death();
                GetComponent<PlayerInput>().DeactivateInput();

                PlayerDeathEventDispatcher.TriggerDeath();
            }

            float ratio = _currentHealth / _maxHealth;

            if (!_lowHealthTriggered && ratio <= _lowHealthThreshold)
            {
                _lowHealthTriggered = true;
                OnLowHealth?.Invoke();
            }
            else if (_lowHealthTriggered && ratio > _lowHealthThreshold)
            {
                _lowHealthTriggered = false;
                OnHealthRecovered?.Invoke();
            }

            healthBarScriptableInt.SendHealthAmount(ratio);
        }

        public void ResetHealth()
        {
            GetComponent<PlayerInput>().ActivateInput();
            _animationExplorationManager.DefaultState();
            _isDead = false;
            _currentHealth = _maxHealth;
            CheckHealth();
        }

        private void Death()
        {
            ResetHealth();
            OnPlayerRespawnInScene?.Invoke();
        }
    }
}