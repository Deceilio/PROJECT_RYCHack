using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_HeadArmourSlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script
        public Image icon; // Reference to the armour equipment icon
        HeadArmour item; // Reference to the certain head armour 

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(HeadArmour headArmour)
        {
            if (headArmour != null)
            {
                item = headArmour;
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
            // BELOW CODE: UI manager head armour slot is true
            player.playerUIManager.headArmourSlotSelected = true;
            player.playerUIManager.itemStatsWindowUI.UpdateArmourItemStats(item);
        }
    }
}