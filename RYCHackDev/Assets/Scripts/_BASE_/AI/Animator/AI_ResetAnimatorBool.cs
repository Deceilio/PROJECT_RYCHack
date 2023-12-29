using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class AI_ResetAnimatorBool : WRLD_RESET_ANIMATOR_BOOL
    {
        AICharacterManager aiCharacter; // Reference to the A.I Character Manager script

        override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, animatorStateInfo, layerIndex);

            if(aiCharacter == null)
            {
                aiCharacter = animator.GetComponent<AICharacterManager>(); 
            }
            // BELOW CODE: This is called when actions ends, and the states return to "empty"
            aiCharacter.isPhaseShifting = false;
        }
    }
}