using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;

public class QuickTimeEventWithInputSystem : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _qteSlot;
    [SerializeField] private Image _progressBarUI;

    [Header("Button Sprites")]
    [SerializeField] private Sprite[] _buttonSprites;

    [Header("Customizable QTE Buttons")]
    [SerializeField] private _customQTEButton[] _customQTEButtonArray = new _customQTEButton[4];

    [Header("QTE Configuration")]
    [SerializeField] private float _successDisplayDuration = 2f;
    [SerializeField] private bool _isTimerRandom;
    [SerializeField] private int _minInput;
    [SerializeField] private int _maxExcludedInput;
    [SerializeField] private QTEInputType _qTEInputType;
    [SerializeField] private float _stirDuration = 10;
    [SerializeField] private int _stirRequiredInput = 5;
    
    [Header("Unity Events")]
    
    private int _currentIndex;
    private float _timer;
    private int _currentPressCount = 0;
    private bool _qteSuccess = false;
    private bool _isQTEActive = false;
    private float _previousAngle = 0f;
    private bool _isClockwise = false;
    private int _turnCount = 0;
    private int _completedQTECount = 0;
    private bool _isController;
    private InputAction _rightStick;
    private float _progressBarValue;
    private bool _progressBarComplete;
    private bool _isLose;

    private enum InputCommand
    {
        A,
        B,
        X,
        Y,
        R_Stick
    }

    private enum QTEInputType
    {
        AllInputRandom,
        OneInputRandom,
        Fixed,
        Stir,
    }

    [System.Serializable]
    private class _customQTEButton
    {
        public InputCommand inputCommand;
        public int requiredInput;
        public float qTEDuration;
        public bool isProgressBar;
    }
    private void Awake()
    {
        if (TryGetComponent<PlayerInput>(out PlayerInput playerInput))
        {
            _rightStick = playerInput.currentActionMap["RightStick"];
            return;
        }
    }

    private void Start()
    {
        if (Gamepad.current == null)
        {
            Debug.LogWarning("No gamepad detected.");
        }
        
        switch (_qTEInputType)
        {
            case QTEInputType.AllInputRandom:
                RandomiseAllInput();
                break;
            case QTEInputType.OneInputRandom:
                RandomiseInput();
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

    public void ChangeController(PlayerInput playerInput)
    {
        if(playerInput.currentControlScheme == "Gamepad")
        {
            _isController = true;
        }
        else
        {
            _isController = false;
        }
    }

    public void StartQTE()
    {
        Debug.Log("StartQTE");
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
                else _turnCount++;
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
        for(int i = 0; i < _customQTEButtonArray.Length; i++)
        {
            _customQTEButtonArray[i].inputCommand = (InputCommand)Random.Range(0,4);
        }
    }

    private void RandomiseInput()
    {
        int randInput = Random.Range(0,4);
        for(int i = 0; i < _customQTEButtonArray.Length; i++)
        {
            _customQTEButtonArray[i].inputCommand = (InputCommand)randInput;
        }
    }

    private void RandomiseRequiredInput()
    {

    }

    [ContextMenu("Put required Input at 4 ")]
    private void PutAllRequiredInput()
    {
        if(_qTEInputType == QTEInputType.OneInputRandom)
        {
            for (int i = 0; i < _customQTEButtonArray.Length; i++)
            {
                _customQTEButtonArray[i].requiredInput = 4;
            }
        }
    }
    private void StirInput()
    {
        _customQTEButtonArray = new _customQTEButton[1];
        _customQTEButtonArray[0].requiredInput = _stirRequiredInput;
        _customQTEButtonArray[0].inputCommand = InputCommand.R_Stick;
        _customQTEButtonArray[0].qTEDuration = _stirDuration;
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