using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The minimap view class using a rasterized map
/// </summary>
[ExecuteInEditMode]
[MHelpDesc("The raster view. Useful when you want render a texture as a background.")]
public class MViewRaster : MView
{
    /// <summary>
    /// Information about the "real" map (the scene), the height and the width is the size of the map in game units
    /// </summary>
    public Vector2 mapDimensions = new Vector2(100.0f, 100.0f);

    private Vector2 _targetMapScale;

    protected override void Awake()
    {
        base.Awake();

        //find the mask texture so we can clip later
        //it is not required
        if (maskImg != null)
        {
            maskTex = (Texture2D)maskImg.mainTexture;
        }

        //this component requires the RectTransform component
        if (mapRTrans == null)
        {
            MUtil.Error("Minimap disabled as the rasterized view doesn't have any RectTransform to work with!", this);
            gameObject.SetActive(false);
            return;
        }

        //get the initial scale
        _targetMapScale = CalculateMapScale();
    }

    protected override void Start()
    {
        base.Start();

        //get the initial scale
        _targetMapScale = CalculateMapScale();
    }

    protected override void MapUpdate()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        base.MapUpdate();

        if (mapRTrans == null) return;
        if (mapDimensions.x == 0.0f || mapDimensions.y == 0.0f)
        {
            Debug.LogWarning("Map dimension cannot be 0! Disabling view!", this);
            gameObject.SetActive(false);
            return;
        }
        
        //scale and transform map before objects otherwise the object will stutter
        ScaleMap();
        TransfromMap();
        //calculate the coordiates first to prevent tofollow obj stutter
        if (toFollowTrans != null) currentCenterCoord = WorldToMap(toFollowTrans.position);
        TransformObjects();
    }

    private void ScaleMap()
    {
        //don't scale if not necessary
        if (mapRTrans.sizeDelta == _targetMapScale) return;

        //if playing the project and zooming smoothly then lerp, otherwise force the size
        if (isZoomingSmoothly && Application.isPlaying)
            mapRTrans.sizeDelta = Vector2.Lerp(mapRTrans.sizeDelta, _targetMapScale, MapDeltaTime * smoothZoomSpeed);
        else
            mapRTrans.sizeDelta = _targetMapScale;
    }

    private void TransfromMap()
    {
        if (toFollowTrans == null) return;
        
        //moving the map, it goes in the opposite direction of the target hence the negative values 
        Vector2 targetMapCoord = WorldToMap(toFollowTrans.position);
        mapRTrans.localPosition = new Vector3(-targetMapCoord.x, -targetMapCoord.y, mapRTrans.localPosition.z);
    }

    private void TransformObjects()
    {
        //go through the objects that have an actor and transform them 
        foreach (KeyValuePair<MObject, MActor> objInstance in objInstances)
        {
            if (objInstance.Value != null)
                TransformObjAccordActor(objInstance.Key, objInstance.Value);
        }
    }

    private void TransformObjAccordActor(MObject obj, MActor actor)
    {
        Transform actorTrans = actor.transform;
        //object's map coordinates
        Vector2 actorCoord = WorldToMap(actorTrans.position);
        Vector2 objCoord = actorCoord - currentCenterCoord;

        //calculate the position within the mask and check if it is within the mask
        Rect locRect = rTrans.rect;
        float xPos = (actorCoord.x + locRect.width / 2 - currentCenterCoord.x) / locRect.width;
        float yPos = (actorCoord.y + locRect.height / 2 - currentCenterCoord.y) / locRect.height;

        bool isVisible = false;
        if (xPos <= 1.0f && xPos >= 0.0f && yPos <= 1.0f && yPos >= 0.0f)
            isVisible = IsWithinMaskTex(new Vector2(xPos, yPos));

        obj.UpdateObject(orientation, objCoord, actorTrans.forward, isVisible, currentCenterCoord, mObjectDefaultSize, CalculateZoomScaleRatio(), isScalingMObjects, isZoomingSmoothly, smoothZoomSpeed);
    }

    /// <summary>
    /// Call whenever you want to update the raster (e.g. after zooming or changing an inspector value)
    /// </summary>
    public void UpdateRasterSize()
    {
        _targetMapScale = CalculateMapScale();
    }

    /// <summary>
    /// Perform the zoom in on the rasterized map
    /// </summary>
    public override void ZoomIn()
    {
        base.ZoomIn();
        UpdateRasterSize();
    }

    /// <summary>
    /// Perform the zoom out on the rasterized map
    /// </summary>
    public override void ZoomOut()
    {
        base.ZoomOut();
        UpdateRasterSize();
    }

    //Calculates the correct texture size according to the current zoom value
    private Vector2 CalculateMapScale()
    {
        //convert the map scale to the ortographic camera alternative
        //ortographic camera => 1 = 2x2 in game units; 60 => 120x120 in game units etc.

        //the scale of the minimap in UI (e.g. 200x200 pixels)
        //rtrans can be null in some in cases in editor (it shouldn't as this is ExecuteInEditMode but still happens)
        float newWidth = 0, newHeight = 0;
        if (rTrans != null)
        {
            newWidth = (rTrans.rect.width / mapDimensions.x) * (mapDimensions.x / 2 / zoomValue) * mapDimensions.x;
            newHeight = (rTrans.rect.height / mapDimensions.y) * (mapDimensions.y / 2 / zoomValue) * mapDimensions.y;
        }
        return new Vector2 (newWidth, newHeight);
    }

    public override bool IsWithinMask(Vector2 offset)
    {
        //as the pivot is x=0.5/y=0.5, we need to add half of the size before continuing
        offset = new Vector2(offset.x + maskImg.rectTransform.rect.width / 2 + mapRTrans.localPosition.x,
            offset.y + maskImg.rectTransform.rect.height / 2 + mapRTrans.localPosition.y);
        return base.IsWithinMask(offset);
    }

    public override Vector2 RectToMap(Vector2 rectCoord)
    {
        return rectCoord;
    }

    /// <summary>
    /// Converts the world to map coordinates, only works for raster map
    /// </summary>
    /// <param name="worldCoord">The world coordinates</param>
    /// <returns>The map coordinates</returns>
    public override Vector2 WorldToMap(Vector3 worldCoord)
    {
        //map origin is used to offset the map origin
        float mapCoordX = 0, mapCoordY = 0;
        switch (orientation)
        {
            case MOrientation.XZ:
                mapCoordX = (worldCoord.x - mapOrigin.x) / mapDimensions.x * mapRTrans.rect.width;
                mapCoordY = (worldCoord.z - mapOrigin.z) / mapDimensions.y * mapRTrans.rect.height;
                break;
            case MOrientation.YZ:
                mapCoordX = (worldCoord.z - mapOrigin.z) / mapDimensions.x * mapRTrans.rect.width;
                mapCoordY = (worldCoord.y - mapOrigin.y) / mapDimensions.y * mapRTrans.rect.height;
                break;
            default:
                MUtil.Error("Unknown orientation is used here!");
                break;
        }
        return new Vector2(mapCoordX, mapCoordY);
    }
}
