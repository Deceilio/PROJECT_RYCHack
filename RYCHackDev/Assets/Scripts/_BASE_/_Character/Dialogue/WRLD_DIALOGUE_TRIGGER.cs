using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_DIALOGUE_TRIGGER : MonoBehaviour
    {
        [HideInInspector] public WRLD_EVENT_MANAGER eventManager;
        public GameObject targetObject;
        public List<WRLD_DIALOGUE> dialogues;

        private void Awake()
        {
            eventManager = FindObjectOfType<WRLD_EVENT_MANAGER>();
        }
        public void TriggerDialogues()
        {
            FindObjectOfType<WRLD_DIALOGUE_MANAGER>().StartDialogues(dialogues);
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Character")) // Assuming the trigger should only activate for objects with the "Player" tag
            {
                if(gameObject.tag == "NPC")
                {
                    // Enable the targetObject when the player is inside the trigger
                    targetObject.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        TriggerDialogues();
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Character")) // Assuming the trigger should only deactivate for objects with the "Player" tag
            {
                // Disable the targetObject when the player exits the trigger
                targetObject.SetActive(false);
            }
        }
    }
}