using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// The component that represents an entity in the minimap
/// </summary>
[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
public class MPin : MMapBase 
{
    /// <summary>
    /// Reference to the MObject associated with this class
    /// </summary>
    private MObject _mObj;
    private RectTransform _rTrans;
    private Image _visualization;

    public RectTransform RTrans { get { return _rTrans; } set { _rTrans = value; } }
    public MObject MObj { get { return _mObj; } set { _mObj = value; } }
    public Image Visualization { get { return _visualization; } set { _visualization = value; } }

    protected override void Awake()
    {
        base.Awake();

        if (_rTrans == null) _rTrans = GetComponent<RectTransform>();
        if (_visualization == null) _visualization = GetComponent<Image>();
        if (_mObj == null) _mObj = transform.GetComponentInParentRecursively<MObject>();
    }

    protected override void MapUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        base.MapUpdate();

        if (_rTrans == null) return;
        
        ApplyVisibility();
    }

    private void ApplyVisibility()
    {
        if (_visualization == null) return;

        if (_mObj.IsVisible && !_visualization.enabled)
        {
            _visualization.enabled = true;
            
            //TODO PlayQueued is probably not the best idea
            if (GetComponent<Animation>() != null && GetComponent<Animation>().clip != null)
                GetComponent<Animation>().PlayQueued(GetComponent<Animation>().clip.name);
        }
        else if (!_mObj.IsVisible && _visualization.enabled)
        {
            //TODO add animation here
            _visualization.enabled = false;
        }
    }
}
