using UnityEngine;

namespace Deceilio.Psychain 
{
    public class BASIC_CombatStanceState : WRLD_STATE
    {
        [Header("ATTACK STATE")]
        public BASIC_AttackState attackState; // Reference to the Attack State script

        [Header("PURSUE TARGET STATE")]
        public BASIC_PursueTargetState pursueTargetState; // Reference to the Pursue Target State script

        [Header("LIST OF A.I CHARACTER ATTACKS")]
        public AICharacterAttackActions[] aiCharacterAttacks; // Reference to A.I Character Attack Actions Script to get list of attacks

        protected bool randomDestinationSet = false; // Check if random destination is set to true or false
        protected float verticalMovementValue = 0; // Value for the vertical movement of the A.I Character
        protected float horizontalMovementValue = 0; // Value for the horizontal movement of the A.I Character
        protected virtual void Awake()
        {
            attackState = GetComponent<BASIC_AttackState>();
            pursueTargetState = GetComponent<BASIC_PursueTargetState>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            // LOGIC: Check the attack range
            // LOGIC: Potentially circle around the player or walk around them
            // LOGIC: If in attack range switch to attack state
            // LOGIC: If the A.I Character is in cooldown after attacking, return this state and continue circling player
            // LOGIC: If the player runs out of range return the pursue target state
            
            aiCharacter.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;

            if(aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if(aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager); // Circle player or walk around them
            }

            UseRotateTowardsTarget(aiCharacter);

            if(aiCharacter.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(aiCharacter);
            }

            CheckStoppingDistance(aiCharacter);
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

            for(int i = 0; i < aiCharacterAttacks.Length; i++)
            {
                AICharacterAttackActions aiCharacterAttackAction = aiCharacterAttacks[i];

                if(aiCharacter.distanceFromTarget <= aiCharacterAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= aiCharacterAttackAction.minimumDistanceNeededToAttack)
                {
                    if(aiCharacter.viewableAngle <= aiCharacterAttackAction.maximumAttackAngle
                        && aiCharacter.viewableAngle >= aiCharacterAttackAction.minimumAttackAngle)
                    {
                        maxScore += aiCharacterAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for(int i = 0; i < aiCharacterAttacks.Length; i++)
            {
                AICharacterAttackActions aiCharacterAttackAction = aiCharacterAttacks[i];

                if(aiCharacter.distanceFromTarget <= aiCharacterAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= aiCharacterAttackAction.minimumDistanceNeededToAttack)
                {
                    if(aiCharacter.viewableAngle <= aiCharacterAttackAction.maximumAttackAngle
                        && aiCharacter.viewableAngle >= aiCharacterAttackAction.minimumAttackAngle)
                    {
                        if(attackState.currentAttack != null)
                            return;

                        temporaryScore += aiCharacterAttackAction.attackScore;

                        if(temporaryScore > randomValue)
                        {
                            attackState.currentAttack = aiCharacterAttackAction;
                        }
                    }
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

