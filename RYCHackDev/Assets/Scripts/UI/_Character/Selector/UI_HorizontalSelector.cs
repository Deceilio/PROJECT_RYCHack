using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Deceilio.Psychain
{
    [RequireComponent(typeof(Animator))]
    public class UI_HorizontalSelector : MonoBehaviour
    {
        // Resources
        public TextMeshProUGUI label; // Label for the horizontal selector
        public TextMeshProUGUI labelHelper; // Label helper for the horizontal selector
        public Image labelIcon; // Label icon for horizontal selector
        public Image labelIconHelper; // Label icon helper for horizontal selector
        public Transform indicatorParent; // Transform component of the indicator parent
        public GameObject indicatorObject; // Indicator game object for the selector
        public Animator selectorAnimator; // Reference to the selector animator
        public HorizontalLayoutGroup contentLayout; // Reference to the Horizontal Layout Group for content layout
        public HorizontalLayoutGroup contentLayoutHelper; // Reference to the Horizontal Layout Group for content layout helper
        string newItemTitle; // Text of the new title item

        // Saving
        public bool saveValue; // Checks the value for the save
        public string selectorTag = "Tag Text"; // Name of the selector tag

        // Settings
        public bool enableIcon = true; // Check to enable or disable icon
        public bool enableIndicators = true;  // Check to enable or disable indicators
        public bool invokeAtStart;  // Check if selector can invoke at start or not
        public bool invertAnimation; // Check if selector have invert animation or not 
        public bool loopSelection; // Check if selector should loop selection or not
        [Range(0.25f, 2.5f)] public float iconScale = 1; // Scale of the icon
        [Range(1, 50)] public int contentSpacing = 15;  // Spacing of the content
        public int defaultIndex = 0; // Default index value
        [HideInInspector] public int index = 0; // Temp index value

        // Items
        public List<Item> itemList = new List<Item>(); // List of items in the selector
        [System.Serializable]
        public class SelectorEvent : UnityEvent<int> { }
        public SelectorEvent onValueChanged;

        [System.Serializable]
        public class Item
        {
            public string itemTitle = "Item Title";
            public Sprite itemIcon;
            public UnityEvent onItemSelect = new UnityEvent();
        }

        void Start()
        {
            if (selectorAnimator == null)
                selectorAnimator = gameObject.GetComponent<Animator>();

            if (label == null || labelHelper == null)
            {
                Debug.LogError("<b>[Horizontal Selector]</b> Cannot initalize the object due to missing resources.", this);
                return;
            }

            SetupSelector();
            UpdateContentLayout();

            if (invokeAtStart == true)
            {
                itemList[index].onItemSelect.Invoke();
                onValueChanged.Invoke(index);
            }
        }

        public void SetupSelector()
        {
            if (itemList.Count != 0)
            {
                if (saveValue == true)
                {
                    if (PlayerPrefs.HasKey("HorizontalSelector" + selectorTag) == true)
                        defaultIndex = PlayerPrefs.GetInt("HorizontalSelector" + selectorTag);
                    else
                        PlayerPrefs.SetInt("HorizontalSelector" + selectorTag, defaultIndex);
                }

                label.text = itemList[defaultIndex].itemTitle;
                labelHelper.text = label.text;

                if (labelIcon != null && enableIcon == true)
                {
                    labelIcon.sprite = itemList[defaultIndex].itemIcon;
                    labelIconHelper.sprite = labelIcon.sprite;
                }

                else if (enableIcon == false)
                {
                    if (labelIcon != null)
                        labelIcon.gameObject.SetActive(false);
                    if (labelIconHelper != null)
                        labelIconHelper.gameObject.SetActive(false);
                }

                index = defaultIndex;

                if (enableIndicators == true)
                    UpdateIndicators();
                else
                    Destroy(indicatorParent.gameObject);
            }
        }

        public void PreviousClick()
        {
            if (loopSelection == false)
            {
                if (index != 0)
                {
                    labelHelper.text = label.text;

                    if (labelIcon != null && enableIcon == true)
                        labelIconHelper.sprite = labelIcon.sprite;

                    if (index == 0)
                        index = itemList.Count - 1;
                    else
                        index--;

                    label.text = itemList[index].itemTitle;

                    if (labelIcon != null && enableIcon == true)
                        labelIcon.sprite = itemList[index].itemIcon;

                    itemList[index].onItemSelect.Invoke();
                    onValueChanged.Invoke(index);
                    selectorAnimator.Play(null);
                    selectorAnimator.StopPlayback();

                    if (invertAnimation == true)
                        selectorAnimator.Play("Forward");
                    else
                        selectorAnimator.Play("Previous");

                    if (saveValue == true)
                        PlayerPrefs.SetInt("HorizontalSelector" + selectorTag, index);
                }
            }

            else
            {
                labelHelper.text = label.text;

                if (labelIcon != null && enableIcon == true)
                    labelIconHelper.sprite = labelIcon.sprite;

                if (index == 0)
                    index = itemList.Count - 1;
                else
                    index--;

                label.text = itemList[index].itemTitle;

                if (labelIcon != null && enableIcon == true)
                    labelIcon.sprite = itemList[index].itemIcon;

                itemList[index].onItemSelect.Invoke();
                onValueChanged.Invoke(index);
                selectorAnimator.Play(null);
                selectorAnimator.StopPlayback();

                if (invertAnimation == true)
                    selectorAnimator.Play("Forward");
                else
                    selectorAnimator.Play("Previous");

                if (saveValue == true)
                    PlayerPrefs.SetInt("HorizontalSelector" + selectorTag, index);
            }

            if (saveValue == true)
                PlayerPrefs.SetInt("HorizontalSelector" + selectorTag, index);

            if (enableIndicators == true)
            {
                for (int i = 0; i < itemList.Count; ++i)
                {
                    GameObject go = indicatorParent.GetChild(i).gameObject;
                    Transform onObj = go.transform.Find("On");
                    Transform offObj = go.transform.Find("Off");

                    if (i == index)
                    {
                        onObj.gameObject.SetActive(true);
                        offObj.gameObject.SetActive(false);
                    }

                    else
                    {
                        onObj.gameObject.SetActive(false);
                        offObj.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void ForwardClick()
        {
            if (loopSelection == false)
            {
                if (index != itemList.Count - 1)
                {
                    labelHelper.text = label.text;

                    if (labelIcon != null && enableIcon == true)
                        labelIconHelper.sprite = labelIcon.sprite;

                    if ((index + 1) >= itemList.Count)
                        index = 0;
                    else
                        index++;

                    label.text = itemList[index].itemTitle;

                    if (labelIcon != null && enableIcon == true)
                        labelIcon.sprite = itemList[index].itemIcon;

                    itemList[index].onItemSelect.Invoke();
                    onValueChanged.Invoke(index);
                    selectorAnimator.Play(null);
                    selectorAnimator.StopPlayback();

                    if (invertAnimation == true)
                        selectorAnimator.Play("Previous");
                    else
                        selectorAnimator.Play("Forward");

                    if (saveValue == true)
                        PlayerPrefs.SetInt("HorizontalSelector" + selectorTag, index);
                }
            }

            else
            {
                labelHelper.text = label.text;

                if (labelIcon != null && enableIcon == true)
                    labelIconHelper.sprite = labelIcon.sprite;

                if ((index + 1) >= itemList.Count)
                    index = 0;
                else
                    index++;

                label.text = itemList[index].itemTitle;

                if (labelIcon != null && enableIcon == true)
                    labelIcon.sprite = itemList[index].itemIcon;

                itemList[index].onItemSelect.Invoke();
                onValueChanged.Invoke(index);
                selectorAnimator.Play(null);
                selectorAnimator.StopPlayback();

                if (invertAnimation == true)
                    selectorAnimator.Play("Previous");
                else
                    selectorAnimator.Play("Forward");

                if (saveValue == true)
                    PlayerPrefs.SetInt("HorizontalSelector" + selectorTag, index);
            }

            if (saveValue == true)
                PlayerPrefs.SetInt("HorizontalSelector" + selectorTag, index);

            if (enableIndicators == true)
            {
                for (int i = 0; i < itemList.Count; ++i)
                {
                    GameObject go = indicatorParent.GetChild(i).gameObject;
                    Transform onObj = go.transform.Find("On"); ;
                    Transform offObj = go.transform.Find("Off");

                    if (i == index)
                    {
                        onObj.gameObject.SetActive(true);
                        offObj.gameObject.SetActive(false);
                    }

                    else
                    {
                        onObj.gameObject.SetActive(false);
                        offObj.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void CreateNewItem(string title)
        {
            Item item = new Item();
            newItemTitle = title;
            item.itemTitle = newItemTitle;
            itemList.Add(item);
        }

        public void RemoveItem(string itemTitle)
        {
            var item = itemList.Find(x => x.itemTitle == itemTitle);
            itemList.Remove(item);
            SetupSelector();
        }

        public void AddNewItem()
        {
            Item item = new Item();
            itemList.Add(item);
        }

        public void UpdateUI()
        {
            label.text = itemList[index].itemTitle;

            if (labelIcon != null && enableIcon == true)
                labelIcon.sprite = itemList[index].itemIcon;

            UpdateContentLayout();
            UpdateIndicators();
        }

        public void UpdateIndicators()
        {
            if (enableIndicators == false)
                return;

            foreach (Transform child in indicatorParent)
                Destroy(child.gameObject);

            for (int i = 0; i < itemList.Count; ++i)
            {
                GameObject go = Instantiate(indicatorObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(indicatorParent, false);
                go.name = itemList[i].itemTitle;
                Transform onObj = go.transform.Find("On");
                Transform offObj = go.transform.Find("Off");

                if (i == index)
                {
                    onObj.gameObject.SetActive(true);
                    offObj.gameObject.SetActive(false);
                }

                else
                {
                    onObj.gameObject.SetActive(false);
                    offObj.gameObject.SetActive(true);
                }
            }
        }

        public void UpdateContentLayout()
        {
            if (contentLayout != null)
                contentLayout.spacing = contentSpacing;

            if (contentLayoutHelper != null)
                contentLayoutHelper.spacing = contentSpacing;

            if (labelIcon != null)
            {
                labelIcon.transform.localScale = new Vector3(iconScale, iconScale, iconScale);
                labelIconHelper.transform.localScale = new Vector3(iconScale, iconScale, iconScale);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(label.transform.parent.GetComponent<RectTransform>());
        }
    }
}