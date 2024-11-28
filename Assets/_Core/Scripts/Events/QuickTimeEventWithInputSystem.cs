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
    [SerializeField] private InputCommand[] _customQTEButtons;

    [Header("QTE Configuration")]
    [SerializeField] private float _qteDuration = 5f;
    [SerializeField] private float _successDisplayDuration = 2f;
    [SerializeField] private int _requiredPressCount = 10;

    [Header("Rotation Configuration")]
    [SerializeField] private int _turnsRequired = 5;

    [Header("QTE Sequence Settings")]
    [SerializeField] private int _totalQTECount = 5;

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
    private float _angleThreshold = 10f;
    private int _turnCount = 0;
    private int _completedQTECount = 0;
    private bool isController;
    private enum InputCommand
    {
        A,
        B,
        X,
        Y,
        R_Stick
    }

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component missing.");
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
            Debug.Log("GetInput3");
            Debug.Log(context.action.name);
            switch(context.action.name)
            {
                case "ButtonNorth" :
                    Debug.Log("ButtonNorth");
                    QTEAction(InputCommand.Y);
                    break;
                case "ButtonSouth" :
                    Debug.Log("ButtonSouth");
                    QTEAction(InputCommand.A);
                    break;
                case "ButtonEast" :
                    Debug.Log("ButtonEast");
                    QTEAction(InputCommand.B);
                    break;
                case "ButtonWest" :
                    Debug.Log("ButtonWest");
                    QTEAction(InputCommand.X);
                    break;
                case "RightStick" :
                    Debug.Log("RightButton");
                    QTEAction(InputCommand.R_Stick);
                    break;
            }
        }
    }

    private void QTEAction(InputCommand inputCommand)
    {
        Debug.Log("QTEAction1");
        if (!_qteSuccess)
        {
            Debug.Log("QTEAction2");
            if (inputCommand == InputCommand.R_Stick)
            {
                Debug.Log("QTEAction3");
                if(CheckStickRotation())
                {
                    Debug.Log("QTEAction4");
                    _qteSuccess = true;
                    qteSlot.color = Color.green;
                    StartCoroutine(DisplaySuccessForDuration());
                }
            }
            if (CheckButtonPress(inputCommand))
            {
                Debug.Log("QTEAction5");
                if (_currentPressCount >= _requiredPressCount)
                {
                    Debug.Log("QTEAction6");
                    _qteSuccess = true;
                    qteSlot.color = Color.green;
                    StartCoroutine(DisplaySuccessForDuration());
                }
            }
        }
    }

    public void ChangeController(PlayerInput playerInput)
    {
        Debug.Log("ChangeController1");
        if(playerInput.currentControlScheme == "Gamepad")
        {
            Debug.Log("ChangeController2");
            isController = true;
        }
        else
        {
            Debug.Log("ChangeController3");
            isController = false;
        }
    }

    public void StartQTE()
    {
        Debug.Log("StartQTE1");
        if (_completedQTECount >= _totalQTECount)
        {
            Debug.Log("StartQTE2");
            return;
        }

        _isQTEActive = true;
        _timer = _qteDuration;
        _currentPressCount = 0;
        _qteSuccess = false;
        Debug.Log("StartQTE3");
        if (_completedQTECount < _totalQTECount - 1)
        {
            Debug.Log("StartQTE4");
            _currentIndex = _completedQTECount;
            qteSlot.sprite = _buttonSprites[(int)_customQTEButtons[_currentIndex]];
            qteSlot.color = Color.white;
        }
        else
        {
            Debug.Log("StartQTE5");
            _currentIndex = _customQTEButtons.Length - 1;
            qteSlot.sprite = _buttonSprites[(int)_customQTEButtons[_currentIndex]];
            qteSlot.color = Color.white;
        }
    }

    public void EndQTE()
    {
        Debug.Log("EndQTE1");
        _isQTEActive = false;

        if (!_qteSuccess)
        {
            Debug.Log("EndQTE2");
            qteSlot.color = Color.red;
            _onQTEFailure.Invoke();
        }
    }

    private bool CheckButtonPress(InputCommand inputCommand)
    {
        Debug.Log("CheckButtonPress1");
        switch (inputCommand)
        {
            case InputCommand.A:
                Debug.Log("CheckButtonPress2");
                if(_customQTEButtons[_currentIndex] == InputCommand.A)
                {
                    Debug.Log("CheckButtonPress3");
                    _currentPressCount++;
                }
                else 
                {
                    _currentPressCount = 0;
                    Debug.Log("CheckButtonPress4");
                }
                break;
            case InputCommand.B:
                Debug.Log("CheckButtonPress5");
                if(_customQTEButtons[_currentIndex] == InputCommand.B)
                {
                    Debug.Log("CheckButtonPress6");
                    _currentPressCount++;
                }
                else 
                {
                    _currentPressCount = 0;
                    Debug.Log("CheckButtonPress7");
                }
                break;
            case InputCommand.X:
                Debug.Log("CheckButtonPress8");
                if(_customQTEButtons[_currentIndex] == InputCommand.X)
                {
                    Debug.Log("CheckButtonPress9");
                    _currentPressCount++;
                }
                else 
                {
                    _currentPressCount = 0;
                    Debug.Log("CheckButtonPress10");
                }
                break;
            case InputCommand.Y:
                Debug.Log("CheckButtonPress11");
                if(_customQTEButtons[_currentIndex] == InputCommand.Y)
                {
                    Debug.Log("CheckButtonPress12");
                    _currentPressCount++;
                }
                else
                {
                    _currentPressCount = 0;
                    Debug.Log("CheckButtonPress13");
                }
                break;
        }
        Debug.Log("CheckButtonPress14");
        return true;
    }

    private bool CheckStickRotation()
    {
        Debug.Log("CheckStickRotation1");
        /*if (_customQTEButtons[_currentIndex] == InputCommand.R_Stick)
        {
            Debug.Log("CheckStickRotation2");
            return false;
        }*/

        Debug.Log("CheckStickRotation3");
        Vector2 rightStick = Gamepad.current.rightStick.ReadValue();
        float angle = Mathf.Atan2(rightStick.y, rightStick.x) * Mathf.Rad2Deg;

        if (angle < 0) 
        {
            Debug.Log("CheckStickRotation4");
            angle += 360;
        }

        float angleDifference = angle - _previousAngle;

        if (angleDifference > 0)
        {
            Debug.Log("CheckStickRotation5");
            angleDifference -= 360;
        }

        if (angleDifference < 0 && angleDifference > -180)
        {
            Debug.Log("CheckStickRotation6");
            if (_previousAngle < 60 && angle > 300)
            {
                Debug.Log("CheckStickRotation7");
                _turnCount++;
                
                if (_turnCount >= _turnsRequired)
                {
                    Debug.Log("CheckStickRotation8");
                    _currentPressCount = _turnCount;
                    return true;
                }
            }
            Debug.Log("CheckStickRotation9");
            _previousAngle = angle;
        }
        Debug.Log("CheckStickRotation10");
        return false;
    }

    private IEnumerator DisplaySuccessForDuration()
    {
        Debug.Log("DisplaySuccessForDuration1");
        yield return new WaitForSeconds(_successDisplayDuration);
        qteSlot.color = Color.white;
        _onQTESuccess.Invoke();

        _completedQTECount++;

        if (_completedQTECount < _totalQTECount)
        {
            Debug.Log("DisplaySuccessForDuration2");
            StartQTE();
        }
        else
        {
            Debug.Log("DisplaySuccessForDuration3");
            qteSlot.sprite = null;
        }
    }
}
