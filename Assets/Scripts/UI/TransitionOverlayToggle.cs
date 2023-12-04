using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionOverlayToggle : MonoBehaviour
{
    public Image transitionOverlay;

    public GameObject guidedTutorial;

    private void Start()
    {
        guidedTutorial.SetActive(false);
        FadeSceneOpen();
    }

    private void FadeSceneOpen()
    {
        transitionOverlay.DOFade(0f, 5f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            guidedTutorial.SetActive(true);
        }));
    }

    private void FadeSceneClose()
    {
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic);
    }
}
