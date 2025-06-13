using UnityEngine;

namespace MoonlitMixes.ExplorationTools
{
    public class TreeHealth : MonoBehaviour
    {
        [Header("Number of times the bark can be recovered")]
        [SerializeField] private int _maxHits = 2;
        private int _currentHits = 0;

        [Header("Object to deactivate when the tree is cut")]
        [SerializeField] private GameObject _desativeObject;

        public bool CanChop()
        {
            return _currentHits < _maxHits;
        }

        public void Chop()
        {
            if (CanChop())
            {
                _currentHits++;

                if (_currentHits >= _maxHits)
                {
                    if (_desativeObject != null)
                    {
                        _desativeObject.SetActive(false);
                    }
                }
            }
        }
    }
}