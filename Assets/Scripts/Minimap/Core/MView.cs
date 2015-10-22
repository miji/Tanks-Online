using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The minimap view class. Handles the visualization and contains the data related to visualizing the information.
/// </summary>
//executed in edit mode to make sure it is initialized
[ExecuteInEditMode]
[MHelpDesc("The basic view. Only useful if you want to react to zoom etc. but don't want to render any map.")]
public class MView : MMapBase
{
    /// <summary>
    /// The map orientation in use
    /// </summary>
    public MOrientation orientation = MOrientation.YZ;
    /// <summary>
    /// The zoom value that we are using now
    /// </summary>
    public float zoomValue = 10.0f;
    /// <summary>
    /// The step at which the zoom changes
    /// </summary>
    public float zoomStep = 2.0f;
    /// <summary>
    /// The largest magnification value
    /// </summary>
    public float zoomInLimit = 4.0f;
    /// <summary>
    /// The largest reduction value
    /// </summary>
    public float zoomOutLimit = 20.0f;
    /// <summary>
    /// Do we lerp the zoom?
    /// </summary>
    public bool isZoomingSmoothly = true;
    /// <summary>
    /// The higher value, the faster transition from one zoom state to another. Lerp multiplier.
    /// </summary>
    public float smoothZoomSpeed = 5.0f;
    /// <summary>
    /// Do we scale pins while zooming?
    /// </summary>
    public bool isScalingMObjects = true;
    /// <summary>
    /// The default size of the objects. Change this if you want to share objects between different views and only want to change the size of them, then you can keep the same prefab
    /// </summary>
    public float mObjectDefaultSize = 1.0f;
    /// <summary>
    /// Is rotating the map according to the forward vector of the actor that is being followed
    /// </summary>
    public bool isRotatingMap = false;
    /// <summary>
    /// Current map angle (only if is rotating map)
    /// </summary>
    public float mapAngle = 0;
    /// <summary>
    /// Current map center coordinates in map coordinates (different for different views etc.)
    /// </summary>
    public Vector2 currentCenterCoord;
    /// <summary>
    /// Pivot RectTransform reference (rotates if map is rotating etc.). Looks up automatically in the hierarchy based on the name.
    /// </summary>
    public RectTransform pivotRTrans;
    /// <summary>
    /// Map RectTransform reference (the map visualization GO). Looks up automatically in the hierarchy based on the name.
    /// </summary>
    public RectTransform mapRTrans;
    /// <summary>
    /// The mask image. Looks up automatically in the hierarchy based on the name.
    /// </summary>
    public Image maskImg;
    /// <summary>
    /// The center of the map in world coordinates (is used to offset the pins to the correct location)
    /// </summary>
    public Vector3 mapOrigin;

    //the local reference to the objects
    protected Dictionary<MObject, MActor> objInstances = new Dictionary<MObject, MActor>();

    protected Transform toFollowTrans;
    protected RectTransform rTrans;
    //the mask texture
    protected Texture2D maskTex;

    protected float zoomOriginalValue;

    protected override void Awake()
    {
        base.Awake();

        rTrans = GetComponent<RectTransform>();
        zoomOriginalValue = zoomValue;
        
        //find the mask texture so we can clip later (not required)
        if (maskImg != null)
        {
            maskTex = (Texture2D)maskImg.mainTexture;
        }

#if UNITY_EDITOR
        UpdateReferences();
#endif
    }

    protected override void Start()
    {
        base.Start();

        //needed if executing in edit mode in the editor
#if UNITY_EDITOR
        UpdateReferences();
#endif
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach(MObject mObj in objInstances.Keys) 
        {
            mObj.mObjectDestroyedHandler -= mObj_mObjectDestroyedHandler;
        }
    }

    protected override void MapUpdate()
    {
        base.MapUpdate();

        if (!MUtil.ExecuteInEditMode()) return;

        if (isRotatingMap && toFollowTrans != null)
        {
            float newAngle = MMapTools.CalculateAngle(orientation, toFollowTrans.forward);
            if (newAngle != mapAngle)
            {
                mapAngle = newAngle;
                pivotRTrans.eulerAngles = new Vector3(pivotRTrans.eulerAngles.x, pivotRTrans.eulerAngles.y, mapAngle);
            }
        }
    }

    public virtual void UpdateReferences()
    {
        RectTransform[] childRTrans = GetComponentsInChildren<RectTransform>(true);

        foreach (RectTransform childRTran in childRTrans)
        {
            switch (childRTran.name)
            {
                case MVal.MapGoName:
                    if (mapRTrans == null)
                        mapRTrans = childRTran;
                    break;

                case MVal.PivotGoName:
                    if (pivotRTrans == null)
                        pivotRTrans = childRTran;
                    break;

                case MVal.MaskGoName:
                    if (maskImg == null)
                        maskImg = childRTran.GetComponent<Image>();                   
                    break;
            }
        }

        if (maskImg == null)
        {
            Mask parentMask = transform.GetComponentInParentRecursively<Mask>(MVal.MaskGoName);

            if (parentMask != null)
                maskImg = parentMask.GetComponent<Image>();
        }
    }
     
    /// <summary>
    /// Initialize this view
    /// </summary>
    /// <param name="toFollowTransPar">The target to follow</param>
    public virtual void InitView(Transform toFollowTransPar) 
    {
        this.toFollowTrans = toFollowTransPar;
    }

    /// <summary>
    /// Add new pin on this view
    /// </summary>
    public GameObject AddNewObj(MActor newActor, MObject prefab)
    {
        GameObject newObjGo = InstantiateObject(prefab);
        MObject mObj = MUtil.GetComponentInChildren<MObject>(newObjGo);
        if (mObj == null) MUtil.Error("mObj is null!", this);

        objInstances.Add(mObj, newActor);
        mObj.mObjectDestroyedHandler += mObj_mObjectDestroyedHandler;
        return newObjGo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    private void mObj_mObjectDestroyedHandler(MObject sender)
    {
        RemoveObj(sender);
    }

    public void RemoveObj(MObject obj)
    {
        obj.DestroyObj();
        objInstances.Remove(obj);
    }

    /// <summary>
    /// Instantiates the MObject, parents it and names it
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns>The instnatiated object reference</returns>
    private GameObject InstantiateObject(MObject prefab)
    {
        RectTransform parent = pivotRTrans ?? rTrans;
        
        //instantiate new object, rename it
        RectTransform newMmObj = (RectTransform)Instantiate(prefab.transform,
            new Vector3(parent.position.x, parent.position.y, parent.position.z), parent.rotation);
        newMmObj.name += "_" + prefab.transform.name + "_" + System.DateTime.Now.Second;

        MObject mObj = newMmObj.GetComponentInChildren<MObject>();
        RectTransform depthFolder = (RectTransform)parent.FindChild(mObj.DepthIndex.ToString());

        if (depthFolder == null)
        {
            depthFolder = MMapTools.CreateEmptyRTransGo(mObj.DepthIndex.ToString(), parent);

            //you can't edit the sibling index in foreach loop directly as that changes order and thus wouldn't work
            //it has to be saved in an arbitrary list and then processed
            List<Transform> depthFolders = MUtil.GetDirectChildren(parent);
            depthFolders.Sort(CompareTransformNames);

            foreach (Transform depthTrans in depthFolders)
            {
                int result;
                if (System.Int32.TryParse(depthTrans.name, out result))
                {
                    depthTrans.SetSiblingIndex(result);
                }
            }
        }

        mObj.transform.SetParent(depthFolder, true);
        mObj.InitObject(mObjectDefaultSize, CalculateZoomScaleRatio(), isScalingMObjects, isZoomingSmoothly, smoothZoomSpeed);

        return newMmObj.gameObject;
    }

    /// <summary>
    /// Compare transform a and b alphabetically
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>System.Comparison<T> integer</returns>
    private static int CompareTransformNames(Transform a, Transform b)
    {
        return a.name.CompareTo(b.name);
    }

    /// <summary>
    /// Remove an existing pin from this view
    /// </summary>
    /// <param name="mActor">The actor whose pin to remove</param>
    public virtual void RemoveActor(MActor mActor)
    {
        //has to be casted to Array otherwise wouldn't work
        var objsToRemove = objInstances.Where(kvpObjAct => kvpObjAct.Value == mActor).ToArray();
        foreach (var objToRemove in objsToRemove)
        {
            RemoveObj(objToRemove.Key);
        }
    }

    /// <summary>
    /// Peform zoom in action
    /// </summary>
    public virtual void ZoomIn() 
    {
        zoomValue -= zoomStep;
        if (zoomValue < zoomInLimit) zoomValue = zoomInLimit;
    }
    
    /// <summary>
    /// Perfrom zoom out action
    /// </summary>
    public virtual void ZoomOut() 
    {
        zoomValue += zoomStep;
        if (zoomValue > zoomOutLimit) zoomValue = zoomOutLimit;
    }

    /// <summary>
    /// Calculate the zoom scale ratio (no zoom change = 1.0f)
    /// </summary>
    /// <returns>The zoom scale ratio</returns>
    protected float CalculateZoomScaleRatio()
    {
        return zoomOriginalValue / zoomValue;
    }

    /// <summary>
    /// Converts the world coords to map coordinates
    /// </summary>
    /// <param name="worldCoord">The world coordinates</param>
    /// <returns>The map coordinates</returns>
    public virtual Vector2 WorldToMap(Vector3 worldCoord) 
    {
        return Vector3.zero;
    }
    
    /// <summary>
    /// Map rectangle to map coordinates
    /// </summary>
    /// <param name="rectCoord">Coordinates within the map rectangle</param>
    /// <returns>Map coordinates</returns>
    public virtual Vector2 RectToMap(Vector2 rectCoord)
    {
        return Vector2.zero;
    }

    /// <summary>
    /// Is the specific position within mask not transparent?
    /// </summary>
    /// <param name="offset">The offset from the pivot point set to x=0.0 and y=0.0 (top left) (not 0.5/0.5)</param>
    /// <returns>True if has color in the pixel specified by map offset</returns>
    public virtual bool IsWithinMask(Vector2 offset)
    {
        if (maskImg == null) return true;

        Vector2 normTexCoord = offset / maskImg.rectTransform.rect.width;
        return IsWithinMaskTex(normTexCoord);
    }

    //calculate the actor's position within the mask and check if it is in the visible area
    //by checking the mask's alpha value
    //it works both for RT and Raster because in both cases the mask copies the size of the view as it should
    protected bool IsWithinMaskTex(Vector2 normTexCoord)
    {
        //mask texture is null so that means there is no mask restriction => true
        if (maskTex == null) return true;

        //no need to check if outside of texture coordinates
        if (normTexCoord.x > 0.0f && normTexCoord.x <= 1.0f && normTexCoord.y > 0.0f && normTexCoord.y <= 1.0f)
        {
            int x = Mathf.FloorToInt(normTexCoord.x * maskTex.width);
            int y = Mathf.FloorToInt(normTexCoord.y * maskTex.height);
            return maskTex.GetPixel(x, y).a != 0.0f;
        }
        else
        {
            return false;
        }
    }
}
