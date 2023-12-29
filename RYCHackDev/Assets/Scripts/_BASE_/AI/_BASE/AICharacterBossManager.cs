using UnityEngine;

namespace Deceilio.Psychain
{
    public class AICharacterBossManager : MonoBehaviour
    {
        [Header("BOSS DATA")]
        public string bossName; // Name of the Boss

        UI_BossHealthBar bossHealthBar; // Reference to the UI Boss Health Bar script
        AICharacterManager aiCharacter; // Reference to the A.I Character Manager script
        BASIC_BossCombatStanceState bossCombatStanceState; // Reference to the Boss Combat Stance script

        [Header("SECOND PHASE DATA")]
        public GameObject secondPhaseParticleFX; // Particle effects for the second phase

        private void Awake()
        {
            aiCharacter = GetComponent<AICharacterManager>();
            bossHealthBar = FindObjectOfType<UI_BossHealthBar>();
            bossCombatStanceState = GetComponentInChildren<BASIC_BossCombatStanceState>();
        }
        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(aiCharacter.aiCharacterStatsManager.maxHealth);
        }
        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);

            if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
            {
                bossCombatStanceState.hasPhaseShifted = true;
                ShiftToSecondPhase();
            }
        }

        public void ShiftToSecondPhase()
        {
            aiCharacter.animator.SetBool("isInvulnerable", true);
            aiCharacter.animator.SetBool("isPhaseShifting", true);
            // BELOW CODE: Play an animation with an event that triggers particle fx/weapon fx
            aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Phase Shift", true);
            // BELOW CODE: Switch attack actions
            bossCombatStanceState.hasPhaseShifted = true;
        }
    }
}
