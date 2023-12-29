using UnityEngine;

namespace Deceilio.Psychain
{
    public class ADVANCE_CombatStanceStateHumanoid : WRLD_STATE
    {
        [Header("ATTACK STATE")]
        public ADVANCE_AttackStateHumanoid attackState; // Reference to the Attack State script

        [Header("PURSUE TARGET STATE")]
        public ADVANCE_PursueTargetStateHumanoid pursueTargetState; // Reference to the Pursue Target State script

        [Header("LIST OF A.I CHARACTER ATTACKS")]
        public ItemBasedAttackActions[] aiCharacterAttacks; // Reference to Item Based Attack Actions Script to get list of attacks

        protected bool randomDestinationSet = false; // Check if random destination is set to true or false
        protected float verticalMovementValue = 0; // Value for the vertical movement of the A.I Character
        protected float horizontalMovementValue = 0; // Value for the horizontal movement of the A.I Character

        [Header("STATE FLAGS")]
        bool willPerformBlock = false; // Check if the A.I is performing block or not
        bool willPerformDodge = false; // Check if the A.I is performing dodge or not
        bool willPerformParry = false; // Check if the A.I is performing parry or not

        [Header("DODGE DATA")]
        bool hasPerformedDodge = false;  // Check if the A.I had performed dodge or not
        bool hasRandomDodgeDirection = false; // Check if the A.I is performing dodge in random direction
        Quaternion targetDodgeDirection; // Target dodge direction to dodge for the A.I

        [Header("PARRY DATA")]
        bool hasPerformedParry = false;  // Check if the A.I had performed parry or not

        protected virtual void Awake()
        {
            attackState = GetComponent<ADVANCE_AttackStateHumanoid>();
            pursueTargetState = GetComponent<ADVANCE_PursueTargetStateHumanoid>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            // LOGIC: Check the attack range
            // LOGIC: Potentially circle around the player or walk around them
            // LOGIC: If in attack range switch to attack state
            // LOGIC: If the A.I Character is in cooldown after attacking, return this state and continue circling player
            // LOGIC: If the player runs out of range return the pursue target state

            if (aiCharacter.combatStyle == AICombatStyle.SwordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(aiCharacter);
            }
            else if (aiCharacter.combatStyle == AICombatStyle.Claws)
            {
                return ProcessClawsCombatStyle(aiCharacter);
            }
            else if (aiCharacter.combatStyle == AICombatStyle.Archer)
            {
                return ProcessArcherCombatStyle(aiCharacter);
            }
            else
            {
                return this;
            }
        }      
        private WRLD_STATE ProcessSwordAndShieldCombatStyle(AICharacterManager aiCharacter)
        {
            // BELOW CODE: If A.I is falling, or is performing an action, then stop all the movement
            if (!aiCharacter.isGrounded || aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            // BELOW CODE: If A.I has gotten too far from the target, return the A.I Character to it's pursue target state
            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            // BELOW CODE: Randomize the walking pattern of our A.I Character so they circle the player around
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager); // Circle player or walk around them
            }

            if (aiCharacter.allowAIToPerformBlock)
            {
                // BELOW CODE: Roll for a block chance
                RollForBlockChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformDodge)
            {
                // BELOW CODE: Roll for a dodge chance
                RollForDodgeChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformParry)
            {
                // BELOW CODE: Roll for a parry chance
                RollForParryChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformParry)
            {
                if(aiCharacter.currentTarget.canBeRiposted)
                {
                    // BELOW CODE: Riposte the A.I Character character
                    CheckforRiposte(aiCharacter);
                    return this;
                }
            }

            if (willPerformBlock)
            {
                // BELOW CODE: Block using off-hand (left hand)
                BlockUsingOffHand(aiCharacter);
            }

            if (aiCharacter.currentTarget.isAttacking) // Check if A.I Character is going to attack so it going to be dodgable
            {
                if (willPerformDodge) 
                {
                    // BELOW CODE: Dodge using off-hand (left hand)
                    Dodge(aiCharacter);
                }
            }

            if (aiCharacter.currentTarget.isAttacking) // Check if A.I Character is going to attack so it going to be parryable
            {
                if (willPerformParry && !hasPerformedParry)
                {
                    // BELOW CODE: Parry using off-handd (left hand)
                    ParryCurrentTarget(aiCharacter);
                    return this;
                }
            }

            UseRotateTowardsTarget(aiCharacter);

            // BELOW CODE: If in attack range return Attack State
            if (aiCharacter.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                // BELOW CODE: Reset all the flags
                ResetStateFlags();
                return attackState;
            }
            else
            {
                // BELOW CODE: If A.I Character is cooldown after attacking, return this state and continue circling the player
                GetNewAttack(aiCharacter);
            }

            CheckStoppingDistance(aiCharacter);
            return this;
        }
        private WRLD_STATE ProcessClawsCombatStyle(AICharacterManager aiCharacter)
        {
            return this;
        }
        private WRLD_STATE ProcessArcherCombatStyle(AICharacterManager aiCharacter)
        {
            return this;
        }
        protected void UseRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Rotate manually
            Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed);
        }
        protected void DecideCirclingAction(AICharacterAnimatorManager aiCharacterAnimatorManager)
        {
            // Circle with only forward vertical movement
            // Circle with running
            // Circle with walking only <--
            WalkAroundTarget(aiCharacterAnimatorManager);
        }
        protected void WalkAroundTarget(AICharacterAnimatorManager aiCharacterAnimatorManager)
        {
            verticalMovementValue = 0.5f;

            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }
        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            int maxScore = 0;

            for (int i = 0; i < aiCharacterAttacks.Length; i++)
            {
                ItemBasedAttackActions aiCharacterAttackAction = aiCharacterAttacks[i];

                if (aiCharacter.distanceFromTarget <= aiCharacterAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= aiCharacterAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= aiCharacterAttackAction.maximumAttackAngle
                        && aiCharacter.viewableAngle >= aiCharacterAttackAction.minimumAttackAngle)
                    {
                        maxScore += aiCharacterAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < aiCharacterAttacks.Length; i++)
            {
                ItemBasedAttackActions aiCharacterAttackAction = aiCharacterAttacks[i];

                if (aiCharacter.distanceFromTarget <= aiCharacterAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= aiCharacterAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= aiCharacterAttackAction.maximumAttackAngle
                        && aiCharacter.viewableAngle >= aiCharacterAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += aiCharacterAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = aiCharacterAttackAction;
                        }
                    }
                }
            }
        }

        // A.I Character State Rolls
        private void RollForBlockChance(AICharacterManager aiCharacter)
        {
            int blockChance = Random.Range(0, 100);
            if (blockChance <= aiCharacter.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }
        private void RollForDodgeChance(AICharacterManager aiCharacter)
        {
            int dodgeChance = Random.Range(0, 100);
            if (dodgeChance <= aiCharacter.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }
        private void RollForParryChance(AICharacterManager aiCharacter)
        {
            int parryChance = Random.Range(0, 100);
            if (parryChance <= aiCharacter.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;

            }
        }

        // BELOW CODE: Will call whenever we exit the state, so when we return all flags are reset and can be re-rolled
        private void ResetStateFlags()
        {
            hasRandomDodgeDirection = false;
            hasPerformedDodge = false;
            hasPerformedParry = false;

            randomDestinationSet = false;
            
            willPerformBlock = false;
            willPerformDodge = false;
            willPerformParry = false;
        }

        // A.I Character Actions
        private void BlockUsingOffHand(AICharacterManager aiCharacter)
        {
            if(aiCharacter.isBlocking == false)
            {
                if(aiCharacter.allowAIToPerformBlock)
                {
                    aiCharacter.isBlocking = true;
                    aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.leftHandWeapon;
                    aiCharacter.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
                }
            }
        }
        private void Dodge(AICharacterManager aiCharacter)
        {
            if (!hasPerformedDodge)
            {
                if (!hasRandomDodgeDirection)
                {
                    float randomDodgeDirection;

                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler(aiCharacter.transform.eulerAngles.x, randomDodgeDirection, aiCharacter.transform.eulerAngles.z);
                }

                if (aiCharacter.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetDodgeDirection, 1f);
                    aiCharacter.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = aiCharacter.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if (rotationDifference <= 5)
                    {
                        hasPerformedDodge = true;
                        aiCharacter.transform.rotation = targetDodgeDirection;
                        aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Rolling", true, true);
                    }
                }
            }
        }
        private void ParryCurrentTarget(AICharacterManager aiCharacter)
        {
            if(aiCharacter.distanceFromTarget <= 2)
            {
                hasPerformedParry = true;
                aiCharacter.isParrying = true;
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacterCombatManager.shield_parry, true);
            }
        }
        private void CheckforRiposte(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;
            }

            if (aiCharacter.distanceFromTarget >= 1.0)
            {
                UseRotateTowardsTarget(aiCharacter);
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
            }
            else
            {
                aiCharacter.isBlocking = false;

                if (!aiCharacter.isPerformingAction && !aiCharacter.currentTarget.isBeingRiposted && !aiCharacter.currentTarget.isBeingBackstabbed)
                {
                    aiCharacter.rigidBody.velocity = Vector3.zero;
                    aiCharacter.animator.SetFloat("Vertical", 0);
                    aiCharacter.characterCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }
        private void CheckStoppingDistance(AICharacterManager aiCharacter)
        {
            if (aiCharacter.distanceFromTarget <= aiCharacter.stoppingDistance)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                aiCharacter.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
        }
    }
}