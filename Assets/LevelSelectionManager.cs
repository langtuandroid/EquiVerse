using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonContainer;

    private Dictionary<string, bool> levelCompletionStatus;
    private List<LevelButton> levelButtons = new List<LevelButton>();

    void Start()
    {
        levelCompletionStatus = GameManager.levelCompletionStatus;
        RefreshButtons();
        UpdateLevelButtons();
    }

    public void RefreshButtons()
    {
        foreach(var button in levelButtons) Destroy(button.gameObject);
        levelButtons.Clear();
        
        for (int worldIndex = 1; worldIndex < GameManager.totalWorlds + 1; worldIndex++)
        {
            for (int levelIndex = 1; levelIndex <= 5; levelIndex++)
            {
                GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonContainer);
                LevelButton levelButton = buttonObj.GetComponent<LevelButton>();
                levelButton.Initialize(worldIndex, levelIndex);
                levelButtons.Add(levelButton);
            }
        }
    }

    void UpdateLevelButtons()
    {
        foreach (var levelButton in levelButtons)
        {
            //tring levelKey = $"WORLD_{GameManager.WORLD_INDEX}_LEVEL_{GameManager.LEVEL_INDEX}";
            //levelButton.levelText.text = GameManager.WORLD_INDEX + " - " + GameManager.LEVEL_INDEX;
            //bool isCompleted = levelCompletionStatus.ContainsKey(levelKey) && levelCompletionStatus[levelKey];
            //levelButton.UpdateButton(isCompleted);
        }
    }
}