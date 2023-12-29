using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class PlayerStatesManager : CharacterStatesManager
    {
        PlayerManager player; //Reference to the Player Manager script

        [HideInInspector] public UI_PoisonBuildUpBar poisonBuildUpBar; // Reference to the Posion Build Up Bar script
        [HideInInspector] public UI_PoisonAmountBar poisonAmountBar; // Reference to the Poison Amount Bar script

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            poisonBuildUpBar = FindObjectOfType<UI_PoisonBuildUpBar>();
            poisonAmountBar = FindObjectOfType<UI_PoisonAmountBar>();
        }

        protected override void Start()
        {
            base.Start();
            currentConsumableBeingUsed = consumableEquippedSlots[currentConsumableIndex];
            player.quickSlotsUI.UpdateConsumableItemQuickSlotUI(currentConsumableBeingUsed);
        }

        protected override void Update()
        {
            base.Update();
        }

        public void ChangeConsumableItem()
        {
            if(player.isGrounded)
            {
                currentConsumableIndex = currentConsumableIndex + 1;

                if (currentConsumableIndex > consumableEquippedSlots.Length - 1)
                {
                    currentConsumableIndex = -1;
                    LoadConsumableOnSlot(nullConsumableItem);
                }
                else if (consumableEquippedSlots[currentConsumableIndex] != null)
                {
                    currentConsumableBeingUsed = consumableEquippedSlots[currentConsumableIndex];
                    LoadConsumableOnSlot(consumableEquippedSlots[currentConsumableIndex]);
                }
                else
                {
                    currentConsumableIndex = currentConsumableIndex + 1;
                }
            }
        }

        public void LoadConsumableOnSlot(WRLD_CONSUMABLE_ITEM consumableItem)
        {
            currentConsumableBeingUsed = consumableItem;
            player.quickSlotsUI.UpdateConsumableItemQuickSlotUI(consumableItem);
        }

        public void HealPlayerFromState(int healAmount)
        {
            player.playerStatsManager.HealPlayer(amountToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, character.characterStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            character.characterWeaponSlotManager.LoadBothWeaponsOnSlots();
        } 
        public void RecoverSkillPointsFromState(int recoverAmount)
        {
            player.playerStatsManager.RecoverSkillPoints(amountToBeRecoverSkillPoints);
            GameObject recoverParticles = Instantiate(currentParticleFX, character.characterStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            character.characterWeaponSlotManager.LoadBothWeaponsOnSlots();
        }

        protected override void ProcessBuildupDecay()
        {
            if(player.characterStatsManager.poisonBuildUp >= 0)
            {
                player.characterStatsManager.poisonBuildUp -= 1;

                poisonBuildUpBar.gameObject.SetActive(true);
                poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(player.characterStatsManager.poisonBuildUp));
            }
        }
    }
}