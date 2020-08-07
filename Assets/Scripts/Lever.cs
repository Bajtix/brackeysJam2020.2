using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    public bool toggle;
    public UnityEvent onEnable;
    public UnityEvent onDisable;
    public bool isOn = false;

    public override void E()
    {
        Debug.Log("E");

        if (toggle)
            isOn = !isOn;
        else
            if(!isOn)
                isOn = true;
        GetComponent<Animator>().SetBool("Flipped", isOn);
        if (isOn) onEnable.Invoke();
        
        if (!isOn) onDisable.Invoke();
    }
}
