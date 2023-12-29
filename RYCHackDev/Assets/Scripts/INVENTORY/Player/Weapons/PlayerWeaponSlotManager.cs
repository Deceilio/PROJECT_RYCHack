using UnityEngine;

namespace Deceilio.Psychain
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        [HideInInspector] public PlayerManager player; // Reference to the Player Manager script
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public override void LoadWeaponOnSlot(WRLD_WEAPON_ITEM weaponItem, bool isLeft)
        {
            if(weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageHitbox();
                    player.quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
                    //player.playerAnimatorManager.PlayTargetActionAnimation(weaponItem.offHandIdleAnimation, false, true, true, true);
                }
                else
                {
                    if(player.isTwoHandingWeapon)
                    {
                        // BELOW CODE: Move current left hand weapon to the back or disable it
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        player.playerAnimatorManager.PlayTargetActionAnimation("Left Arm Empty", false, true, true, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();              
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageHitbox();
                    player.quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
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
                    player.quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
                    //player.playerAnimatorManager.PlayTargetActionAnimation(weaponItem.offHandIdleAnimation, false, true, true, true);
                }
                else 
                {
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageHitbox();
                    player.quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem); 
                    player.animator.runtimeAnimatorController = weaponItem.weaponController; 
                }   
            }
        }
        public void SuccessfullyThrowFireBomb()
        {
            Destroy(player.playerStatesManager.instantiatedFXModel);
            BombConsumableItem bombItem = player.playerStatesManager.currentConsumableBeingUsed as BombConsumableItem;
            
            GameObject activeBombModel = Instantiate(bombItem.bombModel, rightHandSlot.transform.position, player.playerCameraManager.cameraPivotTransform.rotation);
            activeBombModel.transform.rotation = Quaternion.Euler(player.playerCameraManager.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
        
            // BELOW CODE: Detect bomb damage collider
            WRLD_BOMB_DAMAGE_HITBOX damageCollider = activeBombModel.GetComponentInChildren<WRLD_BOMB_DAMAGE_HITBOX>();
            damageCollider.explosionDamage = bombItem.baseDamage;
            damageCollider.explosionSplashDamage = bombItem.explosiveDamage;
            // BELOW CODE: Add force to the rigidbody to move it through the air
            damageCollider.bombRigidBody.AddForce(activeBombModel.transform.forward * bombItem.forwardVelocity);
            damageCollider.bombRigidBody.AddForce(activeBombModel.transform.up * bombItem.upwardVelocity);
            // BELOW CODE: Check for friendly fire
            damageCollider.entityTeamIDNumber = player.playerStatsManager.entityTeamIDNumber;
            LoadWeaponOnSlot(player.playerInventoryManager.rightHandWeapon, false);

            // BELOW CODE: Create explosion and after splash damage/effects
        }
    }
}