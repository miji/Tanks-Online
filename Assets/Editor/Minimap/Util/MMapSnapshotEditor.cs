using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;

[CustomEditor(typeof(MMapSnapshot))]
public class MMapSnapshotEditor : Editor
{
    protected MMapSnapshot _mMapSnapshot;

    protected virtual void Awake()
    {
        this._mMapSnapshot = (MMapSnapshot)target;
    }

    public override void OnInspectorGUI()
    {
        
        EditorGUI.BeginChangeCheck();
        
#if !UNITY_WEBPLAYER
        Color origColor = GUI.color;
        GUI.color = Color.green;
        if (GUILayout.Button("Take map snapshot", new GUILayoutOption[] { GUILayout.Height(14) }))
        {
            _mMapSnapshot.TakeSnapshot();
            //update the project to see the outcome
            AssetDatabase.Refresh();
        }
        GUI.color = origColor;
#else
        EditorGUILayout.HelpBox("Snapshots are not supported in the Web Player editor at the moment, switch to other platform, take a snapshot and revert back. You can use the snapshots in Web Player build just fine.", MessageType.Error);
#endif


        int width = EditorGUILayout.IntField("Width in px", _mMapSnapshot.resWidth);

        if (width <= 0) width = 1;
        if (width > 4096) width = 4096;

        int height = EditorGUILayout.IntField("Height in px", _mMapSnapshot.resHeight);

        if (height <= 0) height = 1;
        if (height > 4096) height = 4096;

        int msaa = EditorGUILayout.IntField("Applied MSAA", _mMapSnapshot.msaa);

        if (msaa != 1)
            EditorGUILayout.HelpBox("If the output texture is pitch fblack, either your camera's backround is black and is pointing outside of the level or try setting MSAA to 1. There seem to be a problem with DX11 and RenderTexture sometimes.", MessageType.Info);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mMapSnapshot, _mMapSnapshot.name + " changed");
            _mMapSnapshot.resWidth = width;
            _mMapSnapshot.resHeight = height;
            _mMapSnapshot.msaa = msaa;
            EditorUtility.SetDirty(target);
        }
    }
}