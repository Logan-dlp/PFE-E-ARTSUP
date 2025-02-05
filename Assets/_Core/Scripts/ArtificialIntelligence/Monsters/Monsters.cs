using System;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;

    public enum MonstersComportement
    {
        Passive,
        Aggressive,
    }
    
    public class Monsters : MonoBehaviour
    {
        [SerializeField] private TargetTest _playerReference;
        [SerializeField] private MonstersComportement _comportement;
        
        private IMonstersState _currentMonstersState;
        private MonstersData _monstersData;

        private void Start()
        {
            _monstersData = new MonstersData()
            {
                MonsterGameObject = gameObject,
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
            if (_comportement == MonstersComportement.Aggressive)
            {
                if (Vector3.Distance(_playerReference.transform.position, _monstersData.InitialPosition) < _monstersData.AttackRadius)
                {
                    _monstersData.PlayerGameObject = _playerReference;
                }
            }
            
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

        public void Attacked(TargetTest target)
        {
            if (_comportement == MonstersComportement.Passive)
            {
                _monstersData.PlayerGameObject = target;
            }
        }
    }
}