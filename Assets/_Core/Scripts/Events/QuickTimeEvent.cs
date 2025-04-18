﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using MoonlitMixes.Datas;
using MoonlitMixes.Datas.QTE;
using MoonlitMixes.Events;
using MoonlitMixes.Player;
using Random = UnityEngine.Random;

namespace MoonlitMixes.Events
{
    public class QuickTimeEvent : MonoBehaviour
    {
        [Header("Button Sprites")]
        [SerializeField] private Sprite[] _buttonSpritesArray;
        [SerializeField] private Sprite[] _buttonSpritesPressedArray;
        [SerializeField] private Image _qteSlot;
        [SerializeField] private Image _progressBarUI;

        [SerializeField] private ScriptableQTEConfig _qTEConfig;
        [SerializeField] private ScriptableQTEEvent _qTEEvent;
        [SerializeField] private ScriptableBoolEvent _scriptableBoolEvent;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField, Range(.1f, 1f)] private float _delayRatioFailure;
        [SerializeField, Range(.1f, 1)] private float _pressedSpriteTime = .1f;

        private int _currentIndex;
        private int _currentPressCount = 0;
        private int _turnCount = 0;
        private int _completedQTECount = 0;
        private int _failureCount;
        private float _timer;
        private float _progressBarValue;
        private float _previousAngle = 0f;
        private bool _qteSuccess = false;
        private bool _isQTEActive = false;
        private bool _progressBarComplete;
        private bool _isLose;
        private InputAction _rightStick;
        
        private void OnEnable()
        {
            _qTEEvent.ScriptableQTEConfigAction += QTE;
        }

        private void OnDisable()
        {
            _qTEEvent.ScriptableQTEConfigAction -= QTE;
        }

        private void Start()
        {
            //Les inputs au claviers ont pas étés fait étant donné qu'on a pas encore réfléchi à comment
            //adapter le QTE du joystick au clavier
            if (Gamepad.current == null)
            {
                Debug.LogWarning("No gamepad detected.");
                return;
            }
        }

        public void QTE(ScriptableQTEConfig scriptableQTEConfig)
        {
            ResetValues();
            _scriptableBoolEvent = scriptableQTEConfig.ScriptableBoolEvent;
            _qTEConfig = scriptableQTEConfig;
            _qteSlot = _qTEConfig.QteSlot;
            _progressBarUI = _qTEConfig.ProgressBarUI;

            switch (_qTEConfig.qTEInputType)
            {
                case QTEInputType.AllInputRandom:
                    RandomiseAllInput();
                    break;
                case QTEInputType.OneInputRandom:
                    RandomiseOneInput();
                    break;
                case QTEInputType.Fixed:
                    break;
                case QTEInputType.Stir:
                    _rightStick = _playerInput.currentActionMap.FindAction("RightStick");
                    StirInput();
                    break;
            }

            StartQTE();
        }

        private void ResetValues()
        {
            _completedQTECount = 0;
            _currentPressCount = 0;
            _currentIndex = 0;
            _failureCount = 0;
            _timer = 0;
            _qteSuccess = false;
            _isQTEActive = false;
            _progressBarValue = 0;
            _isLose = false;
            _progressBarComplete = false;
        }

        private void Update()
        {   
            if (_isQTEActive)
            {
                if(!_qteSuccess)
                {
                    if(_timer > 0)
                    {
                        _timer -= Time.deltaTime;
                    }
                    if (_timer <= 0 && !_isLose)
                    {
                        QTEFailure();
                    }

                    //Permet d'update la progress bar
                    if(!_progressBarComplete)
                    {
                        UpdateProgressBar();
                    }
                }
            }
            else
            {
                _qteSlot.enabled = false;
            }
        }

        private void UpdateProgressBar()
        {
            if(_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar && _progressBarValue > 0)
            {
                _progressBarValue -= Time.deltaTime;
                _progressBarUI.fillAmount = _progressBarValue / _qTEConfig.CustomQTEButtonList[_currentIndex].requiredInput;
            }
        }

        public void GetInput(InputAction.CallbackContext context)
        {
            if(context.started && _isQTEActive)
            {
                //J'ai pas trouvé d'autre façons que passer par des strings pour vérifier l'input
                switch(context.action.name)
                {
                    case "ButtonNorth" :
                        QTEAction(InputCommand.Y);
                        break;
                    case "ButtonSouth" :
                        QTEAction(InputCommand.A);
                        break;
                    case "ButtonEast" :
                        QTEAction(InputCommand.B);
                        break;
                    case "ButtonWest" :
                        QTEAction(InputCommand.X);
                        break;
                    case "RightStick" :
                        CheckStickRotation(context);
                        break;
                }
            }
        }

        private void QTEAction(InputCommand inputCommand)
        {
            if (!_qteSuccess)
            {
                if (CheckButtonPress(inputCommand))
                {
                    if (_currentPressCount >= _qTEConfig.CustomQTEButtonList[_currentIndex].requiredInput ||
                    _progressBarValue >= _qTEConfig.CustomQTEButtonList[_currentIndex].requiredInput)
                    {
                        _progressBarUI.fillAmount = 1;
                        _progressBarComplete = true;
                        _qteSuccess = true;
                        _qteSlot.color = Color.green;
                        StartCoroutine(DisplaySuccessForDuration());
                    }
                }
            }
        }

        public void StartQTE()
        {
            //Evite de relancer un QTE
            if (_completedQTECount >= _qTEConfig.CustomQTEButtonList.Count)
            {
                Debug.Log("");
                return;
            }

            _qteSlot.enabled = true;
            _isQTEActive = true;
            _timer = _qTEConfig.CustomQTEButtonList[_currentIndex].qTEDuration;
            _currentPressCount = 0;
            _qteSuccess = false;

            if (_completedQTECount < _qTEConfig.CustomQTEButtonList.Count - 1)
            {
                _currentIndex = _completedQTECount;         
            }
            else
            {
                _currentIndex = _qTEConfig.CustomQTEButtonList.Count - 1;
            }

            _qteSlot.sprite = _buttonSpritesArray[GetSprite(_qTEConfig.CustomQTEButtonList[_currentIndex])];
            _qteSlot.color = Color.white;

            //Ecoute les inputs du joystick
            if(_qTEConfig.CustomQTEButtonList[_currentIndex].inputCommand == InputCommand.R_Stick)
            {
                _rightStick.performed += CheckStickRotation;
            }

            if(_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar)
            {
                _progressBarUI.enabled = true;
            }
            else
            {
                _progressBarUI.enabled = false;
            }
        }

        public void EndQTE()
        {
            _isLose = true;
            _isQTEActive = false;
            _scriptableBoolEvent.SendBool(false);
            FindFirstObjectByType<PlayerInteraction>().QuitInteraction();
            if (!_qteSuccess)
            {
                _qteSlot.color = Color.red;
            }
        }

        private void QTEFailure()
        {
            if(_failureCount == 2)
            {
                EndQTE();
            }
            else
            {
                _qteSuccess = false;
                _qteSlot.color = Color.red;
                _progressBarComplete = false;
                _progressBarUI.fillAmount = 0;
                StartCoroutine(DisplayFailureForDuration());
            }
        }

        private bool CheckButtonPress(InputCommand inputCommand)
        {
            if (_qTEConfig.CustomQTEButtonList[_currentIndex].inputCommand != inputCommand)
            {
                ResetPressCount();
                return false;
            }

            if (_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar)
            {
                _progressBarValue++;
                StartCoroutine(SpritePressedChange());              
            }
            else
            {
                _currentPressCount++;
                StartCoroutine(SpritePressedChange());
            }
            
            return true;

            void ResetPressCount()
            {
                _currentPressCount = 0;
                QTEFailure();
            }
        }

        private void CheckStickRotation(InputAction.CallbackContext context)
        {
            /*Recupere la position du stick la soustrait a celle precedente pour verifier que le 
            joystick tourne bien dans le bon sens et de si le joueur ne s'est pas arrete; si le 
            joystick atteint un certain angle s'est reset, augment le compteur de tous*/
            Vector2 rightStickVector = context.ReadValue<Vector2>();
            float angle = Mathf.Atan2(rightStickVector.y, rightStickVector.x) * Mathf.Rad2Deg;

            if (angle < 0) 
            {
                angle += 360;
            }

            float angleDifference = angle - _previousAngle;

            if (angleDifference > 0)
            {
                angleDifference -= 360;
            }

            if (angleDifference < 0 && angleDifference > -180)
            {
                if (_previousAngle < 60 && angle > 300)
                {  
                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _turnCount++;
                    }
                }
                _previousAngle = angle;
            }

            if (_turnCount >= _qTEConfig.CustomQTEButtonList[_currentIndex].requiredInput || _progressBarValue >= _qTEConfig.CustomQTEButtonList[_currentIndex].requiredInput)
            {
                _turnCount = 0;
                _qteSuccess = true;
                _qteSlot.color = Color.green;
                _rightStick.performed -= CheckStickRotation;
                _progressBarComplete = true;
                _progressBarUI.fillAmount = 1;
                StartCoroutine(DisplaySuccessForDuration());
            }
        }

        private int GetSprite(CustomQTEButton _QTEButton)
        {
            switch (_QTEButton.inputCommand)
            {
                case InputCommand.A:
                    return 0;
                case InputCommand.B:
                    return 1;
                case InputCommand.Y:
                    return 2;
                case InputCommand.X:
                    return 3;
                default:
                    return 4;
            }
        }

        private void RandomiseAllInput()
        {
            //Place tous les parametres necessaires pour les inputs du Cut
            RandomiseInputNumber();

            for(int i = 0; i < _qTEConfig.CustomQTEButtonList.Count; i++)
            {
                _qTEConfig.CustomQTEButtonList[i].inputCommand = (InputCommand)Random.Range(0,Enum.GetNames(typeof(InputCommand)).Length - 1);
            }

            if(_qTEConfig.IsTimerRandom) 
            {
                RandomiseTimer();
            }
            else
            {
                float timer = _qTEConfig.MaxTimer/_qTEConfig.CustomQTEButtonList.Count;
                //Partage le timer entre tous les inputs
                if(timer < 2)
                {
                    timer = 3;
                }

                for(int i = 0; i < _qTEConfig.CustomQTEButtonList.Count; i++)
                {
                    _qTEConfig.CustomQTEButtonList[i].qTEDuration = timer;
                }
            }

            PutAllRequiredInput();
        }

        private void RandomiseInputNumber()
        {
            int randRange = Random.Range(_qTEConfig.MinInput, _qTEConfig.MaxInput + 1);
            _qTEConfig.CustomQTEButtonList.Clear();
            for(int i = 0; i < randRange; i++)
            {
                _qTEConfig.CustomQTEButtonList.Add(new CustomQTEButton());
            }
        }
        
        private void RandomiseOneInput()
        {
            //Place tous les parametres necessaires pour les inputs du Crush
            _qTEConfig.CustomQTEButtonList.Clear();
            _qTEConfig.CustomQTEButtonList.Add(new CustomQTEButton());
            _qTEConfig.CustomQTEButtonList[0].inputCommand = (InputCommand)Random.Range(0,Enum.GetNames(typeof(InputCommand)).Length -1);
            _qTEConfig.CustomQTEButtonList[0].qTEDuration = _qTEConfig.MaxTimer;
            _qTEConfig.CustomQTEButtonList[0].isProgressBar = true;
            _qTEConfig.CustomQTEButtonList[0].requiredInput = RandomiseRequiredInput();
        }

        private int RandomiseRequiredInput()
        {
            return Random.Range(_qTEConfig.MinRequiredInput,_qTEConfig.MaxRequiredInput + 1);
        }

        private void RandomiseTimer()
        {
            //Partage le timer entre tous les inputs
            float randTimer = Random.Range(_qTEConfig.MinTimer, _qTEConfig.MaxTimer + 1)/_qTEConfig.CustomQTEButtonList.Count;
            for(int i = 0; i < _qTEConfig.CustomQTEButtonList.Count; i++)
            {
                _qTEConfig.CustomQTEButtonList[i].qTEDuration = randTimer;
            }
        }

        //Context Menu pour si on veut tout mettre à la valeur par défaut si on fait une serie d'input custom
        [ContextMenu("Put required Input at the default value")]
        private void PutAllRequiredInput()
        {
            for (int i = 0; i < _qTEConfig.CustomQTEButtonList.Count; i++)
            {
                _qTEConfig.CustomQTEButtonList[i].requiredInput = _qTEConfig.DefaultRequiredInputValue;
            }
        }

        private void StirInput()
        {
            //Place tous les parametres necessaires pour les inputs du Stir
            _qTEConfig.CustomQTEButtonList.Clear();
            _qTEConfig.CustomQTEButtonList.Add(new CustomQTEButton());
            _qTEConfig.CustomQTEButtonList[0].requiredInput = _qTEConfig.StirRequiredInput;
            _qTEConfig.CustomQTEButtonList[0].inputCommand = InputCommand.R_Stick;
            _qTEConfig.CustomQTEButtonList[0].qTEDuration = _qTEConfig.StirDuration;

            if(_qTEConfig.IsStirProgressBar)
            {
                _qTEConfig.CustomQTEButtonList[0].isProgressBar = true;
            }
        }

        private IEnumerator DisplaySuccessForDuration()
        {
            _playerInput.DeactivateInput();
            yield return new WaitForSeconds(_qTEConfig.SuccessDisplayDuration);
            _playerInput.ActivateInput();
            _qteSlot.color = Color.white;

            _completedQTECount++;
            _progressBarComplete = false;
            _progressBarValue = 0;
            _progressBarUI.fillAmount = 0;
            _progressBarUI.enabled = false;

            if (_completedQTECount < _qTEConfig.CustomQTEButtonList.Count)
            {
                StartQTE();
            }
            else
            {
                _isQTEActive = false;
                FindFirstObjectByType<PlayerInteraction>().QuitInteraction();
                _scriptableBoolEvent.SendBool(true);
            }
        }

        private IEnumerator DisplayFailureForDuration()
        {
            _playerInput.DeactivateInput();
            yield return new WaitForSeconds(_qTEConfig.SuccessDisplayDuration * _delayRatioFailure);
            _playerInput.ActivateInput();
            _completedQTECount = 0;
            _failureCount++;
            _qteSlot.color = Color.white;
            StartQTE();
        }

        private IEnumerator SpritePressedChange()
        {
            _qteSlot.sprite = _buttonSpritesPressedArray[GetSprite(_qTEConfig.CustomQTEButtonList[_currentIndex])];
            yield return new WaitForSeconds(_pressedSpriteTime);
            _qteSlot.sprite = _buttonSpritesArray[GetSprite(_qTEConfig.CustomQTEButtonList[_currentIndex])];
        }
    }
}