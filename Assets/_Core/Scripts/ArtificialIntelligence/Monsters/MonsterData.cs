using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI.StateMachine
{
    public class MonsterData
    {
        public GameObject MonsterGameObject { get; set; }
        public Animator Animator { get; set; }
        public NavMeshAgent NavMeshAgent { get; set; }
        public GameObject PlayerReference { get; set; }
        public Vector3 InitialPosition { get; set; }
        
        public float StopDistanceToAttack { get; set; }
        public float AttackRadius { get; set; }
        public float DetectionStop { get; set; }
        
        public bool FinishedAttacking { get; set; }
    }
}