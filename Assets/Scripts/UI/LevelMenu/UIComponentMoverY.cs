using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIComponentMoverY : MonoBehaviour
{
    public float moveDistance;
    public float moveDuration;
    
    private RectTransform uiComponent;

    private void OnEnable()
    {
        uiComponent = GetComponent<RectTransform>();
        MoveUIComponent();
    }

    private void MoveUIComponent()
    {
        uiComponent.DOAnchorPosY(moveDistance, moveDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
