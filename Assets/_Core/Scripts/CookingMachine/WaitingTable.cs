using UnityEngine;
using UnityEngine.AI;

public class WaitingTable : MonoBehaviour
{
    [SerializeField] private GameObject[] _pivotWaitingItemsArray;
    private ItemData[] _itemDataArray = new ItemData[7];

    public void PlaceItem(ItemData itemToAdd, GameObject itemGameObject)
    {
        for (int i = 0; i < _itemDataArray.Length; i++)
        {
            if(_itemDataArray[i] == null)
            {
                _itemDataArray[i] = itemToAdd;
                itemGameObject.transform.SetParent(_pivotWaitingItemsArray[i].transform);
                itemGameObject.transform.localPosition = new Vector3(0, 0, 0);
                break;
            }
        }
    }

    public bool CheckAvailablePlace()
    {
        foreach(ItemData item in _itemDataArray)
        {
            if(item == null)
            {
                return true;
            }
        }
        return false;
    }
}
