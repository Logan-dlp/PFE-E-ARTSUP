using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using MoonlitMixes.Player;
using Random = UnityEngine.Random;

namespace MoonlitMixes.QTE
{
    public class QuickTimeEvent : MonoBehaviour
    {

        [Header("Button Sprites")]
        [SerializeField] private Sprite[] _buttonSpritesArray;

        [SerializeField] private Image _qteSlot;
        [SerializeField] private Image _progressBarUI;

        [SerializeField] private ScriptableQTEConfig _qTEConfig;
        [SerializeField] private ScriptableQTEEvent _qTEEvent;
        [SerializeField] private ScriptableBoolEvent _scriptableBoolEvent;
        [SerializeField, Range(.1f, 1f)] private float _delayRatioFailure;

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
                case ScriptableQTEConfig.QTEInputType.AllInputRandom:
                    RandomiseAllInput();
                    break;
                case ScriptableQTEConfig.QTEInputType.OneInputRandom:
                    RandomiseOneInput();
                    break;
                case ScriptableQTEConfig.QTEInputType.Fixed:
                    break;
                case ScriptableQTEConfig.QTEInputType.Stir:
                    _rightStick = FindFirstObjectByType<PlayerInput>().currentActionMap.FindAction("RightStick");
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
                        if(_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar && _progressBarValue > 0)
                        {
                            _progressBarValue -= Time.deltaTime;
                            _progressBarUI.fillAmount = _progressBarValue / _qTEConfig.CustomQTEButtonList[_currentIndex].requiredInput;
                        }
                    }
                }
            }
            else
            {
                _qteSlot.enabled = false;
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
                        QTEAction(ScriptableQTEConfig.InputCommand.Y);
                        break;
                    case "ButtonSouth" :
                        QTEAction(ScriptableQTEConfig.InputCommand.A);
                        break;
                    case "ButtonEast" :
                        QTEAction(ScriptableQTEConfig.InputCommand.B);
                        break;
                    case "ButtonWest" :
                        QTEAction(ScriptableQTEConfig.InputCommand.X);
                        break;
                    case "RightStick" :
                        CheckStickRotation(context);
                        break;
                }
            }
        }

        private void QTEAction(ScriptableQTEConfig.InputCommand inputCommand)
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

            _qteSlot.sprite = GetSprite(_qTEConfig.CustomQTEButtonList[_currentIndex]);
            _qteSlot.color = Color.white;

            //Ecoute les inputs du joystick
            if(_qTEConfig.CustomQTEButtonList[_currentIndex].inputCommand == ScriptableQTEConfig.InputCommand.R_Stick)
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

        private bool CheckButtonPress(ScriptableQTEConfig.InputCommand inputCommand)
        {
            switch (inputCommand)
            {
                case ScriptableQTEConfig.InputCommand.A:
                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].inputCommand != ScriptableQTEConfig.InputCommand.A) 
                    {
                        ResetPressCount();
                        return false;
                    }

                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _currentPressCount++;
                    }
                    break;

                case ScriptableQTEConfig.InputCommand.B:
                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].inputCommand != ScriptableQTEConfig.InputCommand.B)
                    {
                        ResetPressCount();
                        return false;
                    }

                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _currentPressCount++;
                    }
                    break;

                case ScriptableQTEConfig.InputCommand.X:
                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].inputCommand != ScriptableQTEConfig.InputCommand.X)
                    {
                        ResetPressCount();
                        return false;
                    }
                    
                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _currentPressCount++;
                    }
                    break;

                case ScriptableQTEConfig.InputCommand.Y:

                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].inputCommand != ScriptableQTEConfig.InputCommand.Y)
                    {
                        ResetPressCount();
                        return false;
                    }
                    
                    if(_qTEConfig.CustomQTEButtonList[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _currentPressCount++;
                    }
                    break;
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

        private Sprite GetSprite(ScriptableQTEConfig.CustomQTEButton _QTEButton)
        {
            switch (_QTEButton.inputCommand)
            {
                case ScriptableQTEConfig.InputCommand.A:
                    return _buttonSpritesArray[0];
                case ScriptableQTEConfig.InputCommand.B:
                    return _buttonSpritesArray[1];
                case ScriptableQTEConfig.InputCommand.Y:
                    return _buttonSpritesArray[2];
                case ScriptableQTEConfig.InputCommand.X:
                    return _buttonSpritesArray[3];
                default:
                    return _buttonSpritesArray[4];
            }
        }

        private void RandomiseAllInput()
        {
            //Place tous les parametres necessaires pour les inputs du Cut
            RandomiseInputNumber();

            for(int i = 0; i < _qTEConfig.CustomQTEButtonList.Count; i++)
            {
                _qTEConfig.CustomQTEButtonList[i].inputCommand = (ScriptableQTEConfig.InputCommand)Random.Range(0,Enum.GetNames(typeof(ScriptableQTEConfig.InputCommand)).Length - 1);
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
                _qTEConfig.CustomQTEButtonList.Add(new ScriptableQTEConfig.CustomQTEButton());
            }
        }
        private void RandomiseOneInput()
        {
            //Place tous les parametres necessaires pour les inputs du Crush
            _qTEConfig.CustomQTEButtonList.Clear();
            _qTEConfig.CustomQTEButtonList.Add(new ScriptableQTEConfig.CustomQTEButton());
            _qTEConfig.CustomQTEButtonList[0].inputCommand = (ScriptableQTEConfig.InputCommand)Random.Range(0,Enum.GetNames(typeof(ScriptableQTEConfig.InputCommand)).Length -1);
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
            _qTEConfig.CustomQTEButtonList.Add(new ScriptableQTEConfig.CustomQTEButton());
            _qTEConfig.CustomQTEButtonList[0].requiredInput = _qTEConfig.StirRequiredInput;
            _qTEConfig.CustomQTEButtonList[0].inputCommand = ScriptableQTEConfig.InputCommand.R_Stick;
            _qTEConfig.CustomQTEButtonList[0].qTEDuration = _qTEConfig.StirDuration;

            if(_qTEConfig.IsStirProgressBar)
            {
                _qTEConfig.CustomQTEButtonList[0].isProgressBar = true;
            }
        }

        private IEnumerator DisplaySuccessForDuration()
        {
            yield return new WaitForSeconds(_qTEConfig.SuccessDisplayDuration);
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
            yield return new WaitForSeconds(_qTEConfig.SuccessDisplayDuration * _delayRatioFailure);
            _completedQTECount = 0;
            _failureCount++;
            _qteSlot.color = Color.white;
            StartQTE();
        }
    }
}