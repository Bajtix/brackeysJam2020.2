using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    public bool toggle;
    public UnityEvent onEnable;
    public UnityEvent onDisable;
    public bool isOn = false;


    private TimeEntity te;

    private void Start()
    {
        te = GetComponent<TimeEntity>();
    }
    public override void E()
    {
        if (TimeController.instance.playbackMode)
        {
            if (te != null)
            {
                isOn = bool.Parse(te.metadata);
            }
        }
        else
        {
            if (te != null)
            {
                te.metadata = isOn.ToString();
            }
        }

        Debug.Log("E");

        if (toggle)
        {
            isOn = !isOn;
        }
        else
            if (!isOn)
            {
                isOn = true;
            }

        GetComponent<Animator>().SetBool("Flipped", isOn);
        if (isOn)
        {
            onEnable.Invoke();
        }

        if (!isOn)
        {
            onDisable.Invoke();
        }
    }
}
