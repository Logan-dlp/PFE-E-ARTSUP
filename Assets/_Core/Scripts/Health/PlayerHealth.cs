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

        public event Action OnLowHealth;
        public event Action OnHealthRecovered;
        public event Action OnDeath;

        [SerializeField] private float _timeBeforeGettingOutOfFight;
        [SerializeField] private float _healthRegeneration;
        [SerializeField] private float _animationDeathTime;
        [SerializeField] private float _animationRespawnTime;
        [SerializeField] private GameObject _deathAnimation;
        [SerializeField] private PlayerHealthData _playerHealthData;
        [SerializeField] private float _lowHealthThreshold = 0.3f;
        private bool _canAnimate = true;
        private bool _lowHealthTriggered;
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
            if (_canAnimate && _animationExplorationManager.Animator.speed == 0) _animationExplorationManager.Animator.speed = 1;
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
                OnDeath?.Invoke();
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
            _isDead = false;
            _currentHealth = _maxHealth;
            CheckHealth();
        }
        private IEnumerator DeathAnimation()
        {
            _canAnimate = false;
            yield return new WaitForSeconds(_animationDeathTime);
            _deathAnimation.SetActive(true);
            _animationExplorationManager.StandUp(true);
            OnPlayerRespawnInScene?.Invoke();

            yield return new WaitForSeconds(_animationRespawnTime);
            _canAnimate = true;

            _animationExplorationManager.Animator.speed = 1;

            PlayerDeathEventDispatcher.TriggerDeath();
            _deathAnimation.SetActive(false);
            _animationExplorationManager.Animator.speed = 1;

            yield return new WaitForSeconds(0.5f);
            _animationExplorationManager.StandUp(false);
            ResetHealth();
        }
        public void StartStandUp() //fonction appelée par un event dans l'animation "stand up"
        {
            _animationExplorationManager.Animator.speed = 0.0f;
        }
        public void ActivateTheInput() //fonction appelée par un event dans l'animation "stand up"
        {
            _animationExplorationManager.Animator.speed = 1;

            StartCoroutine(DelayInput());
        }
        private IEnumerator DelayInput()
        {
            yield return null;
            GetComponent<PlayerInput>().ActivateInput();
        }
    }
}