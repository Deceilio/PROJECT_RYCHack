using UnityEngine;

namespace Deceilio.Psychain
{ 
    public class CharacterStatsManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script

        [Header("ENTITY TEAM I.D")]
        public string characterName; // Name of the character
        public int entityTeamIDNumber = 0; // Reference to the entity team ID number

        [Header("HEALTH DATA")]
        public int maxHealth; // Max health for the player
        public int currentHealth; // Current health of the player
        public float healthRegenAmount = 5; // Value of the regen amount for the health regen
        public float healthRegenTimer = 0; // Timer for the health regen

        [Header("STAMINA DATA")]
        public float maxStamina; // Max stamina of the player
        public float currentStamina; // Current stamina of the player
        public float staminaRegenAmount = 5; // How much the regen amount for the stamina regen
        public float staminaRegenAmountWhilstBlocking = 5; // How much the regen amount for the stamina regen whilst blocking
        public float staminaRegenTimer = 0; // Timer for the stamina regen

        [Header("SKILLS DATA")]
        public float maxSkillPoints; // Max skill points for the player
        public float currentSkillPoints; // Current skill points for the player

        [Header("LEVEL DATA")]
        public int healthLevel = 10; // Variable for the health value/level
        public int staminaLevel = 10; // Variable for the stamina Level
        public int skillLevel = 10; // Variable for the skill value/level
        // public int poiseLevel = 10; // Variable for the poise Level
        // public int strengthLevel = 10; // Variable for the strength Level
        // public int dexterityLevel = 10; // Variable for the dexerity Level
        // public int intelligenceLevel = 10; // Variable for the intelligence Level
        // public int charismaLevel = 10; // Variable for the charisma Level

        [Header("EQUIPMENT LOAD")]
        public float currentEquipLoad = 0; // Variable for the current equipment load
        public float maxEquipLoad = 0; // Variable for the max equipment load
        public EncumbranceLevel encumbranceLevel; // Reference to the EncumbranceLevel enum to get the level for encumbrance

        //Poise Logic: Poise can reduce during the fight and player can loose some Poise Defense, and it will comeback over time if the player is not hit by the A.I Character for few seconds..
        [Header("POISE DATA")] 
        public float totalPoiseDefense; // The total poise during damage calculation
        public float attackingPoiseBonus; // The poise you gain during an attack with a weapon
        public float armourPoiseBonus; // The poise you gain from wearing whatever armour you currently wearing
        public float totalPoiseResetTime = 15; // To reset the total poise timer, so it will go back to whatever it was (Combination of: Attacking & Armour)
        public float poiseResetTimer = 0; // To reset the current poise timer,  so it will go back to whatever it was (Normal Poise)

        [Header("LINKS DATA")] 
        public int currentLinksCount = 0; // Current links count 
        public int linksAwaradedOnDeath = 50; // Links which will be rewarded after the character death

        [Header("ARMOUR ABSORPTIONS")]
        [Header("PHYSICAL")]
        public float physicalDamageAbsorptionHead;// Physical damage absorption rate for the head armours
        public float physicalDamageAbsorptionChest;// Physical damage absorption rate for the chest armours
        public float physicalDamageAbsorptionLegs;// Physical damage absorption rate for the legs armours
        public float physicalDamageAbsorptionHands;// Physical damage absorption rate for the hands armours
        [Header("FIRE")]
        public float fireDamageAbsorptionHead;// Fire damage absorption rate for the head armours
        public float fireDamageAbsorptionChest;// Fire damage absorption rate for the chest armours
        public float fireDamageAbsorptionLegs;// Fire damage absorption rate for the legs armours
        public float fireDamageAbsorptionHands;// Fire damage absorption rate for the hands armours
        // Elemental absorption, Lightning, Magic, Dark, etc if we need it.

        [Header("BLOCKING ABSORPTIONS")]
        public float blockingPhysicalDamageAbsorption;// Physical damage absorption rate for when blocking
        public float blockingFireDamageAbsorption; // Fire damage absorption rate when blocking
        public float blockingStabilityRating; // Stability rating for blocking

        [Tooltip("Any damage dealth by this player is modified by this amounts.")]
        [Header("DAMAGE TYPE MODIFIERS")]
        public float physicalDamagePercentageModifier = 100; // Physical damage percentage dealth by the character
        public float fireDamagePercentageModifier = 100; // Fire damage percentage dealth by the character

        [Tooltip("Incoming damage after armour calculation is modified by this value.")]
        [Header("DAMAGE ABSORPTION MODIFIERS")]
        public float physicalAbsorptionPercentageModifier = 0; // Physical damage absorption percentage dealth on the character
        public float fireAbsorptionPercentageModifier = 0; // Fire damage absorption percentage dealth on the character

        [Header("RESISTANCES")]
        public float poisonResistance; // Resistance by the Poison

        [Header("POISON")]
        public bool isPoisoned; // Checks if the character is poisoned or not
        public float poisonBuildUp = 0; // The build up over time that poisons the player after reaching 100
        public float poisonAmount = 100; // The amount of poison the player has to process before becoming unpoisoned

        [Header("Health Bars")]
        [HideInInspector] public UI_HealthBar healthBar; // Reference the Health Bar script
        public UI_AICharacterHealthBar aiCharacterHealthBar; // Reference to the A.I Character Health Bar component
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {
            totalPoiseDefense = armourPoiseBonus;
            CalculateAndSetMaxEquipload();
        }
        
        protected virtual void Update()
        {
            UsePoiseResetTimer();
        }

        public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if(character.isInvulerable)
                return;

            if(character.isDead)
                return;

            float totalPhysicalDamageAbsorption = 1 - 
                (1 - physicalDamageAbsorptionHead / 100) *
                (1 - physicalDamageAbsorptionChest / 100) *
                (1 - physicalDamageAbsorptionLegs / 100) *
                (1 - physicalDamageAbsorptionHands / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 - 
                (1 - fireDamageAbsorptionHead / 100) *
                (1 - fireDamageAbsorptionChest / 100) *
                (1 - fireDamageAbsorptionLegs / 100) *
                (1 - fireDamageAbsorptionHands / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            Debug.Log("Total" + totalPhysicalDamageAbsorption + "%"); 

            float finalDamage = physicalDamage + fireDamage; // + Magic damage, Dark damage, etc if we need it.

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log("Total Damage Dealth is " + finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }
        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        public float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }
        public float SetMaxSkillPointsFromSkillLevel() 
        {
            maxSkillPoints = skillLevel * 10;
            return maxSkillPoints;
        }
        public virtual void DeductStamina(float staminaToDeduct)
        {
            currentStamina = currentStamina - staminaToDeduct;
        }
        public virtual void DeductSkillPoints(int skillPoints)
        {
            currentSkillPoints = currentSkillPoints - skillPoints;     
        }
        public virtual void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        public virtual void RecoverSkillPoints(int recoverAmount)
        {
            currentSkillPoints = currentSkillPoints + recoverAmount;

            if(currentSkillPoints > maxSkillPoints)
            {
                currentSkillPoints = maxSkillPoints;
            }
        }

        public virtual void UsePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !character.isPerformingAction)
            {
                totalPoiseDefense = armourPoiseBonus;
            }
        }

        public void CalculateAndSetMaxEquipload()
        {
            float totalEquipLoad = 40;

            for (int i = 0; i < staminaLevel; i++)
            {
                // BELOW CODE: Change returns based on stamina level
                if(i < 26)
                {
                    totalEquipLoad = totalEquipLoad + 1.2f;
                }
                if(i >= 25 && i <= 50)
                {
                    totalEquipLoad = totalEquipLoad + 1.4f;
                }
                if(i > 50)
                {
                    totalEquipLoad = totalEquipLoad + 1;
                }
            }

            maxEquipLoad = totalEquipLoad;
        }

        public void CalculateAndSetCurrentEquipLoad(float equipLoad)
        {
            currentEquipLoad = equipLoad;

            encumbranceLevel = EncumbranceLevel.Light;

            if(currentEquipLoad > (maxEquipLoad * 0.3f))
            {
                encumbranceLevel = EncumbranceLevel.Medium;
            }
            if(currentEquipLoad > (maxEquipLoad * 0.7f))
            {
                encumbranceLevel = EncumbranceLevel.Heavy;
            }
            if(currentEquipLoad > (maxEquipLoad))
            {
                encumbranceLevel = EncumbranceLevel.Overloaded;
            }
        }

        public void BreakGuard()
        {
            // BELOW CODE: Only for boss A.I Character
            character.characterAnimatorManager.PlayTargetActionAnimation("Guard Break", true);
        }
    }
}
