using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;      
    public GameObject worldNamePrefab;        
    public Transform verticalContainer;
    public List<string> worldNames = new List<string>();

    private List<LevelButton> levelButtons = new List<LevelButton>();
    private List<GameObject> worldNameObjects = new List<GameObject>();
    private List<GameObject> buttonGridObjects = new List<GameObject>();

    void Start()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        foreach (var button in levelButtons) Destroy(button.gameObject);
        levelButtons.Clear();

        foreach (var worldNameObj in worldNameObjects) Destroy(worldNameObj);
        worldNameObjects.Clear();

        foreach (var buttonGridObj in buttonGridObjects) Destroy(buttonGridObj);
        buttonGridObjects.Clear();

        for (int worldIndex = 1; worldIndex <= GameManager.totalWorlds; worldIndex++)
        {
            if (worldIndex <= worldNames.Count)
            {
                GameObject worldNameObj = Instantiate(worldNamePrefab, verticalContainer);
                worldNameObjects.Add(worldNameObj);
                TextMeshProUGUI worldNameText = worldNameObj.GetComponent<TextMeshProUGUI>();
                worldNameText.text = worldNames[worldIndex - 1];
            }

            GameObject buttonGridObj = new GameObject("ButtonGrid");
            GridLayoutGroup gridLayout = buttonGridObj.AddComponent<GridLayoutGroup>();
            buttonGridObj.transform.SetParent(verticalContainer, false);
            buttonGridObjects.Add(buttonGridObj);

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 5;
            gridLayout.spacing = new Vector2(30f, 30f);
            gridLayout.cellSize = new Vector2(300f, 150f);
            gridLayout.childAlignment = TextAnchor.MiddleCenter;

            for (int levelIndex = 1; levelIndex <= 5; levelIndex++)
            {
                GameObject buttonObj = Instantiate(levelButtonPrefab, buttonGridObj.transform);
                LevelButton levelButton = buttonObj.GetComponent<LevelButton>();
                levelButton.Initialize(worldIndex, levelIndex);
                levelButtons.Add(levelButton);
            }
        }
    }
}
