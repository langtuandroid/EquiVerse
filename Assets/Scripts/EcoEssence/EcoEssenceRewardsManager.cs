using System;
using TMPro;
using UnityEngine;

public class EcoEssenceRewardsManager : MonoBehaviour
{
    public TextMeshProUGUI ecoEssenceText;
    public static int totalEcoEssence;

    private int visualEcoEssence;

    private const float updateInterval = 0.01666667f;
    private const int speedFactor = 150;
    
    private DateTime lastVisualUpdate = DateTime.Now;

    private void Start()
    {
        visualEcoEssence = totalEcoEssence;
    }

    private void Update()
    {
        UpdateVisualEcoEssence();
    }

    public void UpdateVisualEcoEssence()
    {
        DateTime now = DateTime.Now;
        if ((now - lastVisualUpdate).TotalSeconds > updateInterval)
        {
            int difference = Math.Abs(visualEcoEssence - totalEcoEssence);
            int changeAmount = Math.Max(1, difference / speedFactor); 

            if (visualEcoEssence < totalEcoEssence) {
                visualEcoEssence = Math.Min(visualEcoEssence + changeAmount, totalEcoEssence);
            } else if (visualEcoEssence > totalEcoEssence) {
                visualEcoEssence = Math.Max(visualEcoEssence - changeAmount, totalEcoEssence);
            }

            lastVisualUpdate = now;
            UpdateEcoEssenceText();
        }
    }

    public static void IncrementEcoEssence(int amount)
    {
        totalEcoEssence += amount;
    }

    private void UpdateEcoEssenceText()
    {
        if (ecoEssenceText != null)
        {
            ecoEssenceText.text = visualEcoEssence.ToString();
        }
    }
}