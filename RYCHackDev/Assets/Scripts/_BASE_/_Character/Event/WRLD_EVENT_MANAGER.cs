using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Adnan.RYCHack 
{
    public class WRLD_EVENT_MANAGER : MonoBehaviour
    {
        [Header("EVENTS")]
        public bool marcusEventFinished = false; // Checks for if the marcus event finished or not

        [Header("DATA")]
        public float skyboxSpeed;
        public Animator marcusAnimator;
        public GameObject winnerOver;
        public GameObject marcusObject;
        private void Update()
        {
            float rotationValue = Time.time * skyboxSpeed;
            rotationValue = rotationValue % 360f;  // Keep the value between 0 and 360

            RenderSettings.skybox.SetFloat("_Rotation", rotationValue);
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