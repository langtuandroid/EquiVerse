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

    public GameObject screenOverlay;

    public GameObject rayCaster;
    private void Start()
    {
        maxPlantUpgradeButton.SetActive(false);
        screenOverlay.SetActive(true);
        rayCaster.SetActive(false);
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
        rayCaster.SetActive(true);
    }
}
