using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using FMOD.Studio;
using System.Collections.Generic;

public class World1LevelSoundController : MonoBehaviour
{
    [Serializable]
    public class AudioParameter
    {
        public string name;
        public float value;
        public PARAMETER_ID id;
    }

    [Serializable]
    public class AudioEvent
    {
        public string name;
        public FMODUnity.EventReference eventReference;
        public EventInstance instance;
        public List<AudioParameter> parameters = new List<AudioParameter>();
    }

    public List<AudioEvent> audioEvents = new List<AudioEvent>();

    private void Start()
    {
        InitializeAudioEvents();
        StartCoroutine(StartAmbienceAfterDelay());
    }

    private void InitializeAudioEvents()
    {
        foreach (var audioEvent in audioEvents)
        {
            audioEvent.instance = FMODUnity.RuntimeManager.CreateInstance(audioEvent.eventReference);

            foreach (var parameter in audioEvent.parameters)
            {
                FMOD.Studio.EventDescription eventDescription;
                audioEvent.instance.getDescription(out eventDescription);
                FMOD.Studio.PARAMETER_DESCRIPTION parameterDescription;
                eventDescription.getParameterDescriptionByName(parameter.name, out parameterDescription);
                parameter.id = parameterDescription.id;
            }
        }
    }

    private IEnumerator StartAmbienceAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Adjust delay as needed
        StartAudioEvent("Ambience");
        FadeAudioParameter("Ambience", "World1LevelAmbienceVolume", 1f, 5f);
    }

    public void FadeAudioParameter(string eventName, string parameterName, float targetValue, float duration)
    {
        var audioEvent = audioEvents.Find(e => e.name == eventName);
        if (audioEvent == null)
        {
            Debug.LogError("Event not found: " + eventName);
            return;
        }

        var parameter = audioEvent.parameters.Find(p => p.name == parameterName);
        if (parameter == null)
        {
            Debug.LogError("Parameter not found: " + parameterName + " in event: " + eventName);
            return;
        }

        var tweener = DOTween
            .To(() => parameter.value, x => parameter.value = x, targetValue, duration)
            .SetEase(Ease.Linear)
            .OnUpdate(() => SetParameter(audioEvent.instance, parameter.id, parameter.value)).SetUpdate(true);
    }

    public void StartAudioEvent(string eventName)
    {
        var audioEvent = audioEvents.Find(e => e.name == eventName);
        if (audioEvent != null)
        {
            audioEvent.instance.start();
        }
        else
        {
            Debug.LogError("Event not found: " + eventName);
        }
    }

    public void StartAudioEventAndFadeIn(string eventName)
    {
        StartAudioEvent(eventName);
        FadeAudioParameter(eventName, "World1LevelMainMusicVolume", 1.0f, 1f);
    }

    public void StopAudioEvent(string eventName)
    {
        var audioEvent = audioEvents.Find(e => e.name == eventName);
        if (audioEvent != null)
        {
            audioEvent.instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            Debug.LogError("Event not found: " + eventName);
        }
    }

    private void SetParameter(EventInstance instance, PARAMETER_ID id, float value)
    {
        instance.setParameterByID(id, value);
    }
}
