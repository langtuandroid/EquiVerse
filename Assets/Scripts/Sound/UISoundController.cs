using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundController : MonoBehaviour
{
    public void PlayButtonClickSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ButtonClick");
    }

    public void PlayButtonHoverSound(Button button)
    {
        if (button.interactable)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ButtonHover");
        }
    }

    public void PlayOpeningUIElementSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");    
    }
}
