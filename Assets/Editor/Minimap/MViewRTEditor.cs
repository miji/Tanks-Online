using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(MViewRT))]
public class MViewRTEditor : MViewEditor
{
    private MViewRT _mViewRt;

    protected override void Awake()
    {
        if (_mViewRt == null)
            this._mViewRt = (MViewRT)target;

        base.Awake();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        Object camera = MUtilEditor.ObjectFieldColored("RenderTexture camera", _mViewRt.MapCamera, typeof(Camera), true);
        Object maskImg = MUtilEditor.ObjectFieldColored("Mask image", _mViewRt.maskImg, typeof(Image), true);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mViewRt, _mViewRt.name + " changed");
            _mViewRt.MapCamera = (Camera)camera;
            _mViewRt.maskImg = (Image)maskImg;
            EditorUtility.SetDirty(target);
        }
    }
}
