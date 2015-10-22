using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MObjRotateFreeze))]
public class MObjRotateFreezeEditor : Editor
{
    MObjRotateFreeze _mObjRotateFreeze;

    protected virtual void Awake()
    {
        if (_mObjRotateFreeze == null)
            this._mObjRotateFreeze = (MObjRotateFreeze)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        if (_mObjRotateFreeze.MObj == null)
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
        if (_mObjRotateFreeze.MObj == null) _mObjRotateFreeze.MObj = _mObjRotateFreeze.transform.GetComponentInParentRecursively<MObject>();
        if (_mObjRotateFreeze.RTrans == null) _mObjRotateFreeze.RTrans = _mObjRotateFreeze.GetComponent<RectTransform>();
        Debug.Log("X");
        if (_mObjRotateFreeze.PivotTrans == null) _mObjRotateFreeze.PivotTrans = _mObjRotateFreeze.transform.GetComponentInParentRecursively<RectTransform>(MVal.PivotGoName);
        EditorUtility.SetDirty(target);
    }
}