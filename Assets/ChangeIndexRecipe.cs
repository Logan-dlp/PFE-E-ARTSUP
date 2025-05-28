using UnityEngine;

public class ChangeIndexRecipe : MonoBehaviour
{
    [SerializeField] private GameObject[] _itemDescriptionArray;

    public void DesactivateAllObject()
    {
        foreach (GameObject item in _itemDescriptionArray)
        {
            item.SetActive(false);
        }
    }
}
