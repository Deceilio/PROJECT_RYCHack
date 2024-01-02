using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adnan.RYCHack
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        CharacterManager character; // Reference to the Character Manager script
        AudioSource audioSource; // Reference to the Audio Source component

        // Attacking grunts
        // Taking damage grunts

        [Header("TAKING DAMAGE SOUNDS")]
        public AudioClip[] takingDamageSounds; // Audio clips for taking damage sounds
        public List<AudioClip> potentialDamageSounds; // Audio clips for potential damage sounds
        private AudioClip lastDamageSoundPlayed; // Tells which was the last damage sound played

        [Header("WEAPONS EFFECT SOUNDS")]
        private List<AudioClip> potentialWeaponEffects; // List of all the Weapon effects sound played when you use the weapon
        private AudioClip lastWeaponEffect; // Tells which was the last weapon effect sound played                                               

        // Footsteps sounds

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            character = GetComponent<CharacterManager>();
        }
        public virtual void PlaySoundFX(AudioClip soundFX)
        {
            audioSource.PlayOneShot(soundFX);
        }
        public virtual void PlayRandomDamageSoundFX()
        {
            potentialDamageSounds = new List<AudioClip>();

            foreach(var damageSound in takingDamageSounds)
            {
                // BELOW CODE: If the potential damage sound has not been played before, we add it as a potential(stops repeated damage sounds)
                if(damageSound != lastDamageSoundPlayed)
                {
                    potentialDamageSounds.Add(damageSound);
                }
            }

            int randomValue = Random.Range(0, potentialDamageSounds.Count);
            lastDamageSoundPlayed = takingDamageSounds[randomValue];
            audioSource.PlayOneShot(takingDamageSounds[randomValue], 0.4f);
        }
        public virtual void PlayRandomSoundFXFromArray(AudioClip[] soundArray)
        {
            int index = Random.Range(0, soundArray.Length);

            PlaySoundFX(soundArray[index]);
        }
    }
}
