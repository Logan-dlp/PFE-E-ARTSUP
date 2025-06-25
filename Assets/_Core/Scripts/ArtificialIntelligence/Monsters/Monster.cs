using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using MoonlitMixes.Item;
using FMODUnity;
using FMOD.Studio;

namespace MoonlitMixes.AI
{
    using StateMachine;
    using StateMachine.States;
    using Health;

    public class Monster : MonoBehaviour
    {
        [Header("Comportement")]
        [SerializeField] private MonsterComportement _comportement;

        [Header("Combat Settings")]
        [SerializeField] private float _speedAttack;
        [SerializeField] private float _stopDistanceToAttack;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _detectionStop;
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackForce = 2;
        [SerializeField] private float _attackDuration = .45f;

        [SerializeField] private EventReference _soundAttackBat;
        [SerializeField] private EventReference _soundMoveBat;
        [SerializeField] private EventReference _soundAttackGolem;
        [SerializeField] private EventReference _soundMoveGolem;
        [SerializeField] private EventReference _soundAttackSlime;
        [SerializeField] private EventReference _soundMoveSlime;
        [SerializeField] private EventReference _soundAttackWillowraith;
        [SerializeField] private EventReference _soundMoveWillowraith;
        [SerializeField] private EventReference _soundDeathSmallEnemy;
        [SerializeField] private EventReference _soundDeathBigEnemy;

        //[SerializeField] private GameObject _itemDrop;
        
        private GameObject _playerReference;
        private IMonsterState _currentMonsterState;
        private MonsterData _monsterData;
        private EnemyHealth _enemyHealth;
        private Rigidbody _rigidbody;
        private Vector3 _attackRayOffset = new(0, .5f, 0);
        private Animator _animator;
        private bool _havePlayer = true;


        private void Start()
        {
            _animator = GetComponent<Animator>();
            _enemyHealth = GetComponent<EnemyHealth>();
            _rigidbody = GetComponent<Rigidbody>();

            _playerReference = FindFirstObjectByType<PlayerHealth>()?.gameObject;

            _monsterData = new MonsterData()
            {
                MonsterGameObject = gameObject,
                Animator = _animator,
                NavMeshAgent = GetComponent<NavMeshAgent>(),
                PlayerReference = _playerReference,
                InitialPosition = transform.position,
                StopDistanceToAttack = _stopDistanceToAttack,
                AttackRadius = _comportement == MonsterComportement.Aggressive ? _attackRadius : _detectionStop,
                DetectionStop = _detectionStop,
                FinishedAttacking = false,
                BaseSpeed = GetComponent<NavMeshAgent>().speed,
                AttackSpeed = _speedAttack,
            };
            TransitionTo(new MonsterStateIdle());
        }

        private void Update()
        {
            if (_comportement == MonsterComportement.Aggressive
                && Vector3.Distance(_playerReference.transform.position, _monsterData.InitialPosition) < _monsterData.DetectionStop)
            {
                _monsterData.PlayerReference = _playerReference;
            }

            IMonsterState nextMonsterState = _currentMonsterState?.Update(_monsterData);
            if (nextMonsterState != null)
            {
                TransitionTo(nextMonsterState);
            }
            if (Vector3.Distance(this.transform.position, _playerReference.transform.position) < 13) _havePlayer = true;
            else _havePlayer = false;
        }
        public void DropItem(GameObject item)
        {
            Instantiate(item, this.transform.position, Quaternion.identity);
        }
        private void TransitionTo(IMonsterState nextMonsterState)
        {
            _currentMonsterState?.Exit(_monsterData);
            _currentMonsterState = nextMonsterState;
            _currentMonsterState?.Enter(_monsterData);
        }

        private void OnDrawGizmos()
        {
            Vector3 center = _monsterData != null ? _monsterData.InitialPosition : transform.position;

            Gizmos.color = new Color(255, 0, 0, .5f);
            Gizmos.DrawSphere(center, _detectionStop);

            if (_comportement == MonsterComportement.Aggressive)
            {
                Gizmos.color = new Color(0, 0, 255, .5f);
                Gizmos.DrawSphere(center, _attackRadius);
            }
        }

        public void Attack()
        {
            if (Physics.Raycast(transform.position + _attackRayOffset, transform.forward, out RaycastHit hit, _monsterData.StopDistanceToAttack))
            {
                if (hit.transform.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.AddDamage(_attackDamage, transform.forward, _attackForce, _attackDuration);
                }
            }
        }
        public void FinishAnimationAttack()
        {
            _monsterData.FinishedAttacking = true;
        }

        public void Damage(GameObject player, int damage, Vector3 direction, float force)
        {
            FindFirstObjectByType<PlayerHealth>().EnterFightMode();

            if (_comportement == MonsterComportement.Passive)
                _monsterData.PlayerReference = player;

            _enemyHealth.TakeDamage(damage);
            StartCoroutine(Knockback(direction, force));

            if (_enemyHealth._currentHealth <= 0)
            {
                _animator.SetTrigger("Death");
                //Instantiate(_itemDrop,this.transform.position,Quaternion.identity);
                //player.GetComponent<UseTools>().CollectItems(GetComponent<ItemListSource>());
            }
        }

        private IEnumerator Knockback(Vector3 direction, float force)
        {
            _monsterData.NavMeshAgent.enabled = false;
            _rigidbody.isKinematic = false;
            _rigidbody.linearVelocity = direction * force;

            yield return new WaitForSeconds(.5f);

            _monsterData.NavMeshAgent.enabled = true;
            _rigidbody.isKinematic = true;
        }

        private void Death()
        {
            Destroy(gameObject);
        }

        public void PlaySound_Attack_Bat() => PlayFMOD(_soundAttackBat);
        public void PlaySound_Move_Bat() => PlayFMOD(_soundMoveBat);
        public void PlaySound_Attack_Golem() => PlayFMOD(_soundAttackGolem);
        public void PlaySound_Move_Golem() => PlayFMOD(_soundMoveGolem);
        public void PlaySound_Attack_Slime() => PlayFMOD(_soundAttackSlime);
        public void PlaySound_Move_Slime() => PlayFMOD(_soundMoveSlime);
        public void PlaySound_Attack_Willowraith() => PlayFMOD(_soundAttackWillowraith);
        public void PlaySound_Move_Willowraith() => PlayFMOD(_soundMoveWillowraith);
        public void PlaySound_Death_SmallEnemy() => PlayFMOD(_soundDeathSmallEnemy);
        public void PlaySound_Death_BigEnemy() => PlayFMOD(_soundDeathBigEnemy);

        private void PlayFMOD(EventReference sound)
        {
            if (_havePlayer)
            {
                if (sound.IsNull) { Debug.LogWarning("son = null"); return; }
                Debug.Log("play the sound" + sound);
                RuntimeManager.PlayOneShot(sound, transform.position);
            }
        }
    }
}