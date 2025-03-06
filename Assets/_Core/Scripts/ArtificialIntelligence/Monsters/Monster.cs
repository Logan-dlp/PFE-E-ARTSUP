using MoonlitMixes.Player;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;
    
    public class Monster : MonoBehaviour
    {
        [SerializeField] private MonsterComportement _comportement;
        [SerializeField] private float _stopDistanceToAttack;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _detectionStop;

        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackForce = 2;
        [SerializeField] private float _attackDuration = .45f;

        private GameObject _playerReference;
        private IMonsterState _currentMonsterState;
        private MonsterData _monsterData;
        private Vector3 _attackRayOffset = new(0, .5f, 0);

        private void Start()
        {
            _playerReference = FindFirstObjectByType<PlayerLife>().gameObject;
            
            _monsterData = new MonsterData()
            {
                MonsterGameObject = gameObject,
                Animator = GetComponent<Animator>(),
                NavMeshAgent = GetComponent<NavMeshAgent>(),
                PlayerReference = null,
                InitialPosition = transform.position,
                
                StopDistanceToAttack = _stopDistanceToAttack,
                AttackRadius = _attackRadius,
                DetectionStop = _detectionStop,
                
                FinishedAttacking = false,
            };
            
            TransitionTo(new MonsterStateIdle());
        }

        private void Update()
        {
            if (_comportement == MonsterComportement.Aggressive
                && Vector3.Distance(_playerReference.transform.position, _monsterData.InitialPosition) < _monsterData.AttackRadius)
            {
                _monsterData.PlayerReference = _playerReference;
            }
            
            IMonsterState nextMonsterState = _currentMonsterState?.Update(_monsterData);
            if (nextMonsterState != null)
            {
                TransitionTo(nextMonsterState);
            }
        }

        private void TransitionTo(IMonsterState nextMonsterState)
        {
            _currentMonsterState?.Exit(_monsterData);
            _currentMonsterState = nextMonsterState;
            _currentMonsterState?.Enter(_monsterData);
        }
        
        private void OnDrawGizmos()
        {
            if (_monsterData != null)
            {
                Gizmos.color = new Color(255, 0, 0, .5f);
                Gizmos.DrawSphere(_monsterData.InitialPosition, _detectionStop);
                
                if (_comportement == MonsterComportement.Aggressive)
                {
                    Gizmos.color = new Color(0, 0, 255, .5f);
                    Gizmos.DrawSphere(_monsterData.InitialPosition, _attackRadius);
                }
            }
            else
            {
                Gizmos.color = new Color(255, 0, 0, .5f);
                Gizmos.DrawSphere(transform.position, _detectionStop);

                if (_comportement == MonsterComportement.Aggressive)
                {
                    Gizmos.color = new Color(0, 0, 255, .5f);
                    Gizmos.DrawSphere(transform.position, _attackRadius);
                }
            }
        }

        public void Attack()
        {
            if (Physics.Raycast(transform.position + _attackRayOffset, transform.forward, out RaycastHit hit, _monsterData.StopDistanceToAttack))
            {
                if (hit.transform.TryGetComponent<PlayerLife>(out PlayerLife playerLife))
                {
                    playerLife.AddDamage(_attackDamage, transform.forward, _attackForce, _attackDuration);
                }
            }
        }
        
        public void FinishAnimationAttack()
        {
            _monsterData.FinishedAttacking = true;
        }
        
        public void Damage(GameObject target)
        {
            if (_comportement == MonsterComportement.Passive)
            {
                _monsterData.PlayerReference = target;
            }
        }
    }
}