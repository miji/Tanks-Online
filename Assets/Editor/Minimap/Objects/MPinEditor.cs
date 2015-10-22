using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(MPin))]
public class MPinEditor : MMapBaseEditor
{
    private MPin _mPin;

    protected override void Awake()
    {
        base.Awake();

        if (_mPin == null)
            this._mPin = (MPin)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();

        if (_mPin.MObj == null)
        {
            MUtilEditor.DrawErrorBox("Not on the same gameobject or a child of a MObject! Assign this anywhere next to / under a MObject component.");
        }

        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mPin, _mPin.name + " changed");
            EditorUtility.SetDirty(target);
        }
    }

    public virtual void OnSceneGUI()
    {
    }

    private void UpdateReferences()
    {
        if (_mPin.MObj == null) _mPin.MObj = _mPin.transform.GetComponentInParentRecursively<MObject>();
        if (_mPin.RTrans == null) _mPin.RTrans = _mPin.GetComponent<RectTransform>();
        if (_mPin.Visualization == null) _mPin.Visualization = _mPin.GetComponent<Image>();
        EditorUtility.SetDirty(target);
    }
}