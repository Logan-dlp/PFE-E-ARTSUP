using MoonlitMixes.Item;
using UnityEngine;

public class ChangeIndexRecipe : MonoBehaviour
{
    [SerializeField] private IndexTemplate _indexTemplate;
    [SerializeField] private ItemData _insect;

    public void ChangeIndex(ItemData itemData)
    {
        _indexTemplate.itemImage.sprite = itemData.ItemSprite;

        switch (itemData.Type)
        {
            case ElementType.Water:
                _indexTemplate.frameImage.sprite = _indexTemplate.frameElementArray[0];
                _indexTemplate.elementImage.sprite = _indexTemplate.elementArray[0];
                break;
            case ElementType.Air:
                _indexTemplate.frameImage.sprite = _indexTemplate.frameElementArray[1];
                _indexTemplate.elementImage.sprite = _indexTemplate.elementArray[1];
                break;
            case ElementType.Earth:
                _indexTemplate.frameImage.sprite = _indexTemplate.frameElementArray[2];
                _indexTemplate.elementImage.sprite = _indexTemplate.elementArray[2];
                break;
            default:
                _indexTemplate.frameImage.sprite = _indexTemplate.frameElementArray[3];
                _indexTemplate.elementImage.sprite = _indexTemplate.elementArray[3];
                break;
        }

        _indexTemplate.title.text = itemData.Title;
        _indexTemplate.description.text = itemData.Description;

        if (itemData == _insect)
        {
            _indexTemplate.actionUI1.sprite = _indexTemplate.actionSpriteArray[2];
            _indexTemplate.actionUI2.transform.parent.gameObject.SetActive(true);
            _indexTemplate.actionUI2.sprite = _indexTemplate.actionSpriteArray[1];
            _indexTemplate.actionArrow.SetActive(true);
        }
        else
        {
            _indexTemplate.actionUI2.transform.parent.gameObject.SetActive(false);
            _indexTemplate.actionArrow.SetActive(false);

            switch (itemData.Usage)
            {
                case ItemUsage.Crush:
                    _indexTemplate.actionUI1.sprite = _indexTemplate.actionSpriteArray[1];
                    break;
                case ItemUsage.Cut:
                    _indexTemplate.actionUI1.sprite = _indexTemplate.actionSpriteArray[2];
                    break;
                default:
                    _indexTemplate.actionUI1.sprite = _indexTemplate.actionSpriteArray[0];
                    if (itemData.CanBeStirred)
                    {
                        _indexTemplate.actionUI2.transform.parent.gameObject.SetActive(true);
                        _indexTemplate.actionArrow.SetActive(true);
                        _indexTemplate.actionUI2.sprite = _indexTemplate.actionSpriteArray[3];
                    }
                    break;
            }
        }
    }
}
