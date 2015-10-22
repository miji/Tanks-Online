using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MView))]
public class MViewEditor : MMapBaseEditor
{
    private MView _mView;

    protected override void Awake()
    {
        base.Awake();

        if (_mView == null)
            this._mView = (MView)target;
        
        _mView.UpdateReferences();
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();
        var newOrient               = EditorGUILayout.EnumPopup("Map orientation", _mView.orientation);
        MUtilEditor.DrawHrLine();
        //float newZoomValue          = EditorGUILayout.FloatField("Zoom value", MView.zoomValue);
        EditorGUILayout.LabelField("Zoom value");
        float newZoomValue = EditorGUILayout.Slider(_mView.zoomValue, _mView.zoomInLimit, _mView.zoomOutLimit);
        float newDefaultStep        = EditorGUILayout.FloatField("Zoom step", _mView.zoomStep);
        float newDefaultInLimit     = EditorGUILayout.FloatField("Max zoom-in value", _mView.zoomInLimit);
        float newDefaultOutLimit    = EditorGUILayout.FloatField("Max zoom-out value", _mView.zoomOutLimit);
        bool newIsZoomingSmoothly   = EditorGUILayout.Toggle("Is zooming smoothly", _mView.isZoomingSmoothly);
        float newSmoothZoomSpeed    = EditorGUILayout.FloatField("Smooth zoom speed", _mView.smoothZoomSpeed);
        bool newIsScalingMObjects       = EditorGUILayout.Toggle("Is scaling objects", _mView.isScalingMObjects);
        MUtilEditor.DrawHrLine();
        float newMObjectDefaultScale = EditorGUILayout.FloatField("Default obj scale", _mView.mObjectDefaultSize);
        MUtilEditor.DrawHrLine();
        bool newIsRotatingMap       = EditorGUILayout.Toggle("Is rotating map", _mView.isRotatingMap);
        MUtilEditor.DrawHrLine();
        Object mapObj = MUtilEditor.ObjectFieldColored("Map transform", _mView.mapRTrans, typeof(RectTransform), true);
        Object pivotObj = MUtilEditor.ObjectFieldColored("Pivot transform", _mView.pivotRTrans, typeof(RectTransform), true);
        EditorGUILayout.EndVertical(); 

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mView, _mView.name + " changed");
            _mView.orientation = (MOrientation)newOrient;
            _mView.mapRTrans = (RectTransform)mapObj;
            _mView.pivotRTrans = (RectTransform)pivotObj;
            _mView.zoomValue = newZoomValue;
            _mView.zoomStep = newDefaultStep;
            _mView.zoomInLimit = newDefaultInLimit;
            _mView.zoomOutLimit = newDefaultOutLimit;
            _mView.isZoomingSmoothly = newIsZoomingSmoothly;
            _mView.smoothZoomSpeed = newSmoothZoomSpeed;
            _mView.isScalingMObjects = newIsScalingMObjects;
            _mView.mObjectDefaultSize = newMObjectDefaultScale;
            _mView.isRotatingMap = newIsRotatingMap;
            EditorUtility.SetDirty(target);
        }
    }

    public virtual void OnSceneGUI()
    {
    }
}