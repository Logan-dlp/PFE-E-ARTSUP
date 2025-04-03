using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueLineData[] lines;  // Liste des lignes de dialogue dans un même dialogue
}