using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public MainMenuSoundController mainMenuSoundController;
    
    [Header("SceneTransition")]
    public Image transitionOverlay;
    public GameObject loadingScreen;
    
    public void LoadNextLevel()
    {
        GameManager.LEVEL_INDEX++;
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            StartCoroutine(LoadAsynchronously("Level " + GameManager.WORLD_INDEX.ToString() + "-" + GameManager.LEVEL_INDEX.ToString()));
        }));
    }

    public void LoadCompanionSelector()
    {
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            StartCoroutine(LoadAsynchronously("CompanionSelectorScene"));
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
