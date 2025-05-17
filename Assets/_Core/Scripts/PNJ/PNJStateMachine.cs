using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.Datas;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace MoonlitMixes.AI.PNJ.StateMachine
{
    public class PNJStateMachine : MonoBehaviour
    {
        public event System.Action OnDespawn;

        public string SelectedPotionName { get; private set; }
        public int FailedAttempts => _failedAttempts;
        public DialogueData BeginDialogueData => _beginDialogueData;
        public DialogueData SuccessDialogueData => _successDialogueData;
        public DialogueData FailureDialogueData => _failureDialogueData;
        public DialogueData NoPotionDialogueData => _noPotionDialogueData;

        [Header("Configuration")]
        [SerializeField] private Transform _waypointsParent;
        [SerializeField] private float _dialogueDuration = 3f;
        [SerializeField] private PotionListData _potionList;

        [Header("Dialogue Settings")]
        [SerializeField] private DialogueData _beginDialogueData;
        [SerializeField] private DialogueData _successDialogueData;
        [SerializeField] private DialogueData _failureDialogueData;
        [SerializeField] private DialogueData _noPotionDialogueData;

        private NavMeshAgent _agent;
        private Animator _animator;
        private PNJData _pnjData;
        public PNJData pnjData
        {
            get => _pnjData;
        }
        private IPNJState _currentState;
        private int _failedAttempts = 0;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            DisablePNJ();
        }

        public void Initialize()
        {
            List<Transform> waypoints = new List<Transform>();
            foreach (Transform child in _waypointsParent)
            {
                waypoints.Add(child);
            }

            _pnjData = new PNJData(gameObject, _agent, _animator, waypoints, _dialogueDuration, _potionList, this);


            SetState(new SpawnState());
        }

        private void Update()
        {
            if (_currentState != null)
            {
                IPNJState nextState = _currentState.UpdateState(_pnjData);
                if (nextState != null && nextState != _currentState)
                {
                    SetState(nextState);
                }
            }
        }

        public void SetState(IPNJState newState)
        {
            _currentState?.ExitState(_pnjData);
            _currentState = newState;
            _currentState.EnterState(_pnjData);
        }

        public void InvokeOnDespawn()
        {
            OnDespawn?.Invoke();
        }

        public void SetSelectedPotion(string potionName)
        {
            SelectedPotionName = potionName;
        }

        public void IncrementFailedAttempts()
        {
            _failedAttempts++;
        }

        public void ResetFailedAttempts()
        {
            _failedAttempts = 0;
        }

        private void DisablePNJ()
        {
            _agent.enabled = false;
            _animator.enabled = false;
        }

        public void EnablePNJ()
        {
            _agent.enabled = true;
            _animator.enabled = true;
        }
    }
}