using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

public static class MEventDelegate
{
    /// <summary>
    /// Get methods of the specific MonoBehaviour class that have 0 to 1 parameter, are public, declared (not inherited) and have the void return type
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static List<MethodInfo> GetMethods(MonoBehaviour target)
    {
        List<MethodInfo> list = new List<MethodInfo>();
        MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

        for (int i = 0; i < methods.Length; ++i)
        {
            MethodInfo mi = methods[i];
            if (mi.GetParameters().Length <= 1 && mi.ReturnType == typeof(void))
            {
                if (mi.Name != "StopAllCoroutines" && mi.Name != "CancelInvoke")
                {
                    list.Add(mi);
                }
            }
        }
        return list;
    }
}