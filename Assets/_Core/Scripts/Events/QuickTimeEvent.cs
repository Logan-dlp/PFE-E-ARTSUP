using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuickTimeEvent : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image qteSlot;

    [Header("Button Sprites")]
    [SerializeField] private Sprite[] _buttonSprites;

    [Header("Customizable QTE Buttons")]
    [SerializeField] private string[] _customQTEButtons;

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

            if (!_qteSuccess)
            {
                if (CheckButtonPress() || CheckStickRotation())
                {
                    if (_currentPressCount >= _requiredPressCount)
                    {
                        _qteSuccess = true;
                        qteSlot.color = Color.green;
                        StartCoroutine(DisplaySuccessForDuration());
                    }
                }
            }

            if (_completedQTECount < _totalQTECount)
            {
                if (_timer <= 0)
                {
                    EndQTE();
                }
            }
        }
    }

    public void StartQTE()
    {
        if (_completedQTECount >= _totalQTECount)
        {
            return;
        }

        _isQTEActive = true;
        _timer = _qteDuration;
        _currentPressCount = 0;
        _qteSuccess = false;

        if (_completedQTECount < _totalQTECount - 1)
        {
            _currentIndex = _completedQTECount;
            qteSlot.sprite = _buttonSprites[_currentIndex];
            qteSlot.color = Color.white;
        }
        else
        {
            _currentIndex = _customQTEButtons.Length - 1;
            qteSlot.sprite = _buttonSprites[_currentIndex];
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

    private bool CheckButtonPress()
    {
        if (_customQTEButtons[_currentIndex] == "TURN") return false;

        bool buttonPressed = false;

        switch (_customQTEButtons[_currentIndex])
        {
            case "A":
                buttonPressed = Gamepad.current.buttonSouth.wasPressedThisFrame;
                break;
            case "B":
                buttonPressed = Gamepad.current.buttonEast.wasPressedThisFrame;
                break;
            case "X":
                buttonPressed = Gamepad.current.buttonWest.wasPressedThisFrame;
                break;
            case "Y":
                buttonPressed = Gamepad.current.buttonNorth.wasPressedThisFrame;
                break;
            case "LT":
                buttonPressed = Gamepad.current.leftTrigger.wasPressedThisFrame;
                break;
            case "RT":
                buttonPressed = Gamepad.current.rightTrigger.wasPressedThisFrame;
                break;
            case "RB":
                buttonPressed = Gamepad.current.rightShoulder.wasPressedThisFrame;
                break;
            case "LB":
                buttonPressed = Gamepad.current.leftShoulder.wasPressedThisFrame;
                break;
        }

        if (buttonPressed)
        {
            _currentPressCount++;
            Debug.Log($"Button pressed {_currentPressCount}/{_requiredPressCount}");
        }

        return buttonPressed;
    }

    private bool CheckStickRotation()
    {
        if (_customQTEButtons[_currentIndex] != "TURN") return false;

        Vector2 rightStick = Gamepad.current.rightStick.ReadValue();
        float angle = Mathf.Atan2(rightStick.y, rightStick.x) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360;

        float angleDifference = angle - _previousAngle;

        if (angleDifference > 0) angleDifference -= 360;

        if (angleDifference < 0 && angleDifference > -180)
        {
            if (_previousAngle < 60 && angle > 300)
            {
                _turnCount++;
                Debug.Log($"Turn count (Counterclockwise): {_turnCount}/{_turnsRequired}");

                if (_turnCount >= _turnsRequired)
                {
                    _currentPressCount = _turnCount;
                    Debug.Log("QTE success: Rotation detected (Counterclockwise).");
                    return true;
                }
            }

            _previousAngle = angle;
        }
        return false;
    }

    private IEnumerator DisplaySuccessForDuration()
    {
        yield return new WaitForSeconds(_successDisplayDuration);
        qteSlot.color = Color.white;
        _onQTESuccess.Invoke();

        _completedQTECount++;

        if (_completedQTECount < _totalQTECount)
        {
            StartQTE();
        }
        else
        {
            Debug.Log("FINI");
            qteSlot.sprite = null;
        }
    }
}