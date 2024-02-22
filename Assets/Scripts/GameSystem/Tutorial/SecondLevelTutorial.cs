using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Spawners;
using UnityEngine;

public class SecondLevelTutorial : MonoBehaviour
{
    public GameObject maxPlantUpgradeButton;
    public GameObject gameUI;
    public GameObject screenOverlay;
    public GameObject rayCaster;
    private void Start()
    {
        maxPlantUpgradeButton.SetActive(false);
        screenOverlay.SetActive(true);
        rayCaster.SetActive(false);
        gameUI.SetActive(false);
    }

    public void ShowGameUI()
    {
        gameUI.SetActive(true);
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
