using MoonlitMixes.AI.StateMachine.States;
using UnityEngine;

namespace MoonlitMixes.AI
{
    using StateMachine;
    
    public abstract class AMonsters : MonoBehaviour
    {
        protected IState _currentState;
        protected AStateMachineData _data;

        private void Update()
        {
            IState nextState = _currentState?.Update(_data);
            if (nextState != null)
            {
                TransitionTo(nextState);
            }
        }

        protected void TransitionTo(IState nextState)
        {
            _currentState?.Exit(_data);
            _currentState = nextState;
            _currentState?.Enter(_data);
        }
    }
}