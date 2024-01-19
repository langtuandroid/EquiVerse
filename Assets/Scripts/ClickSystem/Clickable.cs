using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clickable : MonoBehaviour
{
    public ParticleSystem gunParticles;
    public abstract void OnClick(Vector3 point);
}
