using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Deceilio.Psychain
{
    public class UI_MainMenuManager: MonoBehaviour
    {
        //"LIST"
        public List<PanelItem> panels = new List<PanelItem>();

        //"SETTINGS"
        public bool settingsHelper; // Check if the settings helper is on or not
        public bool instantInOnEnable; // Check if the main panel should instantly start on enable or not
        public int currentPanelIndex = 0; // Reference to the current panel index
        private int currentButtonIndex = 0; // Reference to the current button index
        private int newPanelIndex; // Reference to the new panel index
        [Range(0.75f, 4)] public float disablePanelAfter = 1; // Range to disable the panel
        [Range(0, 1)] public float animationSmoothness = 0.25f; // Range to the smooth the animation
        [Range(0.75f, 4)] public float animationSpeed = 1; // Range of the animation speed

        //"VARIABLES"
        private GameObject currentPanel; // Reference to the current panel
        private GameObject nextPanel; // Reference to the next panle
        private GameObject currentButton; // Reference to the current button
        private GameObject nextButton; // Reference to the next button

        private Animator currentPanelAnimator; // Reference to the current panel animator component
        private Animator nextPanelAnimator; // Reference to the next panel animator component
        private Animator currentButtonAnimator; // Reference to the current button animator component
        private Animator nextButtonAnimator; // Reference to the next button animator component

        //"ANIMATOR STATES"
        string panelFadeIn = "Panel In"; // Name of the panel fade in state
        string panelFadeOut = "Panel Out"; // Name of the panel fade out state
        string panelInstantIn = "Instant In"; // Name of the panel instant in state
        string buttonFadeIn = "Hover to Pressed"; // Name of the button fade in state
        string buttonFadeOut = "Pressed to Normal"; // Name of the button fade out state
        string buttonFadeNormal = "Pressed to Normal"; // Name of the button fade normal state

        bool firstTime = true; // Checks if it's player's first time
        [HideInInspector] public bool gamepadEnabled = false; // Checks if the gamepad is enable or not

        [System.Serializable]
        public class PanelItem
        {
            public string panelName = "My Panel";
            public GameObject panelObject;
            public GameObject panelButton;
            public GameObject defaultSelected;
        }

        void OnEnable()
        {
            if (panels[currentPanelIndex].panelButton != null)
            {
                currentButton = panels[currentPanelIndex].panelButton;
                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play(buttonFadeIn);
            }

            currentPanel = panels[currentPanelIndex].panelObject;
            currentPanel.SetActive(true);
            currentPanelAnimator = currentPanel.GetComponent<Animator>();

            if (instantInOnEnable == true && currentPanelAnimator.gameObject.activeInHierarchy == true)
                currentPanelAnimator.Play(panelInstantIn);
            else if (instantInOnEnable == false && currentPanelAnimator.gameObject.activeInHierarchy == true)
                currentPanelAnimator.Play(panelFadeIn);

            firstTime = false;

            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].panelObject == null) { continue; }
                if (i != currentPanelIndex) { panels[i].panelObject.SetActive(false); }
            }
        }

        public void EnableFirstPanel()
        {
            try
            {
                panels[currentPanelIndex].panelObject.GetComponent<Animator>().Play("Instant In");
                panels[currentPanelIndex].panelButton.GetComponent<Animator>().Play("Instant In");
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(panels[currentPanelIndex].panelObject.GetComponent<RectTransform>());
            }

            catch { }
        }

        public void OpenFirstTab()
        {
            if (currentPanelIndex != 0)
                OpenPanel(panels[0].panelName);

            else if (currentPanelIndex == 0 && settingsHelper == true && firstTime == false)
            {
                OpenPanel(panels[1].panelName);
                OpenPanel(panels[0].panelName);

                if (panels[0].defaultSelected != null && gamepadEnabled == true)
                    EventSystem.current.SetSelectedGameObject(panels[0].defaultSelected);
            }
        }

        public void OpenPanel(string newPanel)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].panelName == newPanel)
                    newPanelIndex = i;
            }

            if (newPanelIndex != currentPanelIndex)
            {
                StopCoroutine("DisablePreviousPanel");

                currentPanel = panels[currentPanelIndex].panelObject;
                currentPanelIndex = newPanelIndex;
                nextPanel = panels[currentPanelIndex].panelObject;
                nextPanel.SetActive(true);

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                nextPanelAnimator = nextPanel.GetComponent<Animator>();

                currentPanelAnimator.SetFloat("Anim Speed", animationSpeed);
                currentPanelAnimator.CrossFade(panelFadeOut, animationSmoothness);
                nextPanelAnimator.SetFloat("Anim Speed", animationSpeed);
                nextPanelAnimator.CrossFade(panelFadeIn, animationSmoothness);

                StartCoroutine("DisablePreviousPanel");

                if (panels[currentButtonIndex].panelButton != null)
                    currentButton = panels[currentButtonIndex].panelButton;

                currentButtonIndex = newPanelIndex;

                if (panels[currentButtonIndex].panelButton != null)
                {
                    nextButton = panels[currentButtonIndex].panelButton;
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    nextButtonAnimator = nextButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeOut);
                    nextButtonAnimator.Play(buttonFadeIn);
                }

                if (panels[currentPanelIndex].defaultSelected != null && gamepadEnabled == true)
                    EventSystem.current.SetSelectedGameObject(panels[currentPanelIndex].defaultSelected);
            }
        }

        public void NextPage()
        {
            if (currentPanelIndex <= panels.Count - 2)
            {
                StopCoroutine("DisablePreviousPanel");

                currentPanel = panels[currentPanelIndex].panelObject;

                if (panels[currentButtonIndex].panelButton != null)
                    currentButton = panels[currentButtonIndex].panelButton;

                if (panels[currentButtonIndex + 1].panelButton != null)
                    nextButton = panels[currentButtonIndex + 1].panelButton;

                currentPanel.gameObject.SetActive(true);
                currentPanelAnimator = currentPanel.GetComponent<Animator>();

                if (currentButton != null)
                {
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeNormal);
                }

                currentPanelAnimator.SetFloat("Anim Speed", animationSpeed);
                currentPanelAnimator.CrossFade(panelFadeOut, animationSmoothness);

                currentPanelIndex += 1;
                currentButtonIndex += 1;
                nextPanel = panels[currentPanelIndex].panelObject;
                nextPanel.gameObject.SetActive(true);

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextPanelAnimator.SetFloat("Anim Speed", animationSpeed);
                nextPanelAnimator.CrossFade(panelFadeIn, animationSmoothness);

                if (nextButton != null)
                {
                    nextButtonAnimator = nextButton.GetComponent<Animator>();
                    nextButtonAnimator.Play(buttonFadeIn);
                }

                if (panels[currentPanelIndex].defaultSelected != null && gamepadEnabled == true)
                    EventSystem.current.SetSelectedGameObject(panels[currentPanelIndex].defaultSelected);
            }
        }

        public void PrevPage()
        {
            if (currentPanelIndex >= 1)
            {
                StopCoroutine("DisablePreviousPanel");

                currentPanel = panels[currentPanelIndex].panelObject;

                if (panels[currentButtonIndex].panelButton != null)
                    currentButton = panels[currentButtonIndex].panelButton;

                if (panels[currentButtonIndex - 1].panelButton != null)
                    nextButton = panels[currentButtonIndex - 1].panelButton;
          
                currentPanel.gameObject.SetActive(true);
                currentPanelAnimator = currentPanel.GetComponent<Animator>();

                if (currentButton != null)
                {
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeNormal);
                }

                currentPanelAnimator.SetFloat("Anim Speed", animationSpeed);
                currentPanelAnimator.CrossFade(panelFadeOut, animationSmoothness);

                currentPanelIndex -= 1;
                currentButtonIndex -= 1;
                nextPanel = panels[currentPanelIndex].panelObject;
                nextPanel.gameObject.SetActive(true);

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextPanelAnimator.SetFloat("Anim Speed", animationSpeed);
                nextPanelAnimator.CrossFade(panelFadeIn, animationSmoothness);

                if (nextButton != null)
                {
                    nextButtonAnimator = nextButton.GetComponent<Animator>();
                    nextButtonAnimator.Play(buttonFadeIn);
                }

                if (panels[currentPanelIndex].defaultSelected != null && gamepadEnabled == true)
                    EventSystem.current.SetSelectedGameObject(panels[currentPanelIndex].defaultSelected);
            }
        }

        public void AddNewItem()
        {
            PanelItem newPanel = new PanelItem();
            panels.Add(newPanel);
        }

        IEnumerator DisablePreviousPanel()
        {
            yield return new WaitForSecondsRealtime(disablePanelAfter);
            currentPanel.SetActive(false);
        }
    }
}