using UnityEngine;

[CreateAssetMenu(fileName = "NewLine", menuName = "Scriptable Objects/DialogueLineData")]
public class DialogueLineData : ScriptableObject
{
    [SerializeField, TextArea] private string _text;
    [SerializeField] private int _speakerIndex;  // 0 pour le personnage 1, 1 pour le personnage 2, etc.
    [SerializeField] private Sprite _speakerSprite;
    [SerializeField] private SpeakerEffectType _effect;

    public string Text => _text;
    public int SpeakerIndex => _speakerIndex;
    public Sprite SpeakerSprite => _speakerSprite;
    public SpeakerEffectType Effect => _effect;
}

public enum SpeakerEffectType
{
    None,
    Tremble,
    Jump,
    Dim
}