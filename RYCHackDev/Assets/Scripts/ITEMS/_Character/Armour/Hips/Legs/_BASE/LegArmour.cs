using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Items/Armours/Leg Armour")]
    public class LegArmour : WRLD_ARMOUR_ITEM
    {
        [Header("MODEL NAME")]
        public string hipModelName; // Reference to the hip model
        public string leftLegName; //Reference to the left leg model
        public string rightLegName; //Reference to the right leg model
        // Left knee pad name, if we need it
        // Right knee pad name, if we need it
    }
}