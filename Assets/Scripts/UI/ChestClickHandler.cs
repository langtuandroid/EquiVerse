using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestClickHandler : MonoBehaviour
{
    public CompanionRevealer companionRevealer;
    public void OnObjectClicked()
    {
        companionRevealer.RevealCompanion();
    }

    private void OnMouseDown()
    {
        OnObjectClicked();
    }
}
