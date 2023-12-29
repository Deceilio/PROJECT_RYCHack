using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/AI/Advance Actions/Item Based Attack Action")]
    public class ItemBasedAttackActions : ScriptableObject
    {
        [Header("ATTACK TYPE")]
        public AIAttackActionType actionAttackType = AIAttackActionType.meleeAttackAction; // Choose the attack action of the item
        public AttackType attackType = AttackType.Light; // Choose the attack type of the item

        [Header("ACTION COMBO SETTINGS")]
        public bool actionCanCombo = false; // Check if the item can perform combo or not

        [Header("WHICH HAND ACTION?")]
        bool isRightHandedAction = true; // Check which hand item is using

        [Header("ACTION SETTINGS")]
        // BELOW CODE: Every attack will have an attack score,
        // BELOW CODE: The higher the score is, the more chance to play that attack
        // BELOW CODE: For example: If A.I Character have 5 attacks, but bite attack is the highest attack score
        // BELOW CODE: Then it is more likely, that attack will play more and play repeatedly, whatever the case.
        public int attackScore = 3;
        public int recoveryTime; // A.I Character recover time after attacking
        public float maximumAttackAngle = 35; // Maximum angle for the particular Attack
        public float minimumAttackAngle = -35; // Minimum angle for the particular Attack
        public float minimumDistanceNeededToAttack = 0; // Minimum distance req by A.I Character to attack 
        public float maximumDistanceNeededToAttack = 3; // Maximum distance req by A.I Character to attack 

        public void PerformAttackAction(AICharacterManager aiCharacter)
        {
            if(isRightHandedAction)
            {
                aiCharacter.UpdateWhichHandPlayerIsUsing(true);
                PerformRightHandItemActionBasedOnAttackType(aiCharacter);
            }
            else
            {
                aiCharacter.UpdateWhichHandPlayerIsUsing(false);
                PerformLeftHandItemActionBasedOnAttackType(aiCharacter);
            }
        }

        // BELOW CODE: Decide which hand perform the actions
        private void PerformRightHandItemActionBasedOnAttackType(AICharacterManager aiCharacter)
        {
            if(actionAttackType == AIAttackActionType.meleeAttackAction)
            {
                // BELOW CODE: Perform right hand melee actions
                PerformRightHandMeleeAction(aiCharacter);
            }
            else if(actionAttackType != AIAttackActionType.rangedAttackAction)
            {
                // TO-DO: Perform right hand range actions
            }
        }
        private void PerformLeftHandItemActionBasedOnAttackType(AICharacterManager aiCharacter)
        {
            if (actionAttackType == AIAttackActionType.meleeAttackAction)
            {
                // BELOW CODE: Perform left hand melee actions
                PerformLeftHandMeleeAction(aiCharacter);
            }
            else if (actionAttackType != AIAttackActionType.rangedAttackAction)
            {
                // TO-DO: Perform left hand range actions
            }
        }

        // BELOW CODE: Right hand actions
        private void PerformRightHandMeleeAction(AICharacterManager aiCharacter)
        {
            if(aiCharacter.isTwoHandingWeapon)
            {
                if (attackType == AttackType.Light)
                {
                    aiCharacter.characterInventoryManager.rightHandWeapon.th_Tap_RB_Action.PerformAction(aiCharacter);
                }
                else if (attackType == AttackType.Heavy)
                {
                    aiCharacter.characterInventoryManager.rightHandWeapon.th_Tap_RT_Action.PerformAction(aiCharacter);
                }
            }
            else
            {
                if (attackType == AttackType.Light)
                {
                    aiCharacter.characterInventoryManager.rightHandWeapon.oh_Tap_RB_Action.PerformAction(aiCharacter);
                }
                else if (attackType == AttackType.Heavy)
                {
                    aiCharacter.characterInventoryManager.rightHandWeapon.oh_Tap_RT_Action.PerformAction(aiCharacter);
                }
            }
        }

        private void PerformLeftHandMeleeAction(AICharacterManager aiCharacter)
        {
            if(aiCharacter.isTwoHandingWeapon)
            {
                if (attackType == AttackType.Light)
                {
                    aiCharacter.characterInventoryManager.leftHandWeapon.th_Tap_RB_Action.PerformAction(aiCharacter);
                }
                else if (attackType == AttackType.Heavy)
                {
                    aiCharacter.characterInventoryManager.leftHandWeapon.th_Tap_RT_Action.PerformAction(aiCharacter);
                }
            }
            else
            {
                if (attackType == AttackType.Light)
                {
                    aiCharacter.characterInventoryManager.leftHandWeapon.oh_Tap_RB_Action.PerformAction(aiCharacter);
                }
                else if (attackType == AttackType.Heavy)
                {
                    aiCharacter.characterInventoryManager.leftHandWeapon.oh_Tap_RT_Action.PerformAction(aiCharacter);
                }
            }
        }
    }
}