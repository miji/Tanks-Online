using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

public class MCreateMinimapWindow : EditorWindow
{
    public string minimapName = "New minimap";
    public Vector2 minimapSize = new Vector2(200.0f, 200.0f);
    public RenderMode renderMode;
    public MOrientation orientation;
    public Transform target;
    public string modelScriptName = noValue;
    public string viewScriptName = noValue;

    public Sprite maskSprite;

    public Sprite viewRasterSprite;
    public Vector2 mapDimensions;

    private Vector2 _scrollPos;
    private const string noValue = "None";

    [MenuItem("Window/AB Minimap/Create new minimap")]
    private static void Open()
    {
        // Get existing open window or if none, make a new one:
        MCreateMinimapWindow window = (MCreateMinimapWindow)EditorWindow.GetWindow(typeof(MCreateMinimapWindow), false, "Create new minimap");
        window.Show();
    }

    private void OnGUI()
    {
        bool isFormValid = true;
        Color origColor = GUI.color;
        EditorGUILayout.BeginVertical();
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);

        EditorGUILayout.HelpBox("You can create a new simple minimap that you can work with further", MessageType.None, true);
        MUtilEditor.DrawHrLine();

        minimapName = EditorGUILayout.TextField("Minimap name", minimapName);
        MUtilEditor.DrawHrLine();

        minimapSize = MUtilEditor.Vector2FieldColored("Minimap size in px", minimapSize, 0, 0);
        if (minimapSize.x <= 0 || minimapSize.y <= 0) isFormValid = false;
        MUtilEditor.DrawHrLine();

        renderMode = (RenderMode) EditorGUILayout.EnumPopup("Canvas render mode", renderMode);
        MUtilEditor.DrawHrLine();

        orientation = (MOrientation)EditorGUILayout.EnumPopup("Map orientation", orientation);
        MUtilEditor.DrawHrLine();

        target = (Transform)EditorGUILayout.ObjectField("Target to follow", target, typeof(Transform), true);
        EditorGUILayout.HelpBox("The object you want to have in the center of the map (the player for example). Leave blank if you want a static map.", MessageType.None);
        MUtilEditor.DrawHrLine();

        #region view
        //TODO optimize
        List<MonoScript> views = MUtilEditor.GetAllScriptsAssignableFrom<MView>();
        string[] implicitViewOptions = new string[] { noValue };
        List<string> viewNames = implicitViewOptions.ToList<string>();
        views.ForEach(x => viewNames.Add(x.GetClass().Name));

        int selectedView = 0;
        for (int i = 0; i < viewNames.Count; i++)
        {
            if (viewNames[i] == viewScriptName) selectedView = i;
        }

        viewScriptName = viewNames[EditorGUILayout.Popup("View to assign", selectedView, viewNames.ToArray())];

        //help box
        //There is 1 implicit option thus -1
        int viewIndex = selectedView - implicitViewOptions.Length;
        MHelpDesc mHelpDescView = null;
        if (viewIndex >= 0)
            mHelpDescView = (MHelpDesc)System.Attribute.GetCustomAttribute(views[viewIndex].GetClass(), typeof(MHelpDesc));

        string viewDescription = string.Empty;
        if (selectedView == 0) viewDescription = "No view is assigned.";
        if (mHelpDescView != null) viewDescription = mHelpDescView.Description;
        if (viewDescription != string.Empty) EditorGUILayout.HelpBox(viewDescription, MessageType.None, true);

        switch (viewScriptName)
        {
            case "MViewRaster":
                viewRasterSprite = (Sprite)MUtilEditor.ObjectFieldColored("Background sprite", viewRasterSprite, typeof(Sprite), false);
                if (viewRasterSprite == null) isFormValid = false;
                mapDimensions = MUtilEditor.Vector2FieldColored("Map dimensions", mapDimensions, 0, 0);
                if (mapDimensions.x <= 0 || mapDimensions.y <= 0) isFormValid = false;
                maskSprite = (Sprite)EditorGUILayout.ObjectField("Optional mask image", maskSprite, typeof(Sprite), false);
                break;

            case "MViewRT":
                maskSprite = (Sprite)EditorGUILayout.ObjectField("Optional mask image", maskSprite, typeof(Sprite), false);
                break;
        }
        MUtilEditor.DrawHrLine();
        #endregion

        #region model
        //TODO optimize
        if (selectedView > 0)
        {
            List<MonoScript> models = MUtilEditor.GetAllScriptsAssignableFrom<MModel>();
            string[] implicitOptions = new string[] { noValue };
            List<string> modelNames = implicitOptions.ToList<string>();
            models.ForEach(x => modelNames.Add(x.GetClass().Name));

            int selected = 0;
            for (int i = 0; i < modelNames.Count; i++)
            {
                if (modelNames[i] == modelScriptName) selected = i;
            }

            modelScriptName = modelNames[EditorGUILayout.Popup("Model to assign", selected, modelNames.ToArray())];

            //help box
            //There is 1 implicit option thus -1
            int modelIndex = selected - implicitOptions.Length;
            MHelpDesc mHelpDesc = null;
            if (modelIndex >= 0)
                mHelpDesc = (MHelpDesc)System.Attribute.GetCustomAttribute(models[modelIndex].GetClass(), typeof(MHelpDesc));

            string description = string.Empty;
            if (selected == 0) description = "No model is assigned. Useful if you want a passive view (e.g. no tracking of actors or target but only to zoom) or no view.";
            if (mHelpDesc != null) description = mHelpDesc.Description;
            if (description != string.Empty) EditorGUILayout.HelpBox(description, MessageType.None, true);
            MUtilEditor.DrawHrLine();
        }
        #endregion

        GUI.color = isFormValid ? Color.green : MUtilEditor.WarningColor;
        GUI.enabled = isFormValid;
        if (GUILayout.Button("Create new minimap") && isFormValid)
        {
            OnMinimapCreate();
        }
        GUI.enabled = true;
        GUI.color = origColor;

        EditorUtility.SetDirty(this);

        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        GUI.color = origColor;
    }

    private void OnMinimapCreate()
    {
        GameObject emptyMinimapPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(MVal.CompEmptyMinimap, typeof(GameObject));
        GameObject minimapInst = (GameObject)Instantiate(emptyMinimapPrefab);
        minimapInst.name = minimapName;

        MController controller = minimapInst.GetComponentInChildren<MController>();
        RectTransform controllerRTrans = controller.GetComponent<RectTransform>();
        controllerRTrans.sizeDelta = minimapSize;
        controllerRTrans.anchoredPosition = new Vector2(minimapSize.x / 2, - minimapSize.y / 2);

        if (viewScriptName != noValue)
        {
            RectTransform maskTrans = null;
            if (maskSprite != null)
            {
                maskTrans = MMapTools.CreateEmptyRTransGo(MVal.MaskGoName, controller.transform);
                Image maskImg = maskTrans.gameObject.AddComponent<Image>();
                maskTrans.gameObject.AddComponent<Mask>();
                maskImg.sprite = maskSprite;
            }

            RectTransform viewTrans = MMapTools.CreateEmptyRTransGo("View", maskTrans == null ? controller.transform : maskTrans);
            RectTransform pivotTrans = MMapTools.CreateEmptyRTransGo(MVal.PivotGoName, viewTrans);
            RectTransform mapTrans = MMapTools.CreateEmptyRTransGo(MVal.MapGoName, pivotTrans);
			#pragma warning disable 0618
            Component viewComp = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(viewTrans.gameObject, "Assets/Editor/Minimap/Util/MCreateMinimap.cs (180,34)", viewScriptName);

            MView mView = (MView)viewComp;
            mView.orientation = orientation;
            mView.hasUpdateOverride = true;
            mView.updateType = MUpdateType.Update;

            switch (viewScriptName)
            {
                    //TODO translate class names to string
                case "MViewRaster":
                    Image mapImage = mapTrans.gameObject.AddComponent<Image>();
                    mapImage.sprite = viewRasterSprite;

                    MViewRaster mViewRaster = (MViewRaster)viewComp;
                    mViewRaster.mapDimensions = mapDimensions;
                    break;

                case "MViewRT":
                    mapTrans.gameObject.AddComponent<RawImage>();

                    GameObject rtCamPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(MVal.CompRTCam, typeof(GameObject));
                    GameObject rtCam = (GameObject)Instantiate(rtCamPrefab);
                    Transform rtCamTrans = rtCam.transform;
                    rtCamTrans.parent = controller.transform;
                    rtCam.name = rtCamPrefab.name;
                    
                    MDynamicRT mDynamicRT = rtCam.GetComponent<MDynamicRT>();
                    if (mDynamicRT != null)
                        mDynamicRT.InitializeRT(Mathf.RoundToInt(minimapSize.x), Mathf.RoundToInt(minimapSize.y));

                    MViewRT mViewRt = (MViewRT)viewComp;
                    mViewRt.MapCamera = rtCam.GetComponent<Camera>();

                    //rotate the camera accordingly
                    if (orientation == MOrientation.XZ) 
                    {
                        Vector3 euler = rtCamTrans.eulerAngles;
                        euler.x += 90;
                        rtCamTrans.eulerAngles = euler;
                    }
                    break;
            }

            if (modelScriptName != noValue)
            {
                Component modelComp = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(viewTrans.gameObject, "Assets/Editor/Minimap/Util/MCreateMinimap.cs (226,39)", modelScriptName);
                MModel model = (MModel) modelComp;
                model.toFollowTrans = target;
            }

            //initialize the controller so we can see the result in the editor immediately
            controller.Init();
        }
    }
}