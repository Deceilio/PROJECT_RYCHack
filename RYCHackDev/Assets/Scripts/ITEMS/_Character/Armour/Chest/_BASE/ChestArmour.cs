using UnityEngine;

namespace Deceilio.Psychain 
{
    [CreateAssetMenu(menuName = "Psychain/Items/Armours/Chest Armour")]
    public class ChestArmour : WRLD_ARMOUR_ITEM
    {
        public string chestArmourName; // Reference to the head armour name
        public string leftUpperArmModelName; // Reference to the left upper arm armour
        public string rightUpperArmModelName; // Reference to the right upper arm armour
        // Left elbow pad, if needed
        // Right elbow pad, if needed
    }
}
