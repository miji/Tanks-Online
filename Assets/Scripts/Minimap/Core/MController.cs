using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// The minimap controller class. Handles the changes in the scenes, input, notifies the model and selects the view.
/// </summary>
[ExecuteInEditMode]
public class MController : MMapBase
{
    private Dictionary<MModel, MView> _modelViewDic = new Dictionary<MModel, MView>();

    //TODO revert back to Awake
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        //get the MMModels
        MModel[] mapModels = GetComponentsInChildren<MModel>(true);
        for (int i = 0; i < mapModels.Length; i++)
        {
            MView mapView = mapModels[i].GetComponent<MView>();
            mapModels[i].InitModel(mapView);
            _modelViewDic.Add(mapModels[i], mapView);
        }
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    /// <summary>
    /// Handle the zoom in action
    /// </summary>
    public void ZoomIn()
    {
        foreach (MView mapView in _modelViewDic.Values) mapView.ZoomIn();
    }

    /// <summary>
    /// Handle the zoom out action
    /// </summary>
    public void ZoomOut()
    {
        foreach (MView mapView in _modelViewDic.Values) mapView.ZoomOut();
    }

    /// <summary>
    /// React to clicks in the map by notifying the model
    /// </summary>
    public virtual void InteractionWithMap(MEventSystem evtSystem, MInteractionType interactionType, BaseEventData e)
    {
        evtSystem.OnInteractionEvent(interactionType, e);
    }
}