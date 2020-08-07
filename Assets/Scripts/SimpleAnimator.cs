using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleAnimator : MonoBehaviour
{
    public void SetBoolOn(string name)
    {
        GetComponent<Animator>().SetBool(name, true);
    }
    public void SetBoolOff(string name)
    {
        GetComponent<Animator>().SetBool(name, false);
    }
}
