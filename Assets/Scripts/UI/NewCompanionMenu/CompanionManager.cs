using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Managers;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Companion
{
    public string companionTitle;
    public string companionSecondTitle;
    public string companionDescription;
    
    public GameObject companionPrefab;
    public FMODUnity.EventReference companionSoundEventPath;
} 

public class CompanionManager : MonoBehaviour
{
    public bool isNewCompanionScene;
    public LevelLoader levelLoader;
    public List<Companion> companions;
    [HideInInspector]
    public static int currentCompanionIndex;
    public Transform companionPrefabInstanceLocation;

    public TextMeshProUGUI companionTitleText;
    public TextMeshProUGUI companionSecondTitleText;
    public TextMeshProUGUI companionDescriptionText;

    private void Start()
    {
        if (isNewCompanionScene)
        {
            GenerateCompanionOnPanel();
        }
    }

    public void GenerateCompanionOnPanel()
    {
        Instantiate(companions[currentCompanionIndex].companionPrefab, companionPrefabInstanceLocation);
        companionTitleText.text = companions[currentCompanionIndex].companionTitle;
        companionSecondTitleText.text = companions[currentCompanionIndex].companionSecondTitle;
        companionDescriptionText.text = companions[currentCompanionIndex].companionDescription;
        GameManager.companionsUnlockedIndex++;
    }

    public void PlayCompanionSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(companions[currentCompanionIndex].companionSoundEventPath);
    }

    public void IncrementCompanionIndex()
    {
        currentCompanionIndex++;
        if (currentCompanionIndex < 4)
        {
            levelLoader.LoadNextLevel();
        }
        else
        {
            levelLoader.LoadCompanionSelector();
        }
    }
}
