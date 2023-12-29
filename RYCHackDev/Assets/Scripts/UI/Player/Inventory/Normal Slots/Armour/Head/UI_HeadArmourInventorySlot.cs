using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_HeadArmourInventorySlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script

        public Image icon; // Slot icon
        HeadArmour item; // Reference to armour item 
        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(HeadArmour newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            // BELOW CODE: Remove the current item
            if (player.playerUIManager.headArmourSlotSelected)
            {
                // BELOW CODE: Add the current equipped head armour (if any) to our head inventory
                if (player.playerEquipmentManager.currentHeadArmour != null)
                {
                    player.playerEquipmentManager.headArmourInventory.Add(player.playerEquipmentManager.currentHeadArmour);
                }
                // BELOW CODE: Remove the current equipped head armour and replace it with our new head armour
                player.playerEquipmentManager.currentHeadArmour = item;
                // BELOW CODE: Remove our newly equipped head armour from our inventory
                player.playerEquipmentManager.headArmourInventory.Remove(item);
                // BELOW CODE: Load the new armour
                player.playerEquipmentManager.EquipAllArmourModels();
            }
            else // Condition if you didn't selected the equipment slot instead you clicked on inventory, so it should reset
            {
                return;
            }

            // BELOW CODE: Update the new armour to reflect on the ui/armour screen
            player.playerUIManager.equipmentWindowUI.LoadArmourOnEquipmentScreen(player.playerEquipmentManager);
            player.playerUIManager.ResetAllSelectedSlots();
        }
    }
}