using MoonlitMixes.Player;
using UnityEngine;

namespace MoonlitMixes.Health
{
    public class PlayerHealth : AHealth
    {
        [SerializeField] private float _timeBeforeGettingOutOfFight;
        [SerializeField] private float _healthRegenetion;

        private bool _isInFight;
        private float _timeBeforeOutOfFight;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            if(_timeBeforeOutOfFight >= 0)
            {
                _timeBeforeOutOfFight -= .02f;
            }
            else
            {
                _isInFight = false;
            }
            
            if(_currentHealth < _maxHealth && !_isInFight)
            {
                _currentHealth += _healthRegenetion * .02f;
                CheckHealth();
            }
        }

        public override void TakeDamage(float damage)
        {
            Debug.Log("");
            RemoveHealth(damage);
            _isInFight = true;
            _timeBeforeOutOfFight = _timeBeforeGettingOutOfFight;
        }

        public void AddDamage(int damage, Vector3 direction, float force, float duration)
        {
            _currentHealth -= damage;
            StartCoroutine(_playerMovement.Knockback(direction, force, duration));
        }

        protected override void CheckHealth()
        {
            if(_currentHealth <= 0)
            {
                Debug.Log("PlayerDeath");
            }
            
            healthBarScriptableInt.SendHealthAmount(_currentHealth / _maxHealth);
        }
    }
}