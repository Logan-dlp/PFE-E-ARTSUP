using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;
    
    public class Slime : AMonsters
    {
        private void Start()
        {
            base._data = new SlimeData()
            {
                SlimeGameObject = gameObject,
                NavMeshAgent = GetComponent<NavMeshAgent>(),
                InAttack = false,
                InitialPosition = transform.position,
                AttackRadius = 5,
            };
            
            TransitionTo(new StateSlimeIdle());
        }
    }
}