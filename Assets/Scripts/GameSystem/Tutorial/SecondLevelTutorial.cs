using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;

public class SecondLevelTutorial : MonoBehaviour
{
    public GameObject maxPlantUpgradeButton;

    public GameObject screenOverlay;
    private void Start()
    {
        maxPlantUpgradeButton.SetActive(false);
        screenOverlay.SetActive(true);
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

    public void CloseFirstEnemyPopup()
    {
        screenOverlay.SetActive(false);
    }
}
