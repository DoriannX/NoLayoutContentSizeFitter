using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BoundingBox
{
    private readonly Vector3[] _corners = new Vector3[4];
    private Vector3 _size = new();

    public float CalculateBoundingBoxAxisSize(Vector3[] corners, int axis)
    {
        Assert.AreEqual(corners.Length, 4, "Corners array must have 4 elements");
        float width = (corners[1] - corners[0]).x;
        float height = (corners[3] - corners[0]).y;
        return axis == 0 ? width : height;
    }

    public Vector3 CalculateBoundingBoxSize(Vector3[] corners)
    {
        Assert.AreEqual(corners.Length, 4, "Corners array must have 4 elements");
        _size.Set((corners[3] - corners[1]).x, (corners[1] - corners[0]).y, 0);
        return _size;
    }

    public Vector3[] GetWorldCorners(RectTransform corners)
    {
        corners.GetWorldCorners(_corners);
        return _corners;
    }

    public Vector3[] CalculateBoundingBox(List<RectTransform> children)
    {
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        foreach(RectTransform child in children)
        {
            child.GetWorldCorners(_corners);
            foreach(Vector3 corner in _corners)
            {
                minX = Mathf.Min(minX, corner.x);
                minY = Mathf.Min(minY, corner.y);
                maxX = Mathf.Max(maxX, corner.x);
                maxY = Mathf.Max(maxY, corner.y);
            }
        }
        _corners[0].Set(minX, minY, 0);
        _corners[1].Set(minX, maxY, 0);
        _corners[2].Set(maxX, maxY, 0);
        _corners[3].Set(maxX, minY, 0);
        return _corners;

    }

    public Vector3 CalculateBoundingBoxCenter(Vector3[] corners)
    {
        Assert.AreEqual(corners.Length, 4, "Corners array must have 4 elements");
        return (corners[0] + corners[1] + corners[2] + corners[3]) / 4;
    }
}