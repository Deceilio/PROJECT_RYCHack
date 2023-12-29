using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Deceilio.Psychain
{
    public class UI_PopUpManager : MonoBehaviour
    {
        [Header("YOU DIED! POP UP")]
        [SerializeField] GameObject youDiedPopUpGameObject; // Reference to the you died pop up game object
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText; // Background text for you died pop up object
        [SerializeField] TextMeshProUGUI youDiedPopUpText; // Pop Up text for you died pop up object
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup; // Canvas group for you died pop up (Allow to fade)

        public void ShowYouDiedPopUp()
        {
            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19));
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if(duration > 0f)
            {
                text.characterSpacing = 0; // Reset the character spacing
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                }
            }
        }
    
        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if(duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                }
            }

            canvas.alpha = 1;
            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if(duration > 0)
            {
                while(delay > 0)
                {
                    delay = delay - Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                }
            }

            canvas.alpha = 0;
            yield return null;
        }
    }
}