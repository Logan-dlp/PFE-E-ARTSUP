using UnityEngine;

namespace MoonlitMixes.AI.StateMachine.States
{
    public class StateSlimeAttack : IState
    {
        public void Enter(AStateMachineData data)
        {
            
        }

        public IState Update(AStateMachineData data)
        {
            SlimeData slimeData = data as SlimeData;
            
            if (slimeData.PlayerGameObject == null)
            {
                return new StateSlimeIdle();
            }
            
            return null;
        }

        public void Exit(AStateMachineData data)
        {
            
        }
    }
}
