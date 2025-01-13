using UnityEngine;

namespace MoonlitMixes.Health
{
    public class EnemyHealth : AHealth
    {
        public override void TakeDamage(float damage)
        {
            RemoveHealth(damage);
        }

        protected override void CheckHealth()
        {
            if(_currentHealth <= 0)
            {
                Debug.Log("EnemyDead");
            }
            
            healthBarScriptableInt.SendHealthAmount(_currentHealth / _maxHealth);
        }
    }
}