using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain 
{
    public class WRLD_PASS_THROUGH_FOG_WALL : WRLD_INTERACTABLE
    {
        WRLD_EVENT_MANAGER worldEventManager; // Reference to the World Event Manager script
        [SerializeField] private AudioSource bossThemePlayer; // Reference to the Boss music 
        
        protected override void Awake()
        {
            worldEventManager = FindObjectOfType<WRLD_EVENT_MANAGER>();
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            player.PassThroughFogWallInteraction(transform);
            bossThemePlayer.Play();
            worldEventManager.ActivateBossFight();
        }
    }
}