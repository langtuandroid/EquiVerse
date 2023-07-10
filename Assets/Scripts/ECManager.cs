using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ECManager : MonoBehaviour
{
    public TextMeshProUGUI totalPointsText;

    [HideInInspector]
    public static float totalPoints;

    private float addScoreInterval = 2f;

    private void Start()
    {
        totalPoints += 4;    
    }

    private void FixedUpdate()
    {
        StartCoroutine(AddScore());
    }

    private IEnumerator AddScore()
    {
        totalPoints += ObjectSpawner.generationValue;
        totalPointsText.text = totalPoints.ToString();
        yield return new WaitForSeconds(addScoreInterval);
    }
}
