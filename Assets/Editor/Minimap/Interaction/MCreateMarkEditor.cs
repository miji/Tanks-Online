using UnityEditor.VersionControl;
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MCreateMark))]
public class MCreateMarkEditor : Editor
{
    private MCreateMark _mCreateMark;

    protected virtual void OnEnable()
    {
        if (_mCreateMark == null)
            this._mCreateMark = (MCreateMark)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();
        Object markPrefab = EditorGUILayout.ObjectField("Mark prefab", _mCreateMark.markPrefab, typeof(MObject), true);

        if (_mCreateMark.View == null)
        {
            MUtilEditor.DrawErrorBox("Not on the same gameobject or a child of a MView! Assign this anywhere next to / under a MView component.");
        }

        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mCreateMark, _mCreateMark.name + " changed");
            _mCreateMark.markPrefab = (MObject) markPrefab;
            EditorUtility.SetDirty(target);
        }
    }

    public virtual void OnSceneGUI() { }

    private void UpdateReferences()
    {
        if (_mCreateMark.View == null) _mCreateMark.View = _mCreateMark.transform.GetComponentInParentRecursively<MView>();
        EditorUtility.SetDirty(target);
    }
}