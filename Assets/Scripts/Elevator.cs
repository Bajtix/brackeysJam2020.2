using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float[] stops;
    public int destStop;
    private int currentStop;
    private float progress = 0;
    public float elevatorSpeed;

    private void Update()
    {
        if (destStop != currentStop)
        {
            progress += Time.deltaTime * elevatorSpeed;
        }
        else
        {
            progress = 0;
        }

        if (progress >= 1)
            currentStop = destStop;
    }
}
