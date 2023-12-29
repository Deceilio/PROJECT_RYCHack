using UnityEngine;
using UnityEditor;

namespace Deceilio.Psychain
{
    [CustomEditor(typeof(UI_ElementSound))]
    public class UI_ElementSoundEditor: Editor
    {
        private GUISkin customSkin; // Reference to the GUI Skin data for the custom skin
        private UI_ElementSound uisTarget; // Reference to the ui target
        private int currentTab; // Reference to the current tab of ui

        private void OnEnable()
        {
            uisTarget = (UI_ElementSound)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            UI_EditorHandler.DrawComponentHeader(customSkin, "UIS Top Header");

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Settings");
         
            currentTab = UI_EditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var audioSource = serializedObject.FindProperty("audioSource");
            var hoverSound = serializedObject.FindProperty("hoverSound");
            var clickSound = serializedObject.FindProperty("clickSound");
            var enableHoverSound = serializedObject.FindProperty("enableHoverSound");
            var enableClickSound = serializedObject.FindProperty("enableClickSound");

            switch (currentTab)
            {
                case 0:
                    UI_EditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    UI_EditorHandler.DrawProperty(audioSource, customSkin, "Audio Source");
                    if (enableHoverSound.boolValue == true) { UI_EditorHandler.DrawProperty(hoverSound, customSkin, "Hover Sound"); }
                    if (enableClickSound.boolValue == true) { UI_EditorHandler.DrawProperty(clickSound, customSkin, "Click Sound"); }
                    break;

                case 1:
                    UI_EditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    enableHoverSound.boolValue = UI_EditorHandler.DrawToggle(enableHoverSound.boolValue, customSkin, "Enable Hover Sound");
                    enableClickSound.boolValue = UI_EditorHandler.DrawToggle(enableClickSound.boolValue, customSkin, "Enable Click Sound");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}