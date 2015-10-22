using UnityEngine;
using System.Collections;

/// <summary>
/// Contains all the important values that the MObject components might use
/// </summary>
public class MObject : MonoBehaviour, ICanvasRaycastFilter 
{
    /// <summary>
    /// The depth value. Higher value indicates that the object will be rendered last among other objects withing this view
    /// </summary>
    public int depthIndex = 0;
    /// <summary>
    /// Whether you want to block the UI raycasts or let them pass through
    /// </summary>
    public bool blockRaycasts = false;

    private MOrientation _orientation;
    private Vector2 _targetPosition;
    private Vector3 _targetForwardVec; 
    private bool _isVisible = false;
    private Vector2 _toFollowMapCoord;
    private float _scaleRatio;
    private bool _isScaling;
    private bool _isScalingSmoothly;
    private float _defaultScale;
    private float _smoothScaleSpeed;

    public int DepthIndex { get { return depthIndex; } }
    public MOrientation Orientation { get { return _orientation; } }
    public Vector2 TargetPosition { get { return _targetPosition; } }
    public Vector3 TargetForwardVector { get { return _targetForwardVec; } }
    public bool IsVisible { get { return _isVisible; } }
    public Vector2 ToFollowMapCoord { get { return _toFollowMapCoord; } }
    public float ScaleRatio         { get { return _scaleRatio; } }
    public bool IsScaling { get { return _isScaling; } }
    public bool IsScalingSmoothly   { get { return _isScalingSmoothly; } }
    public float DefaultScale { get { return _defaultScale; } }
    public float SmoothScaleSpeed   { get { return _smoothScaleSpeed; } }

    public delegate void MObjectDestroyedHandler(MObject sender);
    public event MObjectDestroyedHandler mObjectDestroyedHandler;

    /// <summary>
    /// Some variables have to be initialized in Start/Awake to make sure the MObject is displayed properly on the first frame
    /// </summary>
    public void InitObject(float defaultScale, float scaleRatio, bool isScaling, bool isScalingSmoothly, float smoothScaleSpeed)
    {
        this._defaultScale = defaultScale;
        this._scaleRatio = scaleRatio;
        this._isScaling = isScaling;
        this._isScalingSmoothly = isScalingSmoothly;
        this._smoothScaleSpeed = smoothScaleSpeed;
    }

    /// <summary>
    /// Update MObject's variables
    /// </summary>
    public void UpdateObject(MOrientation orientation, Vector2 targetPosition, Vector3 targetForwardVector, bool isVisible, Vector2 toFollowMapCoord, float defaultScale, float scaleRatio, bool isScaling, bool isScalingSmoothly, float smoothScaleSpeed)
    {
        this._orientation = orientation;
        this._targetPosition = targetPosition;
        this._targetForwardVec = targetForwardVector;
        this._isVisible = isVisible;
        this._toFollowMapCoord = toFollowMapCoord;
        this._scaleRatio = scaleRatio;
        this._isScalingSmoothly = isScalingSmoothly;
        this._defaultScale = defaultScale;
        this._smoothScaleSpeed = smoothScaleSpeed;
    }

    /// <summary>
    /// The recommended way to destry the object from itself
    /// </summary>
    public void SelfDestruct()
    {
        if (mObjectDestroyedHandler != null)
            mObjectDestroyedHandler(this);
    }

    /// <summary>
    /// The recommended way to destroy the object from the view
    /// </summary>
    public void DestroyObj()
    {
        //TODO notify and cooperate with the components before destroying (if animation etc.)
        if (!Application.isPlaying)
        {          
            DestroyImmediate(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Whether you want this MObject to block raycasts or let them pass through.
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <param name="eventCamera"></param>
    /// <returns></returns>
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return blockRaycasts;
    }
}
