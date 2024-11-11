using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeFitterSize
{
    public class SizeObject
    {
        public int Axis { get; }

        public bool IsVertical { get; }

        public Transform Parent { get; }

        public List<RectTransform> Children { get; }
        
        public Vector3 Size { get; }

        public SizeObject(int axis, bool isVertical, Transform parent, List<RectTransform> children, Vector3 size)
        {
            Axis = axis;
            IsVertical = isVertical;
            Parent = parent;
            Children = children;
            Size = size;
        }
    }

    public enum SizeType
    {
        Prefered,
        Min,
        Flexible
    }

    private static float GetTotalSize(SizeObject sizeObject, SizeType sizeType)
    {
        int axis = sizeObject.Axis;
        bool isVertical = sizeObject.IsVertical;
        Transform parent = sizeObject.Parent;
        List<RectTransform> children = sizeObject.Children;
        Vector3 size = sizeObject.Size;

        float totalMin = 0;
        float totalPrefered = 0;
        float totalFlexible = 0;
        bool alongOtherAxis = isVertical ^ (axis == 1);
        int rectChildrenCount = children.Count;
        
        if (rectChildrenCount == 0)
        {
            if(parent.GetComponent<ILayoutElement>() != null)
            {
                totalMin = LayoutUtility.GetMinSize(parent as RectTransform, axis);
                totalPrefered = LayoutUtility.GetPreferredSize(parent as RectTransform, axis);
                totalFlexible = LayoutUtility.GetFlexibleSize(parent as RectTransform, axis);
            }
        }
        else
        {
            float corner = axis == 0 ? size.x : size.y;
            totalMin = corner;
            totalPrefered = corner;
            totalFlexible = corner;
        }
        

        switch (sizeType)
        {
            case SizeType.Prefered:
                return Mathf.Max(totalMin, totalPrefered);
            case SizeType.Min:
                return totalMin;
            case SizeType.Flexible:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(sizeType), sizeType, null);
        }

        return 0;
    }
    
    public static float GetTotalMinSize(SizeObject sizeObject)
    {
        return GetTotalSize(sizeObject, SizeType.Min);
    }

    public static float GetTotalPreferedSize(SizeObject sizeObject)
    {
        return GetTotalSize(sizeObject, SizeType.Prefered);
    }


}