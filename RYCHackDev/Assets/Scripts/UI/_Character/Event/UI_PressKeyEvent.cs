using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Deceilio.Psychain
{
    public class UI_PressKeyEvent : MonoBehaviour
    {
        // Settings
        public InputAction hotkey; // Hotkey for the press event

        // Events
        public UnityEvent onPressEvent; // Event for button press

        void Start()
        {
            hotkey.Enable();
        }

        void Update()
        {
            if (hotkey.triggered)
                onPressEvent.Invoke();
        }
    }
}