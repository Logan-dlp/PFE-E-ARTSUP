using UnityEngine;
using System;

namespace MoonlitMixes.ExplorationTools
{
    public class TreeHealth : MonoBehaviour, IDamageable
    {
        [Header("Number of times the bark can be recovered")]
        [SerializeField] private int _maxHits = 2;
        private int _currentHits = 0;

        [Header("Object to deactivate when the tree is cut")]
        [SerializeField] private GameObject _desativeObject;

        public event Action OnBecameUnusable;

        public bool CanChop() => _currentHits < _maxHits;

        public bool CanInteract() => CanChop();

        public void Chop()
        {
            if (!CanChop()) return;

            _currentHits++;

            if (_currentHits >= _maxHits)
            {
                if (_desativeObject != null)
                    _desativeObject.SetActive(false);

                OnBecameUnusable?.Invoke();
            }
        }
    }
}