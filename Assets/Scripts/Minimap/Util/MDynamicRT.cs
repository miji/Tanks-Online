using UnityEngine;
using System.Collections;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class MDynamicRT : MonoBehaviour
{
    public int colorDepth = 24;
    public int antiAliasing = 2;
    public FilterMode filterMode = FilterMode.Trilinear;

    [SerializeField, HideInInspector]
    private bool _resolutionAsController = true;
    [SerializeField, HideInInspector]
    private Vector2 resolutionOverride = new Vector2(256.0f, 256.0f);
    [SerializeField, HideInInspector]
    private RenderTexture _rt;
    [SerializeField, HideInInspector]
    private bool _hasBeenInitialized = false;

    public bool ResolutionAsController
    {
        get { return _resolutionAsController; }
        set 
        { 
            _resolutionAsController = value;
            RecreateRT();
        }
    }


    public Vector2 ResolutionOverride
    {
        get { return resolutionOverride; }
        set 
        {
            if (resolutionOverride != value)
            {
                resolutionOverride = value;
                RecreateRT();
            }
        }
    }


    public delegate void RTChangedHandler(RenderTexture newRenderTexture);
    public event RTChangedHandler rtChangedHandler;

    private void Start()
    {
        //should be in Start
        if (!_hasBeenInitialized && _rt == null)
        {
            InitializeRT();
        }
    }

    /// <summary>
    /// The default RT initializaiton using the controller size, only used as a backup
    /// </summary>
	private void InitializeRT () 
    {
        MController controller = transform.GetComponentInParentRecursively<MController>();
        if (controller == null) return;
        RectTransform controllerRTrans = controller.GetComponent<RectTransform>();
        if (controllerRTrans == null) return;

        if (_resolutionAsController)
            InitializeRT(Mathf.RoundToInt(controllerRTrans.rect.width), Mathf.RoundToInt(controllerRTrans.rect.height));
        else
            InitializeRT(Mathf.RoundToInt(resolutionOverride.x), Mathf.RoundToInt(resolutionOverride.y));
	}

    /// <summary>
    /// The recommended way to initialize the Dynamic RT Camera
    /// </summary>
    /// <param name="rtWidth"></param>
    /// <param name="rtHeight"></param>
    public void InitializeRT(int rtWidth, int rtHeight)
    {
        _rt = new RenderTexture(rtWidth, rtHeight, colorDepth);

        _rt.useMipMap = false;
        _rt.name = "Dynamically created RT by AB Minimap";

        _rt.antiAliasing = antiAliasing;
        _rt.filterMode = filterMode;
        _hasBeenInitialized = true;

        _rt.DiscardContents();
        _rt.Create();

        GetComponent<Camera>().targetTexture = _rt;
        GetComponent<Camera>().ResetAspect();

        if (rtChangedHandler != null)
            rtChangedHandler(_rt);
    }

    void RecreateRT()
    {
        if (_rt != null)
            DestroyImmediate(_rt);

        InitializeRT();
    }

    void OnDestroy()
    {
        if (_rt != null)
            DestroyImmediate(_rt);
    }
}
