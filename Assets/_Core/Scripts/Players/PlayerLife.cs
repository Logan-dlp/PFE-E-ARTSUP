using UnityEngine;

namespace MoonlitMixes.Player
{
    public class PlayerLife : MonoBehaviour
    {
        [SerializeField] private int _health = 100;
        
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        public void AddDamage(int damage, Vector3 direction, float force, float duration)
        {
            _health -= damage;
            StartCoroutine(_playerMovement.Knockback(direction, force, duration));
        }
    }
}