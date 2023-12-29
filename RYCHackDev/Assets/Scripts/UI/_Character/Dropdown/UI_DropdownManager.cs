using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Deceilio.Psychain
{
    [RequireComponent(typeof(Animator))]
    public class UI_DropdownManager: MonoBehaviour, IPointerExitHandler
    {
        // Resources
        public Animator dropdownAnimator; // Animator for the drop down
        public GameObject triggerObject; // Trigger object for the drop down 
        public TextMeshProUGUI selectedText; // Selected text of the drop down
        public Image selectedImage; // Selected image of the drop down
        public Transform itemParent; // Parent object for the drop down's item
        public GameObject itemObject; // Item object of the drop down
        public GameObject scrollbar; // Scroll bar for the drop down
        public VerticalLayoutGroup itemList; // Verticle Layout group to have list of all the item
        [HideInInspector] public Transform currentListParent; // Transform for the Current List Parent
        public Transform listParent; // Transform for the List Parent 

        // Settings
        public bool enableIcon = true; // Enable or disable dropdown icon
        public bool enableTrigger = true; // Enable or disable dropdown trigger
        public bool enableScrollbar = true; // Enable or disable dropdown scrollbar
        public bool setHighPriorty = true;  // Enable or disable high priority
        public bool outOnPointerExit = false; // Enable or disable outing on pointer exit
        public bool isListItem = false; // Check if the drop down is a list item or not
        public bool invokeAtStart = false; // Check to invoke at start or not
        public AnimationType animationType; // Enum for the drop down animation type
        public int selectedItemIndex = 0; // Index for the selected item

        // Saving
        public bool saveSelected = false; // Save the selected or not
        public string dropdownTag = "Dropdown"; // Tag for the drop down

        // Item list
        [SerializeField]
        public List<Item> dropdownItems = new List<Item>(); // All the drop down items
        [System.Serializable]
        public class DropdownEvent : UnityEvent<int> { }
        public DropdownEvent dropdownEvent;

        // Other variables
        [HideInInspector] public bool isOn; // Checks if the drop down is on or not
        [HideInInspector] public int index = 0; // Index of drop down
        [HideInInspector] public int siblingIndex = 0; // Sibling index of drop down
        [HideInInspector] public TextMeshProUGUI setItemText; // Item text reference
        [HideInInspector] public Image setItemImage; // Item image reference
        Button triggerButton; // Trigger button for drop down
        EventTrigger triggerEvent; // Trigger event for drop down
        Sprite imageHelper; // Sprite for the image helper
        string textHelper; // Name of the text helper

        public enum AnimationType
        {
            FADING,
            SLIDING,
            STYLISH
        }

        [System.Serializable]
        public class Item
        {
            public string itemName = "Dropdown Item";
            public Sprite itemIcon;
            public UnityEvent OnItemSelection = new UnityEvent();
        }

        void Start()
        {
            try
            {
                if (dropdownAnimator == null)
                    dropdownAnimator = gameObject.GetComponent<Animator>();

                itemList = itemParent.GetComponent<VerticalLayoutGroup>();

                if (dropdownItems.Count != 0)
                    SetupDropdown();

                currentListParent = transform.parent;

                if (enableTrigger == true && triggerObject != null)
                {
                    triggerButton = gameObject.GetComponent<Button>();
                    triggerEvent = triggerObject.AddComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((eventData) => { Animate(); });
                    triggerEvent.GetComponent<EventTrigger>().triggers.Add(entry);
                }
            }

            catch
            {
                Debug.LogError("[Dropdown Manager] Cannot initalize the object due to missing resources.", this);
            }

            if (enableScrollbar == true)
                itemList.padding.right = 25;
            else
                itemList.padding.right = 8;

            if (setHighPriorty == true)
                transform.SetAsLastSibling();

            if (saveSelected == true)
            {
                if (invokeAtStart == true)
                    dropdownItems[PlayerPrefs.GetInt(dropdownTag + "DarkUIDropdown")].OnItemSelection.Invoke();
                else
                    ChangeDropdownInfo(PlayerPrefs.GetInt(dropdownTag + "DarkUIDropdown"));
            }
        }

        public void SetupDropdown()
        {
            foreach (Transform child in itemParent)
                GameObject.Destroy(child.gameObject);

            index = 0;

            for (int i = 0; i < dropdownItems.Count; ++i)
            {
                GameObject go = Instantiate(itemObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(itemParent, false);

                setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                textHelper = dropdownItems[i].itemName;
                setItemText.text = textHelper;

                Transform goImage;
                goImage = go.gameObject.transform.Find("Icon");
                setItemImage = goImage.GetComponent<Image>();
                imageHelper = dropdownItems[i].itemIcon;
                setItemImage.sprite = imageHelper;

                Button itemButton;
                itemButton = go.GetComponent<Button>();

                itemButton.onClick.AddListener(Animate);
                itemButton.onClick.AddListener(delegate
                {
                    ChangeDropdownInfo(index = go.transform.GetSiblingIndex());
                    dropdownEvent.Invoke(index = go.transform.GetSiblingIndex());

                    if (saveSelected == true)
                        PlayerPrefs.SetInt(dropdownTag + "DarkUIDropdown", go.transform.GetSiblingIndex());
                });

                if (dropdownItems[i].OnItemSelection != null)
                    itemButton.onClick.AddListener(dropdownItems[i].OnItemSelection.Invoke);

                if (invokeAtStart == true)
                    dropdownItems[i].OnItemSelection.Invoke();
            }

            try
            {
                selectedText.text = dropdownItems[selectedItemIndex].itemName;
                selectedImage.sprite = dropdownItems[selectedItemIndex].itemIcon;
                currentListParent = transform.parent;
            }

            catch
            {
                selectedText.text = dropdownTag;
                currentListParent = transform.parent;
                Debug.Log("[Dropdown Manager] There is no dropdown item in the list.", this);
            }
        }

        public void ChangeDropdownInfo(int itemIndex)
        {
            if (selectedImage != null)
                selectedImage.sprite = dropdownItems[itemIndex].itemIcon;

            if (selectedText != null)
                selectedText.text = dropdownItems[itemIndex].itemName;

            selectedItemIndex = itemIndex;
        }

        public void Animate()
        {
            if (isOn == false && animationType == AnimationType.FADING)
            {
                dropdownAnimator.Play("Fading In");
                isOn = true;

                if (isListItem == true)
                {
                    siblingIndex = transform.GetSiblingIndex();
                    gameObject.transform.SetParent(listParent, true);
                }
            }

            else if (isOn == true && animationType == AnimationType.FADING)
            {
                dropdownAnimator.Play("Fading Out");
                isOn = false;

                if (isListItem == true)
                {
                    gameObject.transform.SetParent(currentListParent, true);
                    gameObject.transform.SetSiblingIndex(siblingIndex);
                }
            }

            else if (isOn == false && animationType == AnimationType.SLIDING)
            {
                dropdownAnimator.Play("Sliding In");
                isOn = true;

                if (isListItem == true)
                {
                    siblingIndex = transform.GetSiblingIndex();
                    gameObject.transform.SetParent(listParent, true);
                }
            }

            else if (isOn == true && animationType == AnimationType.SLIDING)
            {
                dropdownAnimator.Play("Sliding Out");
                isOn = false;

                if (isListItem == true)
                {
                    gameObject.transform.SetParent(currentListParent, true);
                    gameObject.transform.SetSiblingIndex(siblingIndex);
                }
            }

            else if (isOn == false && animationType == AnimationType.STYLISH)
            {
                dropdownAnimator.Play("Stylish In");
                isOn = true;

                if (isListItem == true)
                {
                    siblingIndex = transform.GetSiblingIndex();
                    gameObject.transform.SetParent(listParent, true);
                }
            }

            else if (isOn == true && animationType == AnimationType.STYLISH)
            {
                dropdownAnimator.Play("Stylish Out");
                isOn = false;

                if (isListItem == true)
                {
                    gameObject.transform.SetParent(currentListParent, true);
                    gameObject.transform.SetSiblingIndex(siblingIndex);
                }
            }

            if (setHighPriorty == true)
                transform.SetAsLastSibling();

            if (enableTrigger == true && isOn == false)
            {
                triggerObject.SetActive(false);
                triggerButton.interactable = true;
            }

            else if (enableTrigger == true && isOn == true)
            {
                triggerObject.SetActive(true);
                triggerButton.interactable = false;
            }

            if (enableTrigger == true && outOnPointerExit == true)
            {
                triggerObject.SetActive(false);
                triggerButton.interactable = true;
            }
        }

        public void GetSelectedDropdownName(TextMeshProUGUI tmpText)
        {
            if (tmpText != null)
                tmpText.text = dropdownItems[selectedItemIndex].itemName;
            else
                Debug.Log("Dropdown - Selected item name: " + dropdownItems[selectedItemIndex].itemName);
        }

        public void UpdateValues()
        {
            if (enableScrollbar == true)
            {
                itemList.padding.right = 25;
                scrollbar.SetActive(true);
            }

            else
            {
                itemList.padding.right = 8;
                scrollbar.SetActive(false);
            }

            if (enableIcon == false)
                selectedImage.gameObject.SetActive(false);
            else
                selectedImage.gameObject.SetActive(true);
        }

        public void CreateNewItem(string title, Sprite icon)
        {
            Item item = new Item();
            item.itemName = title;
            item.itemIcon = icon;
            dropdownItems.Add(item);
            SetupDropdown();
        }

        public void CreateNewItemFast(string title, Sprite icon)
        {
            Item item = new Item();
            item.itemName = title;
            item.itemIcon = icon;
            dropdownItems.Add(item);
        }

        public void AddNewItem()
        {
            Item item = new Item();
            dropdownItems.Add(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (outOnPointerExit == true && isOn == true)
            {
                Animate();
                isOn = false;

                if (isListItem == true)
                    gameObject.transform.SetParent(currentListParent, true);
            }
        }
    }
}