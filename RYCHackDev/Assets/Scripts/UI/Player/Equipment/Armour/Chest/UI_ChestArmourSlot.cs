using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_ChestArmourSlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script
        public Image icon; // Reference to the armour equipment icon
        ChestArmour item; // Reference to the certain chest armour
        
        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(ChestArmour chestArmour)
        {
            if (chestArmour != null)
            {
                item = chestArmour;
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
            // BELOW CODE: UI manager chest armour slot is true
            player.playerUIManager.chestArmourSlotSelected = true;
            player.playerUIManager.itemStatsWindowUI.UpdateArmourItemStats(item);
        }
    }
}