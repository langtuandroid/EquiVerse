using System.Collections;
using DG.Tweening;
using Managers;
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
        
        private void Start()
        {
            if (GameManager.firstTimePlaying)
            {
                GameManager.WORLD_INDEX = 1;
                GameManager.LEVEL_INDEX = 1;
                GameManager.firstTimePlaying = false;
            }
        }

        public void ClickPlay()
        {
            transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
            {
                StartCoroutine(LoadAsynchronously("Level " + GameManager.WORLD_INDEX.ToString() + "-" + GameManager.LEVEL_INDEX.ToString()));
            }));
        }

        public void ClickQuit()
        {
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
            }
            else
            {
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
    }
}
