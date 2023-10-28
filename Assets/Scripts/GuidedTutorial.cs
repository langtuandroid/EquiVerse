using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GuidedTutorial : MonoBehaviour
{
    public GameObject screenOverlay;
    public GameObject guidedTutorial;
    public GameObject GameUI;
    public GameObject[] tutorialSteps;
    public GameObject objectSpawner;
    public GameObject rabbitButton;
    public GameObject finishLevelButton;
    public GameObject finishLevelStep;

    private int stepIndex = 0;
    private bool cameraStepCompleted = false;
    private bool buttonPressed = false;

    private void Start()
    {
        screenOverlay.SetActive(true);
        guidedTutorial.SetActive(true);
        GameUI.SetActive(false);
        objectSpawner.SetActive(false);
        finishLevelButton.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (CameraMovement.cameraMovedInAllDirections && !cameraStepCompleted)
        {
            GameUI.SetActive(true);
            tutorialSteps[stepIndex].SetActive(false);
            tutorialSteps[stepIndex + 1].SetActive(true);
            stepIndex += 1;
            cameraStepCompleted = true;
        }

        if (ECManager.totalPoints >= 150)
        {
            finishLevelButton.SetActive(true);
            finishLevelStep.SetActive(true);
        }
    }

    public void NextStep()
    {
        tutorialSteps[stepIndex].SetActive(false);
        tutorialSteps[stepIndex + 1].SetActive(true);
        stepIndex += 1;
        
        if (stepIndex == 2)
        {
            screenOverlay.SetActive(false);
            CameraMovement.cameraLocked = false;
        }

        if (stepIndex == 5)
        {
            rabbitButton.SetActive(true);
        }
    }

    public void NextStepOnce()
    {
        if (!buttonPressed)
        {
            tutorialSteps[stepIndex].SetActive(false);
            tutorialSteps[stepIndex + 1].SetActive(true);
            stepIndex += 1;
            buttonPressed = true;
            objectSpawner.SetActive(true);
        }
    }

    public void FinishTutorial()
    {
        finishLevelStep.SetActive(false);
    }
}
