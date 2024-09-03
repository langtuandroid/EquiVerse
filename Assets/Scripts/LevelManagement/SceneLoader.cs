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
    }

    public void LoadNextLevel()
    {
        newCompanionSoundController.NewCompanionMusicVolumeFade(0, 1.2f);
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            (int level, int world) = GameManager.GetNextLevelFromPlayerPrefs();
            if (world == 1)
            {
                StartCoroutine(LoadAsynchronously("Level " + world + "-" + level));
            }
            else
            {
                StartCoroutine(LoadAsynchronously("Level " + 1 + "-" + 5)); 
            }
        });
    }

    public void LoadProgressHub()
    {
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            StartCoroutine(LoadAsynchronously("ProgressHubScene"));
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
