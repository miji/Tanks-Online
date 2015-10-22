using UnityEngine;
using System.Collections;

/// <summary>
/// Static class containing global values that are used throughout the project, notable paths and strings
/// </summary>
public static class MVal
{
    #region paths
    public const string CompEmptyMinimap = "Assets/Prefabs/Minimap/DefaultComponents/MinimapEmpty.prefab";
    public const string CompRTCam = "Assets/Prefabs/Minimap/DefaultComponents/RTCamera.prefab";
    #endregion

    #region GameObject names
    public const string PivotGoName = "Pivot";
    public const string MapGoName = "Map";
    public const string MaskGoName = "Mask";
    #endregion
}
