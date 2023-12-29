using UnityEngine;
using System.Collections;

namespace Deceilio.Psychain
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player; // Reference to the Player Manager script
        [HideInInspector] public UI_StaminaBar staminaBar; // Reference to the stamina bar Script
        [HideInInspector] public UI_SkillsPointBar skillsPointBar; // Reference to the Skill Point Bar script
        private float sprintingTimer = 0;
        
        protected override void Awake()
        {
            base.Awake();
            player= GetComponent<PlayerManager>();

            healthBar = FindObjectOfType<UI_HealthBar>();
            staminaBar = FindObjectOfType<UI_StaminaBar>();
            skillsPointBar = FindObjectOfType<UI_SkillsPointBar>();
        }
        protected override void Start()
        {
            base.Start();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxSkillPoints = SetMaxSkillPointsFromSkillLevel();
            currentSkillPoints = maxSkillPoints;
            skillsPointBar.SetMaxSkillPoints(maxSkillPoints);
            skillsPointBar.SetCurrentSkillPoints(currentSkillPoints);
        }

        protected override void Update()
        {
            healthBar.SetCurrentHealth(currentHealth);       
        }

        // public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        // {
        //     if(player.isInvulerable)
        //         return;

        //     base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamagingMe);
        //     healthBar.SetCurrentHealth(currentHealth);       
        //     player.playerAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);

        //     if(currentHealth <= 0)
        //     {
        //         ProcessPlayerDeath();
        //     }
        // }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage);
            healthBar.SetCurrentHealth(currentHealth);
        }
        public override void DeductStamina(float staminaToDeduct)
        {
            base.DeductStamina(staminaToDeduct);
            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
        }
        public override void DeductSkillPoints(int skillPoints)
        {
            base.DeductSkillPoints(skillPoints);

            if(currentSkillPoints < 0)
            {
                currentSkillPoints = 0;
            }

            skillsPointBar.SetCurrentSkillPoints(currentSkillPoints);
        }
        public void DeductSprintingStamina(float staminaToDeduct)
        {
            if(player.isSprinting)
            {
                sprintingTimer = sprintingTimer + Time.deltaTime;

                if(sprintingTimer > 0.1f)
                {
                    // Reset Timer
                    sprintingTimer = 0;
                    // Deduct Stamina
                    currentStamina = currentStamina - staminaToDeduct;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
            else
            {
                sprintingTimer = 0;
            }
        }
        public void RegenStamina()
        {
            // BELOW CODE: Do not regenerate stamina if we performing an action or sprinting
            if(player.isPerformingAction || player.isSprinting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime; 
                    
                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    if(player.isBlocking)
                    {
                        currentStamina += staminaRegenAmountWhilstBlocking * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                    else 
                    {
                        currentStamina += staminaRegenAmount * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }  
                }
            }
        }
        private void ProcessPlayerDeath()
        {
            currentHealth = 0;
            player.playerAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            // BELOW CODE: Handle player death
            player.isDead = true;
            player.playerUIManager.popUpUIManager.ShowYouDiedPopUp();
        }
        
        public override void HealPlayer(int healAmount)
        {
            base.HealPlayer(healAmount);
            healthBar.SetCurrentHealth(currentHealth);
        }

        public override void RecoverSkillPoints(int recoverAmount)
        {
            base.RecoverSkillPoints(recoverAmount);
            skillsPointBar.SetCurrentSkillPoints(currentSkillPoints);
        }

        public void AddLinks(int links)
        {
            currentLinksCount = currentLinksCount + links;
        }

        public override void UsePoiseResetTimer()
        {
            base.UsePoiseResetTimer();
        }
    }
}