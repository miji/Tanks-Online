using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(MMapBase))]
public class MMapBaseEditor : Editor
{
    private MMapBase _mMapBase;
    private AnimBool _changeDefaultUpdateAnim;
    private bool _showFoldout;
   
    protected virtual void Awake()
    {
        this._mMapBase = (MMapBase)target;
    }

    protected virtual void OnEnable()
    {
        _changeDefaultUpdateAnim = new AnimBool(false, Repaint);
    }
    
    public override void OnInspectorGUI()
    {
        Color origColor = GUI.color;
        EditorGUI.BeginChangeCheck();

        if (_mMapBase.hasUpdateOverride) GUI.color = MUtilEditor.WarningColor;
        else GUI.color = MUtilEditor.UnimportantColor;

        //foldout label
        MUpdateType currentUpdateIn = _mMapBase.hasUpdateOverride ? _mMapBase.updateType : MSettings.Instance.GlobalUpdateIn;
        string foldoutLabel = _mMapBase.hasUpdateOverride ? "" + currentUpdateIn : "" + currentUpdateIn;
        if (currentUpdateIn == MUpdateType.CustomUpdate)
            foldoutLabel += " (" + MUtil.FrequencyToReadableFps(_mMapBase.hasUpdateOverride ? _mMapBase.customUpdateFrequency : MSettings.Instance.CustomUpdateFrequency) + " fps)";
        else if (currentUpdateIn == MUpdateType.FixedUpdate)
            foldoutLabel += " (from " + MUtil.FrequencyToReadableFps(Time.maximumDeltaTime) + " to " + MUtil.FrequencyToReadableFps(Time.fixedDeltaTime) + " fps)";
        else if (currentUpdateIn == MUpdateType.LateUpdate || currentUpdateIn == MUpdateType.Update)
            foldoutLabel += " (updates every frame)";

        _showFoldout = EditorGUILayout.Foldout(_showFoldout, foldoutLabel);

        GUI.color = origColor;
        //if foldout not visible, return
        if (!_showFoldout) return;

        _changeDefaultUpdateAnim.target = EditorGUILayout.ToggleLeft("Enable update override", _mMapBase.hasUpdateOverride);
        _mMapBase.hasUpdateOverride = _changeDefaultUpdateAnim.target;
        

        //if toggle true
        if (EditorGUILayout.BeginFadeGroup(_changeDefaultUpdateAnim.faded))
        {
            Enum updateType = EditorGUILayout.EnumPopup("Update type", _mMapBase.updateType);
            MUpdateType updateIn = (MUpdateType)updateType;
            float updateFrequency = _mMapBase.customUpdateFrequency;
            bool updateOptimization = _mMapBase.antispikeOptiLocal;

            if (updateIn == MUpdateType.CustomUpdate)
            {
                updateFrequency = EditorGUILayout.FloatField("Update frequency", _mMapBase.customUpdateFrequency);

                if (MSettings.Instance.AntispikeOptimization)
                    updateOptimization = EditorGUILayout.Toggle("Antispike optimization", _mMapBase.antispikeOptiLocal);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_mMapBase, _mMapBase.name + " changed");
                _mMapBase.updateType = updateIn;
                _mMapBase.customUpdateFrequency = updateFrequency;
                _mMapBase.antispikeOptiLocal = updateOptimization;
                EditorUtility.SetDirty(target);
            }
        }
        EditorGUILayout.EndFadeGroup();        
    }
}