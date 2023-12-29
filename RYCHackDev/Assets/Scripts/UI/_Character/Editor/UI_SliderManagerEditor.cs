using UnityEngine;
using UnityEditor;

namespace Deceilio.Psychain
{
    [CustomEditor(typeof(UI_SliderManager))]
    public class UI_SliderManagerEditor : Editor
    {
        private GUISkin customSkin; // Reference to the GUI Skin data for the custom skin
        private UI_SliderManager slmTarget; // Reference to the slider manager target
        private int currentTab; // Reference to the current tab of slider manager

        private void OnEnable()
        {
            slmTarget = (UI_SliderManager)target;
           
            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            UI_EditorHandler.DrawComponentHeader(customSkin, "Slider Top Header");

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Resources");
            toolbarTabs[1] = new GUIContent("Settings");

            currentTab = UI_EditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var valueText = serializedObject.FindProperty("valueText");
            var popupValueText = serializedObject.FindProperty("popupValueText");
            var enableSaving = serializedObject.FindProperty("enableSaving");
            var sliderTag = serializedObject.FindProperty("sliderTag");
            var usePercent = serializedObject.FindProperty("usePercent");
            var showValue = serializedObject.FindProperty("showValue");
            var showPopupValue = serializedObject.FindProperty("showPopupValue");
            var useRoundValue = serializedObject.FindProperty("useRoundValue");
            var valueMultiplier = serializedObject.FindProperty("valueMultiplier");

            switch (currentTab)
            {
                case 0:
                    UI_EditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    UI_EditorHandler.DrawProperty(valueText, customSkin, "Value Text");
                    UI_EditorHandler.DrawProperty(popupValueText, customSkin, "Popup Value Text");
                    break;

                case 1:
                    UI_EditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    UI_EditorHandler.DrawProperty(valueMultiplier, customSkin, "Value Multiplier");
                    usePercent.boolValue = UI_EditorHandler.DrawToggle(usePercent.boolValue, customSkin, "Use Percent");
                    showValue.boolValue = UI_EditorHandler.DrawToggle(showValue.boolValue, customSkin, "Show Value");
                    showPopupValue.boolValue = UI_EditorHandler.DrawToggle(showPopupValue.boolValue, customSkin, "Show Popup Value");
                    useRoundValue.boolValue = UI_EditorHandler.DrawToggle(useRoundValue.boolValue, customSkin, "Use Round Value");
                    enableSaving.boolValue = UI_EditorHandler.DrawToggle(enableSaving.boolValue, customSkin, "Save Value");

                    if (enableSaving.boolValue == true)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(35);

                        EditorGUILayout.LabelField(new GUIContent("Slider Tag"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                        EditorGUILayout.PropertyField(sliderTag, new GUIContent(""), true);

                        GUILayout.EndHorizontal();
                        EditorGUILayout.HelpBox("Each slider should has its own unique tag.", MessageType.Info);
                    }

                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}