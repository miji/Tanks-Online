using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

/// <summary>
/// The settings window for the ABMinimap asset
/// </summary>
public class MSettingsWindow : EditorWindow
{
    private Vector2 _scrollPos;

    [MenuItem("Window/AB Minimap/Settings")]
    private static void Open()
    {
        // Get existing open window or if none, make a new one:
        MSettingsWindow window = (MSettingsWindow)EditorWindow.GetWindow(typeof(MSettingsWindow), false, "ABMinimap settings");
        window.Show();
    }

    private void OnGUI()
    {
        Color origColor = GUI.color;
        EditorGUILayout.BeginVertical();
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);

        EditorGUILayout.HelpBox("Hover over options for more details", MessageType.None);

        MUtilEditor.DrawHrLine();
        
        //Core settings
        GUILayout.Label("Core settings", EditorStyles.boldLabel);
        MSettings.Instance.ExecuteInEditMode = EditorGUILayout.Toggle(new GUIContent("Execute in edit mode", "Executing in edit mode will update the views and other scripts so, for example, moving the target or scaling will update the map while in the editor. Useful if you want to see real-time changes but can cause problems when experimenting with the scripts."), MSettings.Instance.ExecuteInEditMode);
        
        MUtilEditor.DrawHrLine();
        
        //Update settings
        GUILayout.Label("Global Update settings", EditorStyles.boldLabel);

        //show how many fps we will get
        string updateInfoLabel = "Global update runs";
        if (MSettings.Instance.GlobalUpdateIn == MUpdateType.CustomUpdate)
            updateInfoLabel += " at " + MUtil.FrequencyToReadableFps(MSettings.Instance.CustomUpdateFrequency) + " fps";
        else if (MSettings.Instance.GlobalUpdateIn == MUpdateType.FixedUpdate)
            updateInfoLabel += " from " + MUtil.FrequencyToReadableFps(Time.maximumDeltaTime) + " to " + MUtil.FrequencyToReadableFps(Time.fixedDeltaTime) + " fps";
        else if (MSettings.Instance.GlobalUpdateIn == MUpdateType.LateUpdate || MSettings.Instance.GlobalUpdateIn == MUpdateType.Update)
            updateInfoLabel += " every frame";

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox(updateInfoLabel, MessageType.None);
        if (GUILayout.Button("Show all overrides"))
        {
            MPrefabsWUpdOverrideWindow.GetWindow(typeof(MPrefabsWUpdOverrideWindow), true, "Show all prefabs with Global Update overrides", true);
        }
        EditorGUILayout.EndHorizontal();

        MSettings.Instance.GlobalUpdateIn = (MUpdateType)EditorGUILayout.EnumPopup(new GUIContent("Update type", "Running at lower framerates helps the performance but can cause jittering and other effects if not set properly. Set to Update if you want disable this optimization. Can be overriden individually on scripts."), MSettings.Instance.GlobalUpdateIn);

        if (MSettings.Instance.GlobalUpdateIn == MUpdateType.CustomUpdate)
        {
            MSettings.Instance.CustomUpdateFrequency = EditorGUILayout.FloatField(new GUIContent("Update frequency", "Update map-related scripts every (value) seconds unless a local override is specified"), MSettings.Instance.CustomUpdateFrequency);
            MSettings.Instance.AntispikeOptimization = EditorGUILayout.Toggle(new GUIContent("Antispike optimization", "Experimental feature that tries to decrease spikes by delaying executions of scripts. Can cause issues such as jittering."), MSettings.Instance.AntispikeOptimization);
        }

        MUtilEditor.DrawHrLine();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(MSettings.Instance);

        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        GUI.color = origColor;
    }
}
