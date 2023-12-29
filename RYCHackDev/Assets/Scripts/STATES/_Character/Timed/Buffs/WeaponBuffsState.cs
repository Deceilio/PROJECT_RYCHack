using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Character States/Timed States/Buff/Weapon Buff State")]
    public class WeaponBuffsState : WRLD_CHARACTER_STATE
    {
        [Header("BUFFS INFORMATION")]
        [SerializeField] BuffClass buffClass; // Reference to the buff class enum
        [SerializeField] float lengthOfBuff; // Duration of buff effect
        public float timeRemainingOnBuff; // How much time remain for buff to remain
        [HideInInspector] public bool isRightHandedBuff; // Buff for right hand weapon

        [Header("BUFF SFX")]
        [SerializeField] AudioClip buffAmbientSound; // Buff surrounding sound when applied
        [SerializeField] float ambientSoundVolume = 0.3f; // The volume of buff surround sound

        [Header("DAMAGE INFORMATION")]
        [SerializeField] float buffBaseDamagePercentageMultiplier = 15; // How much % of the base damage is multiplied

        [Header("POISE BUFF")]
        [SerializeField] bool buffPoiseDamage; // Poise damage after buff is applied
        [SerializeField] float buffBasePoiseDamagePercentageMultiplier = 15; // How much % of the base poise damage is multiplied

        [Header("GENERAL")]
        [SerializeField] bool buffHasStarted = false; // Checks if the buff is started or not
        private CharacterWeaponManager weaponManager; // Reference to the Character Weapon Manager script

        public override void ProcessState(CharacterManager character)
        {
            base.ProcessState(character);

            if(!buffHasStarted)
            {
                timeRemainingOnBuff = lengthOfBuff;
                buffHasStarted = true;

                weaponManager = character.characterWeaponSlotManager.rightHandDamageHitbox.GetComponentInParent<CharacterWeaponManager>();
                weaponManager.audioSource.loop = true;
                weaponManager.audioSource.clip = buffAmbientSound;
                weaponManager.audioSource.volume = ambientSoundVolume;

                float baseWeaponDamage = weaponManager.damageHitbox.physicalDamage
                    + weaponManager.damageHitbox.fireDamage;

                float physicalBuffDamage = 0;

                float fireBuffDamage = 0;

                float poiseBuffDamage = 0;

                if(buffPoiseDamage)
                {
                    poiseBuffDamage = weaponManager.damageHitbox.poiseDamage * (buffBasePoiseDamagePercentageMultiplier / 100);
                }

                switch (buffClass)
                {
                    case BuffClass.Physical:     
                        physicalBuffDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100);
                        break;
                    case BuffClass.Fire:
                        fireBuffDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100);
                        break;
                    default:
                        break;
                }

                weaponManager.BuffWeapon(buffClass, physicalBuffDamage, fireBuffDamage, poiseBuffDamage);
            }

            if(buffHasStarted)
            {
                timeRemainingOnBuff = timeRemainingOnBuff - 1;

                Debug.Log("TIME REMAINING ON BUFF: " % WRLD_COLORIZE_EDITOR.Magenta % WRLD_FONT_FORMAT.Bold + timeRemainingOnBuff);

                if(timeRemainingOnBuff <= 0)
                {
                    weaponManager.DebuffWeapon();

                    if(isRightHandedBuff)
                    {
                        character.characterStatesManager.rightHandWeaponBuffsState = null;
                    }
                }
            }
        }
    }
}