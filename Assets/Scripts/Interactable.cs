using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onSlice;
    public UnityEvent onE;

    public virtual void Slice(Vector3 motion)
    {
        onSlice.Invoke();
    }

    public virtual void E()
    {
        onE.Invoke();
    }
}
