using UnityEngine;
using System.Collections;

/// <summary>
/// The Actor class. Assign the component on the in-world actor that you want to represent in the minimap
/// </summary>
[ExecuteInEditMode]
public class MActor : MonoBehaviour
{
    /// <summary>
    /// The minimap pin prefab you want to associate with this actor
    /// </summary>
    public MObject objPrefab;
    public MObject ObjPrefab { get { return objPrefab; } }

    /// <summary>
    /// The static handler when an actor is created
    /// </summary>
    /// <param name="mActor">The newly created actor</param>
    public delegate void ActorCreatedHandler(MActor sender);
    public static event ActorCreatedHandler ActorCreated;

    /// <summary>
    /// The static handler when an actor is created
    /// </summary>
    /// <param name="mActor">The destroyed actor</param>
    public delegate void ActorDestroyedHandler(MActor sender);
    public static event ActorDestroyedHandler ActorDestroyed;

    private void Start() 
    {
        //notify the listeners that this actor has been created
        //NOTE: has to be in Start(), not Awake() otherwise actors that are already in the scene might not work when execution order is not used
        if (MActor.ActorCreated != null)
            MActor.ActorCreated(this); 
    }

    private void OnDestroy()
    {
        //notify the listeners that this has been destroyed
        if (MActor.ActorDestroyed != null)
            MActor.ActorDestroyed(this);
    }
}