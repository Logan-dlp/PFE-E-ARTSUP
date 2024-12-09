using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class QuickTimeEvent : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _qteSlot;
    [SerializeField] private Image _progressBarUI;

    [Header("Button Sprites")]
    [SerializeField] private Sprite[] _buttonSprites;

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
    [SerializeField] private bool isStirProgressBar;
    
    [Header("Customizable QTE Buttons")]
    [SerializeField] private _customQTEButton[] _customQTEButtonArray;

    private int _currentIndex;
    private int _currentPressCount = 0;
    private int _turnCount = 0;
    private int _completedQTECount = 0;
    private float _timer;
    private float _progressBarValue;
    private float _previousAngle = 0f;
    private bool _qteSuccess = false;
    private bool _isQTEActive = false;
    private bool _progressBarComplete;
    private bool _isLose;
    private InputAction _rightStick;

    private enum InputCommand
    {
        A,
        B,
        X,
        Y,
        R_Stick,
    }

    private enum QTEInputType
    {
        AllInputRandom,
        OneInputRandom,
        Fixed,
        Stir,
    }

    [Serializable]
    private class _customQTEButton
    {
        public InputCommand inputCommand;
        [Min(1)] public int requiredInput;
        [Min(1)] public float qTEDuration;
        public bool isProgressBar;
    }
    private void Awake()
    {
        //Permet de récuper les inputs du joystick de telle façon à que le faire tourner fonctionne
        //ce qui n'es pas possible avec juste les unity event
        if (TryGetComponent<PlayerInput>(out PlayerInput playerInput))
        {
            _rightStick = playerInput.currentActionMap["RightStick"];
        }
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
        
        switch (_qTEInputType)
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
                StirInput();
                break;
        }
        
        StartQTE();
    }

    private void Update()
    {   
        if (_isQTEActive)
        {
            if(_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            if (_timer <= 0 && !_isLose)
            {
                EndQTE();
            }

            //Permet d'update la progress bar
            if(!_progressBarComplete)
            {
                if(_customQTEButtonArray[_currentIndex].isProgressBar && _progressBarValue > 0)
                {
                    _progressBarValue -= Time.deltaTime;
                    _progressBarUI.fillAmount = _progressBarValue / _customQTEButtonArray[_currentIndex].requiredInput;
                }
            }
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
                if (_currentPressCount >= _customQTEButtonArray[_currentIndex].requiredInput ||
                _progressBarValue >= _customQTEButtonArray[_currentIndex].requiredInput)
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
        if (_completedQTECount >= _customQTEButtonArray.Length)
        {
            return;
        }

        _isQTEActive = true;
        _timer = _customQTEButtonArray[_currentIndex].qTEDuration;
        _currentPressCount = 0;
        _qteSuccess = false;

        if (_completedQTECount < _customQTEButtonArray.Length - 1)
        {
            _currentIndex = _completedQTECount;         
        }
        else
        {
            _currentIndex = _customQTEButtonArray.Length - 1;
        }

        _qteSlot.sprite = GetSprite(_customQTEButtonArray[_currentIndex]);
        _qteSlot.color = Color.white;
        
        //Ecoute les inputs du joystick
        if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.R_Stick)
        {
            _rightStick.performed += CheckStickRotation;
        }
        
        if(_customQTEButtonArray[_currentIndex].isProgressBar == true)
        {
            _progressBarUI.gameObject.SetActive(true);
        }
    }

    public void EndQTE()
    {
        _isLose = true;
        _isQTEActive = false;

        if (!_qteSuccess)
        {
            _qteSlot.color = Color.red;
        }
    }

    private void QTEFailure()
    {
        _completedQTECount = 0;
        StartQTE();
    }
    
    private bool CheckButtonPress(InputCommand inputCommand)
    {
        switch (inputCommand)
        {
            case InputCommand.A:
                if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.A)
                {
                    if(_customQTEButtonArray[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _currentPressCount++;
                    }
                }
                else
                {
                    _currentPressCount = 0;
                    QTEFailure();
                    return false;
                }
                break;
            case InputCommand.B:
                if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.B)
                {
                    if(_customQTEButtonArray[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _currentPressCount++;
                        Debug.Log("B");
                    }
                }
                else
                {
                    _currentPressCount = 0;
                    QTEFailure();
                    return false;
                }
                break;
            case InputCommand.X:
                if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.X)
                {
                    if(_customQTEButtonArray[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _currentPressCount++;
                    }
                }
                else
                {
                    _currentPressCount = 0;
                    QTEFailure();
                    return false;
                }
                break;
            case InputCommand.Y:
                
                if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.Y)
                {
                    if(_customQTEButtonArray[_currentIndex].isProgressBar)
                    {
                        _progressBarValue++;
                    }
                    else
                    {
                        _currentPressCount++;
                    }
                }
                else
                {
                    _currentPressCount = 0;
                    QTEFailure();
                    return false;
                }
                break;
        }
        return true;
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
                if(_customQTEButtonArray[_currentIndex].isProgressBar)
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

        if (_turnCount >= _customQTEButtonArray[_currentIndex].requiredInput || _progressBarValue >= _customQTEButtonArray[_currentIndex].requiredInput)
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

    private Sprite GetSprite(_customQTEButton _QTEButton)
    {
        switch (_QTEButton.inputCommand)
        {
            case InputCommand.A:
                return _buttonSprites[0];
            case InputCommand.B:
                return _buttonSprites[1];
            case InputCommand.Y:
                return _buttonSprites[2];
            case InputCommand.X:
                return _buttonSprites[3];
            default:
                return _buttonSprites[4];
        }
    }

    private void RandomiseAllInput()
    {
        //Place tous les parametres necessaires pour les inputs du Cut
        RandomiseInputNumber();

        for(int i = 0; i < _customQTEButtonArray.Length; i++)
        {
            _customQTEButtonArray[i].inputCommand = (InputCommand)Random.Range(0,Enum.GetNames(typeof(InputCommand)).Length - 1);
        }
        
        if(_isTimerRandom) 
        {
            RandomiseTimer();
        }
        else
        {
            float timer = Random.Range(_minTimer, _maxTimer)/_customQTEButtonArray.Length;
            
            for(int i = 0; i < _customQTEButtonArray.Length; i++)
            {
                _customQTEButtonArray[i].qTEDuration = timer;
            }
        }
        
        PutAllRequiredInput();
    }

    private void RandomiseInputNumber()
    {
        _customQTEButtonArray = new _customQTEButton[Random.Range(_minInput,_maxInput + 1)];
        for(int i = 0; i < _customQTEButtonArray.Length; i++)
        {
            _customQTEButtonArray[i] = new _customQTEButton();
        }
    }
    private void RandomiseOneInput()
    {
        //Place tous les parametres necessaires pour les inputs du Crush
        _customQTEButtonArray = new _customQTEButton[1];
        _customQTEButtonArray[0] = new _customQTEButton();
        _customQTEButtonArray[0].inputCommand = (InputCommand)Random.Range(0,Enum.GetNames(typeof(InputCommand)).Length);
        _customQTEButtonArray[0].qTEDuration = _maxTimer;
        _customQTEButtonArray[0].isProgressBar = true;
        _customQTEButtonArray[0].requiredInput = RandomiseRequiredInput();
    }

    private int RandomiseRequiredInput()
    {
        return Random.Range(_minRequiredInput,_maxRequiredInput + 1);
    }
    
    private void RandomiseTimer()
    {
        int randTimer = Random.Range(_minTimer, _maxTimer + 1);
        for(int i = 0; i < _customQTEButtonArray.Length; i++)
        {
            _customQTEButtonArray[i].qTEDuration = randTimer;
        }
    }

    //Context Menu pour si on veut tout mettre à la valeur par défaut si on fait une serie d'input custom
    [ContextMenu("Put required Input at the default value")]
    private void PutAllRequiredInput()
    {
        for (int i = 0; i < _customQTEButtonArray.Length; i++)
        {
            _customQTEButtonArray[i].requiredInput = _defaultRequiredInputValue;
        }
    }

    private void StirInput()
    {
        //Place tous les parametres necessaires pour les inputs du Stir
        _customQTEButtonArray = new _customQTEButton[1];
        _customQTEButtonArray[0] = new _customQTEButton();
        _customQTEButtonArray[0].requiredInput = _stirRequiredInput;
        _customQTEButtonArray[0].inputCommand = InputCommand.R_Stick;
        _customQTEButtonArray[0].qTEDuration = _stirDuration;
        
        if(isStirProgressBar)
        {
            _customQTEButtonArray[0].isProgressBar = true;
        }
    }

    private IEnumerator DisplaySuccessForDuration()
    {
        yield return new WaitForSeconds(_successDisplayDuration);
        _qteSlot.color = Color.white;

        _completedQTECount++;
        _progressBarComplete = false;
        _progressBarValue = 0;
        _progressBarUI.fillAmount = 0;
        _progressBarUI.gameObject.SetActive(false);
        
        if (_completedQTECount < _customQTEButtonArray.Length)
        {
            StartQTE();
        }
        else
        {
            _qteSlot.sprite = null;
        }
    }
}