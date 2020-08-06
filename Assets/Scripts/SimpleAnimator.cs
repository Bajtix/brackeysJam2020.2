using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class SimpleAnimator : MonoBehaviour
{
    public void Play()
    {
        GetComponent<Animation>().Play();
    }
}
