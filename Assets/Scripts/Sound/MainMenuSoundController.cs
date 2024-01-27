using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMOD.Studio;
using UnityEngine;

public class MainMenuSoundController : MonoBehaviour
{
    [Header("FMOD Event")]
    public FMODUnity.EventReference musicEvent;
    public FMODUnity.EventReference ambienceEvent;
    public new EventInstance music, ambience;
    
    private PARAMETER_ID mainMenuVolumeParameter;
    private float mainMenuVolumeValue = 0f;
    private Tween mainMenuVolumeTween;

    private void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        music.start();
        
        ambience = FMODUnity.RuntimeManager.CreateInstance(ambienceEvent);
        ambience.start();

        FMOD.Studio.EventDescription volumeDescription;
        music.getDescription(out volumeDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("MainMenuVolume", out volumeParameterDescription);
        mainMenuVolumeParameter = volumeParameterDescription.id;
    }
    
    public void FadeMainMenuVolume(float mainMenuVolumeValue, float duration)
    {
        mainMenuVolumeTween?.Kill();
        if (duration == 0.0f)
        {
            SetMainMenuVolume(mainMenuVolumeValue);
        }
        else
        {
            mainMenuVolumeTween = DOTween
                .To(() => GetMainMenuVolume(), x => SetMainMenuVolume(x), mainMenuVolumeValue, duration)
                .SetEase(Ease.Linear);
        }
    }
    
    public void SetMainMenuVolume(float volume) {
        mainMenuVolumeValue = volume;
        music.setParameterByID(mainMenuVolumeParameter, volume);
        ambience.setParameterByID(mainMenuVolumeParameter, volume);
    }
    
    public float GetMainMenuVolume() {
        return mainMenuVolumeValue;
    }
}
