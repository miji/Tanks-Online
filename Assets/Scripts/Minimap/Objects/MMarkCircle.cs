using UnityEngine;
using System.Collections;

/// <summary>
/// The circlur mark object in the minimap 
/// </summary>
public class MMarkCircle : MonoBehaviour 
{
    /// <summary>
    /// Reference to the MObject associated with this class
    /// </summary>
    private MObject _mObj;

    public MObject MObj { get { return _mObj; } set { _mObj = value; } }

    void Awake()
    {
        if (_mObj == null) _mObj = transform.GetComponentInParentRecursively<MObject>();
    }

    void Start () 
    {
        GetComponent<Animation>().PlayQueued(GetComponent<Animation>().clip.name);
    }
    
    void Update () 
    {
        //TODO animation event instead
        if (!GetComponent<Animation>().isPlaying)
            _mObj.SelfDestruct();
    }
}
