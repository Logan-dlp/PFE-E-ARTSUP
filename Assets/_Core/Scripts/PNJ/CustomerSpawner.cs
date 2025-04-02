using MoonlitMixes.Player;
using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ
{
    public class CustomerSpawner : MonoBehaviour
    {
        public event System.Action OnAllCustomersGone;

        [SerializeField] private List<GameObject> _pnjPrefabs;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _timeBetweenSpawns = 2f;
        [SerializeField] private int _maxCustomers = 3;
        [SerializeField] private PlayerMovement _playerMovement;

        private int _currentPNJIndex = 0;
        private int _pnjsDespawned = 0;
        private int _activeCustomers = 0;
        private bool _isSpawning = false;

        public void StartSpawning()
        {
            if (!_isSpawning)
            {
                _isSpawning = true;
                _currentPNJIndex = 0;
                _activeCustomers = 0;
                _playerMovement.BlockMovement(true);
                StartCoroutine(SpawnCustomers());
            }
        }

        private IEnumerator<WaitForSeconds> SpawnCustomers()
        {
            while (_currentPNJIndex < _pnjPrefabs.Count && _activeCustomers < _maxCustomers)
            {
                SpawnNextPNJ();

                while (_activeCustomers > 0)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(_timeBetweenSpawns);
            }
        }

        private void SpawnNextPNJ()
        {
            if (_currentPNJIndex < _pnjPrefabs.Count)
            {
                GameObject pnj = _pnjPrefabs[_currentPNJIndex];
                pnj.SetActive(true);
                pnj.transform.position = _spawnPoint.position;

                PNJStateMachine pnjStateMachine = pnj.GetComponent<PNJStateMachine>();
                if (pnjStateMachine != null)
                {
                    pnjStateMachine.TransitionToState(0);
                    pnjStateMachine.OnDespawn += OnPNJDespawned;
                }

                _activeCustomers++;
                _currentPNJIndex++;
            }
        }

        private void OnPNJDespawned()
        {
            _activeCustomers--;
            _pnjsDespawned++;

            if (_pnjsDespawned >= _maxCustomers)
            {
                NotifyAllCustomersGone();
            }
        }

        private void NotifyAllCustomersGone()
        {
            _playerMovement.BlockMovement(false);

            _isSpawning = false;
        }
    }
}