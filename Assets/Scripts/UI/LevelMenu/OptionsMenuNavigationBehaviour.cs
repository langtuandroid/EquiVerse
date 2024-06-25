using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class OptionTab
{
    public String optionTabName;
    public Button optionTabButton;
    public GameObject optionTabContent;
}

public class OptionsMenuNavigationBehaviour : MonoBehaviour
{
    public List<OptionTab> optionTabs;

    private void Start()
    {
        foreach (OptionTab tab in optionTabs)
        {
            tab.optionTabButton.onClick.AddListener(() => OnTabSelected(tab));
        }
        if (optionTabs.Count > 0)
        {
            ShowTab(optionTabs[0]);
        }
    }

    public void OnTabSelected(OptionTab selectedTab)
    {
        ShowTab(selectedTab);
    }

    private void ShowTab(OptionTab tabToShow)
    {
        foreach (OptionTab tab in optionTabs)
        {
            bool isSelected = tab == tabToShow;
            tab.optionTabContent.SetActive(isSelected);
        }
    }
    
    
}
