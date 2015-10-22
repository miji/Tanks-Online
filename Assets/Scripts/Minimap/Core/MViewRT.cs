using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// The minimap view class using a RenderTexture
/// </summary>
[ExecuteInEditMode]
[MHelpDesc("The RenderTexture view. Useful when you want render the background through a camera with RenderTexture. You can also use image effects on the camera.")]
public class MViewRT : MView 
{
    //the camera rendering RenderTexture
    [SerializeField]
    private Camera _mapCamera;
    [SerializeField]
    private MDynamicRT mDynamicRT;
    [SerializeField]
    private RawImage map;
    //how far is the camera from the target
    private const float DistFromTarget = 200.0f;

    public Camera MapCamera
    {
        get { return _mapCamera; }
        set
        {
            _mapCamera = value;

            mapOrigin = _mapCamera.transform.position;
            _mapCamera.orthographicSize = zoomValue;

            mDynamicRT = _mapCamera.GetComponent<MDynamicRT>();
            if (mDynamicRT != null)
                mDynamicRT.rtChangedHandler += mDynamicRT_rtChangedHandler;
        }   
    }

    protected override void Awake()
    {
        base.Awake();

        //force the correct zoom so we don't start the level by zooming in/out
        if (_mapCamera == null) return;

        mapOrigin = _mapCamera.transform.position;
        _mapCamera.orthographicSize = zoomValue;
    }

    protected override void Start()
    {
        base.Start();

        map = mapRTrans.GetComponent<RawImage>();

        if (map != null && map.texture == null)
            map.texture = _mapCamera.targetTexture;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        mDynamicRT = _mapCamera.GetComponent<MDynamicRT>();
        if (mDynamicRT != null)
            mDynamicRT.rtChangedHandler -= mDynamicRT_rtChangedHandler;
    }

    void mDynamicRT_rtChangedHandler(RenderTexture newRenderTexture)
    {
        if (map != null)
            map.texture = _mapCamera.targetTexture;
    }

    protected override void MapUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        base.MapUpdate();

        //can be null in the editor for example
        if (_mapCamera == null) return;
        ScaleCamera();
        TransformCamera();
        //calculate the coordiates first to prevent tofollow obj stutter
        if (toFollowTrans != null) currentCenterCoord = WorldToMap(toFollowTrans.position);
        TransformObjects();
    }

    private void ScaleCamera()
    {
        //change the zoom on the camera if it has been changed or if lerping
        if (_mapCamera.orthographicSize != zoomValue)
        {
            if (isZoomingSmoothly && Application.isPlaying)
                _mapCamera.orthographicSize = Mathf.Lerp(_mapCamera.orthographicSize, zoomValue, MapDeltaTime * smoothZoomSpeed);
            else
                _mapCamera.orthographicSize = zoomValue;
        }
    }

    private void TransformCamera()
    {
        //move the camera according to the target if exists
        if (toFollowTrans == null) return;

        switch (orientation)
        {
            case MOrientation.XZ:
                _mapCamera.transform.position = new Vector3(toFollowTrans.position.x, toFollowTrans.position.y + DistFromTarget, toFollowTrans.position.z);
                _mapCamera.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                break;
            case MOrientation.YZ:
                _mapCamera.transform.position = new Vector3(toFollowTrans.position.x + DistFromTarget, toFollowTrans.position.y, toFollowTrans.position.z);
                _mapCamera.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                break;
            default: 
                MUtil.Error("Unknown orientation is used here!");
                break;
        }
    }

    private void TransformObjects()
    {
        //TODO garbage collection triggered here
        //transform the objects this frame
        foreach (KeyValuePair<MObject, MActor> objInstance in objInstances)
        {
            if (objInstance.Value != null)
                TransformObjAccordActor(objInstance.Key, objInstance.Value);
        }
    }

    private void TransformObjAccordActor(MObject obj, MActor actor)
    {
        Transform actorTrans = actor.transform;

        //get the position of the actor within the world
        Vector2 actorVpPos = WorldToMap(actorTrans.position);
        //and it's map coordinates
        Vector2 objCoord = actorVpPos - currentCenterCoord;

        //if mask either doesn't exist or exists and the actor is in the visible area of the mask
        //also cut out actors outside of visible area of the view
        float xPos = (actorVpPos.x + rTrans.rect.width / 2 - currentCenterCoord.x) / rTrans.rect.width;
        float yPos = (actorVpPos.y + rTrans.rect.height / 2 - currentCenterCoord.y) / rTrans.rect.height;
       
        bool isVisible = false;
        if (xPos <= 1.0f && xPos >= 0.0f && yPos <= 1.0f && yPos >= 0.0f)
            isVisible = IsWithinMaskTex(new Vector2(xPos, yPos));
        
        obj.UpdateObject(orientation, objCoord, actorTrans.forward, isVisible, currentCenterCoord, mObjectDefaultSize, CalculateZoomScaleRatio(), isScalingMObjects, isZoomingSmoothly, smoothZoomSpeed);
    }

    public override Vector2 WorldToMap(Vector3 worldCoord)
    {
        //get the position of the actor within the world
        Vector3 vpPos = _mapCamera.WorldToViewportPoint(worldCoord) - _mapCamera.WorldToViewportPoint(mapOrigin);
        //and it's map coordinates
        Vector2 objCoord = new Vector2(vpPos.x * rTrans.rect.width, vpPos.y * rTrans.rect.height);
        return objCoord;
    }

    public override Vector2 RectToMap(Vector2 rectCoord)
    {
        //TODO replace rtrans with RT size?
        float xPos = (rectCoord.x - rTrans.rect.width / 2 + currentCenterCoord.x);
        float yPos = (rectCoord.y - rTrans.rect.height / 2 + currentCenterCoord.y);
        return new Vector2(xPos, yPos);
    }
} 
