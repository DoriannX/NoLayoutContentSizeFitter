using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomContentSizeFitter : ContentSizeFitter
{
    [SerializeField] private float _refreshRate;
    private RectTransform _rect;
    private CustomTimer _timer;

    private ContentSizeFitterSize _contentSizeFitterSize;
    private ChildrenGetter _childrenGetter;
    private BoundingBox _boundingBox;
    private Canvas _canvas;

    private RectTransform Rect => _rect ??= GetComponent<RectTransform>();

    private ContentSizeFitterSize ContentSizeFitterSize => _contentSizeFitterSize ??= new ContentSizeFitterSize();

    private ChildrenGetter ChildrenGetter => _childrenGetter ??= new ChildrenGetter();

    private BoundingBox BoundingBox => _boundingBox ??= new BoundingBox();

    private Canvas Canvas => _canvas ??= GetComponentInParent<Canvas>();

    private List<RectTransform> _children;

    private CustomTimer Timer
    {
        get
        {
            if (_timer != null)
            {
                return _timer;
            }

            _timer = new CustomTimer(1 / _refreshRate,
                Refresh);
            _timer.Start();

            return _timer;
        }
    }

    protected override void OnEnable()
    {
        Invoke(nameof(SetDirty), 0);
    }

    public new void SetDirty()
    {
        base.SetDirty();
    }

    private void Update()
    {
        Timer.Update(Time.deltaTime);
    }

    private void Refresh()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public override void SetLayoutHorizontal()
    {
        HandleSelfFittingAlongAxis(0);
    }

    public override void SetLayoutVertical()
    {
        HandleSelfFittingAlongAxis(1);
    }

    private void HandleSelfFittingAlongAxis(int axis)
    {
        FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
        if (fitting == FitMode.Unconstrained)
        {
            return;
        }
        
        _children = ChildrenGetter.GetChildren(transform)
            .Select(currentTransform => currentTransform as RectTransform)
            .ToList();
        
        ContentSizeFitterSize.SizeObject sizeObject = new(axis, axis == 1, transform, _children,
            BoundingBox.CalculateBoundingBoxSize(BoundingBox.CalculateBoundingBox(_children)));
        float minSize = ContentSizeFitterSize.GetTotalMinSize(sizeObject);
        float preferedSize = ContentSizeFitterSize.GetTotalPreferedSize(sizeObject);
        Rect.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, fitting == FitMode.MinSize ? minSize : preferedSize);
        UpdateChildrenPosition(_children,
            ((RectTransform)transform).TransformPoint(((RectTransform)transform).rect.center),
            BoundingBox.CalculateBoundingBoxCenter(BoundingBox.CalculateBoundingBox(_children)));
    }

    private static void UpdateChildrenPosition(List<RectTransform> children, Vector3 parentBoundingBoxCenter,
        Vector3 boundingBoxCenter)
    {
        Vector3 displacement = boundingBoxCenter - parentBoundingBoxCenter;
        foreach (RectTransform child in children)
        {
            LayoutElement layoutElement = child.GetComponent<LayoutElement>();

            if (layoutElement != null && layoutElement.ignoreLayout)
            {
                continue;
            }

            child.position -= displacement;
        }
    }
}