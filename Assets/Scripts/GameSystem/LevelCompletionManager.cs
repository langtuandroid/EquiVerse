using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Input;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompletionManager : MonoBehaviour
{
    [Header("Sound")] 
    public World1LevelSoundController soundController;
    
    public Button endOfLevelButton;
    public LeafPointManager leafPointManager;
    
    [Header("SceneTransition")]
    public Image transitionOverlay;
    public GameObject loadingScreen;
    
    private Tween lockTween;
    private int lockIndex = 0;
    public GameObject[] locks;
    
    public TextMeshProUGUI completionTimeText;
    public AchievementManager achievementManager;
    private LevelTimer levelTimer;
    
    public Raycaster raycaster;
    public CameraMovement cameraMovement;
    public GameObject gameUI;
    
    private void Start()
    {
        levelTimer = gameObject.AddComponent<LevelTimer>();
        levelTimer.StartLevelTimer(); 
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.RightControl))
        {
            LevelCompleted();
        }
    }

    private void LevelCompleted()
    {
        raycaster.gameObject.SetActive(false);
        cameraMovement.CameraLocked = true;
        endOfLevelButton.interactable = false;
        gameUI.SetActive(false);
        levelTimer.EndLevelTimer();
        levelTimer.DisplayCompletionTime(completionTimeText);
        soundController.FadeAudioParameter("Music", "World1LevelMainMusicVolume", 0f, 1.2f);
        soundController.FadeAudioParameter("Ambience", "World1LevelAmbienceVolume", 0f, 1.2f);
        soundController.FadeAudioParameter("BattleMusic", "EnemyMusicVolume", 0f, 1.2f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CompleteLevel");
        
        achievementManager.ActivateAchievements();
        StartCoroutine(DisplayPopupAfterDelay());
    }

    private IEnumerator DisplayPopupAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 0;
    }

    public void TransitionToNextScene()
    {
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() => {
            soundController.StopAudioEvent("Music");
            soundController.StopAudioEvent("Ambience");
            if (GameManager.WORLD_INDEX == 1 && GameManager.LEVEL_INDEX == 5)
            {
                StartCoroutine(LoadAsynchronously("DemoFinishedScene"));
            }
            else
            {
                StartCoroutine(LoadAsynchronously("NewCompanionScene")); 
            }
            GameManager.LEVEL_INDEX++;
        })).SetUpdate(true);
    }
    
    IEnumerator LoadAsynchronously(string sceneIndex)
    {
        Time.timeScale = 1.0f;
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        yield return operation;
        loadingScreen.SetActive(false);
    }
    
    public void BuyEndOfLevel() {
        if (LeafPointManager.totalPoints >= leafPointManager.endOfLevelCost) {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
            leafPointManager.DecrementPoints(leafPointManager.endOfLevelCost);
            RemoveLock();
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
            leafPointManager.FlickerTotalPointsElement();
        }
    }
    
    private void RemoveLock()
    {
        lockTween.Complete();
        if (lockIndex < locks.Length)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Unlock");
            lockTween = locks[lockIndex].transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                locks[lockIndex].SetActive(false);
                if (lockIndex == locks.Length - 1)
                {
                    LevelCompleted();
                }
                else
                {
                    lockIndex++; 
                }
            });
        }
    }
}
