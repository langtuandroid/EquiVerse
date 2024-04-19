using DG.Tweening;
using Managers;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class TransitionOverlayToggle : MonoBehaviour
    {
        public Image transitionOverlay;
        public GameObject transitionOverlayObject;
        public GameObject guidedTutorial;
        public TutorialManager tutorialManager;

        private void Start()
        {
            guidedTutorial.SetActive(false);
            transitionOverlayObject.SetActive(true);
            FadeSceneOpen();
        }

        private void FadeSceneOpen()
        {
            transitionOverlay.DOFade(0f, 4f).SetEase(Ease.InSine).OnComplete((() =>
            {
                tutorialManager.SetStep(0);
            }));
        }
    }
}
