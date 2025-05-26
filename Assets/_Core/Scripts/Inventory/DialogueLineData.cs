 
using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "NewLine", menuName = "Scriptable Objects/DialogueLineData")]
    public class DialogueLineData : ScriptableObject
    {
        [SerializeField, TextArea] private string _text;
        [SerializeField] private SpeakerSlot _speakerSlot;
        [SerializeField] private Sprite _speakerSprite;
        [SerializeField] private SpeakerEffectType _effect;

        [SerializeField, HideInInspector] private float _trembleIntensityX = 5f;
        [SerializeField, HideInInspector] private float _trembleIntensityY = 5f;

        [SerializeField, Range(0.1f, 5f), HideInInspector] private float _fadeInDuration = 0.5f;
        [SerializeField, Range(0.1f, 5f), HideInInspector] private float _fadeOutDuration = 0.5f;

        public float FadeInDuration => _fadeInDuration;
        public float FadeOutDuration => _fadeOutDuration;

        public string Text => _text;
        public SpeakerSlot SpeakerSlot => _speakerSlot;
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
        public bool IsFadeEffect => _effect == SpeakerEffectType.FadeIn || _effect == SpeakerEffectType.FadeOut;
    }

    public enum SpeakerSlot
    {
        Slot1 = 0,
        Slot2 = 1,
        Slot3 = 2,
        Slot4 = 3
    }

    public enum SpeakerEffectType
    {
        None,
        Tremble,
        Jump,
        FadeIn,
        FadeOut
    }
}