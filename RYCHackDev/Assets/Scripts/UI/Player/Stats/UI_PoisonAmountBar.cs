using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_PoisonAmountBar : MonoBehaviour
    {
        [HideInInspector] public Slider slider; // Reference to the Poison Build Up Slider

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 100;
            gameObject.SetActive(false);
        }
        public void SetPoisonAmount(int poisonAmount)
        {
            if(poisonAmount > 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            
            slider.value = poisonAmount;
        }
    }
}
