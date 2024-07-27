using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanionSelectionButtonBehaviour : MonoBehaviour
{
    public CompanionManager companionManager;
    public GameObject buttonsParent;
    public Button nextLevelButton;
    public Color defaultColor, markedColor;
    public TextMeshProUGUI leftToChooseText;

    public static List<Companion> selectedCompanions = new List<Companion>();

    private GameObject currentCompanionPrefabInstance;
    private List<Button> markedButtons = new List<Button>();

    private int maxCompanionsToSelect = 3;

    void Start()
    {
        selectedCompanions.Clear();
        ResetMarkedButtons();

        nextLevelButton.interactable = false;
        FillButtonNames();

        UpdateLeftToChooseText();
    }

    void FillButtonNames()
    {
        
        int companionIndex = PlayerPrefs.GetInt("CompanionIndex", -1) + 1;

        Button[] buttons = buttonsParent.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < companionIndex)
            {
                ConfigureButton(buttons[i], i);
            }
            else
            {
                SetButtonAsPlaceholder(buttons[i]);
            }
        }
    }

    void ConfigureButton(Button button, int index)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = companionManager.companions[index].companionTitle;
        button.onClick.AddListener(() => ToggleMarkButton(button, index));
    }

    void SetButtonAsPlaceholder(Button button)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = "?";
        button.interactable = false;
    }

    void ToggleMarkButton(Button button, int index)
    {
        if (markedButtons.Contains(button))
        {
            UnmarkButton(button);
        }
        else
        {
            if (markedButtons.Count < maxCompanionsToSelect)
            {
                MarkButton(button, index);
            }
            else
            {
                Debug.Log("Maximum number of marked buttons reached.");
            }
        }

        nextLevelButton.interactable = markedButtons.Count == maxCompanionsToSelect;
        SelectCompanion(index);

        UpdateLeftToChooseText();
    }

    void MarkButton(Button button, int index)
    {
        markedButtons.Add(button);
        button.image.color = markedColor;

        selectedCompanions.Add(companionManager.companions[index]);
    }

    void UnmarkButton(Button button)
    {
        markedButtons.Remove(button);
        button.image.color = defaultColor;

        Companion companionToRemove = companionManager.companions.Find(c => c.companionTitle == button.GetComponentInChildren<TextMeshProUGUI>().text);
        if (companionToRemove != null)
        {
            selectedCompanions.Remove(companionToRemove);
        }
    }

    void SelectCompanion(int index)
    {
        GenerateCompanionOnPanel(index);
    }

    void GenerateCompanionOnPanel(int index)
    {
        if (index >= 0 && index < companionManager.companions.Count)
        {
            Destroy(currentCompanionPrefabInstance);
            currentCompanionPrefabInstance = Instantiate(companionManager.companions[index].companionPrefab, companionManager.companionPrefabInstanceLocation);
            companionManager.companionTitleText.text = companionManager.companions[index].companionTitle;
            companionManager.companionSecondTitleText.text = companionManager.companions[index].companionSecondTitle;
            companionManager.companionDescriptionText.text = companionManager.companions[index].companionDescription;
        }
    }

    void UpdateLeftToChooseText()
    {
        int leftToChoose = maxCompanionsToSelect - markedButtons.Count;
        leftToChooseText.text = leftToChoose.ToString();
    }

    private void ResetMarkedButtons()
    {
        foreach (Button button in markedButtons)
        {
            button.image.color = defaultColor;
        }
        markedButtons.Clear();
    }
}
