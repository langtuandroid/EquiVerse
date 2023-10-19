using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class ECManager : MonoBehaviour
{
    public TextMeshProUGUI totalPointsText;

    [HideInInspector]
    public static float totalPoints;
    public float startingPoints = 10f;
    public float lowValuePoints = 10f;
    
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

}
