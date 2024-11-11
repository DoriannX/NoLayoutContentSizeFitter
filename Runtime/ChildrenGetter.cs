using System.Collections.Generic;
using UnityEngine;

public class ChildrenGetter
{
    private readonly List<Transform> _children = new();
    
    public List<Transform> GetChildren(Transform parent)
    {
        _children.Clear();
        for (int i = 0; i < parent.childCount; i++)
        {
            _children.Add(parent.GetChild(i));
        }

        return _children;
    }
}