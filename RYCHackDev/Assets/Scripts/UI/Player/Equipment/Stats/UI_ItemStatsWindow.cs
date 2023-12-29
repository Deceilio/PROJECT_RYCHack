using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deceilio.Psychain
{
    public class UI_ItemStatsWindow : MonoBehaviour
    {
        [Header("ITEMS STATS DATA")]
        public TextMeshProUGUI itemIDText; // ID of the item
        public TextMeshProUGUI itemNameText; // Name of the item
        public Image itemIconImage; // Image of the item icon
        public TextMeshProUGUI itemLoreText; // Item lore text

        [Header("EQUIPMENT STATS WINDOWS")]
        public GameObject weaponStats; // Reference to the weapon stats game object
        public GameObject armourStats; // Reference to the armour stats game object
        public GameObject skillStats; // Reference to the armour stats game object

        [Header("WEAPON STATS")]
        public TextMeshProUGUI physicalDamageText; // Reference to the physical damage text object
        public TextMeshProUGUI physicalAbsorptionText; // Reference to the physical damage absorption text object

        [Header("SKILL STATS")]
        public TextMeshProUGUI skillCostText; // Reference to the skill cost text object
        public TextMeshProUGUI skillTypeText; // Reference to the skill type text object

        [Header("ARMOUR STATS")]
        public TextMeshProUGUI armourPhysicalAbsorptionText; // Reference to the armour physical absorption text object
        public TextMeshProUGUI armourPoisonAbsorptionText; // Reference to the armour poison absorption text object
        
        public void UpdateWeaponItemStats(WRLD_WEAPON_ITEM weapon)
        {
            CloseAllStatWindows();
            if (weapon != null)
            {
                if (weapon.itemID != 0)
                {
                    itemIDText.text = weapon.itemID.ToString();
                }
                else
                {
                    itemIDText.text = "";
                }

                if (weapon.itemName != null)
                {
                    itemNameText.text = weapon.itemName;
                }
                else
                {
                    itemNameText.text = "";
                }

                if (weapon.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.enabled = true;
                    itemIconImage.sprite = weapon.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.enabled = false;
                    itemIconImage.sprite = null;
                }

                if (weapon.itemLore != null)
                {
                    itemLoreText.text = weapon.itemLore;
                }
                else
                {
                    itemLoreText.text = "";
                }

                physicalDamageText.text = weapon.physicalDamage.ToString();
                weaponStats.SetActive(true);
            }
            else
            {
                itemIDText.text = "";
                itemNameText.text = "";
                itemLoreText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                weaponStats.SetActive(false);
            }
        }

        public void UpdateArmourItemStats(WRLD_ARMOUR_ITEM armour)
        {
            CloseAllStatWindows();
            if (armour != null)
            {
                if (armour.itemID != 0)
                {
                    itemIDText.text = armour.itemID.ToString();
                }
                else
                {
                    itemIDText.text = "";
                }

                if (armour.itemName != null)
                {
                    itemNameText.text = armour.itemName;
                }
                else
                {
                    itemNameText.text = "";
                }

                if (armour.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.enabled = true;
                    itemIconImage.sprite = armour.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.enabled = false;
                    itemIconImage.sprite = null;
                }

                if (armour.itemLore != null)
                {
                    itemLoreText.text = armour.itemLore;
                }
                else
                {
                    itemLoreText.text = "";
                }

                armourPhysicalAbsorptionText.text = armour.physicalDefense.ToString();
                armourPoisonAbsorptionText.text = armour.poisonResistance.ToString();   
                armourStats.SetActive(true);
            }
            else
            {
                itemIDText.text = "";
                itemNameText.text = "";
                itemLoreText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                armourStats.SetActive(false);
            }
        }

        private void CloseAllStatWindows()
        {
            weaponStats.SetActive(false);
            armourStats.SetActive(false);
            skillStats.SetActive(false);
        }

        // TO:DO: Update etc/misc item stats
    }
}