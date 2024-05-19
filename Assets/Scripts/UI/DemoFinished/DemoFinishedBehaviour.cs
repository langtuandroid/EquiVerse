using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoFinishedBehaviour : MonoBehaviour
{
    [Header("SceneTransition")]
    public Image transitionOverlay;
    public GameObject loadingScreen;
    
    public void BackToMainMenu()
    {
        StartCoroutine(LoadAsynchronously(0));
    }
    

    public void GoToDiscord()
    {
        Application.OpenURL(GameManager.discordUrl);
    }

    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).SetUpdate(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        yield return operation;
        loadingScreen.SetActive(false);
    }


}
