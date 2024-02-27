using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Color color1;
    public Color color2;
    public float duration = 1f;

    private void Start()
    {
        // Ensure text is visible at the start
        textComponent.color = color1;

        // Create a sequence for blinking effect
        Sequence blinkSequence = DOTween.Sequence();
        blinkSequence.Append(textComponent.DOColor(color2, duration / 2)); // Change to color2
        blinkSequence.Append(textComponent.DOColor(color1, duration / 2)); // Change back to color1
        blinkSequence.SetLoops(-1); // Loop indefinitely
    }
}
