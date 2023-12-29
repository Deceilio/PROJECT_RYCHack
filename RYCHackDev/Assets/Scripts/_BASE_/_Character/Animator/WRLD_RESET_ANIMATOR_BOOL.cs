using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_RESET_ANIMATOR_BOOL : StateMachineBehaviour
    {
        CharacterManager character; // Reference to the Character Manager script
        override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if(character == null)
            {
                character = animator.GetComponent<CharacterManager>(); 
            }

            // BELOW CODE: This is called when actions ends, and the states return to "empty"
            character.canMove = true;
            character.canRotate = true;
            character.canRoll = true;
            character.isPerformingAction = false;
            character.applyRootMotion = false;
            character.isJumping = false;
            character.canDoCombo = false;
            character.isInvulerable = false;
            character.isFiringSkill = false;
            character.isUsingLeftHand = false;
            character.isUsingRightHand = false;
            character.isPerformingBackstab = false;
            character.isPerformingRiposte = false;
            character.isBeingBackstabbed = false;
            character.isBeingRiposted = false;
            character.canBeParried = false;
            character.canBeRiposted = false;
            character.isAttacking = false;
            character.characterCombatManager.previousPoiseDamageTaken = 0; // After the damage animation ends reset the poise damage
        }
    }
}