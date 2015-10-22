using UnityEngine;
using System;

/// <summary>
/// Minimap settings
/// </summary>

public class MSettings : MScriptableSingleton<MSettings>
{
    [SerializeField]
    private bool executeInEditMode = true;

    public bool ExecuteInEditMode
    {
        get { return executeInEditMode; }
        set { executeInEditMode = value; }
    }
    [SerializeField]
    private bool antispikeOptimization = true;

    public bool AntispikeOptimization
    {
        get { return antispikeOptimization; }
        set { antispikeOptimization = value; }
    }
    [SerializeField]
    private float customUpdateFrequency = 0.015f;

    public float CustomUpdateFrequency
    {
        get { return customUpdateFrequency; }
        set { customUpdateFrequency = value; }
    }
    [SerializeField]
    private MUpdateType globalUpdateType = MUpdateType.CustomUpdate;

    public MUpdateType GlobalUpdateIn
    {
        get { return globalUpdateType; }
        set { globalUpdateType = value; }
    }
}