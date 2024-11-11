using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ObsoleteCustomRectTransform
{
    public Vector3 PreviousPos { get; private set; }

    private Vector3 _tempPos;

    private readonly RectTransform _rectTransform;

    public string Name => _rectTransform.name;

    public Vector3 Position
    {
        get => _rectTransform.position;
        set => _rectTransform.position = value;
    }

    public Action HasChange;
    public bool HasChanged;

    public ObsoleteCustomRectTransform(RectTransform rectTransform)
    {
        if (rectTransform == null)
        {
            Assert.IsNull(rectTransform, "The rect transform shouldn't be null");
        }
        _rectTransform = rectTransform;
        _tempPos = Position;
    }

    public void Update()
    {
        if (_tempPos == Position)
        {
            return;
        }

        PreviousPos = _tempPos;
        _tempPos = Position;
        HasChange?.Invoke();
        HasChanged = true;
    }
    
    public void ResetChangedThisFrame()
    {
        HasChanged = false;
    }
    
    public T GetComponent<T>() where T : Component
    {
        return _rectTransform.GetComponent<T>();
    }

    public void GetWorldCorners(Vector3[] fourCornersArray)
    {
        _rectTransform.GetWorldCorners(fourCornersArray);
    }
}