using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CompanionRevealer : MonoBehaviour
{
    public Button giftButton;
    public RectTransform companion;

    public void RevealCompanion()
    {
        giftButton.transform.DOScale(0, 1.5f).SetEase(Ease.InBack).OnComplete((() =>
        {
            companion.gameObject.SetActive(true);
            companion.DOSizeDelta(new Vector2(750, 800), 1.5f).SetEase(Ease.OutBack);
        }));
    }
}
