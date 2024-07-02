using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    public SceneLoader sceneLoader;
    public List<Companion> companions;
    [HideInInspector]
    public Transform companionPrefabInstanceLocation;

    public TextMeshProUGUI companionTitleText;
    public TextMeshProUGUI companionSecondTitleText;
    public TextMeshProUGUI companionDescriptionText;
    
    public Button nextLevelButton;
    
    private void Start()
    {
        if (isNewCompanionScene)
        {
            GenerateCompanionOnPanel();
            GameManager.companionsUnlockedIndex++;
            GameManager.currentCompanionIndex++;
            GameManager.SaveGameData();
        }
    }

    public void GenerateCompanionOnPanel()
    {
        Instantiate(companions[GameManager.currentCompanionIndex].companionPrefab, companionPrefabInstanceLocation);
        companionTitleText.text = companions[GameManager.currentCompanionIndex].companionTitle;
        companionSecondTitleText.text = companions[GameManager.currentCompanionIndex].companionSecondTitle;
        companionDescriptionText.text = companions[GameManager.currentCompanionIndex].companionDescription;
    }

    public void PlayCompanionSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(companions[GameManager.currentCompanionIndex].companionSoundEventPath);
    }

    public void IncrementCompanionIndex()
    {
        nextLevelButton.interactable = false;
        if (GameManager.currentCompanionIndex < 4)
        {
            sceneLoader.LoadNextLevel();
        }
        else
        {
            sceneLoader.LoadCompanionSelector();
        }
    }
}
