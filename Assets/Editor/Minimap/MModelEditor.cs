using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;

[CustomEditor(typeof(MModel))]
public class MModelEditor : Editor 
{
    protected MModel mModel;

    protected virtual void Awake()
    {
        this.mModel = (MModel)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        UnityEngine.Object mmTarget = EditorGUILayout.ObjectField("Target", mModel.toFollowTrans, typeof(Transform), true);
                
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(mModel, mModel.name + " changed");
            mModel.toFollowTrans = (Transform)mmTarget;
            EditorUtility.SetDirty(target);
        }
    }
}