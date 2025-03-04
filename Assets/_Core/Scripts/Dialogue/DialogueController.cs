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

    private bool _textIsWritten = false;
    private bool _skipText = false;
    private int _index;
    private int dialogueIndex = 0;

    public void NextDialogue()
    {
        if (_textIsWritten == true)
        {
            _skipText = true;
            return;
        }

        if (dialogueIndex != dialogueScriptable.dialogueSection.Length)
        {
            if (dialogueScriptable.dialogueSection[dialogueIndex].isPlayer)
            {
                _imagePlayer.sprite = dialogueScriptable.dialogueSection[dialogueIndex].sprite;
                _imagePlayer.preserveAspect = true;
                WriteText(dialogueScriptable.dialogueSection[dialogueIndex].dialogue, _textPlayer);
                dialogueIndex++;
            }
            else
            {
                _imageNPC.sprite = dialogueScriptable.dialogueSection[dialogueIndex].sprite;
                _imageNPC.preserveAspect = true;
                WriteText(dialogueScriptable.dialogueSection[dialogueIndex].dialogue, _textNPC);
                dialogueIndex++;
            }
        }
        else if (dialogueIndex == dialogueScriptable.dialogueSection.Length)
        {

        }
    }

    private void WriteText(string text, TMP_Text textBox)
    {
        textBox.maxVisibleCharacters = 0; //Resets whenever this is re-used.
        textBox.text = text;
        _textIsWritten = true;
        StartCoroutine(TypeText(textBox));
    }

    private IEnumerator TypeText(TMP_Text textBox)
    {
        for (_index = 0; _index < textBox.text.Length; _index++)
        {
            textBox.maxVisibleCharacters++;
            yield return new WaitForSeconds(letterDelay);
            if (_skipText == true)
            {
                textBox.maxVisibleCharacters = textBox.text.Length;
                _skipText = false;
                break;
            }
        }
        _textIsWritten = false;
    }
}