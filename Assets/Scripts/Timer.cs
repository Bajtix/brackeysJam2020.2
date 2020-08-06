using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public UnityEvent onTimerTick;

    public void SetTimer(float time)
    {
        LeanTween.delayedCall(time, () =>
        {
            onTimerTick.Invoke();
        });
    }
}
