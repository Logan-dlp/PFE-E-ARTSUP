using UnityEngine;

[CreateAssetMenu(fileName = "NewLine", menuName = "Scriptable Objects/DialogueLineData")]
public class DialogueLineData : ScriptableObject
{
    [TextArea] public string _text;
    public int _speakerIndex;  // 0 pour le personnage 1, 1 pour le personnage 2, etc.
    public Sprite _speakerSprite;
    public SpeakerEffectType _effect;  // Effet à appliquer pendant qu'ils parlent

    public DialogueLineData(string text, int speakerIndex, Sprite speakerSprite, SpeakerEffectType effect = SpeakerEffectType.None)
    {
        this._text = text;
        this._speakerIndex = speakerIndex;
        this._speakerSprite = speakerSprite;
        this._effect = effect;
    }
}

public enum SpeakerEffectType
{
    None,
    Tremble,
    Jump,
    Dim
}