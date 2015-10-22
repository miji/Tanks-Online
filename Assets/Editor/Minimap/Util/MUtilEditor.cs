using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Utility classes containing useful methods for drawing in the editor
/// </summary>
public static class MUtilEditor
{
    public static Object ObjectFieldColored(string label, Object obj, System.Type objType, bool allowSceneObjects)
    {
        Color origColor = GUI.color;
        if (obj == null) GUI.color = MUtilEditor.WarningColor;
        Object result = EditorGUILayout.ObjectField(label, obj, objType, allowSceneObjects);
        if (obj == null) GUI.color = origColor;
        return result;
    }

    //TODO extend this to take specific values, such as negative etc.
    public static Vector2 Vector2FieldColored(string label, Vector2 vec2, Vector2 invalidVector)
    {
        Color origColor = GUI.color;
        if (vec2 == invalidVector) GUI.color = MUtilEditor.WarningColor;
        Vector2 result = EditorGUILayout.Vector2Field(label, vec2);
        if (vec2 == invalidVector) GUI.color = origColor;
        return result;
    }

    public static Vector2 Vector2FieldColored(string label, Vector2 vec2, float invalidX, float invalidY)
    {
        Color origColor = GUI.color;
        if (vec2.x == invalidX || vec2.y == invalidY) GUI.color = MUtilEditor.WarningColor;
        Vector2 result = EditorGUILayout.Vector2Field(label, vec2);
        if (vec2.x == invalidX || vec2.y == invalidY) GUI.color = origColor;
        return result;
    }


    private static Color warningColor = new Color(0.9f, 0.6f, 0.6f);

    public static Color WarningColor
    {
        get { return MUtilEditor.warningColor; }
    }

    private static Color unimportantColor = new Color(0.6f, 0.6f, 0.6f);

    public static Color UnimportantColor
    {
        get { return MUtilEditor.unimportantColor; }
    }

    public static void DrawErrorBox(string text)
    {
        EditorGUILayout.HelpBox(text, MessageType.Error);
    }

    public static void DrawHrLine()
    {
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
    }

    public static void DrawHeadline(string text)
    {
        GUILayout.Box(text, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(20) });
    }

    public static List<MonoScript> GetAllScriptsAssignableFrom<T>() 
    {
        List<MonoScript> scripts = new List<MonoScript>();
        foreach (string assetGuid in AssetDatabase.FindAssets("t:script"))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

            MonoScript mono = (MonoScript)AssetDatabase.LoadAssetAtPath(assetPath, typeof(MonoScript)) as MonoScript;
            if (mono != null && mono.GetClass() != null && typeof(T).IsAssignableFrom(mono.GetClass()))
                scripts.Add(mono);
        }

        return scripts;
    }
}
