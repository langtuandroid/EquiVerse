using DG.Tweening;
using Input;
using Managers;
using UnityEngine;

namespace Tutorial
{
    public class GuidedTutorial : MonoBehaviour
    {
        public GameManager gameManager;
        public ECManager ecManager;
        public GameObject screenOverlay, guidedTutorial, plantSpawner, GameUI, objectSpawner, rabbitButton, finishLevelButton, finishLevelStep;
        public GameObject[] tutorialSteps;

        private int stepIndex = 0;
        private bool cameraStepCompleted, finishLevelStepCompleted, buttonPressed;

        private void Start()
        {
            if (!gameManager.tutorialActivated)
            {
                SetupNonTutorialMode();
            }
            else
            {
                ResetTutorialState();
                SetupTutorialMode();
            }
        }

        private void FixedUpdate()
        {
            //Erwin: tutorialActivated wordt twee keer gecheckt maar dat ik een klein puntje. In dit geval niet super relevant maar je zou de checks kunnen omdraaien, dat performt (soms) beter.
            if (CheckCameraStepCompletion() && gameManager.tutorialActivated)
                HandleCameraStepCompletion();

            if (CheckFinishLevelStepCompletion() && gameManager.tutorialActivated)
                HandleFinishLevelStepCompletion();
        }

        public void NextStep()
        {
            print(stepIndex);
            print(tutorialSteps.Length);
            //if (buttonPressed || stepIndex >= tutorialSteps.Length) return;

            tutorialSteps[stepIndex].SetActive(false);
            stepIndex++;
            
            //Erwin: Als je de hardcoded stappen hier wil voorkomen (als je bijvoorbeeld meerdere tutorials wil maken) zou je gebruik kunnen maken van een lijst van UnityActions
            //Je zou er een soort statemachine van kunnen maken die bijhoudt in welke stap je zit zodat alleen de logica in die stap gecheckt hoeft te worden.
            if (stepIndex <= tutorialSteps.Length)
            {
                tutorialSteps[stepIndex].SetActive(true);
                PopInAnimation(tutorialSteps[stepIndex]);

                // Step-specific logic
                if (stepIndex == 2)
                {
                    screenOverlay.SetActive(false);
                    CameraMovement.cameraLocked = false;
                }

                if (stepIndex == 5)
                {
                    rabbitButton.SetActive(true);
                }
                
                if (stepIndex == 6)
                {
                    print("test");
                }

                if (stepIndex == 7)
                {
                    plantSpawner.SetActive(true);
                }
            }
        }

        public void NextStepOnce()
        {
            if (!buttonPressed)
            {
                tutorialSteps[stepIndex].SetActive(false);
                stepIndex++;
                buttonPressed = true;
                objectSpawner.SetActive(true);

                if (stepIndex < tutorialSteps.Length)
                {
                    tutorialSteps[stepIndex].SetActive(true);
                    PopInAnimation(tutorialSteps[stepIndex]);
                }
            }
        }

        public void FinishTutorial()
        {
            finishLevelStep.SetActive(false);
        }

        //Erwin: Hele random plek voor dit punt maar is het nodig dat de game gepauseerd kan worden? Dan zou je namelijk wat moeten tweaken aan DOTween of SetUpdate(true) kunnen toevoegen.
        private void PopInAnimation(GameObject gameObject)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform?.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        }

        //Erwin: Ook hier zou je met UnityActions een lijst van 'resets' kunnen maken (of gewoon 1 action waarin alles gereset wordt)
        private void SetupNonTutorialMode()
        {
            CameraMovement.cameraLocked = false;
            GameUI.SetActive(true);
            screenOverlay.SetActive(false);
            objectSpawner.SetActive(true);
            rabbitButton.SetActive(true);
            plantSpawner.SetActive(true);
            finishLevelButton.SetActive(false);
            finishLevelStep.SetActive(false);
        }

        private void ResetTutorialState()
        {
            stepIndex = 0;
            cameraStepCompleted = false;
            finishLevelStepCompleted = false;
            buttonPressed = false;
        }

        private void SetupTutorialMode()
        {
            CameraMovement.cameraLocked = true;
            screenOverlay.SetActive(true);
            GameUI.SetActive(false);
            objectSpawner.SetActive(false);
            finishLevelButton.SetActive(false);
            plantSpawner.SetActive(false);

            foreach (var step in tutorialSteps)
            {
                step.SetActive(false);
            }

            tutorialSteps[0].SetActive(true);
            PopInAnimation(guidedTutorial);
        }

        private bool CheckCameraStepCompletion()
        {
            return CameraMovement.cameraMovedInAllDirections && !cameraStepCompleted && gameManager.tutorialActivated;
        }

        private void HandleCameraStepCompletion()
        {
            GameUI.SetActive(true);
            UpdateStepAndAnimate();
            cameraStepCompleted = true;
        }

        private bool CheckFinishLevelStepCompletion()
        {
            return ECManager.totalPoints >= ecManager.endOfLevelCost && !finishLevelStepCompleted && gameManager.tutorialActivated;
        }

        private void HandleFinishLevelStepCompletion()
        {
            finishLevelButton.SetActive(true);
            finishLevelStep.SetActive(true);
            PopInAnimation(finishLevelStep);
            finishLevelStepCompleted = true;
        }

        private void UpdateStepAndAnimate()
        {
            tutorialSteps[stepIndex].SetActive(false);
            stepIndex++;
            if (stepIndex < tutorialSteps.Length)
            {
                tutorialSteps[stepIndex].SetActive(true);
                PopInAnimation(tutorialSteps[stepIndex]);
            }
        }
    }
}
