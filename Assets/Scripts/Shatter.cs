using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : Interactable
{
    public GameObject normal;
    public GameObject shattered;

    public bool timeControlled = true;

    private bool isShattered;

    private void Update()
    {
        if (timeControlled)
        {
            if (TimeController.instance.playbackMode)
            {
                isShattered = bool.Parse(GetComponent<TimeEntity>().metadata);
            }
            else
            {
                GetComponent<TimeEntity>().metadata = isShattered.ToString();
            }
        }
        if (isShattered)
        {
            shattered.SetActive(true);
            normal.SetActive(false);
        }
        else
        {
            shattered.SetActive(false);
            normal.SetActive(true);
        }
        
    }


    public override void Slice(Vector3 motion)
    {
        ShatterObject();
        foreach (Rigidbody r in shattered.GetComponentsInChildren<Rigidbody>())
            r.AddForce(motion);
    }

    public void ShatterObject()
    {
        
        isShattered = true;
        if(GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
