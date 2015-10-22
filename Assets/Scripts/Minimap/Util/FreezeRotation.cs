using UnityEngine;
using System.Collections;

/// <summary>
/// Script that saves the object's rotation at the start and makes sure it stays the same
/// </summary>
public class FreezeRotation : MonoBehaviour 
{
    private Quaternion _originalRot;

    void Awake()
    {
        _originalRot = transform.rotation;
    }

    void Update () 
    {
        if (transform.rotation != _originalRot)
            transform.rotation = _originalRot;
    }
}
