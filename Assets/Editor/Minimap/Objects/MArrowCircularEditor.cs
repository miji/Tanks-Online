using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(MArrowCircular))]
public class MArrowCircularEditor : MMapBaseEditor
{
    private MArrowCircular _mArrowCircular;

    protected override void Awake()
    {
        base.Awake();

        if (_mArrowCircular == null)
            this._mArrowCircular = (MArrowCircular)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();
        float distanceFromOrigin = EditorGUILayout.FloatField("Radius", _mArrowCircular.radius);

        if (_mArrowCircular.MObj == null)
        {
            MUtilEditor.DrawErrorBox("Not on the same gameobject or a child of a MObject! Assign this anywhere next to / under a MObject component.");
        }

        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mArrowCircular, _mArrowCircular.name + " changed");
            _mArrowCircular.radius = distanceFromOrigin;
            EditorUtility.SetDirty(target);
        }
    }

    public virtual void OnSceneGUI()
    {
        
    }

    private void UpdateReferences()
    {
        if (_mArrowCircular.MObj == null) _mArrowCircular.MObj = _mArrowCircular.transform.GetComponentInParentRecursively<MObject>();
        if (_mArrowCircular.RTrans == null) _mArrowCircular.RTrans = _mArrowCircular.GetComponent<RectTransform>();
        if (_mArrowCircular.Visualization == null) _mArrowCircular.Visualization = _mArrowCircular.GetComponent<Image>();
        EditorUtility.SetDirty(target);
    }
}