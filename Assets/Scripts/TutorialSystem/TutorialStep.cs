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

    [HideInInspector]
    public bool completed;
}