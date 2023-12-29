using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class ADVANCE_CohortIdleStateHumanoid : WRLD_STATE
    {
        [Header("PURSUE TARGET STATE")]
        ADVANCE_CohortPursueTargetStateHumanoid pursueTargetState; // Reference to the Pursue Target State script
        [Header("FOLLOW STATE")]
        ADVANCE_CohortFollowStateHumanoid followState; // Reference to the Follow State script

        [Header("A.I DATA")]
        public LayerMask detectionLayer; // Layer to detect to the target
        public LayerMask layersThatBlockLineOfSight; // Layers that block line of sight
        private void Awake()
        {
            pursueTargetState = GetComponent<ADVANCE_CohortPursueTargetStateHumanoid>();
            followState = GetComponent<ADVANCE_CohortFollowStateHumanoid>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

            if(aiCharacter.distanceFromCohort > aiCharacter.maxDistanceFromCohort)
            {
                return followState;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                // BELOW CODE: If a potential target is found, that is not on the same team as A.I, then A.I proceed to the next state
                if (targetCharacter != null)
                {
                    if (targetCharacter.characterStatsManager.entityTeamIDNumber != aiCharacter.aiCharacterStatsManager.entityTeamIDNumber)
                    {
                        Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                        // BELOW CODE: If a potential target is found, it has to be standing infront of the A.I's field of view
                        if (viewableAngle > aiCharacter.minimumDetectionAngle &&
                            viewableAngle < aiCharacter.maximumDetectionAngle)
                        {
                            // BELOW CODE: If the A.I's potential target has an obstruction in between itself and the A.I Character, it will not add player as current target
                            if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                            {
                                return this;
                            }
                            else
                            {
                                aiCharacter.currentTarget = targetCharacter;
                            }
                        }
                    }
                }
            }

            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}