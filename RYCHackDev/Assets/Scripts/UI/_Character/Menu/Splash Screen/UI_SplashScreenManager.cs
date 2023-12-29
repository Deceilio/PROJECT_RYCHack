using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Deceilio.Psychain
{
    [DisallowMultipleComponent]
    public class UI_SplashScreenManager: MonoBehaviour
    {
        //"CONTENT"
        public List<UI_SplashScreen> splashScreenTitles = new List<UI_SplashScreen>(); // Reference to the all the splash screen objects

        //"RESOURCES"
        public GameObject splashScreen; // Reference to the splash screen object
        public GameObject modalWindowParent;  // Reference to the model window parent object
        public GameObject mainPanelParent;  // Reference to the main panel object
        public UI_DissolveEffect transitionHelper; // Reference to the transition object
        public UI_MainMenuManager mainPanelManager;  // Reference to the Main Panel Manager script

        //"SETTINGS"
        public bool disableSplashScreen; // Checks if splash screen should be disable or not
        public bool showOnlyOnce = true; // Checks if splash screen should be shown only once or not
        public bool skipOnAnyKeyPress = false; // Checks if splash screen should be skip on any key press or not
        public float disableTimer = 0; // Timer for the disabiling splash screen
        [Range(0, 5)] public float startDelay = 0.5f; // Delay for starting the splash screen
        public UnityEvent onSplashScreenEnd; // Event for splash screen end

        GameObject currentTitleObj; // Reference to the current title object
        int currentTitleIndex; // Reference to the current title index
        float currentTitleDuration; // Reference to the current title duration

        void OnEnable()
        {
            if (showOnlyOnce && GameObject.Find("[Psychain - Splash Screen Helper]") != null) 
            { 
                disableSplashScreen = true; 
            }

            if (disableSplashScreen)
            {
                splashScreen.SetActive(false);
                modalWindowParent.SetActive(true);

                mainPanelParent.gameObject.SetActive(true);
                transitionHelper.gameObject.SetActive(true);

                mainPanelManager.EnableFirstPanel();

                transitionHelper.location = 0;
                transitionHelper.DissolveOut();

                onSplashScreenEnd.Invoke();
            }

            else
            {
                splashScreen.SetActive(true);
                modalWindowParent.SetActive(false);

                mainPanelParent.gameObject.SetActive(false);
                transitionHelper.gameObject.SetActive(false);

                InitializeTitles();         
            }

            if (showOnlyOnce)
            {
                GameObject tempHelper = new GameObject();
                tempHelper.name = "[Psychain - Splash Screen Helper]";
                DontDestroyOnLoad(tempHelper);
            }
        }

        void Update()
        {
            if (!skipOnAnyKeyPress)
                return;

            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
                || (Mouse.current != null && Mouse.current.press.wasPressedThisFrame)
                || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
            {
                skipOnAnyKeyPress = false;

                StopCoroutine("DisableSplashScreen");
                StopCoroutine("InitializeTitleDuration");
                StopCoroutine("ProcessStartDelay");

                disableTimer = 0;

                StartCoroutine("DisableSplashScreen");
            }
        }

        public void InitializeTitles()
        {
            if (splashScreenTitles.Count != 0)
            {
                for (int i = 0; i < splashScreenTitles.Count; ++i)
                    disableTimer = disableTimer + splashScreenTitles[i].screenTime;

                foreach (Transform child in splashScreenTitles[0].gameObject.transform.parent)
                    child.gameObject.SetActive(false);

                currentTitleIndex = 0;
                currentTitleDuration = splashScreenTitles[currentTitleIndex].screenTime;
                currentTitleObj = splashScreenTitles[currentTitleIndex].gameObject;

                if (startDelay == 0)
                {
                    currentTitleObj.SetActive(true);
                    EnableTransition();
                }

                else
                {
                    StartCoroutine("ProcessStartDelay");
                }
            }
        }

        public void EnableTransition()
        {
            StartCoroutine("DisableSplashScreen");
            StartCoroutine("InitializeTitleDuration");
        }

        IEnumerator ProcessStartDelay()
        {
            yield return new WaitForSecondsRealtime(startDelay);
        
            currentTitleObj.SetActive(true);

            StopCoroutine("ProcessStartDelay");
            EnableTransition();
        }

        IEnumerator InitializeTitleDuration()
        {
            yield return new WaitForSecondsRealtime(currentTitleDuration);
           
            currentTitleObj.SetActive(false);
            currentTitleIndex++;
            
            try
            {
                currentTitleDuration = splashScreenTitles[currentTitleIndex].screenTime;
                currentTitleObj = splashScreenTitles[currentTitleIndex].gameObject;
                currentTitleObj.SetActive(true);
                StartCoroutine("InitializeTitleDuration");
            }

            catch 
            {
                StopCoroutine("InitializeTitleDuration");
            }
        }

        IEnumerator DisableSplashScreen()
        {
            yield return new WaitForSecondsRealtime(disableTimer);

            splashScreen.SetActive(false);
            modalWindowParent.SetActive(true);

            mainPanelParent.gameObject.SetActive(true);
            transitionHelper.gameObject.SetActive(true);

            mainPanelManager.EnableFirstPanel();

            transitionHelper.location = 0;
            transitionHelper.DissolveOut();

            onSplashScreenEnd.Invoke();

            StopCoroutine("StartTransition");
        }
    }
}