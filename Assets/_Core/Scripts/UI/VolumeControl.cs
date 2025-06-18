using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.UI
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private GameObject[] _cursorGameObject;
        [SerializeField] private Vector3 _dialoguePossibleSpeed;
        [SerializeField] private VolumeOptionData volumeOptionData;
        [SerializeField] private GameObject[] _masterVolumeGameObjectIndicator;
        [SerializeField] private GameObject[] _musicVolumeGameObjectIndicator;
        [SerializeField] private GameObject[] _sfxVolumeGameObjectIndicator;
        [SerializeField] private GameObject[] _dialogueVolumeGameObjectIndicator;
        [SerializeField] private GameObject[] _dialogueSpeedGameObjectIndicator;

        private int _volumeOptionIndex = 0;

        private float _masterVolume = 1;
        private float _musicVolume = 1;
        private float _sfxVolume = 1;
        private float _dialogueVolume = 1;
        private float _dialogueSpeed = 0.07f;

        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                if (value < .05f)
                {
                    _masterVolume = 0;
                }
                else
                {
                    _masterVolume = Mathf.Clamp(value, 0, 1);
                }
            }
        }

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                if (value < .05f)
                {
                    _musicVolume = 0;
                }
                else
                {
                    _musicVolume = Mathf.Clamp(value, 0, 1);
                }
            }
        }

        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                if (value < .05f)
                {
                    _sfxVolume = 0;
                }
                else
                {
                    _sfxVolume = Mathf.Clamp(value, 0, 1);
                }
            }
        }

        public float DialogueVolume
        {
            get => _dialogueVolume;
            set
            {
                if (value < .05f)
                {
                    _dialogueVolume = 0;
                }
                else
                {
                    _dialogueVolume = Mathf.Clamp(value, 0, 1);
                }
            }
        }
        public float DialogueSpeed
        {
            get => _dialogueSpeed;
            set => _dialogueSpeed = value;
        }

        private void OnEnable()
        {
            InputManager.Instance.SwitchActionMap("Volume");
            ReplaceAllValue();
        }

        private void OnDisable()
        {
            InputManager.Instance.SwitchActionMap("Menu");
            SaveAllValue();
        }

        private void ReplaceAllValue()
        {
            MasterVolume = volumeOptionData.masterVolume;
            MusicVolume = volumeOptionData.musicVolume;
            SfxVolume = volumeOptionData.sfxVolume;
            DialogueVolume = volumeOptionData.dialogueVolume;
            DialogueSpeed = volumeOptionData.dialogueSpeed;
            UpdateVisual();
        }

        private void SaveAllValue()
        {
            volumeOptionData.masterVolume = MasterVolume;
            volumeOptionData.musicVolume = MusicVolume;
            volumeOptionData.sfxVolume = SfxVolume;
            volumeOptionData.dialogueVolume = DialogueVolume;
            volumeOptionData.dialogueSpeed = DialogueSpeed;
        }

        public void ChangeOption(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                if (callbackContext.ReadValue<Vector2>().y > 0)
                {
                    if (_volumeOptionIndex == 0)
                    {
                        _volumeOptionIndex = 4;
                    }
                    else
                    {
                        _volumeOptionIndex--;
                    }
                }
                else if (callbackContext.ReadValue<Vector2>().y < 0)
                {
                    if (_volumeOptionIndex == 4)
                    {
                        _volumeOptionIndex = 0;
                    }
                    else
                    {
                        _volumeOptionIndex++;
                    }
                }
            }

            foreach (GameObject obj in _cursorGameObject)
            {
                obj.SetActive(false);
            }

            _cursorGameObject[_volumeOptionIndex].SetActive(true);
        }

        public void IncreaseDecrease(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                if (callbackContext.ReadValue<float>() > 0)
                {
                    switch (_volumeOptionIndex)
                    {
                        case 0:
                            MasterVolume += .2f;
                            break;
                        case 1:
                            MusicVolume += .2f;
                            break;
                        case 2:
                            SfxVolume += .2f;
                            break;
                        case 3:
                            DialogueVolume += .2f;
                            break;
                        default:
                            if (_dialogueSpeed == _dialoguePossibleSpeed.x)
                            {
                                _dialogueSpeed = _dialoguePossibleSpeed.y;
                            }
                            else if (_dialogueSpeed == _dialoguePossibleSpeed.y)
                            {
                                _dialogueSpeed = _dialoguePossibleSpeed.z;
                            }
                            break;
                    }
                    UpdateVisual();

                }
                else if (callbackContext.ReadValue<float>() < 0)
                {
                    switch (_volumeOptionIndex)
                    {
                        case 0:
                            MasterVolume -= .2f;
                            break;
                        case 1:
                            MusicVolume -= .2f;
                            break;
                        case 2:
                            SfxVolume -= .2f;
                            break;
                        case 3:
                            DialogueVolume -= .2f;
                            break;
                        default:
                            if (_dialogueSpeed == _dialoguePossibleSpeed.y)
                            {
                                _dialogueSpeed = _dialoguePossibleSpeed.x;
                            }
                            else if (_dialogueSpeed == _dialoguePossibleSpeed.z)
                            {
                                _dialogueSpeed = _dialoguePossibleSpeed.y;
                            }
                            break;
                    }
                    UpdateVisual();
                }

            }
        }

        private void UpdateVisual()
        {
            switch (_volumeOptionIndex)
            {
                case 0:
                    foreach (GameObject obj in _masterVolumeGameObjectIndicator)
                    {
                        obj.SetActive(false);
                    }

                    for (float i = 0; i <= MasterVolume * 5 - 1; i++)
                    {
                        _masterVolumeGameObjectIndicator[(int)i].SetActive(true);
                    }
                    break;

                case 1:
                    foreach (GameObject obj in _musicVolumeGameObjectIndicator)
                    {
                        obj.SetActive(false);
                    }

                    for (float i = 0; i <= MusicVolume * 5 - 1; i++)
                    {
                        _musicVolumeGameObjectIndicator[(int)i].SetActive(true);
                    }
                    break;

                case 2:
                    foreach (GameObject obj in _sfxVolumeGameObjectIndicator)
                    {
                        obj.SetActive(false);
                    }

                    for (float i = 0; i <= SfxVolume * 5 - 1; i++)
                    {
                        _sfxVolumeGameObjectIndicator[(int)i].SetActive(true);
                    }
                    break;

                case 3:
                    foreach (GameObject obj in _dialogueVolumeGameObjectIndicator)
                    {
                        obj.SetActive(false);
                    }

                    for (float i = 0; i <= DialogueVolume * 5 - 1; i++)
                    {
                        _dialogueVolumeGameObjectIndicator[(int)i].SetActive(true);
                    }
                    break;

                default:
                    foreach (GameObject obj in _dialogueSpeedGameObjectIndicator)
                    {
                        obj.SetActive(false);
                    }

                    if (DialogueSpeed == _dialoguePossibleSpeed.x)
                    {
                        _dialogueSpeedGameObjectIndicator[0].SetActive(true);
                    }
                    else if (DialogueSpeed == _dialoguePossibleSpeed.y)
                    {
                        _dialogueSpeedGameObjectIndicator[0].SetActive(true);
                        _dialogueSpeedGameObjectIndicator[1].SetActive(true);
                    }
                    else if (DialogueSpeed == _dialoguePossibleSpeed.z)
                    {
                        _dialogueSpeedGameObjectIndicator[0].SetActive(true);
                        _dialogueSpeedGameObjectIndicator[1].SetActive(true);
                        _dialogueSpeedGameObjectIndicator[2].SetActive(true);
                    }
                    break;
            }


        }
    }
}