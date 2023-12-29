using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deceilio.Psychain
{
    public class UI_SkillWindow : MonoBehaviour
    {
        public UI_ItemStatsWindow itemStatsWindow; // Reference to the UI Item Stats Window script

        [Header("SKIL MENU OBJECT")]
        public GameObject skillInventorySlot; // Reference to the skill inventory slot to spawn
        public GameObject contentPanel; // Reference to the content panel

        private void Awake()
        {
            itemStatsWindow = FindObjectOfType<UI_ItemStatsWindow>();
        }
        public void AddButtonListeners(Button button, TextMeshProUGUI buttonText, WRLD_SKILL_ITEM skillData)
        {
            button.onClick.AddListener(() => OnButtonClick(buttonText, skillData));
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry hoverEntry = new EventTrigger.Entry();
            hoverEntry.eventID = EventTriggerType.PointerEnter;
            hoverEntry.callback.AddListener((eventData) => OnButtonHover(buttonText, skillData));
            trigger.triggers.Add(hoverEntry);
        }

        private void OnButtonClick(TextMeshProUGUI buttonText, WRLD_SKILL_ITEM skillData)
        {
            itemStatsWindow.itemIDText.text = skillData.itemID.ToString();
            itemStatsWindow.itemNameText.text = skillData.itemName.ToString();
            itemStatsWindow.itemIconImage.enabled = true;
            itemStatsWindow.itemIconImage.sprite = skillData.itemIcon;
            itemStatsWindow.itemLoreText.text = skillData.itemLore.ToString();
            itemStatsWindow.skillCostText.text = skillData.skillPointCost.ToString(); // Change text when clicked
            itemStatsWindow.skillTypeText.text = skillData.skillType;
        }

        private void OnButtonHover(TextMeshProUGUI buttonText, WRLD_SKILL_ITEM skillData)
        {
            itemStatsWindow.itemIDText.text = skillData.itemID.ToString();
            itemStatsWindow.itemNameText.text = skillData.itemName.ToString();
            itemStatsWindow.itemIconImage.enabled = true;
            itemStatsWindow.itemIconImage.sprite = skillData.itemIcon;
            itemStatsWindow.itemLoreText.text = skillData.itemLore.ToString();
            itemStatsWindow.skillCostText.text = skillData.skillPointCost.ToString(); // Change text when hovered
            itemStatsWindow.skillTypeText.text = skillData.skillType;
        }
    }
}