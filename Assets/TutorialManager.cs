using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        if (steps[currentStepIndex].name.Equals(name, System.StringComparison.OrdinalIgnoreCase)) { //Step name is the same so we mark the step as completed
            if (!steps[currentStepIndex].completed) {
                steps[currentStepIndex].onComplete.Invoke();
            }
            steps[currentStepIndex].completed = true;

            if (currentStepIndex + 1 < steps.Count) {
                SetStep(currentStepIndex + 1);
            }
        }
    }
    
    public void PopInAnimation(GameObject gameObject)
    {
        gameObject.SetActive(true);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform?.DOScale(1, 0.5f).SetEase(Ease.OutExpo).From(Vector3.zero);
    }
}
