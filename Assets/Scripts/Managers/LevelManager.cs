using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI currentLevelText;

    private void Start()
    {
        currentLevelText.text = "Level " + GameManager.LEVEL_INDEX + " - " + GameManager.WORLD_INDEX;
    }
}
