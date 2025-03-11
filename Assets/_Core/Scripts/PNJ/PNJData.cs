using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoonlitMixes.Potion;

namespace MoonlitMixes.AI.StateMachine
{
    public class PNJData
    {
        public GameObject PNJGameObject { get; }
        public NavMeshAgent Agent { get; }
        public Animator Animator { get; }
        public List<Transform> Waypoints { get; }
        public float DialogueDuration { get; }
        public int CurrentWaypointIndex { get; set; } = 0;

        public PotionListData RequestPotionList { get; }

        public PNJData(GameObject pnj, NavMeshAgent agent, Animator animator, List<Transform> waypoints, float dialogueDuration, PotionListData potionList)
        {
            PNJGameObject = pnj;
            Agent = agent;
            Animator = animator;
            Waypoints = waypoints;
            DialogueDuration = dialogueDuration;
            RequestPotionList = potionList;
        }
    }
}