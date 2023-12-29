using UnityEngine;
using UnityEditor;

namespace Deceilio.Psychain
{
    [CustomEditor(typeof(UI_SplashScreenManager))]
    public class UI_SplashScreenManagerEditor : Editor
    {
        private GUISkin customSkin; // Reference to the GUI Skin data for the custom skin
        private UI_SplashScreenManager ssmTarget; // Reference to the splash screen target
        private int currentTab; // Reference to the current tab of splash screen

        private void OnEnable()
        {
            ssmTarget = (UI_SplashScreenManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Psychain Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            UI_EditorHandler.DrawComponentHeader(customSkin, "SSM Top Header");

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

            var splashScreenTitles = serializedObject.FindProperty("splashScreenTitles");
            var splashScreen = serializedObject.FindProperty("splashScreen");
            var mainPanelParent = serializedObject.FindProperty("mainPanelParent");
            var modalWindowParent = serializedObject.FindProperty("modalWindowParent");
            var transitionHelper = serializedObject.FindProperty("transitionHelper");
            var disableSplashScreen = serializedObject.FindProperty("disableSplashScreen");
            var startDelay = serializedObject.FindProperty("startDelay");
            var onSplashScreenEnd = serializedObject.FindProperty("onSplashScreenEnd");
            var mainPanelManager = serializedObject.FindProperty("mainPanelManager");
            var showOnlyOnce = serializedObject.FindProperty("showOnlyOnce");
            var skipOnAnyKeyPress = serializedObject.FindProperty("skipOnAnyKeyPress");

            switch (currentTab)
            {
                case 0:
                    UI_EditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(12);
                    EditorGUILayout.PropertyField(splashScreenTitles, new GUIContent("Splash Screen Titles"), true);
                    splashScreenTitles.isExpanded = true;

                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);

                    if (ssmTarget.splashScreenTitles.Count != 0 && ssmTarget.splashScreenTitles[ssmTarget.splashScreenTitles.Count - 1] != null)
                    {
                        if (GUILayout.Button("+  Create a new title", customSkin.button))
                        {
                            GameObject go = Instantiate(ssmTarget.splashScreenTitles[1].gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            go.transform.SetParent(ssmTarget.splashScreenTitles[1].transform.parent, false);
                            go.gameObject.name = "New Title";
                            ssmTarget.splashScreenTitles.Add(go.GetComponent<UI_SplashScreen>());
                        }
                    }

                    GUILayout.EndVertical();
                    break;

                case 1:
                    UI_EditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    UI_EditorHandler.DrawProperty(splashScreen, customSkin, "Splash Screen");
                    UI_EditorHandler.DrawProperty(mainPanelManager, customSkin, "Main Panel Manager");
                    UI_EditorHandler.DrawProperty(mainPanelParent, customSkin, "Main Panel Parent");
                    UI_EditorHandler.DrawProperty(modalWindowParent, customSkin, "Modal Window Parent");
                    UI_EditorHandler.DrawProperty(transitionHelper, customSkin, "Transition Helper");
                    break;

                case 2:
                    UI_EditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    disableSplashScreen.boolValue = UI_EditorHandler.DrawToggle(disableSplashScreen.boolValue, customSkin, "Disable Splash Screen");
                    showOnlyOnce.boolValue = UI_EditorHandler.DrawToggle(showOnlyOnce.boolValue, customSkin, "Show Only Once");
                    skipOnAnyKeyPress.boolValue = UI_EditorHandler.DrawToggle(skipOnAnyKeyPress.boolValue, customSkin, "Skip On Any Key Press");
                    UI_EditorHandler.DrawProperty(startDelay, customSkin, "Start Delay");
                    EditorGUILayout.PropertyField(onSplashScreenEnd, new GUIContent("On Splash Screen End"), true);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}