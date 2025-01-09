using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class StateSlimeIdle : IState
    {
        public void Enter(AStateMachineData data)
        {
            Debug.Log($"Enter in {this.GetType().Name}");
        }

        public IState Update(AStateMachineData data)
        {
            Debug.Log($"Update in {this.GetType().Name}");
            SlimeData slimeData = data as SlimeData;
            
            return null;
        }

        public void Exit(AStateMachineData data)
        {
            Debug.Log($"Exit in {this.GetType().Name}");
        }
    }
}