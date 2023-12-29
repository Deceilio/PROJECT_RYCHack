using UnityEngine;
using UnityEditor;

namespace Deceilio.Psychain
{
    [CustomEditor(typeof(UI_DropdownManager))]
    public class UI_DropdownManagerEditor : Editor
    {
        private GUISkin customSkin; // Reference to the GUI Skin data for the custom skin
        private UI_DropdownManager dTarget; // Reference to the dropdown target
        private int currentTab; // Reference to the current tab of dropdown 

        private void OnEnable()
        {
            dTarget = (UI_DropdownManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\pSYCHA Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\DUI Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            UI_EditorHandler.DrawComponentHeader(customSkin, "Dropdown Top Header");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            currentTab = UI_EditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            var triggerObject = serializedObject.FindProperty("triggerObject");
            var selectedText = serializedObject.FindProperty("selectedText");
            var selectedImage = serializedObject.FindProperty("selectedImage");
            var itemParent = serializedObject.FindProperty("itemParent");
            var itemObject = serializedObject.FindProperty("itemObject");
            var scrollbar = serializedObject.FindProperty("scrollbar");
            var itemList = serializedObject.FindProperty("itemList");
            var listParent = serializedObject.FindProperty("listParent");
            var dropdownItems = serializedObject.FindProperty("dropdownItems");
            var dropdownEvent = serializedObject.FindProperty("dropdownEvent");
            var enableIcon = serializedObject.FindProperty("enableIcon");
            var enableTrigger = serializedObject.FindProperty("enableTrigger");
            var enableScrollbar = serializedObject.FindProperty("enableScrollbar");
            var setHighPriorty = serializedObject.FindProperty("setHighPriorty");
            var outOnPointerExit = serializedObject.FindProperty("outOnPointerExit");
            var isListItem = serializedObject.FindProperty("isListItem");
            var invokeAtStart = serializedObject.FindProperty("invokeAtStart");
            var animationType = serializedObject.FindProperty("animationType");
            var selectedItemIndex = serializedObject.FindProperty("selectedItemIndex");
            var saveSelected = serializedObject.FindProperty("saveSelected");
            var dropdownTag = serializedObject.FindProperty("dropdownTag");

            switch (currentTab)
            {
                case 0:
                    UI_EditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(dropdownItems, new GUIContent("Dropdown Items"), true);
                    dropdownItems.isExpanded = true;

                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("+  Add a new item", customSkin.button))
                        dTarget.AddNewItem();

                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Events Header"));
                    EditorGUILayout.PropertyField(dropdownEvent, new GUIContent("Dropdown Event"), true);

                    break;

                case 1:
                    UI_EditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    UI_EditorHandler.DrawProperty(triggerObject, customSkin, "Trigger Object");
                    UI_EditorHandler.DrawProperty(selectedText, customSkin, "Selected Text");
                    UI_EditorHandler.DrawProperty(selectedImage, customSkin, "Selected Image");
                    UI_EditorHandler.DrawProperty(itemObject, customSkin, "Item Prefab");
                    UI_EditorHandler.DrawProperty(itemParent, customSkin, "Item Parent");
                    UI_EditorHandler.DrawProperty(scrollbar, customSkin, "Scrollbar");
                    UI_EditorHandler.DrawProperty(listParent, customSkin, "List Parent");
                    break;

                case 2:
                    UI_EditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    enableIcon.boolValue = UI_EditorHandler.DrawToggle(enableIcon.boolValue, customSkin, "Enable Icon");

                    if (dTarget.selectedImage != null)
                    {
                        if (enableIcon.boolValue == true)
                            dTarget.selectedImage.enabled = true;
                        else
                            dTarget.selectedImage.enabled = false;
                    }

                    else
                    {
                        if (enableIcon.boolValue == true)
                        {
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.HelpBox("'Selected Image' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                            GUILayout.EndHorizontal();
                        }
                    }

                    enableTrigger.boolValue = UI_EditorHandler.DrawToggle(enableTrigger.boolValue, customSkin, "Enable Trigger");

                    if (enableTrigger.boolValue == true && dTarget.triggerObject == null)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("'Trigger Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                        GUILayout.EndHorizontal();
                    }

                    enableScrollbar.boolValue = UI_EditorHandler.DrawToggle(enableScrollbar.boolValue, customSkin, "Enable Scrollbar");

                    if (dTarget.scrollbar != null)
                    {
                        if (enableScrollbar.boolValue == true)
                            dTarget.scrollbar.SetActive(true);
                        else
                            dTarget.scrollbar.SetActive(false);
                    }

                    else
                    {
                        if (enableScrollbar.boolValue == true)
                        {
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.HelpBox("'Scrollbar' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                            GUILayout.EndHorizontal();
                        }
                    }

                    setHighPriorty.boolValue = UI_EditorHandler.DrawToggle(setHighPriorty.boolValue, customSkin, "Set High Priorty");
                    outOnPointerExit.boolValue = UI_EditorHandler.DrawToggle(outOnPointerExit.boolValue, customSkin, "Out On Pointer Exit");
                    isListItem.boolValue = UI_EditorHandler.DrawToggle(isListItem.boolValue, customSkin, "Is List Item");

                    if (isListItem.boolValue == true && dTarget.listParent == null)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("'List Parent' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                        GUILayout.EndHorizontal();
                    }

                    invokeAtStart.boolValue = UI_EditorHandler.DrawToggle(invokeAtStart.boolValue, customSkin, "Invoke At Start");
                    saveSelected.boolValue = UI_EditorHandler.DrawToggle(saveSelected.boolValue, customSkin, "Save Selection");

                    if (saveSelected.boolValue == true)
                    {
                        EditorGUI.indentLevel = 2;
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Tag:"), customSkin.FindStyle("Text"), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(dropdownTag, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        EditorGUI.indentLevel = 0;
                        EditorGUILayout.HelpBox("Each dropdown should has its own unique tag.", MessageType.Info);
                    }

                    UI_EditorHandler.DrawProperty(animationType, customSkin, "Animation Type");

                    if (dTarget.dropdownItems.Count != 0)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Selected Item Index:"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        selectedItemIndex.intValue = EditorGUILayout.IntSlider(selectedItemIndex.intValue, 0, dTarget.dropdownItems.Count - 1);

                        GUILayout.Space(2);
                        EditorGUILayout.LabelField(new GUIContent(dTarget.dropdownItems[selectedItemIndex.intValue].itemName), customSkin.FindStyle("Text"));
                        GUILayout.EndVertical();

                        if (saveSelected.boolValue == true)
                            EditorGUILayout.HelpBox("Save Selection is enabled. This option won't be used if there's a stored value.", MessageType.Info);
                    }

                    else { EditorGUILayout.HelpBox("There is no item in the list.", MessageType.Warning); }
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}