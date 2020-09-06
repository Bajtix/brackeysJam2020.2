using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ConstantTimeDirector : MonoBehaviour
{
    public PlayableDirector director;
    private TimeEntity te;


    private void Start()
    {

        te = GetComponent<TimeEntity>();
    }
    private void FixedUpdate()
    {
        if (TimeController.instance.playbackMode)
        {
            director.time = double.Parse(te.metadata);
        }
        else
        {
            te.metadata = director.time.ToString();
        }
    }
}
