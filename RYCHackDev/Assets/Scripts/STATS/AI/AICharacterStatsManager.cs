using UnityEngine;

namespace Deceilio.Psychain
{
    public class AICharacterStatsManager : CharacterStatsManager
    {
        AICharacterManager aiCharacter; // Reference to the A.I Character Manager script
        public bool tempisDead; // Checks if the A.I Character is dead or not (FOR TEMP)

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }
        protected override void Start()
        {
            base.Start();
            if(!aiCharacter.isBoss)
            {
                aiCharacterHealthBar.SetMaxHealth(maxHealth);
            }
        }
        // protected override void Update()
        // {
        //     if(!aiCharacter.isBoss)
        //     {
        //         aiCharacterHealthBar.SetCurrentHealth(currentHealth);
        //     }
        //     else if(aiCharacter.isBoss && aiCharacter.boss != null)
        //     {
        //         aiCharacter.boss.UpdateBossHealthBar(currentHealth, maxHealth);
        //     }
        // }
        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage);
            if(!aiCharacter.isBoss)
            {
                aiCharacterHealthBar.SetCurrentHealth(currentHealth);
            }
            else if(aiCharacter.isBoss && aiCharacter.boss != null)
            {
                aiCharacter.boss.UpdateBossHealthBar(currentHealth, maxHealth);
            }
        }
        // public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        // {          
        //     base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamagingMe);
        //     if(!aiCharacter.isBoss)
        //     {
        //         aiCharacterHealthBar.SetCurrentHealth(currentHealth);
        //     }
        //     else if(aiCharacter.isBoss && aiCharacter.boss != null)
        //     {
        //         aiCharacter.boss.UpdateBossHealthBar(currentHealth, maxHealth);
        //     }

        //     aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);

        //     if(currentHealth <= 0)
        //     {
        //         UseAICharacterDeath();
        //     }
        // }
        
        public override void UsePoiseResetTimer()
        {
            base.UsePoiseResetTimer();
        }
        private void UseAICharacterDeath()
        {
            currentHealth = 0;
            aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            aiCharacter.isDead = true;
        }
    }
}