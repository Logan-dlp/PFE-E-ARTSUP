using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private DialogueLineData[] _lines;  // Liste des lignes de dialogue dans un même dialogue
    public DialogueLineData[] Lines => _lines;
}