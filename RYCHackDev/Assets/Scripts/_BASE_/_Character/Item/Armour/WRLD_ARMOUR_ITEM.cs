using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_ARMOUR_ITEM : WRLD_ITEM
    {
        [Header("ARMOUR DEFENSE BONUS")]
        public float physicalDefense; // Reference to the bonus rate of the physical defense

        [Header("WEIGHT")]
        public float weight = 0; // Weight of the armour

        [Header("ARMOUR RESISTANCES")]
        public float poisonResistance; // Armour resistance of poison  
        // Magic defense (In our game there is no magic, so it will be something else ig)
        // Fire defense, Lighting Defense, Darkness Defense (In in our game we have elemental damage)
    }
}