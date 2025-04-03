using UnityEngine;

[CreateAssetMenu(fileName = "NewLine", menuName = "Scriptable Objects/DialogueLineData")]
public class DialogueLineData : ScriptableObject
{
    [TextArea] public string text;
    public int speakerIndex;  // 0 pour le personnage 1, 1 pour le personnage 2, etc.
    public Sprite speakerSprite;
    public SpeakerEffectType effect;  // Effet à appliquer pendant qu'ils parlent

    public DialogueLineData(string text, int speakerIndex, Sprite speakerSprite, SpeakerEffectType effect = SpeakerEffectType.None)
    {
        this.text = text;
        this.speakerIndex = speakerIndex;
        this.speakerSprite = speakerSprite;
        this.effect = effect;
    }
}

public enum SpeakerEffectType
{
    None,
    Tremble,
    Jump,
    Dim
}