using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_AICharacterHealthBar : MonoBehaviour
    {
        public Slider healthSlider; // Reference to health bar's bossHealthSlider
        float timeUntilBarIsHidden = 0; // As the name suggest it's time until the health bar is hidden

        [Header("YELLOW BAR")]
        [SerializeField] UI_AICharacterYellowBar yellowBar; // Reference to the AI Character Yellow Bar Script
        [SerializeField] float yellowBarTimer = 3; // Timer for the yellow bar
        [SerializeField] TextMeshProUGUI damageText; // Damage Text
        [SerializeField] int currentDamageTaken; // Current damage taken by the enemy

        private void Awake() 
        {
            yellowBar = GetComponentInChildren<UI_AICharacterYellowBar>();
        }
        private void OnDisable()
        {
            currentDamageTaken = 0; // Resets damage taken text pop up on disable
        }
        public void SetMaxHealth(int maxHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;

            if (yellowBar != null)
            {
                yellowBar.SetMaxStat(maxHealth);
            }
        }
        public void SetCurrentHealth(int currentHealth)
        {
            if (yellowBar != null)
            {
                yellowBar.gameObject.SetActive(true); // Triggers the OnEnable() function   
                yellowBar.timer = yellowBarTimer; // Everytime we get hit we reset the timer

                if (currentHealth > healthSlider.value)
                {
                    yellowBar.slider.value = currentHealth;
                }
            }

            currentDamageTaken = currentDamageTaken + Mathf.RoundToInt(healthSlider.value - currentHealth);
            damageText.text = currentDamageTaken.ToString();

            healthSlider.value = currentHealth;
            //healthSlider.gameObject.SetActive(true);
            timeUntilBarIsHidden = 5;
        }

        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward); // This bit of code face the UI to face camera all the time

            timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

            if (healthSlider != null)
            {
                if(timeUntilBarIsHidden <= 0)
                {
                    timeUntilBarIsHidden = 0;
                    healthSlider.gameObject.SetActive(false);
                }
                else 
                {
                    if(!healthSlider.gameObject.activeInHierarchy)
                    {
                        healthSlider.gameObject.SetActive(true);
                    }    
                }

                if(healthSlider.value <= 0)
                {
                    Destroy(healthSlider.gameObject); 
                }
            }
        }
    }
}
