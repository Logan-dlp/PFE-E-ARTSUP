using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using MoonlitMixes.AI.StateMachine;
using MoonlitMixes.AI.StateMachine.States;

namespace MoonlitMixes.AI
{
    public class PNJStateMachine : MonoBehaviour
    {
        [SerializeField] private Transform _waypointsParent;
        [SerializeField] private float _dialogueDuration = 3f;
        [SerializeField] private float _spawnDelay = 2f;
        [SerializeField] private PotionListData _potionList;
        [SerializeField] private DialogueController _dialogueController;
        [SerializeField] private DialogueController _dialogueControllerSuccess;
        [SerializeField] private DialogueController _dialogueControllerFailure;

        public DialogueController DialogueController => _dialogueController;
        public DialogueController DialogueControllerSuccess => _dialogueControllerSuccess;
        public DialogueController DialogueControllerFailure => _dialogueControllerFailure;
        public string SelectedPotionName { get; private set; }


        private NavMeshAgent _agent;
        private Animator _animator;
        private PNJData _pnjData;
        private IPNJState _currentState;
        private List<IPNJState> _states;
        private static bool _isShopOpen = false;
        private static List<PNJStateMachine> _spawnedPNJs = new List<PNJStateMachine>();
        private Coroutine _spawnCoroutine;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            CloseOrOpenShop.OnShopToggled += HandleShopToggle;
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

        private void OnDestroy()
        {
            CloseOrOpenShop.OnShopToggled -= HandleShopToggle;
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
            }
        }

        private void Update()
        {
            _currentState?.UpdateState(_pnjData, this);
        }

        public void TransitionToState(int stateIndex)
        {
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

        private void HandleShopToggle(bool isOpen)
        {
            _isShopOpen = isOpen;

            if (_isShopOpen)
            {
                if (!_spawnedPNJs.Contains(this))
                {
                    _spawnedPNJs.Add(this);
                    _spawnCoroutine = StartCoroutine(SpawnAfterDelay());
                }
            }
            else
            {
                if (_spawnedPNJs.Contains(this))
                {
                    _spawnedPNJs.Remove(this);
                    DisablePNJ();
                }
            }
        }

        private IEnumerator SpawnAfterDelay()
        {
            yield return new WaitForSeconds(_spawnDelay);
            EnablePNJ();
            TransitionToState(0);
        }

        private void DisablePNJ()
        {
            _agent.enabled = false;
            _animator.enabled = false;
        }

        private void EnablePNJ()

        {
            _agent.enabled = true;
            _animator.enabled = true;
        }

        public void SetSelectedPotion(string potionName)
        {
            SelectedPotionName = potionName;
        }
    }
}