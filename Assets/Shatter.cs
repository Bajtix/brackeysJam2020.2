using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : Interactable
{
    public GameObject normal;
    public GameObject shattered;

    

    public override void Slice(Vector3 motion)
    {
        ShatterObject();
        foreach (Rigidbody r in shattered.GetComponentsInChildren<Rigidbody>())
            r.AddForce(motion);
    }

    public void ShatterObject()
    {
        shattered.SetActive(true);
        normal.SetActive(false);

        if(GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
