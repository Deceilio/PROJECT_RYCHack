using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class CharacterStatesManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script

        [Header("CURRENT CONSUMABLE EQUIPPED")]
        public WRLD_CONSUMABLE_ITEM currentConsumableBeingUsed; // Tells which consumable the character is currently using

        [Header("NULL CONSUMABLE")]
        public WRLD_CONSUMABLE_ITEM nullConsumableItem;  // Empty/Null consumable for empty slot

        [Header("CONSUMABLE INDEX")]
        public int currentConsumableIndex = 0; // Index number for the particular consumable in quick slot 

        [Header("CONSUMABLE ITEMS")]
        public WRLD_CONSUMABLE_ITEM[] consumableEquippedSlots = new WRLD_CONSUMABLE_ITEM[1]; // Contains all the consumables which character has

        [Header("CONSUMABLE ITEMS NAMES")]
        public string[] nameOfAllTheConsumablesCharacterHas; // Contains all the name of consumables which character have

        [Header("STATIC STATES")]
        [SerializeField] List<WRLD_CHARACTER_STATIC_STATE> staticCharacterStates; // List of static statees (Bracelet state, armour state, etc)

        [Header("TIMED STATES")]
        public List<WRLD_CHARACTER_STATE> timedEffects; // List of timed/status states (Buff, Poison build up, curse, toxic, etc)
        [SerializeField] float effectTickTimer = 0; // Timer for state taking effect

        [Header("TIMED STATES VISUAL FX")]
        public List<GameObject> timedStateParticles; // Certain timed state particle system

        // LIST OF INSTANT STATES (Taking Damage, add build up, etc)

        [Header("CURRENT FX MODELS")]
        public GameObject instantiatedFXModel; // The particle effects gameobject which is to be instantiated

        [Header("DAMAGE FX")]
        public GameObject bloodSplatterFX; // Reference to the blood splash vfx

        [Header("VIALS DATA")]
        public int amountToBeHealed; // How much amount needed to be healed
        public int amountToBeRecoverSkillPoints; // How much skill points needed to be recovered
        public GameObject currentParticleFX; // The current particle effects which is playing while effecting the player (drinking vigour, etc)  

        [Header("WEAPON FX")]
        public CharacterWeaponManager rightHandWeaponManager; // Refrence to the Weapon Manager for right hand fx
        public CharacterWeaponManager leftHandWeaponManager; // Reference to the Weapon Manager or left hand fx

        [Header("RIGHT WEAPON FX")]
        public WeaponBuffsState rightHandWeaponBuffsState; // Reference to the weapon buffs state script for right hand weapon 

        [Header("POISON")]
        public Transform buildUpTransform; // The location where build up particle fx will spawn

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {
            foreach(var effect in staticCharacterStates)
            {
                effect.AddStaticState(character);
            }
        }

        protected virtual void Update()
        {
            // BELOW CODE: Access all of the consumables which character have and create array of strings of those consumables name
            if (consumableEquippedSlots != null)
            {
                nameOfAllTheConsumablesCharacterHas = new string[consumableEquippedSlots.Length];

                for (int i = 0; i < consumableEquippedSlots.Length; i++)
                {
                    nameOfAllTheConsumablesCharacterHas[i] = consumableEquippedSlots[i].itemName;
                }
            }

            //InstantiateConsumablesMenuObjects();
        }

        public virtual void ProcessStateInstantly(WRLD_CHARACTER_STATE state)
        {
            state.ProcessState(character);
        }
        public virtual void ProcessAllTimedStates()
        {
            effectTickTimer = effectTickTimer + Time.deltaTime;

            if (effectTickTimer >= 1)
            {
                effectTickTimer = 0;

                ProcessWeaponBuffs();

                // BELOW CODE: Process all active states over game time
                for (int i = timedEffects.Count - 1; i > -1; i--)
                {
                    timedEffects[i].ProcessState(character);
                }
                // BELOW CODE: Decays build up states over game time
                ProcessBuildupDecay();
            }
        }

        public void ProcessWeaponBuffs()
        {
            if(rightHandWeaponBuffsState != null)
            {
                rightHandWeaponBuffsState.ProcessState(character);
            }
        }

        public void AddStaticState(WRLD_CHARACTER_STATIC_STATE state)
        {
            // BELOW CODE: Check the list to make sure we don't add duplicate states

            WRLD_CHARACTER_STATIC_STATE staticState;

            for(int i = staticCharacterStates.Count - 1; i > -1; i--)
            {
                if (staticCharacterStates[i] != null)
                {
                    if (staticCharacterStates[i].stateID == state.stateID)
                    {
                        staticState = staticCharacterStates[i];
                        // BELOW CODE: We remove the actual state from our character
                        staticState.RemoveStaticState(character);
                        // BELOW CODE: We then remove the state from the list of active states
                        staticCharacterStates.Remove(staticState);
                    }
                }
            }

            // BELOW CODE: We add the state to our list of active states
            staticCharacterStates.Add(state);
            // BELOW CODE: We add the actual state to our character
            state.AddStaticState(character);

            // BELOW CODE: Check the list for null items and remove them
            for (int i = staticCharacterStates.Count - 1; i > -1; i--)
            {
                if (staticCharacterStates[i] != null)
                {
                    staticCharacterStates.RemoveAt(i);
                }
            }
        }
        public void RemoveStaticState(int stateID)
        {
            WRLD_CHARACTER_STATIC_STATE staticState;

            for(int i = staticCharacterStates.Count - 1; i > -1; i--)
            {
                if (staticCharacterStates[i] != null)
                {
                    if (staticCharacterStates[i].stateID == stateID)
                    {
                        staticState = staticCharacterStates[i];
                        // BELOW CODE: We remove the actual state from our character
                        staticState.RemoveStaticState(character);
                        // BELOW CODE: We then remove the state from list of active states
                        staticCharacterStates.Remove(staticState); 
                    }
                }
            }

            // BELOW CODE: Check the list of null items and remove items
            for (int i = staticCharacterStates.Count - 1; i > -1; i--)
            {
                if (staticCharacterStates[i] != null)
                {
                    staticCharacterStates.RemoveAt(i);
                }
            }
        }
        public virtual void PlayWeaponFX(bool isLeft)
        {
            if(isLeft == false)
            {
                // BELOW CODE: Play the right hand weapon trail
                if(rightHandWeaponManager != null)
                {
                    rightHandWeaponManager.PlayWeaponTrailFX();
                    //Debug.Log(rightWeaponFX.normalWeaponTrail.name + " is playing" % WRLD_COLORIZE_EDITOR.Red % WRLD_FONT_FORMAT.Bold);
                    //Debug.Log(rightWeaponFX.normalWeaponTrail.name);
                    Debug.Log("Player Playing");
                }
            }
            else
            {
                // BELOW CODE: Play the left hand weapon trail
                if (leftHandWeaponManager != null)
                {
                    leftHandWeaponManager.PlayWeaponTrailFX();
                }
            }
        }
        public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
        }
        public virtual void InterruptEffect()
        {
            // BELOW CODE: Used to destroy effect models
            if(instantiatedFXModel != null)
            {
                Destroy(instantiatedFXModel);   
            }
        }
        protected virtual void ProcessBuildupDecay()
        {
            if(character.characterStatsManager.poisonBuildUp > 0)
            {
                character.characterStatsManager.poisonBuildUp -= 1;
            }
        }
        public virtual void AddTimedStateParticle(GameObject state)
        {
            GameObject stateGameObject = Instantiate(state, buildUpTransform);
            timedStateParticles.Add(stateGameObject);
        }
        public virtual void RemoveTimedStateParticle(StateParticleType stateType)
        {
            for(int i = timedStateParticles.Count - 1; i > -1; i--)
            {
                if (timedStateParticles[i].GetComponent<WRLD_STATE_PARTICLE>().stateType == stateType)
                {
                    Destroy(timedStateParticles[i]);
                    timedStateParticles.RemoveAt(i);
                }
            }
        }
    }
} 