using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MObjRotateActor))]
public class MObjRotateActorEditor : MMapBaseEditor
{
    private MObjRotateActor _mObjRotateActor;

    protected override void Awake()
    {
        base.Awake();

        if (_mObjRotateActor == null)
            this._mObjRotateActor = (MObjRotateActor)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();
        if (_mObjRotateActor.MObj == null)
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
        if (_mObjRotateActor.MObj == null) _mObjRotateActor.MObj = _mObjRotateActor.transform.GetComponentInParentRecursively<MObject>();
        if (_mObjRotateActor.RTrans == null) _mObjRotateActor.RTrans = _mObjRotateActor.GetComponent<RectTransform>();
        EditorUtility.SetDirty(target);
    }
}