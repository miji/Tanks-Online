using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Scales the MObject
/// </summary>
[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
public class MObjScale : MMapBase
{
    /// <summary>
    /// Reference to the MObject associated with this class
    /// </summary>
    private MObject _mObj;
    private RectTransform _rTrans;
    private Vector2 _originalSize;
    private Vector2 _targetScale;
    private bool _hasInitializedScale = false;

    public RectTransform RTrans { get { return _rTrans; } set { _rTrans = value; } }
    public MObject MObj { get { return _mObj; } set { _mObj = value; } }

    protected override void Awake()
    {
        base.Awake();

        if (_rTrans == null) _rTrans = GetComponent<RectTransform>();
        if (_mObj == null) _mObj = transform.GetComponentInParentRecursively<MObject>();
    }

    protected override void Start()
    {
        base.Start();

        if (!_hasInitializedScale)
        {
            _originalSize = new Vector2(_rTrans.rect.width, _rTrans.rect.height) * _mObj.DefaultScale;

            ScaleObject();
            _hasInitializedScale = true;
        }
    }

    protected override void MapUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        base.MapUpdate();
        
        if (_rTrans == null) return;
        if (_mObj.IsScaling) ScaleObject();
    }

    private void ScaleObject()
    {
        //scale
        _targetScale = new Vector2(_originalSize.x * _mObj.ScaleRatio, _originalSize.y * _mObj.ScaleRatio);

        //return if nothing to scale
        if (_rTrans.sizeDelta == _targetScale) return;

        if (_hasInitializedScale && _mObj.IsScalingSmoothly)
        {
            //keep lerping until we have the correct scale
            _rTrans.sizeDelta = Vector2.Lerp(_rTrans.sizeDelta, _targetScale, _mObj.SmoothScaleSpeed * MapDeltaTime);
        }
        else if (!_hasInitializedScale || !_mObj.IsScalingSmoothly)
        {
            _rTrans.sizeDelta = _targetScale;
        }
    }
}
