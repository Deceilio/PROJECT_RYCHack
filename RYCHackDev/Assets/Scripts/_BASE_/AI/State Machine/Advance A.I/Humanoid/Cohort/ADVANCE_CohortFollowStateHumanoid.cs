using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class ADVANCE_CohortFollowStateHumanoid : WRLD_STATE
    {
        [Header("IDLE STATE")]
        ADVANCE_CohortIdleStateHumanoid idleStateHumanoid; // Reference to the Cohort Idle State Humanoid script
        private void Awake()
        {
            idleStateHumanoid = GetComponent<ADVANCE_CohortIdleStateHumanoid>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this;

            if (aiCharacter.isPerformingFunctions)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            UseRotateTowardsTarget(aiCharacter);

            if (aiCharacter.distanceFromCohort > aiCharacter.maxDistanceFromCohort)
            {
                aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (aiCharacter.distanceFromCohort <= aiCharacter.returnDistanceFromCohort)
            {
                return idleStateHumanoid;
            }
            else
            {
                return this;
            }
        }
        private void UseRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = aiCharacter.rigidBody.velocity;

            aiCharacter.navMeshAgent.enabled = true;
            aiCharacter.navMeshAgent.SetDestination(aiCharacter.cohort.transform.position);
            aiCharacter.rigidBody.velocity = targetVelocity;
            aiCharacter.transform.rotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
        }
    }
}