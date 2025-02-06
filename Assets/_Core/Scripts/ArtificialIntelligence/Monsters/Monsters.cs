using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;
    
    public class Monsters : MonoBehaviour
    {
        [SerializeField] private MonstersComportement _comportement;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _detectionStop;

        private TargetTest _playerReference;
        private IMonstersState _currentMonstersState;
        private MonstersData _monstersData;

        private void Start()
        {
            _playerReference = FindFirstObjectByType<TargetTest>();
            
            _monstersData = new MonstersData()
            {
                MonsterGameObject = gameObject,
                Animator = GetComponent<Animator>(),
                NavMeshAgent = GetComponent<NavMeshAgent>(),
                PlayerReference = null,
                InitialPosition = transform.position,
                
                AttackRadius = _attackRadius,
                DetectionStop = _detectionStop,
                
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
                    _monstersData.PlayerReference = _playerReference;
                }
            }
            
            IMonstersState nextMonstersState = _currentMonstersState?.Update(_monstersData);
            if (nextMonstersState != null)
            {
                TransitionTo(nextMonstersState);
            }
        }

        private void TransitionTo(IMonstersState nextMonstersState)
        {
            _currentMonstersState?.Exit(_monstersData);
            _currentMonstersState = nextMonstersState;
            _currentMonstersState?.Enter(_monstersData);
        }
        
        private void OnDrawGizmos()
        {
            if (_monstersData != null)
            {
                Gizmos.color = new Color(255, 0, 0, .5f);
                Gizmos.DrawSphere(_monstersData.InitialPosition, _detectionStop);
                
                if (_comportement == MonstersComportement.Aggressive)
                {
                    Gizmos.color = new Color(0, 0, 255, .5f);
                    Gizmos.DrawSphere(_monstersData.InitialPosition, _attackRadius);
                }
            }
            else
            {
                Gizmos.color = new Color(255, 0, 0, .5f);
                Gizmos.DrawSphere(transform.position, _detectionStop);

                if (_comportement == MonstersComportement.Aggressive)
                {
                    Gizmos.color = new Color(0, 0, 255, .5f);
                    Gizmos.DrawSphere(transform.position, _attackRadius);
                }
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
                _monstersData.PlayerReference = target;
            }
        }
    }
}