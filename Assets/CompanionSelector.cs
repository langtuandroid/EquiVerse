using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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
public class CompanionSelector : MonoBehaviour
{
    public List<Companion> companions;
    [HideInInspector]
    public int currentCompanionIndex;
    public Transform companionPrefabInstanceLocation;

    private void Start()
    {
        currentCompanionIndex = 0;
        Instantiate(companions[currentCompanionIndex].companionPrefab, companionPrefabInstanceLocation);
        
    }

    public void PlayCompanionSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(companions[currentCompanionIndex].companionSoundEventPath);
    }
}
