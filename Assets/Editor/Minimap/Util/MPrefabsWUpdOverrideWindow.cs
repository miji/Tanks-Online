using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Text;

/// <summary>
/// The settings window for the ABMinimap asset
/// </summary>
public class MPrefabsWUpdOverrideWindow : EditorWindow
{
    private StringBuilder strStats = new StringBuilder("Click Refresh to see the result!");

    private Vector2 _scrollPos;

    private static void Open()
    {
        // Get existing open window or if none, make a new one:
        MPrefabsWUpdOverrideWindow window = (MPrefabsWUpdOverrideWindow)EditorWindow.GetWindow(typeof(MPrefabsWUpdOverrideWindow), true, "Assets with Global Update override", true);
        window.Show();
        window.CheckAssetsForOverride();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Refresh"))
        {
            CheckAssetsForOverride();
        }

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        if (strStats != null)
            EditorGUILayout.HelpBox(strStats.ToString(), MessageType.None);
        EditorGUILayout.EndScrollView();
    }

    public void CheckAssetsForOverride()
    {
        strStats = new StringBuilder();
        foreach (string assetGuid in AssetDatabase.FindAssets("t:prefab"))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            var go = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

            foreach (MMapBase mapBase in go.GetComponentsInChildren<MMapBase>(true))
            {
                if (mapBase.hasUpdateOverride)
                {
                    strStats.Append(assetPath + " -> " + mapBase.name + " is set to " + mapBase.updateType + "\n");
                }
            }
        }
    }
}
