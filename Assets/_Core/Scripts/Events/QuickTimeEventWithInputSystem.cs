using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class QuickTimeEventWithInputSystem : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image qteSlot;

    [Header("Button Sprites")]
    [SerializeField] private Sprite[] _buttonSprites;

    [Header("Customizable QTE Buttons")]
    [SerializeField] private _customQTEButton[] _customQTEButtonArray;

    [Header("QTE Configuration")]
    [SerializeField] private float _successDisplayDuration = 2f;
    
    [Header("Unity Events")]
    [SerializeField] private UnityEvent _onQTESuccess;
    [SerializeField] private UnityEvent _onQTEFailure;

    private int _currentIndex;
    private float _timer;
    private int _currentPressCount = 0;
    private bool _qteSuccess = false;
    private bool _isQTEActive = false;
    private float _previousAngle = 0f;
    private bool _isClockwise = false;
    private int _turnCount = 0;
    private int _completedQTECount = 0;
    private bool isController;
    private InputAction rightStick;
    
    private enum InputCommand
    {
        A,
        B,
        X,
        Y,
        R_Stick
    }

    [System.Serializable]
    private struct _customQTEButton
    {
        public InputCommand inputCommand;
        public int requiredInput;
        public float qTEDuration;
    }

    private void Awake()
    {
        if (TryGetComponent<PlayerInput>(out PlayerInput playerInput))
        {
            rightStick = playerInput.currentActionMap["RightStick"];
            return;
        }
    }

    private void Start()
    {
        if (Gamepad.current == null)
        {
            Debug.LogWarning("No gamepad detected.");
        }

        StartQTE();
    }

    private void Update()
    {   
        if (_isQTEActive)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                Debug.Log("lose");
            }
        }
    }

    public void GetInput(InputAction.CallbackContext context)
    {
        if(context.started)
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
            if (inputCommand == InputCommand.R_Stick)
            {
                return;
            }

            if (CheckButtonPress(inputCommand))
            {
                if (_currentPressCount >= _customQTEButtonArray[_currentIndex].requiredInput)
                {
                    _qteSuccess = true;
                    qteSlot.color = Color.green;
                    StartCoroutine(DisplaySuccessForDuration());
                }
            }
        }
    }

    public void ChangeController(PlayerInput playerInput)
    {
        if(playerInput.currentControlScheme == "Gamepad")
        {
            isController = true;
        }
        else
        {
            isController = false;
        }
    }

    public void StartQTE()
    {
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
            qteSlot.sprite = GetSprite(_customQTEButtonArray[_currentIndex]);
            qteSlot.color = Color.white;
            
            if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.R_Stick)
            {
                rightStick.performed += CheckStickRotation;
            }
        }
        else
        {
            _currentIndex = _customQTEButtonArray.Length - 1;
            qteSlot.sprite = GetSprite(_customQTEButtonArray[_currentIndex]);
            qteSlot.color = Color.white;
        }
    }

    public void EndQTE()
    {
        _isQTEActive = false;

        if (!_qteSuccess)
        {
            qteSlot.color = Color.red;
            _onQTEFailure.Invoke();
        }
    }

    private bool CheckButtonPress(InputCommand inputCommand)
    {
        switch (inputCommand)
        {
            case InputCommand.A:
                if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.A)
                {
                    _currentPressCount++;
                }
                else 
                {
                    _currentPressCount = 0;
                }
                break;
            case InputCommand.B:
                if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.B)
                {
                    _currentPressCount++;
                }
                else 
                {
                    _currentPressCount = 0;
                }
                break;
            case InputCommand.X:
                if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.X)
                {
                    _currentPressCount++;
                }
                else 
                {
                    _currentPressCount = 0;
                }
                break;
            case InputCommand.Y:
                if(_customQTEButtonArray[_currentIndex].inputCommand == InputCommand.Y)
                {
                    _currentPressCount++;
                }
                else
                {
                    _currentPressCount = 0;
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
                _turnCount++;
            }
            _previousAngle = angle;
        }
        if (_turnCount >= _customQTEButtonArray[_currentIndex].requiredInput)
        {
            _turnCount = 0;
            _qteSuccess = true;
            qteSlot.color = Color.green;
            rightStick.performed -= CheckStickRotation;
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

    private IEnumerator DisplaySuccessForDuration()
    {
        yield return new WaitForSeconds(_successDisplayDuration);
        qteSlot.color = Color.white;
        _onQTESuccess.Invoke();

        _completedQTECount++;
        
        if (_completedQTECount < _customQTEButtonArray.Length)
        {
            StartQTE();
        }
        else
        {
            qteSlot.sprite = null;
        }
    }
}