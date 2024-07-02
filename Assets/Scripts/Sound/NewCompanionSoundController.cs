using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMOD.Studio;
using UnityEngine;

public class NewCompanionSoundController : MonoBehaviour
{
    [Header("FMOD Event")]
    public FMODUnity.EventReference musicEvent;
    public new EventInstance music;
    
    private PARAMETER_ID newCompanionMusicVolumeParameter;
    private float newCompanionMusicVolumeValue = 0f;
    private Tween newCompanionMusicVolumeTween;

    private void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        EventDescription volumeDescription;
        music.getDescription(out volumeDescription);
        PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeDescription.getParameterDescriptionByName("NewCompanionMusicVolume", out volumeParameterDescription);
        newCompanionMusicVolumeParameter = volumeParameterDescription.id;
    }
    
    public void NewCompanionMusicVolumeFade(float newCompanionMusicVolumeValue, float duration)
    {
        newCompanionMusicVolumeTween?.Kill();
        if (duration == 0.0f)
        {
            SetNewCompanionMusicVolume(newCompanionMusicVolumeValue);
        }
        else
        {
            newCompanionMusicVolumeTween = DOTween
                .To(() => GetNewCompanionMusicVolume(), x => SetNewCompanionMusicVolume(x), newCompanionMusicVolumeValue, duration)
                .SetEase(Ease.Linear);
        }
    }
    
    public void SetNewCompanionMusicVolume(float volume) {
        newCompanionMusicVolumeValue = volume;
        music.setParameterByID(newCompanionMusicVolumeParameter, volume);
    }
    
    public float GetNewCompanionMusicVolume() {
        return newCompanionMusicVolumeValue;
    }

    public void StartMusic()
    {
        music.start();
        NewCompanionMusicVolumeFade(1.0f, 3f);
    }

    public void StopMusic()
    {
        NewCompanionMusicVolumeFade(0, 4f);
    }
}
