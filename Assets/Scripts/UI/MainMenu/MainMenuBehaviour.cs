using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public LevelSelectionManager levelSelectionManager;
        
        [Header("SceneTransition")]
        public Image transitionOverlay;
        public GameObject loadingScreen;

        [Header("ToggleMenu's")] 
        public GameObject mainMenu;
        public GameObject optionsMenu;
        public GameObject creditsMenu;
        public GameObject howToPlayMenu;
        public GameObject levelSelectionMenu;

        public GameObject continueAdventureObject;
        public TextMeshProUGUI continueAdventureButtonText;
        public Button continueAdventureButton, newAdventureButton, optionsButton, quitButton;

        public GameObject DiscordPanel;

        public GameObject logoCanvas;
        public GameObject mainMenuCanvas;
        [Header("Sound")] 
        public MainMenuSoundController mainMenuSoundController;
        
        private void Start()
        {
            GameManager.LoadGameData();
            mainMenuCanvas.SetActive(false);
            logoCanvas.SetActive(true);
            DiscordPanel.SetActive(true);
            continueAdventureButton.enabled = true;
            newAdventureButton.enabled = true;
            optionsButton.enabled = true;
            quitButton.enabled = true;
            mainMenuSoundController.FadeMainMenuVolume(mainMenuSoundController.GetMainMenuVolume(), 3f);
            
            if (GameManager.firstTimePlaying)
            {
                GameManager.firstTimePlaying = false;
                continueAdventureObject.SetActive(false);
            }
            else
            {
                continueAdventureObject.SetActive(true);
                (int levelIndex, int world, int level) = GameManager.FindFirstUncompletedLevel();
                continueAdventureButtonText.text = "Continue adventure (" + world + "-" + level + ")";
            }
        }
        
        private void Update()
        {
            if (logoCanvas.activeInHierarchy)
            {
                if (UnityEngine.Input.anyKeyDown)
                { 
                    logoCanvas.SetActive(false);
                    mainMenuCanvas.SetActive(true); 
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.U))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("Delete all playerprefs");
            }
        }

        public void ClickNew()
        {
            continueAdventureButton.enabled = false;
            newAdventureButton.enabled = false;
            optionsButton.enabled = false;
            quitButton.enabled = false;
            DiscordPanel.SetActive(false);
            mainMenuSoundController.FadeMainMenuVolume(1.0f, 1.1f);
            //GameManager.companionsUnlockedIndex = 0;
            //GameManager.currentCompanionIndex = 0;
            transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
            {
                StartCoroutine(LoadAsynchronously("Level 1-1"));
            }));
        }
        
        private void StoreNextLevelInPlayerPrefs(int world, int level)
        {
            PlayerPrefs.SetInt("nextSceneLevelIndex", level);
            PlayerPrefs.SetInt("nextSceneWorldIndex", world);
        }
        
        public void ClickContinue()
        {
            continueAdventureButton.enabled = false;
            newAdventureButton.enabled = false;
            optionsButton.enabled = false;
            quitButton.enabled = false;
            DiscordPanel.SetActive(false);
            mainMenuSoundController.FadeMainMenuVolume(1.0f, 1.1f);
            //TODO
            (int levelIndex, int world, int level) = GameManager.FindFirstUncompletedLevel();
            StoreNextLevelInPlayerPrefs(world, level);
            if ((world == 1 && level == 5) || world > 1)
            {
                transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
                {
                    StartCoroutine(LoadAsynchronously("CompanionSelectorScene"));
                }));
            }
            else
            {
                transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
                {
                    StartCoroutine(LoadAsynchronously("Level " + world + "-" + level));
                }));
            }
        }

        public void ClickQuit()
        {
            continueAdventureButton.enabled = false;
            newAdventureButton.enabled = false;
            optionsButton.enabled = false;
            quitButton.enabled = false;
            Application.Quit();
        }

        IEnumerator LoadAsynchronously(string levelIndex)
        {
            loadingScreen.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
            yield return operation;
            loadingScreen.SetActive(false);
        }

        public void ToggleOptionsMenu()
        {
            if (!optionsMenu.activeInHierarchy)
            {
                optionsMenu.SetActive(true);
                mainMenu.SetActive(false);
                continueAdventureButton.enabled = false;
                newAdventureButton.enabled = false;
                optionsButton.enabled = false;
                quitButton.enabled = false;
            }
            else
            {
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);
                continueAdventureButton.enabled = true;
                newAdventureButton.enabled = true;
                optionsButton.enabled = true;
                quitButton.enabled = true;
            }
        }
        
        public void ToggleCreditsMenu()
        {
            if (!creditsMenu.activeInHierarchy)
            {
                creditsMenu.SetActive(true);
                mainMenu.SetActive(false);
                continueAdventureButton.enabled = false;
                newAdventureButton.enabled = false;
                optionsButton.enabled = false;
                quitButton.enabled = false;
            }
            else
            {
                creditsMenu.SetActive(false);
                mainMenu.SetActive(true);
                continueAdventureButton.enabled = true;
                newAdventureButton.enabled = true;
                optionsButton.enabled = true;
                quitButton.enabled = true;
            }
        }
        
        public void ToggleHowToPlayMenu()
        {
            if (!howToPlayMenu.activeInHierarchy)
            {
                howToPlayMenu.SetActive(true);
                mainMenu.SetActive(false);
                continueAdventureButton.enabled = false;
                newAdventureButton.enabled = false;
                optionsButton.enabled = false;
                quitButton.enabled = false;
            }
            else
            {
                howToPlayMenu.SetActive(false);
                mainMenu.SetActive(true);
                continueAdventureButton.enabled = true;
                newAdventureButton.enabled = true;
                optionsButton.enabled = true;
                quitButton.enabled = true;
            }
        }
        
        public void ToggleLevelSelectionMenu()
        {
            if (!levelSelectionMenu.activeInHierarchy)
            {
                levelSelectionMenu.SetActive(true);
                DiscordPanel.SetActive(false);
                mainMenu.SetActive(false);
                continueAdventureButton.enabled = false;
                newAdventureButton.enabled = false;
                optionsButton.enabled = false;
                quitButton.enabled = false;
                levelSelectionManager.RefreshButtons();
            }
            else
            {
                levelSelectionMenu.SetActive(false);
                DiscordPanel.SetActive(true);
                mainMenu.SetActive(true);
                continueAdventureButton.enabled = true;
                newAdventureButton.enabled = true;
                optionsButton.enabled = true;
                quitButton.enabled = true;
            }
        }
        
        public void OpenDiscord()
        {
            Application.OpenURL(GameManager.discordUrl);
        }

        public void ScaleUpText(Button button)
        {
            button.transform.DOScale(Vector3.one * 1.3f, 0.5f).SetEase(Ease.OutCubic);
        }

        public void ScaleDownText(Button button)
        {
            button.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic);
        }
    }
}
