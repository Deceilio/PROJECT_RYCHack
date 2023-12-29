using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Items/Armours/Hand Armour")]
    public class HandArmour : WRLD_ARMOUR_ITEM
    {
        public string leftHandArmourName; // Reference to the left hand armour 
        public string rightHandArmourName; //Reference to the right hand armour 
        public string leftForeArmModelName; //Reference to the left forearm armour 
        public string rightForeArmModelName; //Reference to the right forearmour armour
    }
}
