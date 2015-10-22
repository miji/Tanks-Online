using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Translates the MObject
/// </summary>
[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
public class MObjRotateFreeze : MMapBase
{
    /// <summary>
    /// Reference to the MObject associated with this class
    /// </summary>
    private MObject _mObj;
    private RectTransform _rTrans;
    private RectTransform _pivotTrans;

    public MObject MObj { get { return _mObj; } set { _mObj = value; } }
    public RectTransform RTrans { get { return _rTrans; } set { _rTrans = value; } }
    public RectTransform PivotTrans { get { return _pivotTrans; } set { _pivotTrans = value; } }

    protected override void Awake()
    {
        base.Awake();

        if (_rTrans == null) _rTrans = GetComponent<RectTransform>();
        if (_mObj == null) _mObj = transform.GetComponentInParentRecursively<MObject>();
    }

    protected override void Start()
    {
        base.Start();

        if (_pivotTrans == null) _pivotTrans = transform.GetComponentInParentRecursively<RectTransform>(MVal.PivotGoName);
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

        //rot
        _rTrans.localEulerAngles = new Vector3(_rTrans.localEulerAngles.x, _rTrans.localEulerAngles.y, -_pivotTrans.localEulerAngles.z);
    }
}
