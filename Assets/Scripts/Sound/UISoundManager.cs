using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public void PlayButtonClickSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ButtonClick");
    }

    public void PlayButtonHoverSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ButtonHover");
    }

    public void PlayBuySound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
    }
}
