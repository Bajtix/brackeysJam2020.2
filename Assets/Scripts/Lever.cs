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

    private void Update()
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

        GetComponent<Animator>().SetBool("Flipped", isOn);
    }
    public override void E()
    {
        

        Debug.Log("E");

        if (toggle)
        {
            isOn = !isOn;
            GetComponent<AudioSource>().Play();
            if (isOn)
            {
                onEnable.Invoke();
            }

            if (!isOn)
            {
                onDisable.Invoke();
            }
        }
        else
        {
            if (!isOn)
            {
                isOn = true;
                GetComponent<AudioSource>().Play();
                onEnable.Invoke();
            }
        }

        
        
    }
}
