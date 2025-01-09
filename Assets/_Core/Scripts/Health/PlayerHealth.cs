using UnityEngine;

namespace MoonlitMixes.Health
{
    public class PlayerHealth : AHealth
    {
        private void Update()
        {
            if(_currentHealth < _maxHealth)
            {

            }
        }

        public void TakeDamage(float damage)
        {
            RemoveHealth(damage);

            
        }

        
    }
}