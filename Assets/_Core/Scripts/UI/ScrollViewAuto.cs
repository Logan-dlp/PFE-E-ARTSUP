using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewAuto : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _prefabListItem;

    [SerializeField] private ItemButtonEvent _eventItemClicked;
    [SerializeField] private ItemButtonEvent _eventItemSelect;
    [SerializeField] private ItemButtonEvent _eventItemSubmit;

    [SerializeField] private int _defaultSelectedIndex = 0;   

    void OnEnable()
    {
        GetButtons();
        
    }

    private void GetButtons()
    {
        ItemButton[] itemArray = FindObjectsByType<ItemButton>(FindObjectsSortMode.InstanceID);
        Array.Reverse(itemArray);

        foreach (ItemButton item in itemArray)
        {
            Debug.Log(item);
            item.onSelectEvent.AddListener((ItemButton) => {HandleEventItemOnSelect(item);});
            item.onClickEvent.AddListener((ItemButton) => {HandleEventItemOnClick(item);});
            item.onSubmitEvent.AddListener((ItemButton) => {HandleEventItemOnSubmit(item);});

            UpdateAllButtonNavReferences(itemArray);
        }

    }

    private void UpdateAllButtonNavReferences(ItemButton[] itemArray)
    {
        if(itemArray.Length < 2)
        {
            return;
        }

        ItemButton item;
        Navigation localNavigation;

        for(int i = 0; i < itemArray.Length; i++)
        {
            item = itemArray[i];
            localNavigation = item.gameObject.GetComponent<Button>().navigation;

            localNavigation.selectOnUp = GetNavigationUp(i, itemArray);
            localNavigation.selectOnDown = GetNavigationDown(i, itemArray);
            localNavigation.selectOnLeft = GetNavigationLeft(i, itemArray);
            localNavigation.selectOnRight = GetNavigationRight(i, itemArray);

            item.gameObject.GetComponent<Button>().navigation = localNavigation;
        }
    }

    private Selectable GetNavigationUp(int i, ItemButton[] itemArray)
    {
        if(itemArray.Length < 5)
        {
            return null;
        }
        else if(i < 5)
        {
            return null;
        }
        
        return itemArray[i - 5].GetComponent<Selectable>();

    }

    private Selectable GetNavigationDown(int i, ItemButton[] itemArray)
    {
        Debug.Log(i);
        if(itemArray.Length < 5)
        {
            return null;
        }
        if(i > itemArray.Length - 5)
        {
            return null;
        }

        return itemArray[i + 5].GetComponent<Selectable>();
    }

    private Selectable GetNavigationLeft(int i, ItemButton[] itemArray)
    {
        if(itemArray.Length < 2)
        {
            return null;
        }

        if(i%5 == 0)
        {
            return null;
        }

        return itemArray[i - 1].GetComponent<Selectable>();
    }

    private Selectable GetNavigationRight(int i, ItemButton[] itemArray)
    {
        if(itemArray.Length < 2)
        {
            return null;
        }

        if(i%5 == 4)
        {
            return null;
        }

        return itemArray[i + 1].GetComponent<Selectable>();
    }

    private void HandleEventItemOnSelect(ItemButton item)
    {
        _eventItemSelect.Invoke(item);
    }

    private void HandleEventItemOnClick(ItemButton item)
    {
        _eventItemClicked.Invoke(item);
    }

    private void HandleEventItemOnSubmit(ItemButton item)
    {
        _eventItemSubmit.Invoke(item);
    }
}
