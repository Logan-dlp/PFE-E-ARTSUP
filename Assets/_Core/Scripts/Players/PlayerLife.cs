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

        public void AddDamage(Vector3 direction, int damage)
        {
            _health -= damage;
            StartCoroutine(playerMovement.Knockback(direction));
        }
    }
}