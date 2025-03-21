using UnityEngine;
using UnityEngine.Events;
using UnityEditor.EventSystems;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private ItemButtonEvent _onSelectEvent;
    [SerializeField] private ItemButtonEvent _onSubmitEvent;
    [SerializeField] private ItemButtonEvent _onClickEvent;

    
}

[System.Serializable]
public class ItemButtonEvent : UnityEvent<ItemButton>
{

}