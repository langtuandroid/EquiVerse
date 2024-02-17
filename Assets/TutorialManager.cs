using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    public List<TutorialStep> steps;
    private int currentStepIndex;

    public void Start() {
        if (steps.Count == 0) {
            enabled = false;
            return;
        }

        SetStep(0);
    }

    public void SetStep(int index) {
        currentStepIndex = index;
        steps[currentStepIndex].onStart.Invoke();
    }



    public void Reset() {
        foreach (var step in steps) {
            step.onReset.Invoke();
        }

    }
    private static TutorialManager _instance;
    public static void CompleteStep(string name) {
        if (_instance == null) {
            _instance = FindObjectOfType<TutorialManager>();
        }
        if (_instance == null) {
            throw new System.Exception("TutorialManager not found in scene while tutorial step was completed");
        } else {
            _instance._CompleteStep(name);
        }
    }

    private void _CompleteStep(string name) {
        if (steps[currentStepIndex].name.Equals(name)) { //Step name is the same so we mark the step as completed
            if (!steps[currentStepIndex].completed) {
                steps[currentStepIndex].onComplete.Invoke();
            }
            steps[currentStepIndex].completed = true;

            if (currentStepIndex + 1 < steps.Count) {
                SetStep(currentStepIndex + 1);
            }
        }
    }
}
