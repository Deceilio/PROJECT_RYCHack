using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_WEAPON_ITEM : WRLD_ITEM
    {
        [Header("WEAPON MODEL")]
        [Tooltip("Prefab for the weapon model")]
        public GameObject modelPrefab; // Prefab model for the weapon

        [Header("WEAPON STATE")]
        [Tooltip("Checks if the weapon is unArmed or not")]
        public bool isUnarmed; // Check if player is armed with existing weapon or not
        
        [Header("WEAPON ANIMATOR")]
        [Tooltip("Choose a specific animator controller for this weapon")]
        public AnimatorOverrideController weaponController; // Reference to the weapon controller for different animators
        //[Tooltip("Idle animation for off hand weapons")]
        //public string offHandIdleAnimation = "Left_Arm_Idle_01"; // Idle animation for off hand weapons
        
        [Header("WEAPON TYPE")]
        [Tooltip("Type of Weapon (Unarmed, Straight Sword, SmallShield, Shield, Bow")]
        public WeaponHoldingType weaponHoldingType; // Reference to the Weapon Type enum

        [Header("WEAPON TYPE")]
        [Tooltip("Type of Weapon (Unarmed, Straight Sword, SmallShield, Shield, Bow")]
        public WeaponType weaponType; // Reference to the Weapon Type enum

        [Header("SKILL TYPE")]
        [Tooltip("Type of Skills embedded on the Weapon (None, Technician, PyromancyCaster, FaithCaster)")]
        public WeaponSkillType weaponSkillType; // Reference to the Weapon Type enum

        [Header("IK SUPPORTED")]
        [Tooltip("Does your weapon going to use IK")]
        public bool ikEnabled = false; // Check if the IK is enabled on the weapon

        [Header("WEAPON DAMAGE")]
        [Tooltip("Physical damage of the weapon")]
        public int physicalDamage = 25; // Physical damage of the weapon
        [Tooltip("Fire damage of the weapon")]
        public int fireDamage; // Fire damage of the weapon

        [Header("WEAPON DAMAGE MODIFIERS")]
        [Tooltip("Damage Multiplier for the Light Attack")]
        public float lightAttackDamageModifier = 1; // Reference to the light attack damage modifier
        [Tooltip("Damage Multiplier for the Heavy Attack")]
        public float heavyAttackDamageModifier = 2; // Reference to the heavy attack damage modifier
        [Tooltip("Damage Multiplier for the Guard Break")]
        public int guardBreakModifier = 1; // Reference to the guard break damage modifier
        [Tooltip("Damage Multiplier for the Critical Attack")]
        public int criticalDamageMultiplier = 4; // Critical damage multiplier for the weapon
        // Running attack modifier
        // Jumping attack modifier

        [Header("WEAPON POISE")]
        [Tooltip("The value of poise break for the weapon.")]
        public float poiseDamage; // The value of poise break
        [Tooltip("The value of poise dealt by attacking with the weapon.")]
        public float attackingPoiseBonus; // The value of poise dealt by Attacking with a weapon

        [Header("DAMAGE ABSORPTION")]
        [Tooltip("Physical damage absorption for the blocking weapon.")]
        public float physicalBlockingDamageAbsorption; // Physical damage absorption for the blocking weapon
        [Tooltip("Fire damage absorption for the blocking weapon.")]
        public float fireBlockingDamageAbsorption; // Physical damage absorption for the blocking weapon

        [Header("STABILITY")]
        [Tooltip("Determine how much stamina is lost while trying to block by a blocking weapon.")]
        public int stability = 67; // Determine how much stamina is lost while trying to block by a blocking weapon

        [Header("WEAPON STAMINA COSTS")]
        [Tooltip("Base stammina cost for the weapon")]
        public int baseStaminaCost; // Base stamina cost for the particular action
        [Tooltip("Base stammina cost when used light attack by this weapon")]
        public float lightAttackStaminaMultiplier; // Check spam light attack, each light attack will count individually
        [Tooltip("Base stammina cost when used heavy attack by this weapon")]
        public float heavyAttackStaminaMultiplier; // Check Spam heavy attack, each heavy attack will count individually

        [Header("WEAPON ITEM ACTIONS")]
        [Header("ONE HANDED ACTIONS")]
        [Tooltip("Use for one handed light attack inputs")]
        public WRLD_ITEM_ACTION oh_Tap_RB_Action; // Item actions for tap RB input actions (light attack)
        [Tooltip("Use for one handed heavy attack inputs")]
        public WRLD_ITEM_ACTION oh_Tap_RT_Action; // Item actions for tap RT input actions (heavy attack)
        [Tooltip("Use for one handed critical attack inputs")]
        public WRLD_ITEM_ACTION oh_Hold_RB_Action; // Item actions for hold RB input actions (critical attack)
        [Tooltip("Use for one handed charge attack inputs")]
        public WRLD_ITEM_ACTION oh_Hold_RT_Action; // Item actions for hold RT input actions (charge attack)
        [Tooltip("Use for one handed skill action inputs")]
        public WRLD_ITEM_ACTION oh_Tap_LB_Action; // Item actions for tap LB input actions (use skill)
        [Tooltip("Use for one handed parry action inputs")]
        public WRLD_ITEM_ACTION oh_Tap_LT_Action; // Item actions for tap LT input actions (parry)
        [Tooltip("Use for one handed blocking action inputs")]
        public WRLD_ITEM_ACTION oh_Hold_LB_Action; // Item actions for hold LB input actions (blocking)

        [Header("TWO HANDED ACTIONS")]
        [Tooltip("Use for two handed light attack inputs")]
        public WRLD_ITEM_ACTION th_Tap_RB_Action; // Item actions for tap RB input actions (light attack)
        [Tooltip("Use for two handed heavy attack inputs")]
        public WRLD_ITEM_ACTION th_Tap_RT_Action; // Item actions for tap RT input actions (heavy attack)
        [Tooltip("Use for two handed critical attack inputs")]
        public WRLD_ITEM_ACTION th_Hold_RB_Action; // Item actions for hold RB input actions (critical attack)
        [Tooltip("Use for two handed charge attack inputs")]
        public WRLD_ITEM_ACTION th_Hold_RT_Action; // Item actions for hold RT input actions (charge attack)
        [Tooltip("Use for two handed skill action inputs")]
        public WRLD_ITEM_ACTION th_Tap_LB_Action; // Item actions for tap LB input actions (use skill)
        [Tooltip("Use for two handed parry action inputs")]
        public WRLD_ITEM_ACTION th_Tap_LT_Action; // Item actions for tap LT input actions (parry)
        [Tooltip("Use for two handed blocking action inputs")]
        public WRLD_ITEM_ACTION th_Hold_LB_Action; // Item actions for hold LB input actions (blocking)

        [Header("WEAPON SOUND FX")]
        [Tooltip("List of all the weapon effects sounds")]
        public AudioClip[] weaponEffectSound; // Weapon slashes sound effects 
        [Tooltip("List of all the block effects sounds")]
        public AudioClip[] blockEffectSound; // Block noises sound effects 
    }
}
