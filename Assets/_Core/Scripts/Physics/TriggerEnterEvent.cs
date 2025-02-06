using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoonlitMixes.Collision
{
    public class TriggerEnterEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider> _callback;
        
        private void OnTriggerEnter(Collider other)
        {
            _callback?.Invoke(other);
        }
    }
}