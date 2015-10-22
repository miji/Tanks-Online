using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MMarkCircle))]
public class MMarkCircleEditor : Editor
{
    private MMarkCircle _mMarkCircle;

    protected void Awake()
    {
        if (_mMarkCircle == null)
            this._mMarkCircle = (MMarkCircle)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        if (_mMarkCircle.MObj == null)
        {
            MUtilEditor.DrawErrorBox("Not on the same gameobject or a child of a MObject! Assign this anywhere next to / under a MObject component.");
        }
        EditorGUILayout.EndVertical();
    }

    public virtual void OnSceneGUI()
    {
    }

    private void UpdateReferences()
    {
        if (_mMarkCircle.MObj == null) _mMarkCircle.MObj = _mMarkCircle.transform.GetComponentInParentRecursively<MObject>();
        EditorUtility.SetDirty(target);
    }
}