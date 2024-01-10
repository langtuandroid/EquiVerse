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

        public GameObject continueAdventureObject;
        public TextMeshProUGUI continueAdventureButton;
        
        private void Start()
        {
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
                continueAdventureButton.text = "Continue adventure (" + GameManager.WORLD_INDEX.ToString() + "-" + GameManager.LEVEL_INDEX.ToString() + ")";
            }
        }

        public void ClickNew()
        {
            GameManager.WORLD_INDEX = 1;
            GameManager.LEVEL_INDEX = 1;
            transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
            {
                StartCoroutine(LoadAsynchronously("Level " + GameManager.WORLD_INDEX.ToString() + "-" + GameManager.LEVEL_INDEX.ToString()));
            }));
        }
        
        public void ClickContinue()
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

        //Erwin: De huidige manier van UI elementen aan/uitzetten werkt goed als je niet veel menus hebt maar zou wat onoverzichtelijk kunnen worden als je meerdere menus hebt.
        //In Memoria hadden we een UIManager waarin je SetMenu("menuName") kon aanroepen om een menu te openen, dan zorgde de UIManager ervoor dat het huidige menu uit ging en het nieuwe aan. Met CloseMenu() gingen sloot het actieve menu.
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
