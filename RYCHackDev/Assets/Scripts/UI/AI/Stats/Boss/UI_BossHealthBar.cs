using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Deceilio.Psychain
{
    public class UI_BossHealthBar : MonoBehaviour
    {
        [HideInInspector] public Slider bossHealthSlider; 

        [Header("BOSS DATA")]
        public TextMeshProUGUI bossName; // Name of the Boss

        [Header("YELLOW BAR")]
        [SerializeField] UI_BossYellowBar yellowBar; // Reference to the AI Character Yellow Bar Script
        [SerializeField] float yellowBarTimer = 3; // Timer for the yellow bar

        private void Awake()
        {
            bossHealthSlider = GetComponentInChildren<Slider>();
            bossName = GetComponentInChildren<TextMeshProUGUI>();
            yellowBar = GetComponentInChildren<UI_BossYellowBar>();
        }

        private void Start()
        {
            SetUIHealthBarToInActive();
        }

        public void SetBossName(string name)
        {
            bossName.text = name;
        }

        public void SetUIHealthBarToActive()
        {
            bossHealthSlider.gameObject.SetActive(true);
        }

        public void SetUIHealthBarToInActive()
        {
            bossHealthSlider.gameObject.SetActive(false);
        }
        
        public void SetBossMaxHealth(int maxHealth)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = maxHealth;

            if (yellowBar != null)
            {
                yellowBar.SetMaxStat(maxHealth);
            }
        }

        public void SetBossCurrentHealth(int currentHealth)
        {
            if (yellowBar != null)
            {
                yellowBar.gameObject.SetActive(true); // Triggers the OnEnable() function   
                yellowBar.timer = yellowBarTimer; // Everytime we get hit we reset the timer

                if (currentHealth > bossHealthSlider.value)
                {
                    yellowBar.slider.value = currentHealth;
                }
            }

            bossHealthSlider.value = currentHealth;
        }
    }
}