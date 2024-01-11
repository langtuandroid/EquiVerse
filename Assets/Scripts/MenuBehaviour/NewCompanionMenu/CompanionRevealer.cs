using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CompanionRevealer : MonoBehaviour
{
    public Button giftButton;
    public RectTransform companion;
    public Button nextLevelButton;

    private void Start()
    {
        DOVirtual.DelayedCall(2.0f, () =>
            {
                giftButton.transform.DOShakePosition(1.0f, new Vector3(5, 5, 0), 10, 90, false, true);
            })
            .SetLoops(-1, LoopType.Restart);
    }

    public void RevealCompanion()
    {
        giftButton.transform.DOScale(0, 2.5f)
            .SetEase(Ease.InBack)
            .OnPlay(() =>
            {
                // Rotate the giftButton during the scale tween
                giftButton.transform.DORotate(new Vector3(0, 0, 900), 2.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InCubic)
                    .SetLoops(-1, LoopType.Incremental);
            })
            .OnComplete(() =>
            {
                companion.gameObject.SetActive(true);
                companion.DOSizeDelta(new Vector2(750, 800), 2f).SetEase(Ease.OutBack).OnComplete((() =>
                {
                    nextLevelButton.gameObject.SetActive(true);
                }));
            });
    }
}
