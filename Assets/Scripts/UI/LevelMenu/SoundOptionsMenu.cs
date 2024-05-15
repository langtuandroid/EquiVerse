using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptionsMenu : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambienceSlider;

    private FMOD.Studio.Bus ambienceBus;
    private FMOD.Studio.Bus musicBus;
    private FMOD.Studio.Bus sfxBus;
    private FMOD.Studio.Bus masterBus;
    
    private string masterVolumeKey = "MasterVolume";
    private string musicVolumeKey = "MusicVolume";
    private string sfxVolumeKey = "SFXVolume";
    private string ambienceVolumeKey = "AmbienceVolume";

    void Start()
    {
        ambienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");

        ambienceSlider.value = PlayerPrefs.GetFloat(ambienceVolumeKey, GetBusVolume(ambienceBus));
        musicSlider.value = PlayerPrefs.GetFloat(musicVolumeKey, GetBusVolume(musicBus));
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolumeKey, GetBusVolume(sfxBus));
        masterSlider.value = PlayerPrefs.GetFloat(masterVolumeKey, GetBusVolume(masterBus));

        SetAmbienceVolume(ambienceSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetMasterVolume(masterSlider.value);

        ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    float GetBusVolume(FMOD.Studio.Bus bus)
    {
        float volume;
        bus.getVolume(out volume);
        return volume;
    }

    void SetAmbienceVolume(float volume)
    {
        ambienceBus.setVolume(volume);
        PlayerPrefs.SetFloat(ambienceVolumeKey, volume);
    }

    void SetMusicVolume(float volume)
    {
        musicBus.setVolume(volume);
        PlayerPrefs.SetFloat(musicVolumeKey, volume);
    }

    void SetSFXVolume(float volume)
    {
        sfxBus.setVolume(volume);
        PlayerPrefs.SetFloat(sfxVolumeKey, volume);
    }

    void SetMasterVolume(float volume)
    {
        masterBus.setVolume(volume);
        PlayerPrefs.SetFloat(masterVolumeKey, volume);
    }
}
