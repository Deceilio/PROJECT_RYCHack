using UnityEngine;

namespace Deceilio.Psychain
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        [HideInInspector] public PlayerManager player; // Reference to the Player Manager script        
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public override void DrainStaminaBasedOnAttack()
        {
            if(player.isUsingRightHand)
            {
                if(currentAttackType == AttackType.Light)
                {
                    player.playerStatsManager.DeductStamina(player.playerInventoryManager.rightHandWeapon.baseStaminaCost * player.playerInventoryManager.rightHandWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.Heavy)
                {
                    player.playerStatsManager.DeductStamina(player.playerInventoryManager.rightHandWeapon.baseStaminaCost * player.playerInventoryManager.rightHandWeapon.heavyAttackStaminaMultiplier);
                }
            }
            else if(player.isUsingLeftHand)
            {
                if (currentAttackType == AttackType.Light)
                {
                    player.playerStatsManager.DeductStamina(player.playerInventoryManager.leftHandWeapon.baseStaminaCost * player.playerInventoryManager.leftHandWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.Heavy)
                {
                    player.playerStatsManager.DeductStamina(player.playerInventoryManager.leftHandWeapon.baseStaminaCost * player.playerInventoryManager.leftHandWeapon.heavyAttackStaminaMultiplier);
                }
            }
        }
        public override void SuccessfullyCastSkill()
        {
            base.SuccessfullyCastSkill();
        }
    }
}