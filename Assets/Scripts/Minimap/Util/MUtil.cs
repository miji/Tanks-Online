using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The utility class for use in the game
/// </summary>
public static class MUtil
{
    /// <summary>
    /// Prints out error msg. Wrapper is used to make sure it is obvious the error comes from the Minimap asset.
    /// </summary>
    /// <param name="msg">Message to display</param>
    /// <param name="obj">Optional parameter of the object sending the error message</param>
    public static void Error(string msg, Object obj = null)
    {
        Debug.LogError("Minimap: " + msg, obj);
    }

    /// <summary>
    /// Prints information log in the console
    /// </summary>
    /// <param name="msg">Message to display</param>
    /// <param name="obj">Optional parameter of the object sending the error message</param>
    public static void Log(string msg, Object obj = null)
    {
        Debug.Log("Minimap: " + msg, obj);
    }

    /// <summary>
    /// Get direct children without directly referencing the transform. Is useful when direct referencing won't work (e.g. while changing sibling index)
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static List<Transform> GetDirectChildren(Transform trans)
    {
        return (from Transform child in trans select (child)).ToList();
    }

    /// <summary>
    /// Get the specified component type in the parent. Starts with itself (the same as GetComponentInChildren)!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <returns></returns>
    //TODO might not get the correct view if there is two of them, it will just get the first one
    public static T GetComponentInParentRecursively<T>(this Transform trans, string parentName = "") where T : Component
    {
        bool lookingAccordName = parentName != "" ? true : false;
        
        for (Transform t = trans; t != null; t = t.parent)
        {
            if (!lookingAccordName || (lookingAccordName && t.name == parentName))
            {
                T result = t.GetComponent<T>();
                if (result != null)
                    return result;
            }
        }

        return null;
    }

    //http://answers.unity3d.com/questions/555101/possible-to-make-gameobjectgetcomponentinchildren.html
    ///////////////////////////////////////////////////////////
    // Essentially a reimplementation of 
    // GameObject.GetComponentInChildren< T >()
    // Major difference is that this DOES NOT skip deactivated 
    // game objects
    ///////////////////////////////////////////////////////////
    /// 
    static public TType GetComponentInChildren<TType>(GameObject objRoot) where TType : Component
    {
        // if we don't find the component in this object 
        // recursively iterate children until we do
        TType tRetComponent = objRoot.GetComponent<TType>();

        if (null == tRetComponent)
        {
            // transform is what makes the hierarchy of GameObjects, so 
            // need to access it to iterate children
            Transform trnsRoot = objRoot.transform;
            int iNumChildren = trnsRoot.childCount;

            // could have used foreach(), but it causes GC churn
            for (int iChild = 0; iChild < iNumChildren; ++iChild)
            {
                // recursive call to this function for each child
                // break out of the loop and return as soon as we find 
                // a component of the specified type
                tRetComponent = GetComponentInChildren<TType>(trnsRoot.GetChild(iChild).gameObject);
                if (null != tRetComponent)
                {
                    break;
                }
            }
        }

        return tRetComponent;
    }

    /// <summary>
    /// If you want to execute in edit mode
    /// </summary>
    /// <returns></returns>
    public static bool ExecuteInEditMode()
    {
        //only if in editor and is set to ExecuteInEditMode or if playing the game
        #if UNITY_EDITOR
        return (Application.isPlaying || MSettings.Instance.ExecuteInEditMode);
        #else
        //only related to editor, otherwise always true
        return true;
        #endif
    }

    public static float FrequencyToReadableFps(float frequency)
    {
        return Mathf.Round(1.0f / frequency * 10) / 10;
    }
}
