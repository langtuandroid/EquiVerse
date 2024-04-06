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

    void Start()
    {
        // Get references to the FMOD buses
        ambienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");

        // Set initial slider values to current bus volumes
        ambienceSlider.value = GetBusVolume(ambienceBus);
        musicSlider.value = GetBusVolume(musicBus);
        sfxSlider.value = GetBusVolume(sfxBus);
        masterSlider.value = GetBusVolume(masterBus);

        // Subscribe to slider value change events
        ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    // Function to get the volume of a bus
    float GetBusVolume(FMOD.Studio.Bus bus)
    {
        float volume;
        bus.getVolume(out volume);
        return volume;
    }

    // Function to set the volume of the Ambience bus
    void SetAmbienceVolume(float volume)
    {
        ambienceBus.setVolume(volume);
    }

    // Function to set the volume of the Music bus
    void SetMusicVolume(float volume)
    {
        musicBus.setVolume(volume);
    }

    // Function to set the volume of the SFX bus
    void SetSFXVolume(float volume)
    {
        sfxBus.setVolume(volume);
    }

    // Function to set the volume of the Master bus
    void SetMasterVolume(float volume)
    {
        masterBus.setVolume(volume);
    }
}
