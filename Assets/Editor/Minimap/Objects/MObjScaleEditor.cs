using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MObjScale))]
public class MObjScaleEditor : MMapBaseEditor
{
    MObjScale _mObjScale;

    protected override void Awake()
    {
        base.Awake();

        if (_mObjScale == null)
            this._mObjScale = (MObjScale)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();
        if (_mObjScale.MObj == null)
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
        if (_mObjScale.MObj == null) _mObjScale.MObj = _mObjScale.transform.GetComponentInParentRecursively<MObject>();
        if (_mObjScale.RTrans == null) _mObjScale.RTrans = _mObjScale.GetComponent<RectTransform>();
        EditorUtility.SetDirty(target);
    }
}