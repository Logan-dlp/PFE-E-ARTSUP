using UnityEngine;

public class ProtoItemGiver : MonoBehaviour
{
    [SerializeField] private GameObject _item;
    public GameObject GiveItem()
    {
        return _item;
    }
}
