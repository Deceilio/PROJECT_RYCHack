using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class ADVANCE_PatrolStateHumanoid : WRLD_STATE
    {
        public ADVANCE_PursueTargetStateHumanoid pursueTargetState; // Reference to the Pursue Target State Humanoid script

        public LayerMask detectionLayer; // Selective layer for the detection of an player, etc
        public LayerMask layersThatBlockLineOfSight; // Selective layer that block line of sight for the enemy

        [Header("PATROL FLAGS")]
        public bool patrolComplete; // Check if the patrolling is finished
        public bool repeatPatrol; // Bool for repeating the patrol after it's finished

        // How long before next patrol
        [Header("PATROL RESET TIMER")]
        public float endOfPatrolRestTime; // The time which take in the end of patrol
        public float endOfPatrolTimer; // Timer to reset the patrol

        [Header("PATROL POSITION")]
        public int patrolDestinationIndex; // Which number of the path enemy will patrol to
        public bool hasPatrolDestination; // Check if enemy have patrol destination assigned
        public Transform currentPatrolDestination; // Current patrol destination for the enemy
        public float distanceFromCurrentPatrolPoint; // Distance from the current patrol point to next one
        public List<Transform> listOfPatrolDestination; // List of all patrol destination
        private void Awake()
        {
            pursueTargetState = GetComponent<ADVANCE_PursueTargetStateHumanoid>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            SearchForTargetWhilstPatroling(aiCharacter);

            // BELOW CODE: If the A.I is performing an action, exit all the movement and return the state
            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }

            // BELOW CODE: If A.I completed the patrol then it will not repeat it, A.I will do this then 
            if (patrolComplete && repeatPatrol)
            {
                // BELOW CODE: Count down the reset time and reset all the patrol stage
                if (endOfPatrolRestTime > endOfPatrolTimer)
                {
                    aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                    endOfPatrolTimer = endOfPatrolTimer + Time.deltaTime;
                    return this;
                }
                else if (endOfPatrolTimer >= endOfPatrolRestTime)
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    currentPatrolDestination = null;
                    patrolComplete = false;
                    endOfPatrolTimer = 0;
                }
            }
            else if (patrolComplete && !repeatPatrol)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                return this;
            }

            if (hasPatrolDestination)
            {
                if (currentPatrolDestination != null)
                {
                    distanceFromCurrentPatrolPoint = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination.transform.position);

                    if (distanceFromCurrentPatrolPoint > 1)
                    {
                        aiCharacter.navMeshAgent.enabled = true;
                        aiCharacter.navMeshAgent.destination = currentPatrolDestination.transform.position;
                        Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, 0.5f);
                        aiCharacter.transform.rotation = targetRotation;
                        aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                    }
                    else
                    {
                        currentPatrolDestination = null;
                        hasPatrolDestination = false;
                    }
                }
            }

            if (!hasPatrolDestination)
            {
                patrolDestinationIndex = patrolDestinationIndex + 1;

                if (patrolDestinationIndex > listOfPatrolDestination.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                currentPatrolDestination = listOfPatrolDestination[patrolDestinationIndex];
                hasPatrolDestination = true;
            }

            return this;
        }

        private void SearchForTargetWhilstPatroling(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Search for a potential target within the detection radius
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
                                return;
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
                return;
            }
            else
            {
                return;
            }
        }
    }
}