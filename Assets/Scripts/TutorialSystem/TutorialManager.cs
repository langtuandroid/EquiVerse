using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    public List<TutorialStep> steps;
    private int currentStepIndex;
    public static bool pickUpLeafpointStepCompleted = false;

    public void Start() {
        if (steps.Count == 0)
        {
            enabled = false;
            return;
        }
    }

    public void SetStep(int index) {
        currentStepIndex = index;
        steps[currentStepIndex].onStart.Invoke();
    }
    
    private static TutorialManager _instance;
    private static TutorialManager GetInstance() {
        if (_instance == null) {
            _instance = FindObjectOfType<TutorialManager>();
        }
        if (_instance == null)
            throw new System.Exception("TutorialManager not found in scene while tutorial step was completed");

        return _instance;
    }

    public static void CompleteStep(string name) {
        GetInstance()._CompleteStep(name, false);
    }

    public static void CompleteStepAndContinueToNextStep(string name) {
        GetInstance()._CompleteStep(name, true);
    }

    private void _CompleteStep(string name, bool autoNextStep) {
        if (steps[currentStepIndex].name.Equals(name, System.StringComparison.OrdinalIgnoreCase)) { //Step name is the same so we mark the step as completed
            if (!steps[currentStepIndex].completed) {
                steps[currentStepIndex].onComplete.Invoke();
            }
            steps[currentStepIndex].completed = true;

            if (autoNextStep && currentStepIndex + 1 < steps.Count) {
                SetStep(currentStepIndex + 1);
            }
        }
    }
    public static void GoToNextStep() {
        GetInstance()._GoToNextStep();
    }

    private void _GoToNextStep() {
        if (!steps[currentStepIndex].completed)
            throw new System.Exception($"TutorialManager tried to advance a step but the last step was not yet completed");

        if (currentStepIndex + 1 < steps.Count) {
            SetStep(currentStepIndex + 1);
        }
    }
    
    public void PopInAnimation(GameObject gameObject) {
        gameObject.SetActive(true);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform?.DOScale(1, 0.5f).SetEase(Ease.OutExpo).From(Vector3.zero).SetUpdate(true);
    }

    public void PlayOpeningUIElementSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
