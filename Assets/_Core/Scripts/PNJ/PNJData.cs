using MoonlitMixes.Datas;
using MoonlitMixes.Potion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class PNJData
{
    [SerializeField] private GameObject pnjGameObject;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float dialogueDuration;

    [SerializeField] private PotionResult[] requestPotionArray;
    [SerializeField] private List<PotionResult> potionValidList;

    [SerializeField] private DialogueData beginDialogueData;
    [SerializeField] private DialogueData secondbeginDialogueData;
    [SerializeField] private DialogueData successDialogueData;
    [SerializeField] private DialogueData failureDialogueData;
    [SerializeField] private DialogueData noPotionDialogueData;

    [SerializeField] private PotionResult selectedPotionResult;
    [SerializeField] private int currentPotionIndex;
    [SerializeField] private int failedAttempt;

    public Action OnDespawn { get; set; }
    public Action<PotionResult> OnPotionSelected { get; set; }

    public GameObject PnjGameObject
    {
        get => pnjGameObject;
        set => pnjGameObject = value;
    }

    public NavMeshAgent Agent
    {
        get => agent;
        set => agent = value;
    }

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public List<Transform> Waypoints
    {
        get => waypoints;
        set => waypoints = value;
    }

    public float DialogueDuration
    {
        get => dialogueDuration;
        set => dialogueDuration = value;
    }

    public PotionResult[] RequestPotionArray
    {
        get => requestPotionArray;
        set => requestPotionArray = value;
    }

    public List<PotionResult> PotionValidList
    {
        get => potionValidList;
        set => potionValidList = value;
    }

    public DialogueData BeginDialogueData
    {
        get => beginDialogueData;
        set => beginDialogueData = value;
    }

    public DialogueData SecondBeginDialogueData
    {
        get => secondbeginDialogueData;
        set => secondbeginDialogueData = value;
    }

    public DialogueData SuccessDialogueData
    {
        get => successDialogueData;
        set => successDialogueData = value;
    }

    public DialogueData FailureDialogueData
    {
        get => failureDialogueData;
        set => failureDialogueData = value;
    }

    public DialogueData NoPotionDialogueData
    {
        get => noPotionDialogueData;
        set => noPotionDialogueData = value;
    }

    public PotionResult SelectedPotionResult
    {
        get => selectedPotionResult;
        set => selectedPotionResult = value;
    }

    public int CurrentPotionIndex
    {
        get => currentPotionIndex;
        set => currentPotionIndex = value;
    }

    public int FailedAttempt
    {
        get => failedAttempt;
        set => failedAttempt = value;
    }
}