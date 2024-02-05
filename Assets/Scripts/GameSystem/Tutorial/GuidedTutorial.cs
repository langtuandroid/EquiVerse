using DG.Tweening;
using Input;
using Managers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial
{
    public class GuidedTutorial : MonoBehaviour
    {
        public GameManager gameManager;
        public ECManager ecManager;
        public GameObject screenOverlay, gameUI, objectSpawner, rabbitButton, finishLevelButton, finishLevelStep;
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
            if (CheckCameraStepCompletion() && gameManager.tutorialActivated)
                HandleCameraStepCompletion();

            if (CheckFinishLevelStepCompletion() && gameManager.tutorialActivated)
                HandleFinishLevelStepCompletion();
        }

        public void NextStep()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
            
            tutorialSteps[stepIndex].SetActive(false);
            stepIndex++;

            if (stepIndex <= tutorialSteps.Length)
            {
                tutorialSteps[stepIndex].SetActive(true);
                PopInAnimation(tutorialSteps[stepIndex]);

                // Step-specific logic
                if (stepIndex == 1)
                {
                    screenOverlay.SetActive(false);
                    CameraMovement.CAMERA_LOCKED = false;
                }

                if (stepIndex == 4)
                {
                    rabbitButton.SetActive(true);
                }

                if (stepIndex == 6)
                {
                    Spawners.PlantSpawner.canSpawnPlants = true;
                }
            }
        }

        public void NextStepOnce()
        {
            
            if (!buttonPressed)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
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

        private void PopInAnimation(GameObject gameObject)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform?.DOScale(1, 0.5f).SetEase(Ease.OutExpo).From(Vector3.zero);
        }

        private void SetupNonTutorialMode()
        {
            CameraMovement.CAMERA_LOCKED = false;
            gameUI.SetActive(true);
            screenOverlay.SetActive(false);
            objectSpawner.SetActive(true);
            rabbitButton.SetActive(true);
            Spawners.PlantSpawner.canSpawnPlants = true;
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
            CameraMovement.CAMERA_LOCKED = true;
            screenOverlay.SetActive(true);
            gameUI.SetActive(false);
            objectSpawner.SetActive(false);
            finishLevelButton.SetActive(false);
            Spawners.PlantSpawner.canSpawnPlants = false;

            foreach (var step in tutorialSteps)
            {
                step.SetActive(false);
            }

            tutorialSteps[0].SetActive(true);
            PopInAnimation(tutorialSteps[0]);
        }

        private bool CheckCameraStepCompletion()
        {
            return CameraMovement.CAMERA_MOVED_IN_ALL_DIRECTIONS && !cameraStepCompleted && gameManager.tutorialActivated;
        }

        private void HandleCameraStepCompletion()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
            gameUI.SetActive(true);
            UpdateStepAndAnimate();
            cameraStepCompleted = true;
        }

        private bool CheckFinishLevelStepCompletion()
        {
            return ECManager.totalPoints >= ecManager.endOfLevelCost && !finishLevelStepCompleted && gameManager.tutorialActivated;
        }

        private void HandleFinishLevelStepCompletion()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
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
