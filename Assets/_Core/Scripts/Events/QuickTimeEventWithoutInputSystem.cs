using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;

public class QuickTimeEventWithInputSystem : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image qteSlot;

    [Header("Button Sprites")]
    [SerializeField] private Sprite[] _buttonSprites;

    [Header("Customizable QTE Buttons")]
    [SerializeField] private GamepadButton[] _customQTEButtons;

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

    private InputAction _qteAction;
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

    private enum GamepadButton
    {
        A,
        B,
        X,
        Y,
        LT,
        RT,
        RB,
        LB,
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

       _qteAction = playerInput.actions.FindAction("QTE Action");

        if (_qteAction == null)
        {
            Debug.LogError("QTE Action not found in Input Action Asset.");
            return;
        }

        _qteAction.Enable();
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
            qteSlot.sprite = _buttonSprites[(int)_customQTEButtons[_currentIndex]];
            qteSlot.color = Color.white;
        }
        else
        {
            _currentIndex = _customQTEButtons.Length - 1;
            qteSlot.sprite = _buttonSprites[(int)_customQTEButtons[_currentIndex]];
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
        if (_customQTEButtons[_currentIndex] == GamepadButton.R_Stick) return false;

        bool buttonPressed = false;
        bool _qte;
        _qteAction

        switch (_customQTEButtons[_currentIndex])
        {
            case GamepadButton.A:
                buttonPressed = _qteAction.ReadValue<float>() > 0.5f;  // Bouton A
                break;
            case GamepadButton.B:
                buttonPressed = _qteAction.ReadValue<float>() > 0.5f;  // Bouton B
                break;
            case GamepadButton.X:
                buttonPressed = _qteAction.ReadValue<float>() > 0.5f;  // Bouton X
                break;
            case GamepadButton.Y:
                buttonPressed = _qteAction.ReadValue<float>() > 0.5f;  // Bouton Y
                break;
            case GamepadButton.LT:
                buttonPressed = _qteAction.ReadValue<float>() > 0.5f;  // Left Trigger
                break;
            case GamepadButton.RT:
                buttonPressed = _qteAction.ReadValue<float>() > 0.5f;  // Right Trigger
                break;
            case GamepadButton.RB:
                buttonPressed = _qteAction.ReadValue<float>() > 0.5f;  // Right Bumper
                break;
            case GamepadButton.LB:
                buttonPressed = _qteAction.ReadValue<float>() > 0.5f;  // Left Bumper
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
        if (_customQTEButtons[_currentIndex] == GamepadButton.R_Stick) return false;

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
