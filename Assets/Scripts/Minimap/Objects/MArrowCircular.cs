using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Shows an arrow in a given radius from the map center if MObject is not visible in the minimap. The arrow points in the direction of the MObject's target position
/// </summary>
[ExecuteInEditMode]
public class MArrowCircular : MMapBase
{
    /// <summary>
    /// How far from the map center are we drawing the circle (same for all zoom levels)
    /// </summary>
    public float radius = 80.0f;

    /// <summary>
    /// The MObject to point at
    /// </summary>
    private MObject _mObj;
    private RectTransform _rTrans;
    private Image _visualization;

    public MObject MObj { get { return _mObj; } set { _mObj = value; } }
    public RectTransform RTrans { get { return _rTrans; } set { _rTrans = value; } }
    public Image Visualization { get { return _visualization; } set { _visualization = value; } }

    protected override void Awake()
    {
 	    base.Awake();

        _rTrans = GetComponent<RectTransform>();
        _visualization = GetComponent<Image>();

        if (_mObj == null) _mObj = transform.GetComponentInParentRecursively<MObject>();
    }

    protected override void MapUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        base.MapUpdate();

        if (_rTrans == null) return;

        ApplyVisibility();
        TransformArrow();
    }

    private void ApplyVisibility()
    {
        //if no visualization, nothing to change
        if (_visualization == null) return;

        if (_mObj.IsVisible && _visualization.enabled)
            _visualization.enabled = false;
        else if (!_mObj.IsVisible && !_visualization.enabled)
            _visualization.enabled = true;
    }

    private void TransformArrow()
    {
        //only works if the obj is not visible
        if (_mObj.IsVisible) return;

        Vector3 dirVec3 = Vector3.zero;
        switch (_mObj.Orientation)
        {
            case MOrientation.XZ:
                dirVec3 = new Vector3(_mObj.TargetPosition.x, 0, _mObj.TargetPosition.y);
                break;

            case MOrientation.YZ:
                dirVec3 = new Vector3(0, -_mObj.TargetPosition.x, _mObj.TargetPosition.y);
                break;

            default:
                MUtil.Error("Uknown orientation here", this);
                break;
        }

        //opposite direction as it points against the object hence the minus
        float angle = MMapTools.CalculateAngle(_mObj.Orientation, -dirVec3);

        _rTrans.localEulerAngles = new Vector3(0, 0, angle);
        _rTrans.localPosition = (Vector3)(_mObj.TargetPosition.normalized * radius) * _mObj.DefaultScale;
    }
}
