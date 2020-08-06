using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onStay;
    public UnityEvent onExit;

    public bool oneTime = false;

    private bool used = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!used)
            if (other.CompareTag("Player"))
            {
                onEnter.Invoke();
                used = oneTime;
            }
    }

    private void OnTriggerStay(Collider other)
    {

            if (other.CompareTag("Player")) { 
                onStay.Invoke();
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!used)
            if (other.CompareTag("Player"))
            {
                onExit.Invoke();
                used = oneTime;
            }
    }
}
