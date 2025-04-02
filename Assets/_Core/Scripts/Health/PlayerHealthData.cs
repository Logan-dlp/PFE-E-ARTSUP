using UnityEngine;

namespace MoonlitMixes.Health
{
    [CreateAssetMenu(fileName = "new_" + nameof(PlayerHealthData), menuName = "Scriptable Objects/Player Health Data")]
    public class PlayerHealthData : ScriptableObject
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;

        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = Mathf.Max(0, value);
        }

        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
        }
    }
}