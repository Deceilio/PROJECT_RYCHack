using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_HandArmourSlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script
        public Image icon; // Reference to the armour equipment icon
        HandArmour item; // Reference to the certain hand armour 
        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(HandArmour handArmour)
        {
            if (handArmour != null)
            {
                item = handArmour;
                icon.sprite = item.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                ClearItem();
            }
        }
        public void ClearItem()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            //gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            // BELOW CODE: UI manager hand armour slot is true
            player.playerUIManager.handArmourSlotSelected = true;
            player.playerUIManager.itemStatsWindowUI.UpdateArmourItemStats(item);
        }
    }
}