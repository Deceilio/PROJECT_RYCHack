using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Item Actions/Attack/Heavy Attack Action")]
    public class HeavyAttackActions : WRLD_ITEM_ACTION
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0)
            return;


            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKForWeapon();
            // BELOW CODE: Play weapon trail fx
            //character.characterStatesManager.PlayWeaponFX(false);

            // BELOW CODE: If we can do jumping attack, then we will do jumping attack, if not then continue
            if (character.isSprinting)
            {
                // BELOW CODE: Play weapon trail fx
                character.characterStatesManager.PlayWeaponFX(false);
                // BELOW CODE: Handle jumping attack
                UseJumpingAttack(character);
                return;
            }

            // BELOW CODE: If we can do a combo, then we will do combo
            // NOTE: This is disable for now because I didn't added heavy attack combos animations yet!
            //if (player.canDoCombo)
            //{
            //    UseHeavyWeaponCombo(character);
            //}
            // BELOW CODE: If not, we perform a regular attack 
            if (character.isPerformingAction) // This fixes the spamming issues with weapon
                return;
            if (character.canDoCombo)
                return;

            UseHeavyAttack(character);
            character.characterCombatManager.currentAttackType = AttackType.Heavy;
        }
        private void UseHeavyAttack(CharacterManager character)
        {
            if(character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_Heavy_Attack_1, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Heavy_Attack_1;
            }
            else if(character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_Heavy_Attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_Heavy_Attack_1;
                }
                else 
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_Heavy_Attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Heavy_Attack_1;
                }
            }
        }

        private void UseJumpingAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_jumping_attack_1, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_jumping_attack_1;
            }
            else if (character.isUsingRightHand)
            {
                if(character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_jumping_attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_jumping_attack_1;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_jumping_attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_jumping_attack_1;
                }
            }
        }

        // private void UseHeavyWeaponCombo(CharacterManager character)
        // {
        //    if (character.canDoCombo)
        //    {
        //        character.characterAnimatorManager.animator.SetBool("canDoCombo", false);

        //        if (character.isUsingLeftHand)
        //        {
        //            // Change the left hand combo animation here
        //            if (character.characterCombatManager.lastAttack == character.characterCombatManager.heavy_Attack_1)
        //            {
        //                character.characterAnimatorManager.animator.SetBool("canDoCombo", false);
        //                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.heavy_Attack_2, true);
        //                character.characterCombatManager.lastAttack2 = character.characterCombatManager.heavy_Attack_2;

        //                character.characterCombatManager.StartCoroutine(character.characterCombatManager.UseLastAttack());

        //            }

        //            if (character.characterCombatManager.lastAttack == character.characterCombatManager.heavy_Attack_2)
        //            {
        //                character.characterAnimatorManager.animator.SetBool("canDoCombo", false);
        //                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.heavy_Attack_3, true);
        //            }
        //        }
        //        else if (character.isUsingRightHand)
        //        {
        //            if (character.characterCombatManager.lastAttack == character.characterCombatManager.heavy_Attack_1)
        //            {
        //                character.characterAnimatorManager.animator.SetBool("canDoCombo", false);
        //                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.heavy_Attack_2, true);
        //                character.characterCombatManager.lastAttack2 = character.characterCombatManager.heavy_Attack_2;

        //                character.characterCombatManager.StartCoroutine(character.characterCombatManager.UseLastAttack());

        //            }

        //            if (character.characterCombatManager.lastAttack == character.characterCombatManager.heavy_Attack_2)
        //            {
        //                character.characterAnimatorManager.animator.SetBool("canDoCombo", false);
        //                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.heavy_Attack_3, true);
        //            }
        //        }
        //    }
        // }
    
    }
}
