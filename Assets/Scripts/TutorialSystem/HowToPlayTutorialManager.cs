using System;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class VideoStep
{
    public string videoStepName;
    public VideoClip videoClip;
    [TextArea(6,10)]
    public string explanation;
}

public class HowToPlayTutorialManager : MonoBehaviour
{
    public List<VideoStep> videosteps;
    public TextMeshProUGUI explanationText;
    public VideoPlayer videoPlayer;

    private void Start()
    {
        UpdateTutorialStep();
    }

    public void GoNext()
    {
        GameManager.currentVideoStepIndex++;
        if (GameManager.currentVideoStepIndex >= videosteps.Count)
        {
            GameManager.currentVideoStepIndex = 0;
        }
        UpdateTutorialStep();
    }
    
    public void GoPrevious()
    {
        GameManager.currentVideoStepIndex--;
        if (GameManager.currentVideoStepIndex < 0)
        {
            GameManager.currentVideoStepIndex = videosteps.Count - 1;
        }
        UpdateTutorialStep();
    }

    private void UpdateTutorialStep()
    {
        if (GameManager.currentVideoStepIndex >= 0 && GameManager.currentVideoStepIndex < videosteps.Count)
        {
            videoPlayer.clip = videosteps[GameManager.currentVideoStepIndex].videoClip;
            explanationText.text = videosteps[GameManager.currentVideoStepIndex].explanation;
        }
    }
}