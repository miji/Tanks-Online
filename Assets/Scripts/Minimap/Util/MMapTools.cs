using UnityEngine;
using System.Collections;

public static class MMapTools
{
    /// <summary>
    /// Calculates the current angle of a minimap object (e.g. MObject) based on the forward vector of the real-life object (e.g. MActor)
    /// </summary>
    /// <param name="objectForwardVec"></param>
    /// <returns></returns>
    public static float CalculateAngle(MOrientation orientation, Vector3 objectForwardVec)
    {
        float clockWise = 0.0f;
        switch (orientation)
        {
            case MOrientation.XZ:
                clockWise = Vector3.Dot(objectForwardVec, Vector3.Cross(Vector3.up, Vector3.forward));
                break;

            case MOrientation.YZ:
                clockWise = Vector3.Dot(objectForwardVec, Vector3.Cross(Vector3.right, Vector3.forward));
                break;
        }
        
        clockWise = clockWise > 0f ? -1f : 1f;
        return clockWise * Vector3.Angle(objectForwardVec, Vector3.forward);
    }

    /// <summary>
    /// Creates a gameobject with a RectTransform set to zeroes with strech mode
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static RectTransform CreateEmptyRTransGo(string name, Transform parent)
    {
        GameObject newParentGo = new GameObject(name);
        newParentGo.transform.parent = parent;

        RectTransform rTrans = newParentGo.AddComponent<RectTransform>();        
        rTrans.anchorMin = Vector2.zero;
        rTrans.anchorMax = Vector2.one;
        rTrans.pivot = new Vector2(0.5f, 0.5f);
        rTrans.sizeDelta = Vector2.zero;
        rTrans.anchoredPosition = Vector2.zero;
        rTrans.localPosition = Vector2.zero;
        rTrans.localRotation = Quaternion.identity;

        return rTrans;
    }
}
