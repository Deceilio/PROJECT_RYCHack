using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_BossYellowBar : MonoBehaviour
    {
        public Slider slider; // Reference to the bossHealthSlider game component
        UI_BossHealthBar parentHealthBar; // Reference to the Boss Health Bar script

        public float timer; // Float for using timer

        private void Awake()
        {
            slider = GetComponent<Slider>();
            parentHealthBar = GetComponentInParent<UI_BossHealthBar>();
        }
        private void OnEnable()
        {
            if (timer <= 0)
            {
                timer = 2f; // The amount of time you want the bar to be seen before subtracting/equalizing
            }
        }
        public void SetMaxStat(int maxStat)
        {
            slider.maxValue = maxStat;
            slider.value = maxStat;
        }
        private void Update()
        {
            if (timer <= 0)
            {
                if (slider.value > parentHealthBar.bossHealthSlider.value)
                {
                    slider.value = slider.value - 0.5f;
                }
                else if (slider.value <= parentHealthBar.bossHealthSlider.value)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                timer = timer - Time.deltaTime;
            }
        }
    }
}