using UnityEngine;
using UnityEngine.EventSystems;

namespace Deceilio.Psychain
{
    public class UI_ElementSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        // Resources
        public AudioSource audioSource; // Reference to the audio source for playing sound
        public AudioClip hoverSound; // Reference to the hover sound
        public AudioClip clickSound; // Reference to the click sound

        // Settings
        public bool enableHoverSound = true; // Checks to enable or disable hover sound
        public bool enableClickSound = true; // Checks to enable or disable click sound

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (audioSource != null && enableHoverSound == true)
                audioSource.PlayOneShot(hoverSound);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (audioSource != null && enableClickSound == true)
                audioSource.PlayOneShot(clickSound);
        }
    }
}