using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Potion
{
    public class PotionInventory : MonoBehaviour
    {
        [SerializeField] private PotionListData potionResultListData;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _slotContainer;

        public List<PotionResult> PotionList => potionResultListData.PotionResults;

        public void UpdatePotionCanvas()
        {
            foreach (Transform child in _slotContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < PotionList.Count; i++)
            {
                GameObject newSlot = Instantiate(_slotPrefab, _slotContainer);
                TextMeshProUGUI nameText = newSlot.GetComponentInChildren<TextMeshProUGUI>();

                Image[] images = newSlot.GetComponentsInChildren<Image>();
                Image potionImage = null;

                foreach (Image img in images)
                {
                    if (img.gameObject != newSlot)
                    {
                        potionImage = img;
                        break;
                    }
                }

                nameText.text = PotionList[i].Recipe.RecipeName;
                potionImage.sprite = PotionList[i].Recipe.PotionSprite;
            }
        }
    }
}