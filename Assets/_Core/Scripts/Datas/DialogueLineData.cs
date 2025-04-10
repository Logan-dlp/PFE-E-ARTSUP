using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "NewLine", menuName = "Scriptable Objects/DialogueLineData")]
    public class DialogueLineData : ScriptableObject
    {
        [SerializeField, TextArea] private string _text;
        [SerializeField] private int _speakerIndex;  // 0 pour le personnage 1, 1 pour le personnage 2, etc.
        [SerializeField] private Sprite _speakerSprite;
        [SerializeField] private SpeakerEffectType _effect;

        [SerializeField, HideInInspector] private float _trembleIntensityX = 5f;
        [SerializeField, HideInInspector] private float _trembleIntensityY = 5f;

        public string Text => _text;
        public int SpeakerIndex => _speakerIndex;
        public Sprite SpeakerSprite => _speakerSprite;
        public SpeakerEffectType Effect => _effect;

        public float TrembleIntensityX
        {
            get => _trembleIntensityX;
            set => _trembleIntensityX = value;
        }

        public float TrembleIntensityY
        {
            get => _trembleIntensityY;
            set => _trembleIntensityY = value;
        }

        public bool IsTrembleEffect => _effect == SpeakerEffectType.Tremble;
    }

    public enum SpeakerEffectType
    {
        None,
        Tremble,
        Jump
    }
}