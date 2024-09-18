using System.Collections;
using DG.Tweening;
using Input;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenuBehaviour : MonoBehaviour
    {
        public static bool pauseMenuEnabled;
        public GraphicRaycaster graphicRaycaster;
        public GameObject pauseMenuUI;
        public GameObject quitWarningPanel;
        public GameObject optionsMenu;
        public GameObject howToPlayMenu;
        public Raycaster raycaster;
        public CameraMovement cameraMovement;

        [Header("SceneTransition")]
        public Image transitionOverlay;
        public GameObject loadingScreen;

        [Header("Sound")]
        public LevelSoundController soundController;

        private Tween pauseMenuAnimationTween;

        private bool isPaused;
        private bool subMenuActive;

        private void Start()
        {
            quitWarningPanel.SetActive(false);
            pauseMenuUI.SetActive(false);
            pauseMenuEnabled = true;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !subMenuActive && pauseMenuEnabled)
            {
                if (isPaused)
                {
                    pauseMenuAnimationTween.Complete();
                    ResumeGame();
                }
                else
                {
                    pauseMenuAnimationTween.Complete();
                    PauseGame();
                }
            }
        }

        private void PauseGame()
        {
            graphicRaycaster.enabled = false;
            raycaster.gameObject.SetActive(false);
            cameraMovement.CameraLocked = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");   
            isPaused = true;
            pauseMenuUI.SetActive(true);
            pauseMenuUI.transform.localScale = Vector3.zero;
            Time.timeScale = 0f;
            pauseMenuAnimationTween = pauseMenuUI.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).SetUpdate(true);
        }

        public void ResumeGame()
        {
            graphicRaycaster.enabled = true;
            raycaster.gameObject.SetActive(true);
            cameraMovement.CameraLocked = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");   
            isPaused = false;
            Time.timeScale = 1f;
            pauseMenuAnimationTween = pauseMenuUI.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo).OnComplete(() => pauseMenuUI.SetActive(false));
        }

        public void BackToMainMenu()
        {
            ResumeGame();
            pauseMenuAnimationTween = pauseMenuUI.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                pauseMenuUI.SetActive(false);
                soundController.StopAudioEvent("Music");
                soundController.StopAudioEvent("Ambience");
                soundController.StopAudioEvent("BattleMusic");
                StartCoroutine(LoadAsynchronously(0));
            }).SetUpdate(true);
        }
        
        public void ToggleOptionsMenu()
        {
            if (optionsMenu.activeSelf)
            {
                subMenuActive = false;
                optionsMenu.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else
            {
                subMenuActive = true;
                optionsMenu.SetActive(true);
                pauseMenuUI.SetActive(false);
            }
        }
        
        public void ToggleHowToPlayMenu()
        {
            if (howToPlayMenu.activeSelf)
            {
                subMenuActive = false;
                howToPlayMenu.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else
            {
                subMenuActive = true;
                howToPlayMenu.SetActive(true);
                pauseMenuUI.SetActive(false);
            }
        }

        public void ToggleQuitWarning()
        {
            if (quitWarningPanel.activeSelf)
            {
                subMenuActive = false;
                quitWarningPanel.SetActive(false);
            }
            else
            {
                subMenuActive = true;
                quitWarningPanel.SetActive(true);
            }
        }
        
        public void RestartLevel()
        {
            ResumeGame();
            soundController.StopAudioEvent("Music");
            soundController.StopAudioEvent("Ambience");
            soundController.StopAudioEvent("BattleMusic");
            StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
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
}
