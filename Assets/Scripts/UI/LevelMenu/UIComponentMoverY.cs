using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIComponentMoverY : MonoBehaviour
{
    public float moveDistance;
    public float moveDuration;

    private RectTransform uiComponent;
    private Vector2 originalPosition;

    private void Start() // Use Start instead of OnEnable to ensure the UI is fully initialized
    {
        uiComponent = GetComponent<RectTransform>();
        originalPosition = uiComponent.anchoredPosition;
        MoveUIComponent();
    }

    private void MoveUIComponent()
    {
        uiComponent.anchoredPosition = originalPosition; // Ensure it starts from the original position
        uiComponent.DOAnchorPosY(originalPosition.y + moveDistance, moveDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
}