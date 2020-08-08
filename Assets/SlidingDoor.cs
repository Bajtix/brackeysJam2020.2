using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SlidingDoor : MonoBehaviour
{
    public Vector3 slide;

    private Vector3 openPos;
    private Vector3 closedPos;

    public float speed = 2;
    private bool opened;
    
    private float progress = 0;

    private void Start()
    {
        closedPos = transform.position;
        openPos = transform.position += slide;
    }

    private void Update()
    {
        progress += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(opened ? closedPos : openPos, opened ? openPos : closedPos,progress);
        
        if(GetComponent<TimeEntity>() != null)
        {
            if (TimeController.instance.playbackMode)
                opened = bool.Parse(GetComponent<TimeEntity>().metadata);
            else
                GetComponent<TimeEntity>().metadata = opened.ToString();
        }
    }

    public void Open()
    {
        progress = 0;
        opened = true;
        GetComponent<AudioSource>().Play();
    }

    public void Close()
    {
        progress = 0;
        opened = false;
        GetComponent<AudioSource>().Play();
    }
}
