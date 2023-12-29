using UnityEngine;

namespace Deceilio.Psychain
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager character; // Reference to the Character Manager script

        [Header("UNARMED WEAPON")]
        public WRLD_WEAPON_ITEM unArmedWeapon; // Weapon item for unarmed weapon, means no weapon hehe :)

        [Header("WEAPON SLOTS")]
        public CharacterWeaponHolderSlot leftHandSlot; // Slot for left hand weapon
        public CharacterWeaponHolderSlot rightHandSlot; // Slot for right hand weapon
        public CharacterWeaponHolderSlot backSlot; // Slot for back weapon 

        [Header("DAMAGE HITBOXE")]
        public WRLD_DAMAGE_HITBOX leftHandDamageHitbox; // Damage hitbox for left hand weapon
        public WRLD_DAMAGE_HITBOX rightHandDamageHitbox; // Damage hitbox for right hand weapon

        [Header("HAND IK TARGETS")]
        public RightHandIKTarget rightHandIKTarget; // Reference to the Right Hand IK Target script
        public LeftHandIKTarget leftHandIKTarget; // Reference to the Left Hand IK Target script

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>(); 
            LoadWeaponHolderSlots();
        }
        protected virtual void LoadWeaponHolderSlots()
        {
            CharacterWeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<CharacterWeaponHolderSlot>();
            foreach (CharacterWeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }

                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }

                else if(weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }
        public virtual void LoadBothWeaponsOnSlots()
        {
            LoadWeaponOnSlot(character.characterInventoryManager.rightHandWeapon, false);
            LoadWeaponOnSlot(character.characterInventoryManager.leftHandWeapon, true);
        }
        public virtual void LoadWeaponOnSlot(WRLD_WEAPON_ITEM weaponItem, bool isLeft)
        {
            if(weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageHitbox();
                }
                else
                {
                    if(character.isTwoHandingWeapon)
                    {
                        // BELOW CODE: Move current left hand weapon to the back or disable it
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        character.characterAnimatorManager.PlayTargetActionAnimation("Left Arm Empty", false, true, true, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();              
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageHitbox();
                    //LoadTwoHandIKTargets(character.isTwoHandingWeapon);
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else 
            {
                weaponItem = unArmedWeapon;

                if(isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageHitbox(); 
                    //character.characterAnimatorManager.PlayTargetActionAnimation(weaponItem.offHandIdleAnimation, false, true, true, true);
                }
                else 
                {
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageHitbox();
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }
        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

            character.characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }
        protected virtual void LoadLeftWeaponDamageHitbox()
        {
            leftHandDamageHitbox = leftHandSlot.currentWeaponModel.GetComponentInChildren<WRLD_DAMAGE_HITBOX>();
            leftHandDamageHitbox.character = GetComponentInParent<CharacterManager>();
            leftHandDamageHitbox.physicalDamage = character.characterInventoryManager.leftHandWeapon.physicalDamage;
            leftHandDamageHitbox.fireDamage = character.characterInventoryManager.leftHandWeapon.fireDamage;
            leftHandDamageHitbox.entityTeamIDNumber = character.characterStatsManager.entityTeamIDNumber;
            leftHandDamageHitbox.poiseDamage = character.characterInventoryManager.leftHandWeapon.poiseDamage;
        }
        protected virtual void LoadRightWeaponDamageHitbox()
        {
            rightHandDamageHitbox = rightHandSlot.currentWeaponModel.GetComponentInChildren<WRLD_DAMAGE_HITBOX>();
            rightHandDamageHitbox.character = GetComponentInParent<CharacterManager>();
            rightHandDamageHitbox.physicalDamage = character.characterInventoryManager.rightHandWeapon.physicalDamage;
            rightHandDamageHitbox.fireDamage = character.characterInventoryManager.rightHandWeapon.fireDamage;
            rightHandDamageHitbox.entityTeamIDNumber = character.characterStatsManager.entityTeamIDNumber;
            rightHandDamageHitbox.poiseDamage = character.characterInventoryManager.rightHandWeapon.poiseDamage;         
        }
        public virtual void OpenDamageCollider()
        {
            character.characterSoundFXManager.PlayRandomDamageSoundFX();
            
            if(character.isUsingRightHand)
            {
                rightHandDamageHitbox.EnableDamageCollider();
            }
            else if(character.isUsingLeftHand) 
            {
                leftHandDamageHitbox.EnableDamageCollider();
            } 
        }
        public virtual void CloseDamageCollider()
        {
            if(rightHandDamageHitbox != null) 
            {
                rightHandDamageHitbox.DisableDamageCollider();
            }

            if(leftHandDamageHitbox != null)
            {
                leftHandDamageHitbox.DisableDamageCollider();
            }
        }
        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.totalPoiseDefense + character.characterInventoryManager.currentItemBeingUsed.attackingPoiseBonus;
        }
        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.armourPoiseBonus;
        }
    }
}
