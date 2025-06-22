using System.Linq;
using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] private ScriptableQuestHolder _scriptableQuestHolder;
        [SerializeField] private Image[] _potionFramesArray;
        [SerializeField] private Sprite[] _daySprites;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _dayImage;

        private void OnEnable()
        {
            _text.text = null;
            _dayImage.sprite = _daySprites[_dayNightCycleInfo.ActualDay];

            foreach (Image image in _potionFramesArray)
            {
                image.gameObject.SetActive(false);
            }

            if (_scriptableQuestHolder.QuestInfo != null)
            {
                for (int i = 0; i < _scriptableQuestHolder.QuestInfo.quests.Length; i++)
                {
                    _text.text += _scriptableQuestHolder.QuestInfo.quests[i].text + "\n \n";
                    _potionFramesArray[i].gameObject.SetActive(true);
                    _potionFramesArray[i].sprite = _scriptableQuestHolder.QuestInfo.quests[i].potion.PotionSprite;
                }
            }
        }
    }
}
