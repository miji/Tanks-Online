using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MObjTranslate))]
public class MObjTranslateEditor : MMapBaseEditor
{
    private MObjTranslate _mObjTranslate;

    protected override void Awake()
    {
        base.Awake();

        if (_mObjTranslate == null)
            this._mObjTranslate = (MObjTranslate)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();
        bool isMovingSmoothly = EditorGUILayout.Toggle("Is moving smoothly", _mObjTranslate.isMovingSmoothly);
        float smoothMoveSpeed = EditorGUILayout.FloatField("Smooth move speed", _mObjTranslate.smoothMoveSpeed);

        if (_mObjTranslate.MObj == null)
        {
            MUtilEditor.DrawErrorBox("Not on the same gameobject or a child of a MObject! Assign this anywhere next to / under a MObject component.");
        }
 
        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mObjTranslate, _mObjTranslate.name + " changed");
            _mObjTranslate.isMovingSmoothly = isMovingSmoothly;
            _mObjTranslate.smoothMoveSpeed = smoothMoveSpeed;
            EditorUtility.SetDirty(target);
        }
    }

    public virtual void OnSceneGUI()
    {
    }

    private void UpdateReferences()
    {
        if (_mObjTranslate.MObj == null) _mObjTranslate.MObj = _mObjTranslate.transform.GetComponentInParentRecursively<MObject>();
        if (_mObjTranslate.RTrans == null) _mObjTranslate.RTrans = _mObjTranslate.GetComponent<RectTransform>();
        EditorUtility.SetDirty(target);
    }
}