using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Managers;
using Input;
using Spawners;
using UI;

public class LevelCompletionManager : MonoBehaviour
{
    [Header("Sound")]
    public World1LevelSoundController soundController;

    public GameManager gameManager;
    
    [Header("UI Elements")]
    public Button endOfLevelButton;
    public Image transitionOverlay;
    public GameObject loadingScreen;
    public GameObject[] locks;
    public GameObject gameUI;
    
    [Header("Managers")]
    public LeafPointManager leafPointManager;
    public LevelStatManager levelStatManager;
    public AchievementManager achievementManager;
    public FoodSpawner foodSpawner;
    public LevelTimer levelTimer;
    
    [Header("Other Components")]
    public Raycaster raycaster;
    public CameraMovement cameraMovement;
    
    private Tween lockTween;
    private int lockIndex = 0;

    private void Start()
    {
        levelTimer.StartLevelTimer();
    }

    private void Update()
    {
        #if(UNITY_EDITOR)
        if (UnityEngine.Input.GetKeyDown(KeyCode.P))
        {
            LevelCompleted();
        }
        #endif
    }

    private void LevelCompleted()
    {
        PlayerPrefs.SetInt("CompanionIndex", gameManager.GetCurrentLevelCompanionIndex() + 1);
        DisableGameplay();
        levelTimer.EndLevelTimer();
        PlayCompletionSounds();
        UpdateLevelStats();
        achievementManager.ActivateAchievements();
        GameManager.SaveGameData();
        Time.timeScale = 0;
    }

    private void DisableGameplay()
    {
        foodSpawner.CanSpawnPlants = false;
        raycaster.gameObject.SetActive(false);
        cameraMovement.CameraLocked = true;
        endOfLevelButton.interactable = false;
        gameUI.SetActive(false);
        PauseMenuBehaviour.pauseMenuEnabled = false;
    }

    private void PlayCompletionSounds()
    {
        soundController.FadeAudioParameter("Music", "World1LevelMainMusicVolume", 0f, 0.5f);
        soundController.FadeAudioParameter("Ambience", "World1LevelAmbienceVolume", 0f, 0.5f);
        soundController.FadeAudioParameter("BattleMusic", "EnemyMusicVolume", 0f, 0.5f);
        
        soundController.StartAudioEvent("CelebrationMusic");
        soundController.FadeAudioParameter("CelebrationMusic", "CelebrationMusicVolume", 1f, 0.5f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CompleteLevel");
    }

    private void UpdateLevelStats()
    {
        float completionTime = levelTimer.LoadCompletionTime();
        TimeSpan timeSpan = TimeSpan.FromSeconds(completionTime);

        levelStatManager.UpdateStat("Completion Time", $"{timeSpan:mm\\:ss}");
        levelStatManager.UpdateStat("Leaf Points Collected", GameManager.totalLeafPointsCollected.ToString());
        levelStatManager.UpdateStat("Animals Spawned", GameManager.animalsSpawned.ToString());
        levelStatManager.UpdateStat("Animals Deaths", GameManager.animalDeaths.ToString());
    }

    public void TransitionToNextScene()
    {
        transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete(() => {
            soundController.StopAudioEvent("Music");
            soundController.StopAudioEvent("Ambience");
            soundController.FadeAudioParameter("CelebrationMusic", "CelebrationMusicVolume", 0f, 1.2f);
            soundController.StopAudioEvent("CelebrationMusic");
            LoadNextScene();
        }).SetUpdate(true);
    }

    private void LoadNextScene()
    {
        string nextScene = (gameManager.currentSceneWorldIndex == 1 && gameManager.currentSceneLevelIndex == 5) ? "DemoFinishedScene" : "NewCompanionScene";
        StartCoroutine(LoadAsynchronously(nextScene));
    }

    private IEnumerator LoadAsynchronously(string sceneName)
    {
        Time.timeScale = 1.0f;
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return operation;
        loadingScreen.SetActive(false);
    }

    public void BuyEndOfLevel()
    {
        if (LeafPointManager.totalPoints >= leafPointManager.endOfLevelCost)
        {
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
                lockIndex++;
                if (lockIndex == locks.Length)
                {
                    LevelCompleted();
                }
            });
        }
    }
}
