using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The base class for classes that need to perform or react to OnMapUpdate 
/// </summary>
[ExecuteInEditMode]
public class MMapBase : MonoBehaviour 
{
    /// <summary>
    /// If you want to override the global update feature
    /// </summary>
    public bool hasUpdateOverride = false;
    /// <summary>
    /// If global update override is active, which update to use
    /// </summary>
    public MUpdateType updateType = MUpdateType.CustomUpdate;
    /// <summary>
    /// If global update override is active and custom update is used, what update frequency should we use 
    /// </summary>
    public float customUpdateFrequency = 0.015f;
    /// <summary>
    /// If you want to enable optimization that will gradually disolve execution times over time. Useful especially with lower framerates and many scripts (there might not be visible effect with high framerates). However, the optimization puts some randomness into the execution order.
    /// </summary>
    public bool antispikeOptiLocal = false;
    /// <summary>
    /// This is the delta time that you should use inside the MapUpdate method
    /// </summary>
    protected float MapDeltaTime = 0.0f;

    private float _nextCustomUpdate = 0.0f;
    private float _timeOfLastCustomUpdate = 0.0f;
   
    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDestroy() { }

    protected virtual void Update() 
    {
        if (!MUtil.ExecuteInEditMode()) return;

        if (!hasUpdateOverride && MSettings.Instance.GlobalUpdateIn == MUpdateType.Update || hasUpdateOverride && updateType == MUpdateType.Update)
        {
            MapDeltaTime = Time.deltaTime;
            MapUpdate();
        }
        else if (!hasUpdateOverride && MSettings.Instance.GlobalUpdateIn == MUpdateType.CustomUpdate || hasUpdateOverride && updateType == MUpdateType.CustomUpdate)
        {
            CustomUpdate();
        }
	}

    protected virtual void FixedUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        if (!hasUpdateOverride && MSettings.Instance.GlobalUpdateIn == MUpdateType.FixedUpdate || hasUpdateOverride && updateType == MUpdateType.FixedUpdate)
        {
            MapDeltaTime = Time.fixedDeltaTime;
            MapUpdate();
        }
    }

    protected virtual void LateUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        if (!hasUpdateOverride && MSettings.Instance.GlobalUpdateIn == MUpdateType.LateUpdate || hasUpdateOverride && updateType == MUpdateType.LateUpdate)
        {
            MapDeltaTime = Time.deltaTime;
            MapUpdate();
        }
    }

    protected virtual void CustomUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

#if UNITY_EDITOR
        //if in editor, just update every frame
        if (!Application.isPlaying)
        {
            MapUpdate();
            return;
        }
#endif

        //using real-time will "disolve" the update execution times over time (not guaranteed however), standard time doesn't
        float time = (MSettings.Instance.AntispikeOptimization && (!hasUpdateOverride || (hasUpdateOverride && antispikeOptiLocal)))
            ? Time.realtimeSinceStartup : Time.time;

        if (time >= _nextCustomUpdate)
        {
            MapDeltaTime = time - _timeOfLastCustomUpdate;
            _timeOfLastCustomUpdate = _nextCustomUpdate;
            _nextCustomUpdate = time + (hasUpdateOverride ? customUpdateFrequency : MSettings.Instance.CustomUpdateFrequency);
            MapUpdate();
        }
    }

    /// <summary>
    /// Everything you want to do when the map updates, do it here
    /// </summary>
    protected virtual void MapUpdate() {}
}