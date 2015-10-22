using System;
using UnityEngine;

/// <summary>
/// Class used to hold the interaction data for the minimap's event system
/// </summary>
[Serializable]
public class MInteractionEvent
{
    public MInteractionType mapInteractionType;
    public string methodName;
    public MonoBehaviour target;
}