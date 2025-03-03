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
        [SerializeField] private int _attackForce;

        private GameObject _playerReference;
        private IMonsterState _currentMonsterState;
        private MonsterData _monsterData;
        
        private bool _collisionAttackActive = false;

        private void Start()
        {
            _playerReference = GameObject.FindWithTag("Player");
            
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
                
                Attacking = false,
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

        public void ToggleCollisionAttack()
        {
            _collisionAttackActive = !_collisionAttackActive;
        }
        
        public void FinishAnimationAttack()
        {
            _monsterData.Attacking = true;
        }

        public void Attacked(GameObject player)
        {
            if (_comportement == MonsterComportement.Passive)
            {
                _monsterData.PlayerReference = player;
            }
        }

        public void CollisionEnter(Collider collider)
        {
            if (_collisionAttackActive)
            {
                if (collider.TryGetComponent<PlayerLife>(out PlayerLife player))
                {
                    player.AddDamage(player.transform.position - transform.position, _attackForce);
                }
            }
        }
    }
}