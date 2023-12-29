using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_RESET_IS_JUMPING : StateMachineBehaviour
    {
        CharacterManager character; // Reference to the Character Manager script
        override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if(character == null)
            {
                character = animator.GetComponent<CharacterManager>(); 
            }

            // BELOW CODE: This is called when actions ends, and the states return to "empty"
            character.canMove = false;
            character.canRotate = false;
            character.isJumping = false;
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if(character == null)
            {
                character = animator.GetComponent<CharacterManager>(); 
            }

            // BELOW CODE: This is called when actions ends, and the states return to "empty"
            character.canMove = true;
            character.canRotate = true;
        }
    }
}