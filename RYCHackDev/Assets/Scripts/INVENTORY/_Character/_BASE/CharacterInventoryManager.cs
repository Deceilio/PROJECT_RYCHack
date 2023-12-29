using UnityEngine;

namespace Deceilio.Psychain
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterManager character; // Reference to the Character Manager Script

        [Header("CURRENT ITEM BEING USED")]
        public WRLD_WEAPON_ITEM currentItemBeingUsed; // Tells which item the character is currently using
        
        [Header("CURRENT SLOTS ITEMS")]    
        public WRLD_WEAPON_ITEM rightHandWeapon; // Right hand weapon item access
        public WRLD_WEAPON_ITEM leftHandWeapon;  // Left hand weapon item access
        
        [Header("QUICK SLOTS ITEMS")]
        public WRLD_WEAPON_ITEM[] weaponsInRightHandSlots = new WRLD_WEAPON_ITEM[0]; // Array to have list of weapon in your right hand
        public WRLD_WEAPON_ITEM[] weaponsInLeftHandSlots = new WRLD_WEAPON_ITEM[0]; // Array to have list of weapon in your left hand

        [Header("WEAPON INDEX")]
        public int currentRightWeaponIndex = 0; // Index number for the particular weapon in right hand
        public int currentLeftWeaponIndex = 0; // Index number for the particular weapon in left hand 

        protected virtual void Awake()
        {
            character= GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {
            rightHandWeapon = weaponsInRightHandSlots[0];
            leftHandWeapon = weaponsInLeftHandSlots[0];
            character.characterWeaponSlotManager.LoadBothWeaponsOnSlots(); 
        }

        //BELOW CODE: Call in your save function after loading the equipment
        // public virtual void LoadBraceletEffects()
        // {
        //     if(braceletSlot01 != null)
        //     {
        //         braceletSlot01.EquipRing(character);
        //     }
        //     if (braceletSlot02 != null)
        //     {
        //         braceletSlot02.EquipRing(character);
        //     }
        //     if (braceletSlot03 != null)
        //     {
        //         braceletSlot03.EquipRing(character);
        //     }
        //     if (braceletSlot04 != null)
        //     {
        //         braceletSlot04.EquipRing(character);
        //     }
        // }
    }
}