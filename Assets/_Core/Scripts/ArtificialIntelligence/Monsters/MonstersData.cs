using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI.StateMachine
{
    public class MonstersData
    {
        public GameObject SlimeGameObject { get; set; }
        public Animator Animator { get; set; }
        public NavMeshAgent NavMeshAgent { get; set; }
        public TargetTest PlayerGameObject { get; set; }
        public Vector3 InitialPosition { get; set; }
        public float AttackRadius { get; set; }
        
        public bool Attacking { get; set; }
    }
}