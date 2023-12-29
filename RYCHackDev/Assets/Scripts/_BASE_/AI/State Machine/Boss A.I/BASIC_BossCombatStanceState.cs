using UnityEngine;

namespace Deceilio.Psychain
{
    public class BASIC_BossCombatStanceState : BASIC_CombatStanceState
    {
        [Header("SECOND PHASE DATA")]
        public bool hasPhaseShifted; // Checks if the boss phase shifted to other phase or not
        public AICharacterAttackActions[] secondPhaseAttacks; // Reference to A.I Character Attack Actions Script to get list of attacks for second phase    
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void GetNewAttack(AICharacterManager aiCharacter)
        {
            if(hasPhaseShifted)
            {
                int maxScore = 0;

                for(int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    AICharacterAttackActions aiCharacterAttackAction = secondPhaseAttacks[i];

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

                for(int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    AICharacterAttackActions aiCharacterAttackAction = secondPhaseAttacks[i];

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
            else 
            {
                base.GetNewAttack(aiCharacter);    
            }
        }
    }
}