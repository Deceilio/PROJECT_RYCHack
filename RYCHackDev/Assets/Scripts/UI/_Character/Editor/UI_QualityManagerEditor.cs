using UnityEngine;
using UnityEditor;

namespace Deceilio.Psychain
{
    [CustomEditor(typeof(UI_QualityManager))]
    public class UI_QualityManagerEditor : Editor
    {
        private GUISkin customSkin; // Reference to the GUI Skin data for the custom skin
        private UI_QualityManager qmTarget; // Reference to the quality manager target
        private int currentTab; // Reference to the current tab of quality manager

        private void OnEnable()
        {
            qmTarget = (UI_QualityManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            UI_EditorHandler.DrawComponentHeader(customSkin, "QM Top Header");

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Resources");
            toolbarTabs[1] = new GUIContent("Settings");

            currentTab = UI_EditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var mixer = serializedObject.FindProperty("mixer");
            var masterSlider = serializedObject.FindProperty("masterSlider");
            var musicSlider = serializedObject.FindProperty("musicSlider");
            var sfxSlider = serializedObject.FindProperty("sfxSlider");
            var customDropdown = serializedObject.FindProperty("customDropdown");
            var defaultDropdown = serializedObject.FindProperty("defaultDropdown");
            var preferCustomDropdown = serializedObject.FindProperty("preferCustomDropdown");
            var clickEvent = serializedObject.FindProperty("clickEvent");
            var isMobile = serializedObject.FindProperty("isMobile");

            switch (currentTab)
            {
                case 0:
                    UI_EditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    UI_EditorHandler.DrawProperty(mixer, customSkin, "Audio Mixer");
                    UI_EditorHandler.DrawProperty(masterSlider, customSkin, "Master Slider");
                    UI_EditorHandler.DrawProperty(musicSlider, customSkin, "Music Slider");
                    UI_EditorHandler.DrawProperty(sfxSlider, customSkin, "SFX Slider");
                    UI_EditorHandler.DrawProperty(defaultDropdown, customSkin, "Default Dropdown");
                    GUI.enabled = false;
                    UI_EditorHandler.DrawProperty(customDropdown, customSkin, "Custom Dropdown");
                    GUI.enabled = true;
                    break;
               
                case 1:
                    UI_EditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    GUI.enabled = false;
                    preferCustomDropdown.boolValue = UI_EditorHandler.DrawToggle(preferCustomDropdown.boolValue, customSkin, "Prefer Custom Dropdown");
                    GUI.enabled = true;
                    isMobile.boolValue = UI_EditorHandler.DrawToggle(isMobile.boolValue, customSkin, "Is Mobile Scene");

                    UI_EditorHandler.DrawHeader(customSkin, "Events Header", 10);
                    EditorGUILayout.PropertyField(clickEvent, new GUIContent("Dynamic Res Event"), true);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}