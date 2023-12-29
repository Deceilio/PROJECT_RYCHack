using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_CHARACTER_STATES_MANAGER : MonoBehaviour
    {
        public static WRLD_CHARACTER_STATES_MANAGER instance; // Static object for this script

        [Header("DAMAGE")]
        public TakeDamageState takeDamageState; // Reference to Take Damage State scriptable
        public TakeBlockedDamageState takeBlockedDamageState; // Reference to the Take blocked damage state scriptable

        [Header("POISON")]
        public PoisonBuildUpState poisonBuildUpState; // Reference to the poison build up effect scriptable object
        public PoisonedState poisonedState; // Reference to the poisoned state scriptable object
        public GameObject poisonFX; // Poison VFX 
        public AudioClip poisonSFX; // Poison sound effect

        private void Awake()
        {
           if(instance == null)
           {
               instance = this;
           }
           else
           {
               Destroy(gameObject);
           }
        }
    }
}