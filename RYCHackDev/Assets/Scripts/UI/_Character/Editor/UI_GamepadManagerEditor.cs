using UnityEngine;
using UnityEditor;
using Deceilio.Psychain;

namespace Michsky.UI.Dark
{
    [CustomEditor(typeof(UI_GamepadManager))]
    public class UI_GamepadManagerEditor: Editor
    { 
        private GUISkin customSkin; // Reference to the GUI Skin data for the custom skin
        private UI_GamepadManager gcTarget; // Reference to the gamepad target
        private int currentTab; // Reference to the current tab of gamepad

        private void OnEnable()
        {
            gcTarget = (UI_GamepadManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            UI_EditorHandler.DrawComponentHeader(customSkin, "GM Top Header");

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Settings");

            currentTab = UI_EditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var defaultPanelManager = serializedObject.FindProperty("defaultPanelManager");
            var panelManagers = serializedObject.FindProperty("panelManagers");
            var alwaysUpdate = serializedObject.FindProperty("alwaysUpdate");
            var affectCursor = serializedObject.FindProperty("affectCursor");
            var gamepadHotkey = serializedObject.FindProperty("gamepadHotkey");
            var keyboardObjects = serializedObject.FindProperty("keyboardObjects");
            var gamepadObjects = serializedObject.FindProperty("gamepadObjects");
            var buttons = serializedObject.FindProperty("buttons");

            switch (currentTab)
            {
                case 0:
                    UI_EditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    UI_EditorHandler.DrawPropertyCW(defaultPanelManager, customSkin, "Default Panel Manager", 140);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.Space(12);
                    EditorGUILayout.PropertyField(panelManagers, new GUIContent("Panel Managers"), true);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Core Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.Space(12);
                    EditorGUILayout.PropertyField(keyboardObjects, new GUIContent("Keyboard Objects"), true);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.Space(12);
                    EditorGUILayout.PropertyField(gamepadObjects, new GUIContent("Gamepad Objects"), true);      
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.Space(12);
                    EditorGUILayout.PropertyField(buttons, new GUIContent("Button Objects"), true);
                    GUILayout.EndHorizontal();
                    break;

                case 1:
                    UI_EditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    alwaysUpdate.boolValue = UI_EditorHandler.DrawToggle(alwaysUpdate.boolValue, customSkin, "Always Update");
                    affectCursor.boolValue = UI_EditorHandler.DrawToggle(affectCursor.boolValue, customSkin, "Affect Cursor");
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(gamepadHotkey, new GUIContent(""));
                    GUILayout.EndHorizontal();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}