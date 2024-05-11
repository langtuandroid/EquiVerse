using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanionSelectionButtonBehaviour : MonoBehaviour
{
    public CompanionManager companionManager;
    public GameObject buttonsParent;
    
    private GameObject currentCompanionPrefabInstance;

    void Start()
    {
        FillButtonNames();
    }

    void FillButtonNames()
    {
        Button[] buttons = buttonsParent.GetComponentsInChildren<Button>();
        int buttonCount = buttons.Length;
        int companionCount = companionManager.companions.Count;

        for (int i = 0; i < buttonCount; i++)
        {
            if (i < companionCount)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = companionManager.companions[i].companionTitle;
                int companionIndex = i; // Capture the current value of i
                buttons[i].onClick.AddListener(() => SelectCompanion(companionIndex));
            }
            else
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "?";
                buttons[i].interactable = false;
            }
        }
    }


    private void SelectCompanion(int index)
    {
        Debug.Log("Button with companion " + companionManager.companions[index].companionTitle + " clicked!");
        GenerateCompanionOnPanel(index);
    }
    
    private void GenerateCompanionOnPanel(int index)
    {
        if (index >= 0 && index < companionManager.companions.Count)
        {
            if (currentCompanionPrefabInstance != null)
            {
                Destroy(currentCompanionPrefabInstance);
            }
            
            currentCompanionPrefabInstance = Instantiate(companionManager.companions[index].companionPrefab, companionManager.companionPrefabInstanceLocation);
            companionManager.companionTitleText.text = companionManager.companions[index].companionTitle;
            companionManager.companionSecondTitleText.text = companionManager.companions[index].companionSecondTitle;
            companionManager.companionDescriptionText.text = companionManager.companions[index].companionDescription;
        }
        else
        {
            Debug.LogWarning("Invalid index provided for generating companion on panel.");
        }
    }
}

