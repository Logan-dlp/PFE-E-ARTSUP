using MoonlitMixes.AI.PNJ.StateMachine.States;
using MoonlitMixes.Datas;
using MoonlitMixes.Dialogue;
using MoonlitMixes.Potion;
using System.Collections.Generic;
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
        [SerializeField] private DialogueData _secondbeginDialogueData;

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
                PnjGameObject = gameObject,
                Agent = _agent,
                Animator = _animator,
                Waypoints = waypoints,
                RequestPotionArray = _requestPotionArray,
                PotionValidList = _potionValidList,
                BeginDialogueData = _beginDialogueData,
                FailureDialogueData = _failureDialogueData,
                NoPotionDialogueData = _noPotionDialogueData,
                SuccessDialogueData = _successDialogueData,
                SecondBeginDialogueData = _secondbeginDialogueData,
                OnDespawn = InvokeOnDespawn,
                OnPotionSelected = SetSelectedPotion,
                CurrentPotionIndex = 0
            };

            SetState(new SpawnState());

            PotionChoiceController controller = FindFirstObjectByType<PotionChoiceController>();
            if (controller != null)
            {
                controller.SetRequestedPotions(_requestPotionArray);
            }
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
            _pnjData.SelectedPotionResult = potionResultSelected;
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