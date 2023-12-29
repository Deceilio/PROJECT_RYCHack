using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class RootMotionCreatorWindow : EditorWindow
{
    RootMotionCreatorUniversal creator;
    AnimationClip clip;

    [MenuItem("Assets/Create/Root Motion", priority = 402, validate = true)]
    public static bool ValidateSaveGameObject(MenuCommand command)
    {
        var clip = Selection.activeObject as AnimationClip;

        if (clip != null)
            return true;

        return false;
    }

    [MenuItem("Assets/Create/Root Motion", priority = 402, validate = false)]
    [MenuItem("Tools/Root Motion Creator", validate = false)]
    static void Init()
    {
        InitStyles();

        var clip = Selection.activeObject as AnimationClip;

        var window = (RootMotionCreatorWindow)EditorWindow.GetWindow(typeof(RootMotionCreatorWindow));
        window.clip = clip;
        window.titleContent = new GUIContent("Root Motion Creator");
        window.minSize = new Vector2(300, 400);
        window.Show();
    }

    private void Update()
    {
        if (clip != Selection.activeObject as AnimationClip)
        {
            clip = Selection.activeObject as AnimationClip;
            Repaint();
        }
    }

    bool arcMotion;
    bool motion;
    bool recognition;
    Texture2D StraightMotionImage;
    Texture2D ReverseImage;
    Texture2D ConstSpeedImage;
    Texture2D TurnImage;

    private void OnEnable()
    {
        creator = new RootMotionCreatorUniversal();
        StraightMotionImage = Resources.Load<Texture2D>("straightMotion");
        ReverseImage = Resources.Load<Texture2D>("reverseMotion");
        ConstSpeedImage = Resources.Load<Texture2D>("constSpeed");
        TurnImage = Resources.Load<Texture2D>("turn");
    }

    void OnGUI()
    {
        clip = Selection.activeObject as AnimationClip;
        if (clip == null)
        {
            EditorGUILayout.HelpBox("Please select a animation clip", MessageType.Info);
            return;
        }

        if (creator == null)
            creator = new RootMotionCreatorUniversal();

        //EditorGUILayout.HelpBox("This tool helps you to add Root Motion into \"in place\" animation.\r\nSelect needed motion direction and press Create button.", MessageType.None);
        //GUILayout.Label("This tool helps you to add Root Motion into \"in place\" animation.\r\nSelect needed motion direction and press Create button.");
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Animation:", GUILayout.Width(100));
        GUILayout.Label(clip.name, EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Referenced Model", GUILayout.Width(100));
        creator.ReferencedModel = (GameObject)EditorGUILayout.ObjectField(creator.ReferencedModel, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        if (creator.ReferencedModel == null)
            EditorGUILayout.HelpBox("Drag here model to animate.", MessageType.Info);

        GUILayout.Space(10);

        motion = EditorGUILayout.Foldout(motion, "Motion options", true, EditorStyles.foldout);
        if (motion)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label("Min/Max Speed (m/s)", GUILayout.Width(150));
            GUILayout.Label("From " + creator.MinSpeed.ToString("0.0") + " to " + creator.MaxSpeed.ToString("0.0") + " m/s");
            GUILayout.EndHorizontal();
            EditorGUILayout.MinMaxSlider(" ", ref creator.MinSpeed, ref creator.MaxSpeed, 0, 20);

            GUILayout.BeginHorizontal();
            creator.ConstantSpeed = EditorGUILayout.Toggle("Constant Speed", creator.ConstantSpeed, GUILayout.Height(32));
            GUILayout.Box(ConstSpeedImage, GUILayout.Width(64), GUILayout.Height(32));
            GUILayout.Label("", GUILayout.Width(6994));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            creator.AllowReverseMotion = EditorGUILayout.Toggle("Allow Reverse Motion", creator.AllowReverseMotion, GUILayout.Height(32));
            GUILayout.Box(ReverseImage, GUILayout.Width(64), GUILayout.Height(32));
            GUILayout.Label("", GUILayout.Width(6994));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            creator.StraightMotion = EditorGUILayout.Toggle("Straight Motion", creator.StraightMotion, GUILayout.Height(32));
            GUILayout.Box(StraightMotionImage, GUILayout.Width(64), GUILayout.Height(32));
            GUILayout.Label("", GUILayout.Width(6994));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Allowed Motion Axes", GUILayout.Width(150), GUILayout.Height(32));
            var allowed = creator.AllowedOffsets;
            GUILayout.Label("x", GUILayout.Width(10), GUILayout.Height(32));
            allowed.x = EditorGUILayout.Toggle("", allowed.x == 1, GUILayout.Width(20), GUILayout.Height(32)) ? 1 : 0;
            GUILayout.Label("y", GUILayout.Width(10), GUILayout.Height(32));
            allowed.y = EditorGUILayout.Toggle("", allowed.y == 1, GUILayout.Width(20), GUILayout.Height(32)) ? 1 : 0;
            GUILayout.Label("z", GUILayout.Width(10), GUILayout.Height(32));
            allowed.z = EditorGUILayout.Toggle("", allowed.z == 1, GUILayout.Width(20), GUILayout.Height(32)) ? 1 : 0;
            creator.AllowedOffsets = allowed;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            creator.AddTurn = EditorGUILayout.IntSlider("Add Turn (degrees)", creator.AddTurn, -180, 180, GUILayout.Height(32));
            GUILayout.Box(TurnImage, GUILayout.Width(32), GUILayout.Height(32));
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        recognition = EditorGUILayout.Foldout(recognition, "Recognition options", true, EditorStyles.foldout);
        if (recognition)
        {
            creator.StepHeight = EditorGUILayout.Slider("Step Height (%)", creator.StepHeight * 100, 0, 10) / 100f;
            if (!clip.humanMotion || clip.legacy)
            {
                creator.FootNameRegex = EditorGUILayout.TextField("Foot Bones Name Regex", creator.FootNameRegex);
                creator.MinDepthFootBone = EditorGUILayout.IntField("Min Depth of Foot Bone", creator.MinDepthFootBone);
            }
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        if (motion || recognition)
        if (GUILayout.Button("Reset to Default Options"))
        {
            var model = creator.ReferencedModel;
            creator = new RootMotionCreatorUniversal();
            creator.ReferencedModel = model;
            creator.clip = clip;
        }

        GUILayout.Space(20);

        var contentColor = GUI.contentColor;
        var bkColor = GUI.backgroundColor;

        GUI.contentColor = Color.white;
        GUI.backgroundColor = Color.green;

        if (creator.ReferencedModel != null)
            if (GUILayout.Button("Create Root Motion Curves"))
            {
                try
                {
                    creator.Build(clip);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    EditorUtility.DisplayDialog("Exception", ex.Message, "Ok");
                }
            }

        GUI.backgroundColor = Color.yellow;

        GUILayout.Space(10);
        if (GUILayout.Button("Restore Original Root Curves"))
        {
            try
            {
                creator.clip = clip;
                creator.RestoreOriginalCurves();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                EditorUtility.DisplayDialog("Exception", ex.Message, "Ok");
            }
        }

        GUI.contentColor = contentColor;
        GUI.backgroundColor = bkColor;

        DrawHints(clip);
        CheckRootNodeInModel();
        DrawLinks();
    }

    private void DrawLinks()
    {
        //EditorGUILayout.Link
        var rateUrl = "https://assetstore.unity.com/packages/tools/animation/root-motion-creator-186328#reviews";
        var forumUrl = "https://forum.unity.com/threads/root-motion-creator.1087769/";

        GUILayout.Space(10);
        if (LinkLabel(new GUIContent("☛ Rate and Review this tool")))
            Application.OpenURL(rateUrl);
    }

    static GUIStyle m_LinkStyle;

    static void InitStyles()
    {
        m_LinkStyle = new GUIStyle(EditorStyles.label);
        m_LinkStyle.wordWrap = false;
        // Match selection color which works nicely for both light and dark skins
        m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
        m_LinkStyle.stretchWidth = false;
    }

    
    bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
    {
        if (label == null || m_LinkStyle == null)
            return false;

        var position = GUILayoutUtility.GetRect(label, m_LinkStyle, options);

        Handles.BeginGUI();
        Handles.color = m_LinkStyle.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

        return GUI.Button(position, label, m_LinkStyle);
    }

    private void CheckRootNodeInModel()
    {
        if (clip.humanMotion || creator.ReferencedModel == null)
            return;

        var animator = creator.ReferencedModel.GetComponent<Animator>();
        if (animator == null || animator.avatar == null)
            return;

        var assetPath = AssetDatabase.GetAssetPath(animator.avatar);

        var modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
        if (modelImporter == null)
            return;
        var modelImporterObj = new SerializedObject(modelImporter);
        var rootNodeProperty = modelImporterObj.FindProperty("m_HumanDescription.m_RootMotionBoneName");
        if (rootNodeProperty.stringValue == "")
            EditorGUILayout.HelpBox("Model has no root node. Set root node in \"Rig Root node\" for the model.\r\nRoot motion will not work w/o root node!", MessageType.Warning);
    }

    private void DrawHints(AnimationClip clip)
    {
        var settings = AnimationUtility.GetAnimationClipSettings(clip);

        if (settings.loopBlendPositionXZ)
            EditorGUILayout.HelpBox("You have enabled \"Root Transform Position (XZ) Bake Into Pose\" in animation.\r\nTo make root motion you need to switch off this option.", MessageType.Warning);

        if (creator.AddTurn != 0 && settings.loopBlendOrientation)
            EditorGUILayout.HelpBox("You have enabled \"Root Transform Rotation Bake Into Pose\" in animation.\r\nTo make arc root motion you need to switch off this option.", MessageType.Warning);

        string assetPath = AssetDatabase.GetAssetPath(clip);
        if (!assetPath.ToLower().EndsWith(".anim"))
            EditorGUILayout.HelpBox("You have selected clip inside model. You can add root motion to it, but changes will not be saved.\r\nPress Ctrl-D on animation to extract it from model.", MessageType.Warning);
    }
}
