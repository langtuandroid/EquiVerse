using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenuBehaviour : MonoBehaviour
    {
        public GameObject pauseMenuUI;
        public Raycaster raycaster;

        [Header("SceneTransition")]
        public Image transitionOverlay;
        public GameObject loadingScreen;

        [Header("Sound")]
        public World1LevelSoundController soundController;

        private Tween pauseMenuAnimationTween;

        private bool isPaused = false;

        private void Start()
        {
            pauseMenuUI.SetActive(false);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenuAnimationTween.Complete();
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        private void PauseGame()
        {
            raycaster.gameObject.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");   
            isPaused = true;
            pauseMenuUI.SetActive(true);
            pauseMenuUI.transform.localScale = Vector3.zero;
            pauseMenuAnimationTween = pauseMenuUI.transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() => Time.timeScale = 0f);
        }

        public void ResumeGame()
        {
            raycaster.gameObject.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");   
            isPaused = false;
            Time.timeScale = 1f;
            pauseMenuAnimationTween = pauseMenuUI.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo)
                .OnComplete(() => pauseMenuUI.SetActive(false));
        }

        public void BackToMainMenu()
        {
            ResumeGame();
            soundController.StopAudioEvent("Music");
            soundController.StopAudioEvent("Ambience");
            soundController.StopAudioEvent("BattleMusic");
            StartCoroutine(LoadAsynchronously(0));
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
