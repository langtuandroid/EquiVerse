using System;
using System.Collections;
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
        [Header("SceneTransition")]
        public Image transitionOverlay;
        public GameObject loadingScreen;

        [Header("ToggleMenu's")] 
        public GameObject mainMenu;
        public GameObject optionsMenu;
        public GameObject creditsMenu;

        public GameObject continueAdventureObject;
        public TextMeshProUGUI continueAdventureButtonText;
        public Button continueAdventureButton, newAdventureButton, optionsButton, quitButton;

        public GameObject logoCanvas;
        public GameObject mainMenuCanvas;
        [Header("Sound")] 
        public MainMenuSoundController mainMenuSoundController;
        
        public string discordUrl = "https://discord.gg/hay2fMBggT";
        
        private void Start()
        {
            GameManager.LoadGameData();
            mainMenuCanvas.SetActive(false);
            logoCanvas.SetActive(true);
            mainMenuSoundController.FadeMainMenuVolume(0f, 3f);

            continueAdventureButton.enabled = true;
            newAdventureButton.enabled = true;
            optionsButton.enabled = true;
            quitButton.enabled = true;

            if (GameManager.firstTimePlaying)
            {
                GameManager.WORLD_INDEX = 1;
                GameManager.LEVEL_INDEX = 1;
                GameManager.firstTimePlaying = false;
                continueAdventureObject.SetActive(false);
            }
            else
            {
                continueAdventureObject.SetActive(true);
                continueAdventureButtonText.text = "Continue adventure (" + GameManager.WORLD_INDEX.ToString() + "-" + GameManager.LEVEL_INDEX.ToString() + ")";
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
        }

        public void ClickNew()
        {
            continueAdventureButton.enabled = false;
            newAdventureButton.enabled = false;
            optionsButton.enabled = false;
            quitButton.enabled = false;
            mainMenuSoundController.FadeMainMenuVolume(1.0f, 1.1f);
            GameManager.WORLD_INDEX = 1;
            GameManager.LEVEL_INDEX = 1;
            GameManager.companionsUnlockedIndex = 0;
            GameManager.currentCompanionIndex = 0;
            transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
            {
                StartCoroutine(LoadAsynchronously("Level " + GameManager.WORLD_INDEX.ToString() + "-" + GameManager.LEVEL_INDEX.ToString()));
            }));
        }
        
        public void ClickContinue()
        {
            continueAdventureButton.enabled = false;
            newAdventureButton.enabled = false;
            optionsButton.enabled = false;
            quitButton.enabled = false;
            mainMenuSoundController.FadeMainMenuVolume(1.0f, 1.1f);
            if ((GameManager.WORLD_INDEX == 1 && GameManager.LEVEL_INDEX == 5) || GameManager.WORLD_INDEX > 1)
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
                    StartCoroutine(LoadAsynchronously("Level " + GameManager.WORLD_INDEX.ToString() + "-" +
                                                      GameManager.LEVEL_INDEX.ToString()));
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
        
        public void OpenDiscord()
        {
            Application.OpenURL(discordUrl);
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
