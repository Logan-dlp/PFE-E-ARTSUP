using MoonlitMixes.Potion;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PNJData
{
    public GameObject PNJGameObject { get; }
    public NavMeshAgent Agent { get; }
    public Animator Animator { get; }
    public List<Transform> Waypoints { get; }
    public float DialogueDuration { get; }
    public int CurrentWaypointIndex { get; set; } = 0;

    public PotionListData _requestPotionList;
    public PotionResult _requestPotion;

    public PNJData(GameObject pnj, NavMeshAgent agent, Animator animator, List<Transform> waypoints, float dialogueDuration, PotionListData potionList)
    {
        PNJGameObject = pnj;
        Agent = agent;
        Animator = animator;
        Waypoints = waypoints;
        DialogueDuration = dialogueDuration;
        _requestPotionList = potionList;
    }
}
