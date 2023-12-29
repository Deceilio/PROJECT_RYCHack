using UnityEngine;

namespace Deceilio.Psychain 
{
    public class BASIC_AttackState : WRLD_STATE
    {
        [Header("ROTATE TOWARDS TARGET STATE")]
        public BASIC_RotateTowardsTargetState rotateTowardsTargetState; // Reference to the Rotate Towards Target State
        [Header("COMBAT STANCE STATE")]
        public BASIC_CombatStanceState combatStanceState; // Reference to the Combat Stance State
        [Header("PURSUE TARGET STATE")]
        public BASIC_PursueTargetState pursueTargetState; // Reference to the Pursue Target State

        [Header("ATTACK DATA")]
        public AICharacterAttackActions currentAttack; // Current attack of the A.I Character
        public bool hasPerformedAttack = false; // Check if the attack is performed or not
        bool willDoComboOnNextAttack = false; // Checks if the A.I Character is performing combo or not
        private void Awake()
        {
            rotateTowardsTargetState = GetComponent<BASIC_RotateTowardsTargetState>();
            combatStanceState = GetComponent<BASIC_CombatStanceState>();
            pursueTargetState = GetComponent<BASIC_PursueTargetState>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            // LOGIC: Select one of the A.I Character's many attacks based on attack score
            // LOGIC: If the selected attack is not able to be used because of bad angle or distance, select a new attack
            // LOGIC: If the attack is viable, stop the movement and attack the A.I Character's target
            // LOGIC: Set the A.I Character recovery timer to the attacks recovery time
            // LOGIC: Return the combat stance state

            float distanceFromTarget = Vector3.Distance(aiCharacter.currentTarget.transform.position, aiCharacter.transform.position);

            RotateTowardsTargetWhileAttack(aiCharacter);

            if(aiCharacter.currentTarget.isDead)
                return pursueTargetState;

            if(distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (willDoComboOnNextAttack && aiCharacter.canDoCombo)
            {
                AttackTargetWithCombo(aiCharacter);
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(aiCharacter); // ATTACK
                RollForComboChance(aiCharacter);
            }

            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this; //Goes back to perform the combo
            }

            return rotateTowardsTargetState;
        }

        private void AttackTarget(AICharacterManager aiCharacter)
        {
            aiCharacter.UpdateWhichHandPlayerIsUsing(true);
            aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation(currentAttack.actionAnimation, true);
            //aiCharacter.aiCharacterAnimatorManager.PlayWeaponTrailFX();
            aiCharacter.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(AICharacterManager aiCharacter)
        {
            willDoComboOnNextAttack = false;
            aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation(currentAttack.actionAnimation, true);
            //aiCharacter.aiCharacterAnimatorManager.PlayWeaponTrailFX();
            aiCharacter.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RotateTowardsTargetWhileAttack(AICharacterManager aiCharacter)
        {
            // Rotate Manually (To do more complex Rotation like follow you from jumping from cliff, etc)
            if (aiCharacter.canRotate && aiCharacter.isPerformingAction)
            {
                Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
        }

        private void RollForComboChance(AICharacterManager aiCharacter)
        {
            float comboChance = Random.Range(0, 100);

            if(aiCharacter.allowAIToPerformCombos && comboChance <= aiCharacter.comboLikelyHood)
            {
                if(currentAttack.comboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }
}

