using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ECManager : MonoBehaviour
{
    public TextMeshProUGUI totalPointsText;
    public TextMeshProUGUI endOfLevelText;
    public Slider slider;

    [HideInInspector]
    public static float totalPoints;
    public float startingPoints = 10f;
    public float lowValuePoints = 10f;
    public float endOfLevelCost = 100f;
    
    private void Start()
    {
        totalPoints += startingPoints;
    }

    private void FixedUpdate()
    {
        totalPointsText.text = totalPoints.ToString();
    }

    public void AddLowValuePoints()
    {
        totalPoints += lowValuePoints;
    }

    public void BuyEndOfLevel()
    {
        
        if (totalPoints >= endOfLevelCost)
        {
            totalPoints -= endOfLevelCost;
            slider.value += 1; 
            if ((int)slider.value == (int)slider.maxValue)
            {
               //logic when level is completed
            }
        }
    }

}
