using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundController : MonoBehaviour
{
    public void PlayButtonClickSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ButtonClick");
    }

    public void PlayButtonHoverSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ButtonHover");
    }

    public void PlayOpeningUIElementSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");    
    }
}
