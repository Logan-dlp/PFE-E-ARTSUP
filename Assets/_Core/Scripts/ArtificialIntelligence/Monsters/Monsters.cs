using System;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;
    
    public class Monsters : MonoBehaviour
    {
        private IMonstersState _currentMonstersState;
        private MonstersData _monstersData;

        private void Start()
        {
            _monstersData = new MonstersData()
            {
                SlimeGameObject = gameObject,
                Animator = GetComponent<Animator>(),
                NavMeshAgent = GetComponent<NavMeshAgent>(),
                InitialPosition = transform.position,
                AttackRadius = 5,
                Attacking = false,
            };
            
            TransitionTo(new MonstersStateSlimeIdle());
        }

        private void Update()
        {
            IMonstersState nextMonstersState = _currentMonstersState?.Update(_monstersData);
            if (nextMonstersState != null)
            {
                TransitionTo(nextMonstersState);
            }
        }

        protected void TransitionTo(IMonstersState nextMonstersState)
        {
            _currentMonstersState?.Exit(_monstersData);
            _currentMonstersState = nextMonstersState;
            _currentMonstersState?.Enter(_monstersData);
        }
        
        private void OnDrawGizmos()
        {
            if (_monstersData != null)
            {
                Gizmos.color = new Color(255, 255, 255, .5f);
                Gizmos.DrawSphere(_monstersData.InitialPosition, _monstersData.AttackRadius);
                
                Gizmos.color = new Color(255, 0, 0, .5f);
                Gizmos.DrawSphere(_monstersData.InitialPosition, _monstersData.AttackRadius * 1.5f);
            }
        }
        
        public void FinishPunch()
        {
            _monstersData.Attacking = true;
        }
    }
}