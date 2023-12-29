using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Deceilio.Psychain
{
    [RequireComponent(typeof(Slider))]
    public class UI_SliderManager: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Resources
        public Animator sliderAnimator; // Animator for the slider
        public Slider mainSlider; // Main slider 
        public TextMeshProUGUI valueText; // Value text of the slider
        public TextMeshProUGUI popupValueText; // Pop up value text of the slider

        // Saving
        public bool enableSaving = false; // Checks if enable save or not
        public string sliderTag = "Tag Text"; // Tag of the slider
        public float saveValue; // Save value of the slider

        // Settings
        public bool usePercent = false; // Check is using percent for the slider value
        public bool showValue = true; // Check to show value or not
        public bool showPopupValue = true; // Check to show pop up value or not
        public bool useRoundValue = false; // Check to show round value or not
        public float valueMultiplier = 1; // Value multiplier for the slider

        void Start()
        {
            if (mainSlider == null)
                mainSlider = gameObject.GetComponent<Slider>();

            if (enableSaving == true)
            {
                if (PlayerPrefs.HasKey(sliderTag + "Slider") == false)
                    saveValue = mainSlider.value;
                else
                    saveValue = PlayerPrefs.GetFloat(sliderTag + "Slider");

                mainSlider.value = saveValue;
            }

            mainSlider.onValueChanged.AddListener(delegate
            {
                saveValue = mainSlider.value;
                UpdateUI();

                PlayerPrefs.SetFloat(sliderTag + "Slider", saveValue);
            });

            UpdateUI();
        }

        public void UpdateUI()
        {
            if (useRoundValue == true)
            {
                if (usePercent == true && valueText != null)
                    valueText.text = Mathf.Round(mainSlider.value * valueMultiplier).ToString() + "%";
                else if (usePercent == false && valueText != null)
                    valueText.text = Mathf.Round(mainSlider.value * valueMultiplier).ToString();
            }

            else
            {
                if (usePercent == true && valueText != null)
                    valueText.text = mainSlider.value.ToString("F1") + "%";
                else if (usePercent == false && valueText != null)
                    valueText.text = mainSlider.value.ToString("F1");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (showPopupValue == true)
                sliderAnimator.Play("Value In");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (showPopupValue == true)
                sliderAnimator.Play("Value Out");
        }
    }
}