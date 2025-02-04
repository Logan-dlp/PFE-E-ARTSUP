using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;
    
    public class Slime : AMonsters
    {
        [SerializeField] private TargetTest _playerTargetTest;
        private SlimeData _slimeData;
        
        private void Start()
        {
            base._data = new SlimeData()
            {
                SlimeGameObject = gameObject,
                Animator = GetComponent<Animator>(),
                NavMeshAgent = GetComponent<NavMeshAgent>(),
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
                
                Gizmos.color = new Color(255, 0, 0, .5f);
                Gizmos.DrawSphere(_slimeData.InitialPosition, _slimeData.AttackRadius * 1.5f);
            }
        }

        [ContextMenu("Player Hit Monster")]
        public void PlayerHitMonster()
        {
            _slimeData.PlayerGameObject = _playerTargetTest;
        }
    }
}