using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class CharacterWeaponManager : MonoBehaviour
    {
        [Header("BUFFED OBJECT")]
        [SerializeField] GameObject buffedObject; // Basically an object which will buffed, example - change in blade

        [Header("BUFF FX")]
        [SerializeField] GameObject physicalBuffFX; // VFX will show after the weapon is physical buff
        [SerializeField] GameObject fireBuffFX; // VFX will show after the weapon is fire buff

        [Header("TRAIL FX")]
        [SerializeField] ParticleSystem defaultTrailFX; // Default trail for the weapon buff
        [SerializeField] ParticleSystem fireTrailFX; // Fire trail for the weapon buff

        private bool weaponisBuffed; // Checks if the weapon is buffed or not
        private BuffClass weaponBuffClass; // Reference to the enum for the weapon buff classes

        [HideInInspector] public WRLD_MELEE_WEAPON_DAMAGE_HITBOX damageHitbox; // Reference to the Melee Weapon Damage Hitbox script
        public AudioSource audioSource; // Reference to the audio source component

        private void Awake()
        {
            damageHitbox = GetComponentInChildren<WRLD_MELEE_WEAPON_DAMAGE_HITBOX>();
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        public void BuffWeapon(BuffClass buffClass, float physicalBuffDamage, float fireBuffDamage, float poiseBuffDamage)
        {
            // BELOW CODE: Reset ny active buff
            DebuffWeapon();
            weaponisBuffed = true;
            weaponBuffClass = buffClass;
            audioSource.Play();

            switch(buffClass)
            {
                case BuffClass.Physical: physicalBuffFX.SetActive(true);
                    break;

                case BuffClass.Fire: fireBuffFX.SetActive(true);
                    break;

                default:
                    break;
            }

            damageHitbox.physicalBuffDamage = physicalBuffDamage;
            damageHitbox.fireBuffDamage = fireBuffDamage;
            damageHitbox.poiseBuffDamage = poiseBuffDamage;

        }
        public void DebuffWeapon()
        {
            weaponisBuffed = false;
            audioSource.Stop();
            physicalBuffFX.SetActive(false);
            fireBuffFX.SetActive(false);

            damageHitbox.physicalBuffDamage = 0;
            damageHitbox.fireBuffDamage = 0;
            damageHitbox.poiseBuffDamage = 0;
        }
        public void PlayWeaponTrailFX()
        {
            if(weaponisBuffed)
            {
                // BELOW CODE: If our weapon is physically buffed, Play the default trail
                switch (weaponBuffClass)
                {
                    case BuffClass.Physical:
                        if (defaultTrailFX == null)
                            return;
                        defaultTrailFX.Play();
                        break;

                    // BELOW CODE: If our weapon is fire buffed, Play the default trail
                    case BuffClass.Fire:
                        if (fireTrailFX == null)
                            return;
                        fireTrailFX.Play();
                        break;

                    default:
                        defaultTrailFX.Play();
                        break;
                }
            }
        }
    }
}