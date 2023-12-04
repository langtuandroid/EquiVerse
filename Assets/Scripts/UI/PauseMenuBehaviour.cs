using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBehaviour : MonoBehaviour
{
    public GameObject pauseMenuUI;
    
    [Header("SceneTransition")]
    public Image transitionOverlay;
    public GameObject loadingScreen;
    public GuidedTutorial guidedTutorialManager;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;

    }

    bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }
    
    public void BackToMainMenu()
    {
        ResumeGame();
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            guidedTutorialManager.enabled = false;
            StartCoroutine(LoadAsynchronously(0));
        }));
    }
    
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return operation;
        loadingScreen.SetActive(false);
    }

}
