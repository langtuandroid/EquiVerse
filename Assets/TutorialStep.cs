using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialStep
{
    public string name;
    public UnityEvent onStart;
    public UnityEvent onComplete;
    public UnityEvent onReset;

    [HideInInspector]
    public bool completed;
}