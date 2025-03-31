using UnityEngine;

namespace MoonlitMixes.Health
{
    public abstract class AHealth : MonoBehaviour
    {
        [SerializeField] internal float _maxHealth;
        [SerializeField] protected HealthBarScriptableInt healthBarScriptableInt;
        
        internal float _currentHealth;

        abstract protected void CheckHealth();
        abstract public void TakeDamage(float damage);

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        virtual protected void RemoveHealth(float damage)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            CheckHealth();
        }
    }
}