using MoonlitMixes.AI.PNJ.StateMachine;
using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.Datas;
using MoonlitMixes.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ.Spawner
{
    public class CustomerSpawner : MonoBehaviour
    {
        public static event Action OnClientBellRequested;
        public static event Action OnStartSpawningRequested;

        [SerializeField] private List<GameObject> _pnjPrefabs;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _timeBetweenSpawns = 2f;
        [SerializeField] private int _maxCustomers = 3;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

        private int _currentPNJIndex = 0;
        private bool _isSpawning = false;

        public void StartSpawning()
        {
            if (!_isSpawning)
            {
                _isSpawning = true;
                _currentPNJIndex = 0;
                _playerMovement.BlockMovement(true);


                SpawnNextPNJ();
            }
        }

        private void SpawnNextPNJ()
        {
            if (_currentPNJIndex < _pnjPrefabs.Count)
            {
                StartCoroutine(PlayBellAndSpawn());
            }
        }

        private IEnumerator<WaitForSeconds> PlayBellAndSpawn()
        {
            OnClientBellRequested?.Invoke();
            yield return new WaitForSeconds(0.5f);

            GameObject pnjInstance = _pnjPrefabs[_currentPNJIndex];
            pnjInstance.transform.position = _spawnPoint.position;
            pnjInstance.SetActive(true);

            PNJStateMachine pnjStateMachine = pnjInstance.GetComponent<PNJStateMachine>();
            if (pnjStateMachine != null)
            {
                pnjStateMachine.Initialize();
                pnjStateMachine.SetState(new SpawnState());
                pnjStateMachine.OnDespawn += OnPNJDespawned;
            }

            _currentPNJIndex++;
        }

        private void OnEnable()
        {
            OnStartSpawningRequested += StartSpawning;
        }

        private void OnDisable()
        {
            OnStartSpawningRequested -= StartSpawning;
        }

        public static void RequestSpawning()
        {
            OnStartSpawningRequested?.Invoke();
        }

        private IEnumerator<WaitForSeconds> WaitAndSpawnNext()
        {
            yield return new WaitForSeconds(_timeBetweenSpawns);
            SpawnNextPNJ();
        }

        private void OnPNJDespawned()
        {
            if (_currentPNJIndex < _pnjPrefabs.Count)
            {
                StartCoroutine(WaitAndSpawnNext());
            }
            else
            {
                NotifyAllCustomersGone();
            }
        }

        private void NotifyAllCustomersGone()
        {
            _dayNightCycleInfo.ActualTimePhase++;
            _playerMovement.BlockMovement(false);
            _isSpawning = false;
        }
    }
}