using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(MViewRaster))]
public class MViewRasterEditor : MViewEditor
{
    private MViewRaster _mViewRaster;

    private const float HandleSizeDiv = 15.0f;

    protected override void Awake()
    {
        this._mViewRaster = (MViewRaster)target;
        base.Awake();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        MUtilEditor.DrawHrLine();
        Vector3 newMapOrigin    = EditorGUILayout.Vector3Field("Map origin", _mViewRaster.mapOrigin);
        Vector2 newMapSize = EditorGUILayout.Vector2Field("Map size", _mViewRaster.mapDimensions);
        Object maskImg = MUtilEditor.ObjectFieldColored("Mask image", _mViewRaster.maskImg, typeof(Image), true);

        if (!Application.isPlaying)
            _mViewRaster.UpdateRasterSize();

        if (EditorGUI.EndChangeCheck()) 
        {
            Undo.RecordObject(_mViewRaster, _mViewRaster.name + " changed"); 
            _mViewRaster.mapOrigin = newMapOrigin;
            _mViewRaster.mapDimensions = newMapSize;
            _mViewRaster.maskImg = (Image)maskImg;
            EditorUtility.SetDirty(target);
        }
    }

    public override void OnSceneGUI()
    {
        base.OnSceneGUI();

        DrawMapSizeHandles();
    }

    public void DrawMapSizeHandles()
    {
        Vector3[] points = new Vector3[4];
        Vector2 mapSize = _mViewRaster.mapDimensions;
        Vector3 mapOrigin = _mViewRaster.mapOrigin;

        if (_mViewRaster.orientation == MOrientation.XZ)
        {
            points[0] = new Vector3(-mapSize.x / 2, 0.0f, mapSize.y / 2) + mapOrigin;
            points[1] = new Vector3(mapSize.x / 2, 0.0f, mapSize.y / 2) + mapOrigin;
            points[2] = new Vector3(mapSize.x / 2, 0.0f, -mapSize.y / 2) + mapOrigin;
            points[3] = new Vector3(-mapSize.x / 2, 0.0f, -mapSize.y / 2) + mapOrigin;
        }
        else if (_mViewRaster.orientation == MOrientation.YZ)
        {
            points[0] = new Vector3(0.0f, mapSize.y / 2, -mapSize.x / 2) + mapOrigin;
            points[1] = new Vector3(0.0f, mapSize.y / 2, mapSize.x / 2) + mapOrigin;
            points[2] = new Vector3(0.0f, -mapSize.y / 2, mapSize.x / 2) + mapOrigin;
            points[3] = new Vector3(0.0f, -mapSize.y / 2, -mapSize.x / 2) + mapOrigin;
        }
        else { MUtil.Error("Unknown orientation is used here!"); }

        Handles.color = Color.red;
        Handles.DrawLine(points[0], points[1]);
        Handles.DrawLine(points[0], points[3]);
        Handles.DrawLine(points[1], points[2]);
        Handles.DrawLine(points[2], points[3]);

        //corner handles (scale)
        for (int i = 0; i < points.Length; i++)
        {

            EditorGUI.BeginChangeCheck();
            Vector3 newScale = Handles.FreeMoveHandle(points[i], Quaternion.identity, HandleUtility.GetHandleSize(points[i]) / HandleSizeDiv, points[i],
                Handles.SphereCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_mViewRaster, "Map changed");
                EditorUtility.SetDirty(target);
            }

            float axis1Diff = 0, axis2Diff = 0;
            if (_mViewRaster.orientation == MOrientation.XZ)
            {
                axis1Diff = Mathf.Abs(newScale.x) - Mathf.Abs(points[i].x);
                axis2Diff = Mathf.Abs(newScale.z) - Mathf.Abs(points[i].z);
            }
            else if (_mViewRaster.orientation == MOrientation.YZ)
            {
                axis1Diff = Mathf.Abs(newScale.z) - Mathf.Abs(points[i].z);
                axis2Diff = Mathf.Abs(newScale.y) - Mathf.Abs(points[i].y);
            }
            else { MUtil.Error("Unknown orientation is used here!"); }

            mapSize.x += axis1Diff;
            mapSize.y += axis2Diff;
        }

        //central handle (move)
        float handleSizeMove = HandleUtility.GetHandleSize(mapOrigin) / HandleSizeDiv;
        EditorGUI.BeginChangeCheck();
        Vector3 newOrigin = Handles.FreeMoveHandle(mapOrigin, Quaternion.identity, handleSizeMove * 2, mapOrigin,
              Handles.CircleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mViewRaster, "Map changed");
            _mViewRaster.mapOrigin = newOrigin;
            EditorUtility.SetDirty(target);
        }
        //cross
        Handles.DrawLine(mapOrigin, mapOrigin + Vector3.up * handleSizeMove);
        Handles.DrawLine(mapOrigin, mapOrigin + -Vector3.up * handleSizeMove);
        Handles.DrawLine(mapOrigin, mapOrigin + Vector3.right * handleSizeMove);
        Handles.DrawLine(mapOrigin, mapOrigin + -Vector3.right * handleSizeMove);
        _mViewRaster.mapDimensions = mapSize;
    }
}