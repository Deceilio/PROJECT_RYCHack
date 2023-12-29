using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_LegArmourInventorySlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script

        public Image icon; // Slot icon
        LegArmour item; // Reference to armour item 
        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(LegArmour newItem)
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
            if (player.playerUIManager.legArmourSlotSelected)
            {
                //B ELOW CODE: Add the current equipped leg armour (if any) to our leg inventory
                if (player.playerEquipmentManager.currentLegArmour != null)
                {
                    player.playerEquipmentManager.legArmourInventory.Add(player.playerEquipmentManager.currentLegArmour);
                }
                // BELOW CODE: Remove the current equipped leg armour and replace it with our new leg armour
                player.playerEquipmentManager.currentLegArmour = item;
                // BELOW CODE: Remove our newly equipped leg armour from our inventory
                player.playerEquipmentManager.legArmourInventory.Remove(item);
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