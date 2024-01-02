using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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

        [Header("EVENTS")]
        public bool marcusEventFinished = false; // Checks for if the marcus event finished or not

        [Header("DATA")]
        public float skyboxSpeed;
        public Animator marcusAnimator;
        public GameObject winnerOver;
        public GameObject marcusObject;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UI_BossHealthBar>();
        }
        private void Update()
        {
            float rotationValue = Time.time * skyboxSpeed;
            rotationValue = rotationValue % 360f;  // Keep the value between 0 and 360

            RenderSettings.skybox.SetFloat("_Rotation", rotationValue);

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

        public void RestartScene()
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ContinueScene()
        {
            Destroy(marcusObject);
            winnerOver.SetActive(false);
        }
    }
}