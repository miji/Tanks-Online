using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// The minimap model class. Contains the data that are not related to visualization
/// This is an emmpy model that only references a view. It can be used for static map with no dynamic elements
/// </summary>
[ExecuteInEditMode]
[MHelpDesc("Model with Target: Basic model that only follows the target if target assigned. Helpful if you want to move or rotate according to the target but don't want other actors.")]
public class MModel : MMapBase 
{
    /// <summary>
    /// Target to follow, not setting the target results in a static map
    /// </summary>
    public Transform toFollowTrans;

    //the associated view, might be null if no view is associated
    protected MView mapView;

    public void InitModel(MView mapView)
    {
        this.mapView = mapView;

        //pass the target to the view
        if (mapView != null)
            mapView.InitView(toFollowTrans);
    }
}
