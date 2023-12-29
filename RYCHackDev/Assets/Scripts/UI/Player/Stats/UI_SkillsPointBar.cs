using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_SkillsPointBar : MonoBehaviour
    {
        [HideInInspector] public Slider slider; // Reference to the Skills Point Bar bossHealthSlider
        private void Awake()
        {
            slider = GetComponent<Slider>();
        }
        public void SetMaxSkillPoints(float maxSkillPoints)
        {
            slider.maxValue = maxSkillPoints;
            slider.value = maxSkillPoints;
        }
        public void SetCurrentSkillPoints(float currentSkillPoints)
        {
            slider.value = currentSkillPoints;
        }
    }
}