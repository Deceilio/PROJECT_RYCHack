using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain 
{
    public class WRLD_EVENT_HITBOX_BEGIN_BOSS_FIGHT : MonoBehaviour
    {
        WRLD_EVENT_MANAGER worldEventManager; // Reference to the World Event Manager script
        [SerializeField] private AudioSource bossThemePlayer; // Reference to the Boss music 

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WRLD_EVENT_MANAGER>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Character")
            {
                bossThemePlayer.Play();
                worldEventManager.ActivateBossFight();
                Destroy(gameObject);
            }
        }
    }
}