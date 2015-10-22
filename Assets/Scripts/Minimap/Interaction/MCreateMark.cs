using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Instantiates the mark in the minimap and sets it up
/// </summary>
public class MCreateMark : MonoBehaviour 
{
    /// <summary>
    /// The mark prefab
    /// </summary>
    public MObject markPrefab;

    private MView _view;

    public MView View { get { return _view; } set { _view = value; } }

    public void Start()
    {
        if (_view == null) _view = transform.GetComponentInParentRecursively<MView>();
    }

    public void OnClick(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_view.mapRTrans, eventData.position, eventData.pressEventCamera, out localPoint))
            return;

        //if is not in the mask texture then don't create the mark
        if (!_view.IsWithinMask(localPoint)) return;
        //if nothing to create
        if (markPrefab == null) return;

        GameObject markInst = _view.AddNewObj(null, markPrefab);
        Transform markTrans = markInst.transform;
        markTrans.localEulerAngles = new Vector3(markTrans.localEulerAngles.x, markTrans.localEulerAngles.y, -_view.mapAngle);

        Vector2 mapCoord = _view.RectToMap(localPoint);
        markTrans.localPosition = (Vector3)(mapCoord - _view.currentCenterCoord);
    }

}