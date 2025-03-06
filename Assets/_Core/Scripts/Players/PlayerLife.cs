using System;
using UnityEngine;

namespace MoonlitMixes.Player
{
    public class PlayerLife : MonoBehaviour
    {
        [SerializeField] private int _health = 100;
        
        private PlayerMovement playerMovement;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        public void AddDamage(int damage, Vector3 direction, float force, float duration)
        {
            _health -= damage;
            StartCoroutine(playerMovement.Knockback(direction, force, duration));
        }
    }
}