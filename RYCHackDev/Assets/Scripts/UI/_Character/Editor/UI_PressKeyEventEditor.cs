using UnityEngine;
using UnityEditor;

namespace Deceilio.Psychain
{
    [CustomEditor(typeof(UI_PressKeyEvent))]
    public class UI_PressKeyEventEditor: Editor
    {
        private GUISkin customSkin; // Reference to the GUI Skin data for the custom skin
        private UI_PressKeyEvent pkeTarget; // Reference to the press key event target
        private int currentTab; // Reference to the current tab of press key event

        private void OnEnable()
        {
            pkeTarget = (UI_PressKeyEvent)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            UI_EditorHandler.DrawComponentHeader(customSkin, "PKE Top Header");

            GUIContent[] toolbarTabs = new GUIContent[1];
            toolbarTabs[0] = new GUIContent("Settings");

            currentTab = UI_EditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 0;

            GUILayout.EndHorizontal();

            var hotkey = serializedObject.FindProperty("hotkey");
            var onPressEvent = serializedObject.FindProperty("onPressEvent");

            switch (currentTab)
            {
                case 0:
                    UI_EditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(hotkey, new GUIContent(""), true);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    EditorGUILayout.PropertyField(onPressEvent, new GUIContent("Press Key Events"), true);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}