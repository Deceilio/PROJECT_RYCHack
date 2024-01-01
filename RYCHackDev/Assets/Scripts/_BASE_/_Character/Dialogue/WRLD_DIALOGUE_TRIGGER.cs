using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_DIALOGUE_TRIGGER : MonoBehaviour
    {
        [HideInInspector] public PlayerManager player;
        [HideInInspector] public WRLD_EVENT_MANAGER eventManager;
        public GameObject targetObject;
        public List<WRLD_DIALOGUE> dialogues;
        private bool isPlayerInside = false;

        private void Awake()
        {
            eventManager = FindObjectOfType<WRLD_EVENT_MANAGER>();
            player = FindObjectOfType<PlayerManager>();
        }

        public void TriggerDialogues()
        {
            FindObjectOfType<WRLD_DIALOGUE_MANAGER>().StartDialogues(dialogues);
        }

        private void Update()
        {
            // Check for KeyCode.E input only if the player is inside the trigger
            if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
            {
                TriggerDialogues();
                // Disable the targetObject after triggering dialogues
                targetObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Character"))
            {
                if (gameObject.tag == "NPC")
                {
                    // Enable the targetObject when the player enters the trigger
                    targetObject.SetActive(true);
                    isPlayerInside = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Character"))
            {
                // Disable the targetObject when the player exits the trigger
                targetObject.SetActive(false);
                isPlayerInside = false;
            }
        }
    }
}
