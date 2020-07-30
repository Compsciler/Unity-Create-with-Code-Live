using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{
    // https://forum.unity.com/threads/hiow-to-get-children-gameobjects-array.142617/
    public static GameObject[] GetChildren(this GameObject go)
    {
        GameObject[] children = new GameObject[go.transform.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = go.transform.GetChild(i).gameObject;
        }
        return children;
    }

    public static string ListToString<T>(List<T> list)
    {
        return string.Join(", ", list.Select(p => p.ToString()).ToArray());
    }
}
