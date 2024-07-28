using System;
using TMPro;
using UnityEngine;

public class EcoEssenceRewardsManager : MonoBehaviour
{
    public TextMeshProUGUI ecoEssenceText;
    public static int totalEcoEssence, visualEcoEssence;
    
    private float lastVisualUpdate = 0f;
    private const float updateInterval = 0.01666667f;
    private const int speedFactor = 150;

    private void Start()
    {
        totalEcoEssence = 0;
        visualEcoEssence = totalEcoEssence;
    }

    private void Update()
    {
        UpdateVisualEcoEssence();
    }

    public void UpdateVisualEcoEssence()
    {
        if (Time.time - lastVisualUpdate > updateInterval)
        {
            int difference = totalEcoEssence - visualEcoEssence;
            int changeAmount = (int)(difference * Time.deltaTime * speedFactor);
            changeAmount = Math.Max(1, Math.Abs(changeAmount)); // Ensure at least a change of 1

            visualEcoEssence += Math.Sign(difference) * changeAmount;
            visualEcoEssence = Mathf.Clamp(visualEcoEssence, 0, totalEcoEssence); // Clamp to ensure it doesn't go beyond total

            lastVisualUpdate = Time.time;
            UpdateEcoEssenceText();
        }
    }
    
    public static void IncrementEcoEssence(int amount)
    {
        totalEcoEssence += amount;
    }
    
    void UpdateEcoEssenceText()
    {
        if (ecoEssenceText != null)
        {
            ecoEssenceText.text = visualEcoEssence.ToString();
        }
    }
}