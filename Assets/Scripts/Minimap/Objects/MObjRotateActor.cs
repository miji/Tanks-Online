using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Translates the MObject
/// </summary>
[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
public class MObjRotateActor : MMapBase
{
    /// <summary>
    /// Reference to the MObject associated with this class
    /// </summary>
    private MObject _mObj;
    private RectTransform _rTrans;
    
    public MObject MObj { get { return _mObj; } set { _mObj = value; } }
    public RectTransform RTrans { get { return _rTrans; } set { _rTrans = value; } }

    protected override void Awake()
    {
        base.Awake();

        if (_rTrans == null) _rTrans = GetComponent<RectTransform>();
        if (_mObj == null) _mObj = transform.GetComponentInParentRecursively<MObject>();
    }

    protected override void MapUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        base.MapUpdate();

        if (_rTrans == null) return;
        TransformObject();
    }

    private void TransformObject()
    {
        //if not visible, don't transform rot
        if (!_mObj.IsVisible) return;

        Vector3 dirVec3 = Vector3.zero;
        switch (_mObj.Orientation)
        {
            case MOrientation.XZ:
                dirVec3 = _mObj.TargetForwardVector;
                break;

            case MOrientation.YZ:
                dirVec3 = new Vector3(0, -_mObj.TargetForwardVector.z, _mObj.TargetForwardVector.y);
                break;

            default:
                MUtil.Error("Uknown orientation here", this);
                break;
        }

        _rTrans.localEulerAngles = new Vector3(_rTrans.localEulerAngles.x, _rTrans.localEulerAngles.y,
            MMapTools.CalculateAngle(_mObj.Orientation, dirVec3));
    }
}
