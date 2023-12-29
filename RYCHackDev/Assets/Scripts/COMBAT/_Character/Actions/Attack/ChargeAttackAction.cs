using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Item Actions/Attack/Charge Attack Action")]
    public class ChargeAttackAction : WRLD_ITEM_ACTION
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0)
                return;

            //player.playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

            // BELOW CODE: Play weapon trail fx
            //character.characterStatesManager.PlayWeaponFX(false);

           // BELOW CODE: If we can do a combo, then we will do combo
            if (character.canDoCombo)
            {
                UseChargeWeaponCombo(character);
            }
            // BELOW CODE: If not, we perform a regular attack
            else
            {
                // NOTE: This Fixes the Spamming Issue with weapon
                if (character.isPerformingAction)
                    return;
                if (character.canDoCombo)
                    return;

                UseChargeAttack(character);
            }
        }
        private void UseChargeAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_charge_attack_1, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_1;
            }
            else if (character.isUsingRightHand)
            {
                if(character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_charge_attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_1;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_charge_attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_1;
                }
            }
            else
            {
                // NOTE: This Fixes the Spamming Issue with weapon
                if (character.isPerformingAction)
                    return;
                if (character.canDoCombo)
                    return;
            }
        }
        private void UseChargeWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charge_attack_1)
                    {
                        character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_charge_attack_2, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_2;
                    }
                    else
                    {
                        character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_charge_attack_1, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_1;
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if(character.isTwoHandingWeapon)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_charge_attack_1)
                        {
                            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_charge_attack_2, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_2;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_charge_attack_1, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_1;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charge_attack_1)
                        {
                            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_charge_attack_2, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_2;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_charge_attack_1, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_1;
                        }
                    }
                }
            }
        }
    }
}