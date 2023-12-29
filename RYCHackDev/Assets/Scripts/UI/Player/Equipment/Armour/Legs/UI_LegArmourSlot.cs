using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_LegArmourSlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script
        public Image icon; // Reference to the armour equipment icon
        LegArmour item; // Reference to the certain leg armour

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(LegArmour legArmour)
        {
            if (legArmour != null)
            {
                item = legArmour;
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
            // BELOW CODE: UI manager leg armour slot is true
            player.playerUIManager.legArmourSlotSelected = true;
            player.playerUIManager.itemStatsWindowUI.UpdateArmourItemStats(item);
        }
    }
}