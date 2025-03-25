using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using MoonlitMixes.Events;

namespace MoonlitMixes.Datas.QTE
{
    [CreateAssetMenu(fileName = "ScriptableQTEConfig", menuName = "Scriptable Objects/ScriptableQTEConfig")]
    public class ScriptableQTEConfig : ScriptableObject
    {
        [Header("QTE Configuration")]
        [SerializeField] private float _successDisplayDuration = 2f;
        [SerializeField] private QTEInputType _qTEInputType;

        [Header("Timer Parameters")]
        [SerializeField] private bool _isTimerRandom;
        [SerializeField, MinValue(1), MaxValue(10)] private int _minTimer = 3;
        [SerializeField, MinValue(1), MaxValue(10)] private int _maxTimer = 5;
        
        [Header("Input Parameters")]
        [SerializeField, MinValue(1), MaxValue(10)] private int _minInput = 2;
        [SerializeField, MinValue(3), MaxValue(10)] private int _maxInput = 5;
        [SerializeField, MinValue(3), MaxValue(10)] private int _minRequiredInput = 4;
        [SerializeField, MinValue(3), MaxValue(20)] private int _maxRequiredInput = 4;
        [SerializeField] private int _defaultRequiredInputValue = 4;

        [Header("Stir Parameters")]
        [SerializeField, MinValue(3), MaxValue(10)] private int _stirRequiredInput = 5;
        [SerializeField, MinValue(3), MaxValue(10)] private float _stirDuration = 10;
        [SerializeField] private bool _isStirProgressBar;

        [Header("Customizable QTE Buttons")]
        [SerializeField] private List<CustomQTEButton> _customQTEButtonList;

        private Image _qteSlot;
        private Image _progressBarUI;
        private ScriptableBoolEvent _scriptableBoolEvent;
        
        public ScriptableBoolEvent ScriptableBoolEvent
        {
            get => _scriptableBoolEvent;
            set => _scriptableBoolEvent = value;
        }
        
        public List<CustomQTEButton> CustomQTEButtonList
        {
            get => _customQTEButtonList;
            set => _customQTEButtonList = value;
        }
        
        public bool IsStirProgressBar
        {
            get => _isStirProgressBar;
        }
        
        public float StirDuration
        {
            get => _stirDuration;
        }

        public int StirRequiredInput
        {
            get => _stirRequiredInput;
        }

        public int MaxInput
        {
            get => _maxInput;
        }
        
        public int MinRequiredInput
        {
            get => _minRequiredInput;
        }
        
        public int MaxRequiredInput
        {
            get => _maxRequiredInput;
        }
        
        public int DefaultRequiredInputValue
        {
            get => _defaultRequiredInputValue;
        }
        
        public int MinInput
        {
            get => _minInput;
        }
        
        public int MinTimer
        {
            get => _minTimer;
        }
        
        public int MaxTimer
        {
            get => _maxTimer;
        }
        
        public float SuccessDisplayDuration
        {
            get => _successDisplayDuration;
        }
        
        public QTEInputType qTEInputType
        {
            get => _qTEInputType;
        }

        public bool IsTimerRandom
        {
            get => _isTimerRandom;
        }

        public Image ProgressBarUI
        {
            get => _progressBarUI;
            set => _progressBarUI = value;
        }

        public Image QteSlot
        {
            get => _qteSlot;
            set => _qteSlot = value;
        }
    }
}
