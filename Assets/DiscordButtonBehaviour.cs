using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DiscordButtonBehaviour : MonoBehaviour
{
    private Tween shakeTween;
    
    private void Start()
    {
        shakeTween = DOVirtual.DelayedCall(UnityEngine.Random.Range(3f, 7f), () =>
            {
                transform.DOShakePosition(1.5f, new Vector3(2, 2, 0), 10, 90, false, true);
            })
            .SetLoops(-1, LoopType.Restart);
    }
}
