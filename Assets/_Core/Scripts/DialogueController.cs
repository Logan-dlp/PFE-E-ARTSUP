using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogueScriptable;
    [SerializeField] private TMP_Text _textPlayer;
    [SerializeField] private TMP_Text _textNPC;
    [SerializeField] private Image _imagePlayer;
    [SerializeField] private Image _imageNPC;
    [SerializeField] private float letterDelay = .05f;

    
    private int dialogueIndex = 0;

    public void NextDialogue()
    {
        if(dialogueIndex != dialogueScriptable.dialogueSection.Length)
        {
            if(dialogueScriptable.dialogueSection[dialogueIndex].isPlayer)
            {
                WriteText(dialogueScriptable.dialogueSection[dialogueIndex].dialogue, _textPlayer);
                _imagePlayer.sprite = dialogueScriptable.dialogueSection[dialogueIndex].sprite;
                _imagePlayer.preserveAspect = true;
                dialogueIndex++;
            }
            else
            {
                WriteText(dialogueScriptable.dialogueSection[dialogueIndex].dialogue, _textNPC);
                _imageNPC.sprite = dialogueScriptable.dialogueSection[dialogueIndex].sprite;
                _imageNPC.preserveAspect = true;
                dialogueIndex++;
            }
        }
        else if(dialogueIndex == dialogueScriptable.dialogueSection.Length)
        {

        }
    }

    public void WriteText(string text, TMP_Text textBox)
    {
        textBox.maxVisibleCharacters = 0; //Resets whenever this is re-used.
        textBox.text = text;
        StartCoroutine(TypeText(textBox));
    }

    IEnumerator TypeText(TMP_Text textBox)
    {
        for (int i = 0; i < textBox.text.Length; i++)
        {
            textBox.maxVisibleCharacters++;
            yield return new WaitForSeconds(letterDelay);
        }
    }
}
