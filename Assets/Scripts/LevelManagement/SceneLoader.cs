using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public NewCompanionSoundController newCompanionSoundController;
    
    [Header("SceneTransition")]
    public Image transitionOverlay;
    public GameObject loadingScreen;

    public Button backToMainMenuButton;

    private void Start()
    {
        backToMainMenuButton.gameObject.SetActive(true);
        backToMainMenuButton.interactable = true;
        newCompanionSoundController.StartMusic();
    }

    public void LoadNextLevel()
    {
        newCompanionSoundController.NewCompanionMusicVolumeFade(0, 1.2f);
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            (int level, int world) = GameManager.GetNextLevelFromPlayerPrefs();
            StartCoroutine(LoadAsynchronously("Level " + world + "-" + level));
        });
    }

    public void LoadCompanionSelector()
    {
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            StartCoroutine(LoadAsynchronously("CompanionSelectorScene"));
        }));
    }

    public void LoadMainMenu()
    {
        backToMainMenuButton.interactable = false;
        newCompanionSoundController.NewCompanionMusicVolumeFade(0, 1.2f);
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            backToMainMenuButton.gameObject.SetActive(false);
            StartCoroutine(LoadAsynchronously("MainMenu"));
        }));
    }
    
    IEnumerator LoadAsynchronously(string levelIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
        yield return operation;
        loadingScreen.SetActive(false);
    }
    
}
