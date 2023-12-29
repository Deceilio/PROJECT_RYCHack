using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Item Actions/Attack/Light Attack Action")]
    public class LightAttackAction : WRLD_ITEM_ACTION
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0)
            return;

            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKForWeapon();
            // BELOW CODE: Play weapon trail fx
            //character.characterStatesManager.PlayWeaponFX(false);

            // BELOW CODE: If we can do sprinting attack, then we will do sprinting attack, if not then continue
            if (character.isSprinting)
            {
                // BELOW CODE: Play weapon trail fx
                //character.characterStatesManager.PlayWeaponFX(false);
                // BELOW CODE: Handle Sprinting Attack
                UseSprintingAttack(character);
                return;
            }

            // If we can do a combo, then we will do combo
            if (character.canDoCombo)
            {
                UseLightWeaponCombo(character);
            }
            // If not, we perform a regular attack
            else
            {
                // NOTE: This fixes the spamming issue with the weapon
                if (character.isPerformingAction)
                    return;
                if (character.canDoCombo)
                    return;

                UseLightAttack(character);
            }

            character.characterCombatManager.currentAttackType = AttackType.Light;
        }

        private void UseLightAttack(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0)
            return;

            if(character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_Light_Attack_1, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Light_Attack_1;
            }
            else if (character.isUsingRightHand)
            {       
                if(character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_Light_Attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_Light_Attack_1;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_Light_Attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_Light_Attack_1;
                }
            }
        }

        private void UseSprintingAttack(CharacterManager character)
        {
            if(character.isUsingLeftHand)
            {
                //Change the Left Hand Animation Here
                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_sprint_attack_1, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_sprint_attack_1;
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_sprint_attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_sprint_attack_1;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_sprint_attack_1, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_sprint_attack_1;
                }
            }
        }

        private void UseLightWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Light_Attack_1)
                    {
                        character.animator.SetBool("canDoCombo", false);
                        character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_Light_Attack_2, true);
                        character.characterCombatManager.lastAttack2 = character.characterCombatManager.oh_Light_Attack_2;
                        character.characterCombatManager.StartCoroutine(character.characterCombatManager.UseLastAttack());
                    }
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Light_Attack_2)
                    {
                        character.animator.SetBool("canDoCombo", false);
                        character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_Light_Attack_3, true);
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHandingWeapon)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_Light_Attack_1)
                        {
                            character.animator.SetBool("canDoCombo", false);
                            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_Light_Attack_2, true);
                            character.characterCombatManager.lastAttack2 = character.characterCombatManager.th_Light_Attack_2;
                            character.characterCombatManager.StartCoroutine(character.characterCombatManager.UseLastAttack());
                        }
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_Light_Attack_2)
                        {
                            character.animator.SetBool("canDoCombo", false);
                            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.th_Light_Attack_3, true);
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Light_Attack_1)
                        {
                            character.animator.SetBool("canDoCombo", false);
                            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_Light_Attack_2, true);
                            character.characterCombatManager.lastAttack2 = character.characterCombatManager.oh_Light_Attack_2;
                            character.characterCombatManager.StartCoroutine(character.characterCombatManager.UseLastAttack());
                        }
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_Light_Attack_2)
                        {
                            character.animator.SetBool("canDoCombo", false);
                            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.oh_Light_Attack_3, true);
                        }
                    }
                }
            }
        }
    }
}