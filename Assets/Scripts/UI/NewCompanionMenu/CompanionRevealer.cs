using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CompanionRevealer : MonoBehaviour
{
    public CompanionManager companionManager;

    public GameObject chestCompanion;
    public Button nextLevelButton;
    public GameObject clickOnChestText;
    public GameObject clickOnChestNavigationArrow;
    public GameObject companionHeaderPanel;
    public Animator animator;

    private Tween shakeTween;

    [SerializeField] private NewCompanionSoundController soundController;

    private void Start()
    {
        soundController.StartMusic();
        nextLevelButton.interactable = true;
        clickOnChestText.SetActive(true);
        clickOnChestNavigationArrow.SetActive(true);
        companionHeaderPanel.SetActive(false);
        shakeTween = DOVirtual.DelayedCall(2.5f, () =>
            {
                chestCompanion.transform.DOShakePosition(1f, new Vector3(0.01f, 0.01f, 0.01f), 10, 90, false, true);
                FMODUnity.RuntimeManager.PlayOneShot("event:/NewCompanionScene/GiftShake");
            })
            .SetLoops(-1, LoopType.Restart);
    }

    public void RevealCompanion()
    {
        clickOnChestText.SetActive(false);
        clickOnChestNavigationArrow.SetActive(false);
        shakeTween.Kill();
        soundController.NewCompanionMusicVolumeFade(0, 2f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/NewCompanionScene/RevealDrumroll");
        chestCompanion.transform.DOScale(0, 1.5f)
            .SetEase(Ease.InBack)
            .OnPlay(() =>
            {
                chestCompanion.transform.DORotate(new Vector3(0, 1800, 0), 1.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InCubic)
                    .SetLoops(-1, LoopType.Incremental);
                animator.SetTrigger("Open");
            })
            .OnComplete(() =>
            {
                companionManager.GenerateCompanionOnPedestal();
                FMODUnity.RuntimeManager.PlayOneShot("event:/NewCompanionScene/OnReveal");

                companionHeaderPanel.SetActive(true);
                nextLevelButton.gameObject.SetActive(true);
                soundController.NewCompanionMusicVolumeFade(1, 2f);
                companionManager.PlayCompanionSound();
            });


    }
}
