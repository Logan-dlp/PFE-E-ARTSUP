using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;
    
    public class Slime : AMonsters
    {
        private SlimeData _slimeData;
        
        private void Start()
        {
            base._data = new SlimeData()
            {
                SlimeGameObject = gameObject,
                Animator = GetComponent<Animator>(),
                NavMeshAgent = GetComponent<NavMeshAgent>(),
                InAttack = false,
                InitialPosition = transform.position,
                AttackRadius = 5,
            };
            
            _slimeData = base._data as SlimeData;
            
            TransitionTo(new StateSlimeIdle());
        }

        private void OnDrawGizmos()
        {
            if (_slimeData != null)
            {
                Gizmos.color = new Color(255, 255, 255, .5f);
                Gizmos.DrawSphere(_slimeData.InitialPosition, _slimeData.AttackRadius);
            }
        }
    }
}