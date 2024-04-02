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
    public GameObject clickOnChestText;

    private Tween shakeTween;

    [SerializeField] private NewCompanionSoundController soundController;

    private void Start()
    {
        clickOnChestText.SetActive(true);
        giftButton.interactable = true;
        shakeTween = DOVirtual.DelayedCall(2.5f, () =>
            {
                giftButton.transform.DOShakePosition(2.5f, new Vector3(8, 8, 0), 10, 90, false, true);
                FMODUnity.RuntimeManager.PlayOneShot("event:/NewCompanionScene/GiftShake");
            })
            .SetLoops(-1, LoopType.Restart);
    }

    public void RevealCompanion()
    {
        clickOnChestText.SetActive(false);
        shakeTween.Kill();
        giftButton.interactable = false;
        soundController.NewCompanionMusicVolumeFade(0, 2f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/NewCompanionScene/RevealDrumroll");
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
                FMODUnity.RuntimeManager.PlayOneShot("event:/NewCompanionScene/OnReveal");
                companion.DOSizeDelta(new Vector2(750, 800), 2f).SetEase(Ease.OutBack).OnComplete((() =>
                {
                    nextLevelButton.gameObject.SetActive(true);
                    soundController.NewCompanionMusicVolumeFade(1, 2f);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/FerdinandGoose/FerdinandHonk");
                }));
            });
    }
}
