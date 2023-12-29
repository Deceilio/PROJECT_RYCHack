using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

namespace Deceilio.Psychain
{
    [RequireComponent(typeof(Button))]
    public class UI_ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        //"CONTENT"
        public string buttonText = "Button"; // The text of the button
        public UnityEvent clickEvent; // Event for the button click
        public UnityEvent hoverEvent; // Event for the button hover
        public UnityEvent onLeave; // Event for the button leave
        public AudioClip hoverSound; // The hover sound of the button
        public AudioClip clickSound; // The click sound of the button
        public Button buttonVar; // Reference to the variation button

        //"RESOURCES"
        public TextMeshProUGUI normalText;  // Reference to the normal text of the button
        public TextMeshProUGUI highlightedText; // Reference to the highlighted text of the button
        public AudioSource soundSource; // Reference to the audio source of the button
        public GameObject rippleParent; // Reference to the button ripple effect

        //"SETTINGS"
        public bool useCustomContent = false; // Check if button uses the custom content
        public bool enableButtonSounds = false; // Check if button sounds should be enable or disbale
        public bool useHoverSound = true; // Check if button using hover sound or not
        public bool useClickSound = true; // Check if button using click sound or not
        public bool useRipple = true; // Check if button using ripple or not
        public bool fixWidth = false; // Check if button have fix width or not

        //"RIPPLE"
        public RippleUpdateMode rippleUpdateMode = RippleUpdateMode.UNSCALED_TIME; // Enum for the type of ripple update
        public Sprite rippleShape; // Reference to the ripple sprite
        [Range(0.1f, 5)] public float speed = 1f; // Reference to the ripple speed
        [Range(0.5f, 25)] public float maxSize = 4f; // Reference to the ripple maximum size
        public Color startColor = new Color(1f, 1f, 1f, 1f); // Reference to the ripple start color
        public Color transitionColor = new Color(1f, 1f, 1f, 1f); // Reference to the ripple transition color
        public bool renderOnTop = false; // Check if ripple can render on top or not
        public bool centered = false; // Check if ripple can be centered or not

        //"HELPERS"
        float cachedStateLength = 0.5f; // Length of cached state for the button
        Animator animator; // Animator for the button
        bool isPointerOn; // Checks if button's pointer is on or not

        public enum RippleUpdateMode
        {
            NORMAL,
            UNSCALED_TIME
        }

        void Start()
        {
            if (buttonVar == null) { buttonVar = gameObject.GetComponent<Button>(); }
            if (enableButtonSounds && useClickSound && soundSource != null) { buttonVar.onClick.AddListener(delegate { soundSource.PlayOneShot(clickSound); }); }

            buttonVar.onClick.AddListener(delegate { clickEvent.Invoke(); });

            if (useRipple && rippleParent != null) { rippleParent.SetActive(false); }
            else if (!useRipple && rippleParent != null) { Destroy(rippleParent); }

            if (gameObject.GetComponent<Animator>() != null)
            {
                animator = gameObject.GetComponent<Animator>();
                cachedStateLength = UI_InternalTools.GetAnimatorClipLength(animator, "Highlighted") + 0.1f;
            }
        }

        void OnEnable()
        {
            if (!useCustomContent) 
            {
                UpdateUI(); 
            }
        }

        public void UpdateUI()
        {
            normalText.text = buttonText;
            highlightedText.text = buttonText;

            if (fixWidth) { StartCoroutine("FixWidthHelper"); }
        }

        public void UpdateUIEditor()
        {
            normalText.text = buttonText;
            highlightedText.text = buttonText;
        }

        public void CreateRipple(Vector2 pos)
        {
            if (rippleParent != null)
            {
                GameObject rippleObj = new GameObject();
                rippleObj.AddComponent<Image>();
                rippleObj.GetComponent<Image>().sprite = rippleShape;
                rippleObj.name = "Ripple";
                rippleParent.SetActive(true);
                rippleObj.transform.SetParent(rippleParent.transform);

                if (renderOnTop) { rippleParent.transform.SetAsLastSibling(); }
                else { rippleParent.transform.SetAsFirstSibling(); }

                if (centered) { rippleObj.transform.localPosition = new Vector2(0f, 0f); }
                else { rippleObj.transform.position = pos; }

                rippleObj.AddComponent<UI_Ripple>();
                UI_Ripple tempRipple = rippleObj.GetComponent<UI_Ripple>();
                tempRipple.speed = speed;
                tempRipple.maxSize = maxSize;
                tempRipple.startColor = startColor;
                tempRipple.transitionColor = transitionColor;

                if (rippleUpdateMode == RippleUpdateMode.NORMAL) { tempRipple.unscaledTime = false; }
                else { tempRipple.unscaledTime = true; }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (useRipple && isPointerOn)
            {
                CreateRipple(Mouse.current.position.ReadValue());
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableButtonSounds && useHoverSound && buttonVar.interactable && soundSource != null)
                soundSource.PlayOneShot(hoverSound);

            hoverEvent.Invoke();
            isPointerOn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerOn = false;
            onLeave.Invoke();
        }

        IEnumerator FixWidthHelper()
        {
            yield return new WaitForSeconds(0.1f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
            StopCoroutine("FixWidthHelper");
        }

        IEnumerator DisableAnimator()
        {
            yield return new WaitForSecondsRealtime(cachedStateLength);
            animator.enabled = false;
        }
    }
}