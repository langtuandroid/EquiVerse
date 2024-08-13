using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class ProgressHubNavigationBehaviour : MonoBehaviour
{
    public Button nextLevelButton;
    public Button companionSelectionButton;
    public Button shopEcoEssenceButton;

    public GameObject companionSelectionMenu;
    public GameObject shopEcoEssenceMenu;

    public static bool companionSelectionMenuActive;


    private void Update()
    {
        if (CompanionSelectionManager.maxCompanionsSelected || !companionSelectionMenuActive)
        {
            nextLevelButton.interactable = true;
        }
        else
        {
            nextLevelButton.interactable = false;
        }
    }

    private void Start()
    {
        
        companionSelectionMenuActive = false;
        
        (int levelIndex, int world, int level) = GameManager.FindFirstUncompletedLevel();
        if (world == 1 && level < 5)
        {
            
            companionSelectionButton.gameObject.SetActive(false);
            
            if (world == 1 && level == 2)
            {
                print("first time opening progress hub");
                //show tuturial
            }
        }
        else
        {
            print("companionselectionmenuActive");
            companionSelectionButton.gameObject.SetActive(true);
            companionSelectionMenuActive = true;
        }

        if (companionSelectionMenuActive)
        {
            NavigateToCompanionSelectionButton();
        }
        else
        {
            NavigateToShopEcoEssenceButton();
        }
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