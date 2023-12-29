using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain 
{
    public class WRLD_EVENT_MANAGER : MonoBehaviour
    {
        public List<WRLD_FOG_WALL> fogWalls; // List of fog walls in the scene
        public UI_BossHealthBar bossHealthBar; // Reference to the Boss Health Bar script
        public AICharacterBossManager boss; // Reference to the A.I Character Boss Manager script

        public bool bossFightIsActive; // Checks if character is currently fighting the boss
        public bool bossHasBeenAwakened; // Wake the boss/watched cutscene but died during fight
        public bool bossHasBeenDefeated; // Checks if the boss has been defeated or not

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UI_BossHealthBar>();
        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwakened = true;
            bossHealthBar.SetUIHealthBarToActive();

            foreach(var fogWall in fogWalls)
            {
                fogWall.ActivateFogWall();
            }
        }

        public void BossHasBeenDefeated()
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;

            foreach (var fogWall in fogWalls)
            {
                fogWall.DeactivateFogWall();
            }
        }        
    }
}