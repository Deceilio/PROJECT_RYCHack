using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_DAMAGE_TEST_HITBOX : MonoBehaviour
    {
        public int physicalDamage = 25;
        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if(player != null)
            {
               //player.playerStatsManager.TakeDamage(physicalDamage, 0, "Damage_01", player);
            }
        }
    }
}