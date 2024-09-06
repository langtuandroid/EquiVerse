using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMOD.Studio;
using UnityEngine;

public class NewCompanionSoundController : MonoBehaviour
{
    [Header("FMOD Event")]
    public FMODUnity.EventReference musicEvent;
    private static EventInstance NewCompanionmusic;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Make sure this object persists across scenes

        if (!NewCompanionmusic.isValid())
        {
            CreateMusicInstance(); // Only create instance if it hasn't been created yet
        }
    }

    private void CreateMusicInstance()
    {
        NewCompanionmusic = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        EventDescription volumeDescription;
        NewCompanionmusic.getDescription(out volumeDescription);

    }

    public static void StartMusic()
    {
        if (NewCompanionmusic.isValid())
        {
            PLAYBACK_STATE playbackState;
            NewCompanionmusic.getPlaybackState(out playbackState);

            if (playbackState != PLAYBACK_STATE.PLAYING)
            {
                NewCompanionmusic.start();
            }
        }
        else
        {
            Debug.LogError("Music instance is not valid. Ensure it has been created.");
        }
    }

    public static void StopMusic()
    {
        if (NewCompanionmusic.isValid())
        {
            NewCompanionmusic.stop(STOP_MODE.ALLOWFADEOUT);
            NewCompanionmusic.release();
            NewCompanionmusic = default(EventInstance); // Set to default to allow recreation if needed
        }
        else
        {
            Debug.LogWarning("Attempted to stop music, but the instance was not valid.");
        }
    }

    private void OnDestroy()
    {
        if (NewCompanionmusic.isValid())
        {
            NewCompanionmusic.release();
        }
    }
}
