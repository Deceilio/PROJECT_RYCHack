using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_IS_PERFORMING_FULLY_CHARGED_ATTACK : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("isPerformingFullyChargedAttack", true);
        }
    }
}