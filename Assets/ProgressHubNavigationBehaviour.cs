using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressHubNavigationBehaviour : MonoBehaviour
{
    public Button companionSelectionButton;
    public Button shopEcoEssenceButton;

    public GameObject companionSelectionMenu;
    public GameObject shopEcoEssenceMenu;

    private void Start()
    {
        NavigateToMenu(companionSelectionMenu, companionSelectionButton, shopEcoEssenceMenu, shopEcoEssenceButton);
    }

    public void NavigateToCompanionSelectionButton()
    {
        NavigateToMenu(companionSelectionMenu, companionSelectionButton, shopEcoEssenceMenu, shopEcoEssenceButton);
    }
    
    public void NavigateToShopEcoEssenceButton()
    {
        NavigateToMenu(shopEcoEssenceMenu, shopEcoEssenceButton, companionSelectionMenu, companionSelectionButton);
    }

    private void NavigateToMenu(GameObject activeMenu, Button activeButton, GameObject inactiveMenu, Button inactiveButton)
    {
        activeMenu.SetActive(true);
        inactiveMenu.SetActive(false);
        
        activeButton.interactable = false;
        inactiveButton.interactable = true;
    }
}