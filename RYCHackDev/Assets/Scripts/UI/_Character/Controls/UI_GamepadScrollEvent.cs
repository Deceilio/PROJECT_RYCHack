using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Deceilio.Psychain
{
    public class UI_GamepadScrollEvent: MonoBehaviour
    {
        [Header("Resources")]
        public UI_GamepadManager gamepadManager; // Reference to the gamepad manager

        [Header("Settings")]
        public bool requireSelecting; // Checks if scroll event required selecting
        public bool optimizeUpdates; // Checks if scroll event required optimise updates
        [Range(0.0001f, 0.25f)] public float changeValue = 0.01f; // Change value for the scroll event

        [Header("Slider")]
        public bool useSlider; // Use slider or not
        public bool reversePosition; // Reverse position of the slider or not
        public Slider sliderObject; // Reference to the slider object

        [Header("Scrollbar")]
        public bool useScrolbar; // Checks if using scrollbar or not
        public bool isHorizontalScrollbar; // Checks if it is horizontal scroll bar or nor
        public Scrollbar scrollbarObject; // Reference to the Scrollbar object

        [Header("Horizontal Selector")]
        public bool useHorizontalSelector; // Checks if using horizontal selector or not
        public float selectorCooldown = 0.3f; // Cooldown for the selector
        public UI_HorizontalSelector selectorObject; // Reeference to horizontal selector for selector object
        bool hsIsInCooldown = false; // Checks if the selector is in cooldown or not

        EventTrigger triggerEvent;

        void Start()
        {
            if (gamepadManager == null)
            {
                var tempManager = (UI_GamepadManager)GameObject.FindObjectsOfType(typeof(UI_GamepadManager))[0];
               
                if (tempManager != null)
                    gamepadManager = tempManager;
            }

            if (optimizeUpdates == true)
            {
                triggerEvent = gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry selectEntry = new EventTrigger.Entry();
                selectEntry.eventID = EventTriggerType.Select;
                selectEntry.callback.AddListener((eventData) => { this.enabled = true; });
                triggerEvent.triggers.Add(selectEntry);

                EventTrigger.Entry deselectEntry = new EventTrigger.Entry();
                deselectEntry.eventID = EventTriggerType.Deselect;
                deselectEntry.callback.AddListener((eventData) => { this.enabled = false; });
                triggerEvent.triggers.Add(deselectEntry);

                this.enabled = false;
            }
        }

        void Update()
        {
            if (Gamepad.current == null || gamepadManager == null)
                return;

            if (useSlider == true)
            {                        
                if (requireSelecting == true && useSlider == true && EventSystem.current != sliderObject)
                    return;

                if (reversePosition == false && gamepadManager.hAxis >= 0.1f)
                    sliderObject.value -= changeValue * gamepadManager.hAxis;
                if (reversePosition == true && gamepadManager.hAxis >= 0.1f)
                    sliderObject.value += changeValue * gamepadManager.hAxis;
                else if (reversePosition == false && gamepadManager.hAxis <= -0.1f)
                    sliderObject.value += changeValue * Mathf.Abs(gamepadManager.hAxis);
                else if (reversePosition == true && gamepadManager.hAxis <= -0.1f)
                    sliderObject.value -= changeValue * Mathf.Abs(gamepadManager.hAxis);
            }

            if (useScrolbar == true)
            {
                if (requireSelecting == true && useScrolbar == true && EventSystem.current != scrollbarObject)
                    return;

                if (isHorizontalScrollbar == false && gamepadManager.vAxis >= 0.1f && scrollbarObject.value <= 1)
                    scrollbarObject.value += changeValue * gamepadManager.vAxis;
                else if (isHorizontalScrollbar == false && gamepadManager.vAxis <= -0.1 && scrollbarObject.value >= 0)
                    scrollbarObject.value -= changeValue * Mathf.Abs(gamepadManager.vAxis);
                else if (isHorizontalScrollbar == true && gamepadManager.hAxis >= 0.1 && scrollbarObject.value <= 1)
                    scrollbarObject.value += changeValue * gamepadManager.hAxis;
                else if (isHorizontalScrollbar == true && gamepadManager.hAxis <= -0.1 && scrollbarObject.value >= 0)
                    scrollbarObject.value -= changeValue * Mathf.Abs(gamepadManager.hAxis);
            }

            if (useHorizontalSelector == true && hsIsInCooldown == false)
            {
                if (gamepadManager.hAxis >= 0.75)
                {
                    selectorObject.ForwardClick();
                    hsIsInCooldown = true;
                    StartCoroutine("SelectorCooldownTimer");
                }

                else if (gamepadManager.hAxis <= -0.75)
                {
                    selectorObject.PreviousClick();
                    hsIsInCooldown = true;
                    StartCoroutine("SelectorCooldownTimer");
                }
            }
        }

        IEnumerator SelectorCooldownTimer()
        {
            yield return new WaitForSecondsRealtime(selectorCooldown);
            hsIsInCooldown = false;
        }
    }
}