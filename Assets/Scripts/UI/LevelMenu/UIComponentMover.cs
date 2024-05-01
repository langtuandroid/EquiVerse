using DG.Tweening;
using UnityEngine;

public class UIComponentMover : MonoBehaviour
{
    public float moveDistance = 200f;
    public float moveDuration = 2f;
    
    private RectTransform uiComponent;

    private void OnEnable()
    {
        uiComponent = GetComponent<RectTransform>();
        MoveUIComponent();
    }

    private void MoveUIComponent()
    {
        uiComponent.DOAnchorPosX(moveDistance, moveDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
