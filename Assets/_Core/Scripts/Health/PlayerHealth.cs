using MoonlitMixes.Animation;
using MoonlitMixes.Events;
using MoonlitMixes.Player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Health
{
    public class PlayerHealth : AHealth
    {
        public event Action OnPlayerRespawnInScene;
        public event Action OnPlayerRespawnInOtherScene;

        [SerializeField] private float _timeBeforeGettingOutOfFight;
        [SerializeField] private float _healthRegeneration;
        [SerializeField] private float _animationDeathTime;
        [SerializeField] private float _animationRespawnTime;
        [SerializeField] private GameObject _deathAnimation;
        [SerializeField] private PlayerHealthData _playerHealthData;

        private bool _isInFight;
        private float _timeBeforeOutOfFight;
        private PlayerMovement _playerMovement;
        private AnimationExplorationManager _animationExplorationManager;
        private bool _isDead;

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
                StartCoroutine(DeathAnimation());
            }

            healthBarScriptableInt.SendHealthAmount(_currentHealth / _maxHealth);
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
        private IEnumerator DeathAnimation()
        {
            yield return new WaitForSeconds(_animationDeathTime);
            _deathAnimation.SetActive(true);

            yield return new WaitForSeconds(_animationRespawnTime);
            PlayerDeathEventDispatcher.TriggerDeath();
            _deathAnimation.SetActive(false);
        }
    }
}