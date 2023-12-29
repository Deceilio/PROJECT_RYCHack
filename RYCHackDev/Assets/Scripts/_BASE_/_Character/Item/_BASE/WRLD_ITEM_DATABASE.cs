using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Deceilio.Psychain
{
    public class WRLD_ITEM_DATABASE : MonoBehaviour
    {
        public static WRLD_ITEM_DATABASE instance; // Static object for this script

        public List<MeleeWeaponItem> meleeWeaponItems = new List<MeleeWeaponItem>(); // List for all the weapon items
        public List<WRLD_ARMOUR_ITEM> armourItems = new List<WRLD_ARMOUR_ITEM>(); // List for all the armour items
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public MeleeWeaponItem GetMeleeWeaponItemByID(int meleeWeaponID)
        {
            return meleeWeaponItems.FirstOrDefault(meleeWeapon => meleeWeapon.itemID == meleeWeaponID);
        }
        public WRLD_ARMOUR_ITEM GetArmourItemByID(int armourID)
        {
            return armourItems.FirstOrDefault(armour => armour.itemID == armourID);
        }
    }
}