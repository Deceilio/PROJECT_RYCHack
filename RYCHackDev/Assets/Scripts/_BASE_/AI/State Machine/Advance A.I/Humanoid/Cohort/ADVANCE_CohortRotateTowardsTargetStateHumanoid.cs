using UnityEngine;

namespace Deceilio.Psychain
{
    public class ADVANCE_CohortRotateTowardsTargetStateHumanoid : WRLD_STATE
    {
        [Header("COMBAT STANCE STATE")]
        ADVANCE_CohortCombatStanceStateHumanoid combatStanceState; // Reference to the Combat Stance State Huamnoid Script
        private void Awake()
        {
            combatStanceState = GetComponent<ADVANCE_CohortCombatStanceStateHumanoid>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            aiCharacter.animator.SetFloat("Vertical", 0);
            aiCharacter.animator.SetFloat("Horizontal", 0);

            if (aiCharacter.isPerformingAction)
                return this; // When we enter the state we will still be interacting from the attack animation, so pause it until it finished  

            if (aiCharacter.viewableAngle >= 100 && aiCharacter.viewableAngle <= 180 && !aiCharacter.isPerformingAction)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn Behind", true, true);
                return combatStanceState;
            }
            else if (aiCharacter.viewableAngle <= -101 && aiCharacter.viewableAngle >= -180 && !aiCharacter.isPerformingAction)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn Behind", true, true);
                return combatStanceState;
            }
            else if (aiCharacter.viewableAngle <= -45 && aiCharacter.viewableAngle >= -100 && !aiCharacter.isPerformingAction)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn Right", true, true);
                return combatStanceState;
            }
            else if (aiCharacter.viewableAngle >= 45 && aiCharacter.viewableAngle <= 100 && !aiCharacter.isPerformingAction)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn Left", true, true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}
