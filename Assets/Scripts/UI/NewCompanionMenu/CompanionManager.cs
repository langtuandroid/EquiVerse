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
    public Transform companionPrefabInstanceLocation;

    public TextMeshProUGUI companionTitleText;
    public TextMeshProUGUI companionSecondTitleText;
    public TextMeshProUGUI companionDescriptionText;
    
    private const string CompanionIndexKey = "CurrentCompanionIndex";

    private void Start()
    {
        if (isNewCompanionScene)
        {
            GenerateCompanionOnPanel();
        }
    }

    public void GenerateCompanionOnPanel()
    {
        Instantiate(companions[GameManager.currentCompanionIndex].companionPrefab, companionPrefabInstanceLocation);
        companionTitleText.text = companions[GameManager.currentCompanionIndex].companionTitle;
        companionSecondTitleText.text = companions[GameManager.currentCompanionIndex].companionSecondTitle;
        companionDescriptionText.text = companions[GameManager.currentCompanionIndex].companionDescription;
        GameManager.companionsUnlockedIndex++;
    }

    public void PlayCompanionSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(companions[GameManager.currentCompanionIndex].companionSoundEventPath);
    }

    public void IncrementCompanionIndex()
    {
        GameManager.currentCompanionIndex++;
        PlayerPrefs.SetInt(CompanionIndexKey, GameManager.currentCompanionIndex);
        PlayerPrefs.Save();
        if (GameManager.currentCompanionIndex < 4)
        {
            levelLoader.LoadNextLevel();
        }
        else
        {
            levelLoader.LoadCompanionSelector();
        }
    }
}
