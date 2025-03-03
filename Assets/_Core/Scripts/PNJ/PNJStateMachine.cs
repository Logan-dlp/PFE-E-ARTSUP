using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoonlitMixes.AI.StateMachine;
using MoonlitMixes.AI.StateMachine.States;

namespace MoonlitMixes.AI
{
    public class PNJStateMachine : MonoBehaviour
    {
        [SerializeField] private Transform waypointsParent;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private float dialogueDuration = 3f;

        private PNJData _pnjData;
        private IPNJState _currentState;
        private List<IPNJState> _states;

        private void Start()
        {
            List<Transform> waypoints = new List<Transform>();
            foreach (Transform child in waypointsParent)
            {
                waypoints.Add(child);
            }

            _pnjData = new PNJData(gameObject, agent, animator, waypoints, dialogueDuration);

            _states = new List<IPNJState>
            {
                new SpawnState(),
                new MoveToEndState(),
                new DialogueState(),
                new MoveToStartState(),
                new DespawnState()
            };

            TransitionToState(0);
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
    }
}