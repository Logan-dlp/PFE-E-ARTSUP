using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.Datas;
using MoonlitMixes.Potion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI.PNJ.StateMachine
{
    public class PNJStateMachine : MonoBehaviour
    {
        public event System.Action OnDespawn;

        [Header("Configuration")]
        [SerializeField] private Transform _waypointsParent;
        [SerializeField] private float _dialogueDuration = 3f;
        [SerializeField] private PotionResult[] _requestPotionArray;

        [Header("Dialogue Settings")]
        [SerializeField] private DialogueData _beginDialogueData;
        [SerializeField] private DialogueData _successDialogueData;
        [SerializeField] private DialogueData _failureDialogueData;
        [SerializeField] private DialogueData _noPotionDialogueData;

        private NavMeshAgent _agent;
        private Animator _animator;
        private PNJData _pnjData;
        private IPNJState _currentState;
        private List<PotionResult> _potionValidList = new();

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            DisablePNJ();
        }

        public void Initialize()
        {
            List<Transform> waypoints = new();
            foreach (Transform child in _waypointsParent)
            {
                waypoints.Add(child);
            }

            _pnjData = new PNJData
            {
                pnjGameObject = gameObject,
                agent = _agent,
                animator = _animator,
                waypoints = waypoints,
                dialogueDuration = _dialogueDuration,
                requestPotionArray = _requestPotionArray,
                potionValidList = _potionValidList,
                beginDialogueData = _beginDialogueData,
                failureDialogueData = _failureDialogueData,
                noPotionDialogueData = _noPotionDialogueData,
                successDialogueData = _successDialogueData,
                OnDespawn = InvokeOnDespawn,
                OnPotionSelected = SetSelectedPotion,
            };

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

        public void SetSelectedPotion(PotionResult potionResultSelected)
        {
            IEnumerable<PotionResult> result = _requestPotionArray.Except(_potionValidList);

            bool isPotionValid = false;

            foreach (PotionResult potionResultItem in result)
            {
                if (potionResultItem == potionResultSelected)
                {
                    isPotionValid = true;
                    break;
                }
            }

            if (isPotionValid)
            {
                _potionValidList.Add(potionResultSelected);
            }
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