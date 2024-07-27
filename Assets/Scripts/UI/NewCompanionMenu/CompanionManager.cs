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
            //GameManager.companionsUnlockedIndex++;
            //GameManager.currentCompanionIndex++;
            GameManager.SaveGameData();
        }
    }

    public void GenerateCompanionOnPanel()
    {
        int companionIndex = PlayerPrefs.GetInt("CompanionIndex", -1);
        Instantiate(companions[companionIndex].companionPrefab, companionPrefabInstanceLocation);
        companionTitleText.text = companions[companionIndex].companionTitle;
        companionSecondTitleText.text = companions[companionIndex].companionSecondTitle;
        companionDescriptionText.text = companions[companionIndex].companionDescription;
    }

    public void PlayCompanionSound()
    {
        int companionIndex = PlayerPrefs.GetInt("CompanionIndex", -1);
        FMODUnity.RuntimeManager.PlayOneShot(companions[companionIndex].companionSoundEventPath);
    }

    public void IncrementCompanionIndex()
    {
        int companionIndex = PlayerPrefs.GetInt("CompanionIndex", -1) + 1;
        nextLevelButton.interactable = false;
        if (companionIndex < 4)
        {
            sceneLoader.LoadNextLevel();
        }
        else
        {
            sceneLoader.LoadCompanionSelector();
        }
    }
}
