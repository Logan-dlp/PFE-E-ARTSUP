using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class QuickTimeEvent : MonoBehaviour
{
    [SerializeField] private Image qteSlot;

    [Header("Touch : A, B, X, Y, LT, RT, LB, RB")]
    [SerializeField] private Sprite[] _buttonSprites;
    [SerializeField] private float _qteDuration = 5f;
    [SerializeField] private float _successDisplayDuration = 2f;
    [SerializeField] private int _requiredPressCount = 10;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent _onQTESuccess;
    [SerializeField] private UnityEvent _onQTEFailure;

    private string[] _xboxButtons = { "A", "B", "X", "Y", "LT", "RT", "LB", "RB" };
    private int _currentIndex;
    private float _timer;
    private int _currentPressCount = 0;
    private bool _qteSuccess = false;
    private bool _isQTEActive = false;

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
                if (CheckButtonPress())
                {
                    if (_currentPressCount >= _requiredPressCount)
                    {
                        _qteSuccess = true;
                        qteSlot.color = Color.green;
                        StartCoroutine(DisplaySuccessForDuration());
                    }
                }
            }

            if (_timer <= 0)
            {
                EndQTE();
            }
        }
    }

    public void StartQTE()
    {
        _isQTEActive = true;
        _timer = _qteDuration;
        _currentPressCount = 0;
        _qteSuccess = false;
        _currentIndex = Random.Range(0, _xboxButtons.Length);
        qteSlot.sprite = _buttonSprites[_currentIndex];
        qteSlot.color = Color.white;
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
        bool buttonPressed = false;

        switch (_xboxButtons[_currentIndex])
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

    private IEnumerator DisplaySuccessForDuration()
    {
        yield return new WaitForSeconds(_successDisplayDuration);
        qteSlot.color = Color.white;
        _onQTESuccess.Invoke();
    }
}