using UnityEngine;

namespace MoonlitMixes.Health
{
    public class AHealth : MonoBehaviour
    {
        [SerializeField] internal float _maxHealth;
        internal float _currentHealth;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        virtual protected void RemoveHealth(float damage)
        {
            if (_currentHealth < damage)
            {
                Debug.Log(gameObject.name + "is dead");
            }
            Mathf.Max(0, _currentHealth - damage);
        }
    }
}