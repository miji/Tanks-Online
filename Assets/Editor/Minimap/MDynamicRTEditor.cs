using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(MDynamicRT))]
public class MDynamicRTEditor : Editor
{
    private MDynamicRT _mDynamicRT;
   
    protected virtual void Awake()
    {
        this._mDynamicRT = (MDynamicRT)target;
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        int colorDepth = EditorGUILayout.IntField("Color depth", _mDynamicRT.colorDepth);
        int msaa = EditorGUILayout.IntField("Applied MSAA", _mDynamicRT.antiAliasing);
        
        if (msaa != 1)
            EditorGUILayout.HelpBox("If the output texture is pitch fblack, either your camera's backround is black and is pointing outside of the level or try setting MSAA to 1. There seem to be a problem with DX11 and RenderTexture sometimes.", MessageType.Info);

        FilterMode filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filtering", _mDynamicRT.filterMode);

        bool resAsController = EditorGUILayout.Toggle("Resolution according to the controller", _mDynamicRT.ResolutionAsController);
        Vector2 rtRes = _mDynamicRT.ResolutionOverride;
        if (!resAsController)
            rtRes = MUtilEditor.Vector2FieldColored("RenderTexture res override", _mDynamicRT.ResolutionOverride, 0, 0);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mDynamicRT, _mDynamicRT.name + " changed");
            _mDynamicRT.antiAliasing = msaa;
            _mDynamicRT.colorDepth = colorDepth;
            _mDynamicRT.filterMode = filterMode;
            _mDynamicRT.ResolutionAsController = resAsController;
            _mDynamicRT.ResolutionOverride = rtRes;
            EditorUtility.SetDirty(target);
        }
    }
}