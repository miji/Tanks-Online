using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The minimap model class that contains the list of actors to render
/// </summary>
[ExecuteInEditMode]
[MHelpDesc("Model with Actors and Target: More complex model that follows the target if target assigned and keeps track of all actors within the scene. Helpful if you want to track all actors in the level.")]
public class MModelWActors : MModel
{
    //actors associated with this model
    private readonly List<MActor> _actors = new List<MActor>();

    protected override void Awake()
    {
        if (!MUtil.ExecuteInEditMode()) return;

        base.Awake();
        //listen to changes on actors in the scene
        MActor.ActorCreated += MMActor_MMActorCreated;
        MActor.ActorDestroyed += MMActor_MMActorDestroyed;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //deregister the delegates
        MActor.ActorCreated -= MMActor_MMActorCreated;
        MActor.ActorCreated -= MMActor_MMActorDestroyed;
    }

    /// <summary>
    /// A new actor has been added to the scene
    /// </summary>
    /// <param name="newActor">New actor</param>
    private void MMActor_MMActorCreated(MActor newActor)
    {
        //only create objects when actually running the game otherwise they would stack up in the scene
        if (!Application.isPlaying) return;
       
        _actors.Add(newActor);

        if (mapView != null)
            mapView.AddNewObj(newActor, newActor.ObjPrefab);
    }

    /// <summary>
    /// An actor has been removed from the scene
    /// </summary>
    /// <param name="destroyedActor">Actor to remove</param>
    private void MMActor_MMActorDestroyed(MActor destroyedActor)
    {
        _actors.Remove(destroyedActor);

        if (mapView != null)
            mapView.RemoveActor(destroyedActor);
    }
}
