using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;

public class SecondLevelTutorial : MonoBehaviour
{
    public GameObject maxPlantUpgradeButton;
    
    private void Start()
    {
        maxPlantUpgradeButton.SetActive(false);
    }

    public void ShowMaxPlantUpgradeButton()
    {
        maxPlantUpgradeButton.SetActive(true);
    }

    public void CloseHint(GameObject hint)
    {
        if (hint.activeInHierarchy)
        {
            hint.SetActive(false);
        }
    }
}
