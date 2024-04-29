using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriterController : MonoBehaviour
{
    public Febucci.UI.Core.TypewriterCore typewriter;
    public Button button;

    void OnEnable() => typewriter.onMessage.AddListener(OnTypewriterMessage);
    void OnDisable() => typewriter.onMessage.RemoveListener(OnTypewriterMessage);

    void OnTypewriterMessage(Febucci.UI.Core.Parsing.EventMarker eventMarker)
    {
        switch (eventMarker.name)
        {
            case "ActivateButton":
                StartCoroutine(ActivateButton(button));
                break;
        }
    }

    private IEnumerator ActivateButton(Button button)
    {
        yield return new WaitForSeconds(0.5f);
        button.interactable = true;
    }
}
