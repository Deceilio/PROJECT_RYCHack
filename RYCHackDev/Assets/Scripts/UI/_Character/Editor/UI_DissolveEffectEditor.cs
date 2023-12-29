using UnityEngine;
using UnityEditor;

namespace Deceilio.Psychain
{
    [CustomEditor(typeof(UI_DissolveEffect))]
    public class UI_DissolveEffectEditor : Editor
    {
        private GUISkin customSkin; // Reference to the GUI Skin data for the custom skin
        private UI_DissolveEffect uideTarget; // Reference to the dissolve effect target
        private int currentTab; // Reference to the current tab of dissolve effect

        private void OnEnable()
        {
            uideTarget = (UI_DissolveEffect)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            UI_EditorHandler.DrawComponentHeader(customSkin, "DE Top Header");

            GUIContent[] toolbarTabs = new GUIContent[1];
            toolbarTabs[0] = new GUIContent("Settings");

            currentTab = UI_EditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 0;

            GUILayout.EndHorizontal();

            var location = serializedObject.FindProperty("m_Location");
            var width = serializedObject.FindProperty("m_Width");
            var softness = serializedObject.FindProperty("m_Softness");
            var color = serializedObject.FindProperty("m_Color");
            var effectMaterial = serializedObject.FindProperty("m_EffectMaterial");
            var animationSpeed = serializedObject.FindProperty("animationSpeed");
            var mainPanelMode = serializedObject.FindProperty("mainPanelMode");

            switch (currentTab)
            {
                case 0:
                    UI_EditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    mainPanelMode.boolValue = UI_EditorHandler.DrawToggle(mainPanelMode.boolValue, customSkin, "Enable Main Panel Mode");
                    GUILayout.Space(2);
                    UI_EditorHandler.DrawPropertyPlain(location, customSkin, "Location");
                    GUILayout.Space(2);
                    UI_EditorHandler.DrawPropertyPlain(width, customSkin, "Width");
                    GUILayout.Space(2);
                    UI_EditorHandler.DrawPropertyPlain(softness, customSkin, "Softness");

                    if (mainPanelMode.boolValue == false)
                    {
                        GUILayout.Space(2);
                        UI_EditorHandler.DrawPropertyPlain(animationSpeed, customSkin, "Anim Speed");
                    }

                    GUILayout.Space(2);
                    UI_EditorHandler.DrawPropertyPlain(color, customSkin, "Effect Color");
                    GUILayout.Space(2);
                    UI_EditorHandler.DrawPropertyPlain(effectMaterial, customSkin, "Effect Material");
                    GUILayout.EndVertical();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}