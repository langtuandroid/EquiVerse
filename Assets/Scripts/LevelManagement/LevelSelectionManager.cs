using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonContainer;

    private List<LevelButton> levelButtons = new List<LevelButton>();

    void Start()
    {
        RefreshButtons();
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
}