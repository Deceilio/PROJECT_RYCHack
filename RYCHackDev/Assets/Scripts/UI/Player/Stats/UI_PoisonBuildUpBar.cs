using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_PoisonBuildUpBar : MonoBehaviour
    {
        [HideInInspector] public Slider slider; // Reference to the Poison Build Up slider
        
        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 0;
            gameObject.SetActive(false);
        }
        public void SetPoisonBuildUpAmount(float currentPoisonBuildUp)
        {
            slider.value = currentPoisonBuildUp;

            if(currentPoisonBuildUp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
