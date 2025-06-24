using MoonlitMixes.AI.PNJ.StateMachine;
using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.Datas;
using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Events;
using MoonlitMixes.Player;
using System;
using System.Collections;
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
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

        private readonly Dictionary<int, List<int>> _dayToPNJIndices = new Dictionary<int, List<int>>
        {
            { 0, new List<int> { 0, 1 } },         
            { 1, new List<int> { 2, 3, 4 } },       
            { 2, new List<int> { 5, 6, 7, 8 } }   
        };

        private List<int> _currentDayPNJIndices;
        private int _currentPNJIndex = 0;
        private bool _isSpawning = false;

        public void StartSpawning()
        {
            if (_isSpawning) return;

            _isSpawning = true;
            _currentPNJIndex = 0;
            _playerMovement.BlockMovement(true);

            int currentDay = _dayNightCycleInfo.ActualDay;
            if (_dayToPNJIndices.TryGetValue(currentDay, out _currentDayPNJIndices))
            {
                SpawnNextPNJ();
            }
            else
            {
                Debug.LogWarning($"Aucun PNJ assigné pour le jour {currentDay}");
                NotifyAllCustomersGone();
            }
        }

        private void SpawnNextPNJ()
        {
            if (_currentPNJIndex < _currentDayPNJIndices.Count)
            {
                StartCoroutine(PlayBellAndSpawn());
            }
        }

        private IEnumerator PlayBellAndSpawn()
        {
            OnClientBellRequested?.Invoke();
            yield return new WaitForSeconds(0.5f);

            int pnjIndex = _currentDayPNJIndices[_currentPNJIndex];
            if (pnjIndex < 0 || pnjIndex >= _pnjPrefabs.Count)
            {
                Debug.LogError($"PNJ index {pnjIndex} invalide dans la liste _pnjPrefabs");
                yield break;
            }

            GameObject pnjInstance = _pnjPrefabs[pnjIndex];
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

        private IEnumerator WaitAndSpawnNext()
        {
            yield return new WaitForSeconds(_timeBetweenSpawns);
            SpawnNextPNJ();
        }

        private void OnPNJDespawned()
        {
            if (_currentPNJIndex < _currentDayPNJIndices.Count)
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
            _playerMovement.BlockMovement(false);
            FindFirstObjectByType<ChangeUITimePhase>().IncreaseTimePhase();
            _isSpawning = false;
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
    }
}