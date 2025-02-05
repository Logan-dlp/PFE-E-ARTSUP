using MoonlitMixes.Events;
using MoonlitMixes.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.CookingMachine
{
    public class WaitingTable : MonoBehaviour
    {
        [SerializeField] private GameObject[] _pivotWaitingItemsArray;
        [SerializeField] private ScriptableAxisEvent _scriptableMovementEvent;
        [SerializeField] private ScriptableButtonEvent _scriptableSelectEvent;
        [SerializeField] private ScriptableButtonEvent _scriptableCancelEvent;
        [SerializeField] private ScriptableItemEvent _scriptableItemEvent;

        private GameObject[] _itemGameObjectArray = new GameObject[10];
        private Light[] _lightArray = new Light[10];
        private bool _isActive = false; 
        private int _indexSelectedItem = 0;

        private void OnEnable()
        {
            _scriptableMovementEvent.OnInput += Movement;
            _scriptableSelectEvent.OnInput += Select;
            _scriptableCancelEvent.OnInput += Cancel;
        }

        private void OnDisable()
        {
            _scriptableMovementEvent.OnInput -= Movement;
            _scriptableSelectEvent.OnInput -= Select;
            _scriptableCancelEvent.OnInput -= Cancel;
        }

        private void Awake()
        {
            for (int i = 0; i < _pivotWaitingItemsArray.Length; i++)
            {
                _lightArray[i] = _pivotWaitingItemsArray[i].GetComponent<Light>();
            }
        }

        public void PlaceItem(GameObject itemGameObject)
        {
            for (int i = 0; i < _itemGameObjectArray.Length; i++)
            {
                if(_itemGameObjectArray[i] == null)
                {
                    _itemGameObjectArray[i] = itemGameObject;
                    itemGameObject.transform.SetParent(_pivotWaitingItemsArray[i].transform);
                    itemGameObject.transform.localPosition = new Vector3(0, 0, 0);
                    break;
                }
            }
        }

        public bool CheckAvailablePlace()
        {
            foreach(GameObject item in _itemGameObjectArray)
            {
                if(item == null)
                {
                    return true;
                }
            }
            return false;
        }
        
        private void Select(InputAction.CallbackContext context)
        {
            if(!context.started) return;

            if(_itemGameObjectArray[_indexSelectedItem] == null) return;

            _scriptableItemEvent.SendObject(_itemGameObjectArray[_indexSelectedItem]);
            Destroy(_itemGameObjectArray[_indexSelectedItem]);
            QuitWaitingTable();
        }

        private void Cancel(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            QuitWaitingTable();
        }

        private void QuitWaitingTable()
        {
            FindFirstObjectByType<PlayerInteraction>().QuitInteraction(); 
            _isActive = false;
            UpdateHighlight();
        }

        private void Movement(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            Vector2 vec = context.ReadValue<Vector2>();
            Debug.Log(vec);
            switch((vec.x, vec.y))
            {
                case (1,0):
                    if(_indexSelectedItem == 4 || _indexSelectedItem == 9) 
                    {
                        return;
                    }
                    else
                    {
                        ++_indexSelectedItem;
                    }
                    break;
                case (-1,0):
                    if(_indexSelectedItem == 0 || _indexSelectedItem == 5)
                    {
                        return;
                    }
                    else
                    {
                        --_indexSelectedItem;
                    }
                    break;
                case (0,-1):
                    if(_indexSelectedItem < 5)
                    {
                        _indexSelectedItem += 5;
                    }
                    else
                    {
                        _indexSelectedItem -= 5;
                    }
                    break;
                case (0,1):
                    if(_indexSelectedItem >= 5)
                    {
                        _indexSelectedItem -= 5;
                    }
                    else
                    {
                        _indexSelectedItem += 5;
                    }
                    break;
            }
            UpdateHighlight();
        }

        public void StartHighlight()
        {
            _isActive = true;
            _lightArray[0].intensity = 1;
        }

        private void UpdateHighlight()
        {
            if(_isActive)
            {
                for(int i = 0; i < _lightArray.Length; i++)
                {
                    _lightArray[i].intensity = 0;
                }
                _lightArray[_indexSelectedItem].intensity = 1;
            }
            else
            {
                _lightArray[_indexSelectedItem].intensity = 0;
            }
        }
    }
}