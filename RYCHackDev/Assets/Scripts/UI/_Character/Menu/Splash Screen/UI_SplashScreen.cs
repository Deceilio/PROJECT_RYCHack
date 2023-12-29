using System.Collections;
using UnityEngine;

namespace Deceilio.Psychain
{
    [RequireComponent(typeof(Animator))]
    public class UI_SplashScreen: MonoBehaviour
    {
        [Header("Resources")]
        public UI_DissolveEffect dissolveEffect; // Reference to the Dissolve Effect script
        public AudioSource soundSource; // Reference to the audio source for the source sound
        public AudioClip inSound; // Reference to the splash screen in sound
        public AudioClip outSound; // Reference to the splash screen out sounds
        Animator objAnimator; // Animator for the splash sscreen

        [Header("Settings")]
        [Range(3, 30)] public float screenTime = 8; // Screen time of the splash screen
        [Range(0.1f, 1)] public float titleSpeed = 1; // Title speed of the splash screen
        [Range(1, 10)] public float transitionMultiplier = 4; // Transition multiplier for the splash screen

        void OnEnable()
        {
            if (objAnimator == null)
                objAnimator = gameObject.GetComponent<Animator>();

            objAnimator.SetFloat("Speed", titleSpeed);
            dissolveEffect.gameObject.SetActive(true);
            dissolveEffect.location = 0;
            dissolveEffect.DissolveOut();
            StartCoroutine("StartDissolveIn");

            if (soundSource != null && inSound != null)
                soundSource.PlayOneShot(inSound);
        }

        IEnumerator StartDissolveIn()
        {
            yield return new WaitForSecondsRealtime(screenTime - dissolveEffect.animationSpeed * transitionMultiplier);
            dissolveEffect.DissolveIn();

            if (soundSource != null && inSound != null)
                soundSource.PlayOneShot(outSound);

            StopCoroutine("StartDissolveIn");
        }
    }
}