using MoonlitMixes.AI.PNJ.StateMachine;
using MoonlitMixes.Datas;
using MoonlitMixes.Potion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MoonlitMixes.AI.PNJ
{
    public class PNJData
    {
        public GameObject pnjGameObject;
        public NavMeshAgent agent;
        public Animator animator;
        public List<Transform> waypoints;
        public float dialogueDuration;
        public PotionResult[] requestPotionArray;
        public List<PotionResult> potionValidList;
        public PotionResult selectedPotionResult;
        public DialogueData beginDialogueData;
        public DialogueData successDialogueData;
        public DialogueData failureDialogueData;
        public DialogueData noPotionDialogueData;
        public DialogueData secondbeginDialogueData;
        public System.Action OnDespawn;
        public System.Action<PotionResult> OnPotionSelected;
    }
}