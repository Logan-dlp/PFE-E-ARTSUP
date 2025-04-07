using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using MoonlitMixes.AI.PNJ.StateMachine.States;

namespace MoonlitMixes.AI.PNJ
{
    public class PNJStateMachine : MonoBehaviour
    {
        public event System.Action OnDespawn;
        public string SelectedPotionName { get; private set; }
        public int FailedAttempts => _failedAttempts;

        [SerializeField] private Transform _waypointsParent;
        [SerializeField] private float _dialogueDuration = 3f;
        [SerializeField] private float _spawnDelay = 2f;
        [SerializeField] private PotionListData _potionList;

        [Header("Dialogue system :")]
        [SerializeField] public DialogueData _beginDialogueData;
        [SerializeField] public DialogueData _successDialogueData;
        [SerializeField] public DialogueData _failureDialogueData;
        [SerializeField] public DialogueData _noPotionDialogueData;

        private NavMeshAgent _agent;
        private Animator _animator;
        private PNJData _pnjData;
        private IPNJState _currentState;
        private List<IPNJState> _states;
        private int _failedAttempts = 0;

        public void IncrementFailedAttempts()
        {
            _failedAttempts++;
        }

        public void ResetFailedAttempts()
        {
            _failedAttempts = 0;
        }

        public void InvokeOnDespawn()
        {
            OnDespawn?.Invoke();
        }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            DisablePNJ();
        }

        private void Start()
        {
            List<Transform> waypoints = new List<Transform>();
            foreach (Transform child in _waypointsParent)
            {
                waypoints.Add(child);
            }

            _pnjData = new PNJData(gameObject, _agent, _animator, waypoints, _dialogueDuration, _potionList);

            _states = new List<IPNJState>
            {
                new SpawnState(),
                new MoveToEndState(),
                new DialogueState(),
                new ChoosePotionState(),
                new ChoiceDialogueState(),
                new MoveToStartState(),
                new DespawnState()
            };
        }

        private void Update()
        {
            _currentState?.UpdateState(_pnjData, this);
        }

        public void TransitionToState(int stateIndex)
        {
            if (_currentState == _states[stateIndex]) return;

            if (stateIndex < 0 || stateIndex >= _states.Count) return;

            _currentState?.ExitState(_pnjData);
            _currentState = _states[stateIndex];
            _currentState.EnterState(_pnjData);
        }

        public void NextState()
        {
            int nextIndex = _states.IndexOf(_currentState) + 1;
            if (nextIndex < _states.Count)
            {
                TransitionToState(nextIndex);
            }
        }

        private void DisablePNJ()
        {
            _agent.enabled = false;
            _animator.enabled = false;
        }

        public void SetSelectedPotion(string potionName)
        {
            SelectedPotionName = potionName;
        }
    }
}