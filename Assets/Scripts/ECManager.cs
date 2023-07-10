using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ECManager : MonoBehaviour
{
    public TextMeshProUGUI totalPointsText;

    [HideInInspector]
    public static float totalPoints;

    public float startingPoints = 10f;

    private float addScoreInterval = 0.25f;

    private void Start()
    {
        totalPoints += startingPoints;
        StartCoroutine(AddScore());
    }

    private IEnumerator AddScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(addScoreInterval);
            totalPoints += ObjectSpawner.generationValue;
            totalPointsText.text = totalPoints.ToString("F1");
        }
    }
}
