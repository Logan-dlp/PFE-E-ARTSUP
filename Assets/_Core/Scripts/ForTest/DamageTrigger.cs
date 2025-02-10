using UnityEngine;

namespace MoonlitMixes.Health
{
    public class DamageTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            AHealth playerHealth = other.GetComponent<AHealth>();
            if (playerHealth != null)
            {
                // Met la vie � 0
                playerHealth.TakeDamage(playerHealth._currentHealth);
            }
        }
    }
}