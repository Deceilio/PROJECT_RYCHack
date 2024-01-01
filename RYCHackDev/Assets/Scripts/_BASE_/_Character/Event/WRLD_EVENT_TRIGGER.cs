using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_EVENT_TRIGGER : MonoBehaviour
    {
        [HideInInspector] public WRLD_EVENT_MANAGER eventManager;
        public GameObject helpBox;
        public TextMeshProUGUI helpText;
        void Awake()
        {
            eventManager = FindObjectOfType<WRLD_EVENT_MANAGER>();
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.tag == "Character")
            {
                if (!eventManager.marcusEventFinished)
                {
                    Debug.Log("Marcus Event not finished yet");
                    helpBox.SetActive(true);
                    helpText.text = "You haven't talked to Marcus yet.";
                }
                else if(eventManager.marcusEventFinished)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Character")) 
            {
                helpBox.SetActive(false);
            }
        }
    }
}