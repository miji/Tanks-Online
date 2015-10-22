using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Translates the MObject
/// </summary>
[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
public class MObjTranslate : MMapBase
{
    /// <summary>
    /// Is the lerping its movement on the map
    /// NOTE: might cause quite a lot of issues such as the pin slowly moving across the map if the new position is far away
    /// TODO decide what to do with this
    /// </summary>
    public bool isMovingSmoothly = false;
    /// <summary>
    /// If lerpimg, what is the lerp multiplier
    /// </summary>
    public float smoothMoveSpeed = 0.0f;

    /// <summary>
    /// Reference to the MObject associated with this class
    /// </summary>
    private MObject _mObj;
    private RectTransform _rTrans;

    public RectTransform RTrans { get { return _rTrans; } set { _rTrans = value; } }
    public MObject MObj { get { return _mObj; } set { _mObj = value; } }

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
        //if not visible, don't transform pos
        if (!_mObj.IsVisible) return;

        //pos
        if (!isMovingSmoothly)
        {
            _rTrans.localPosition = new Vector3(_mObj.TargetPosition.x, _mObj.TargetPosition.y, _rTrans.localPosition.z);
        }
        else
        {
            _rTrans.localPosition = Vector3.Lerp(_rTrans.localPosition,
                new Vector3(_mObj.TargetPosition.x, _mObj.TargetPosition.y, _rTrans.localPosition.z),
                smoothMoveSpeed * MapDeltaTime);
        }
    }
}
