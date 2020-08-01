﻿using System.Collections.Generic;
using UnityEngine;

public class TimeEntity : MonoBehaviour
{
    public Behaviour[] componentsToDisable;

    public bool local = false;

    public class TDATA
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TDATA(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }

    private Rigidbody rb;
    private bool rbKinematic = false;
    public Dictionary<ulong, TDATA> timeData;

    private Vector3 destPos;
    private Quaternion destRot;
    private Vector3 destSiz;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rbKinematic = rb.isKinematic;
        }

        timeData = new Dictionary<ulong, TDATA>();
        TimeController.instance.timeEntities.Add(this);
    }



    public void Record(ulong frame)
    {
        if (rb != null)
        {
            rb.isKinematic = rbKinematic;
        }

        foreach (Behaviour c in componentsToDisable)
        {
            c.enabled = true;
        }

        if (timeData.ContainsKey(frame))
        {
            if(local)
                timeData[frame] = new TDATA(transform.localPosition, transform.localRotation, transform.localScale);
            else
                timeData[frame] = new TDATA(transform.position, transform.rotation, transform.localScale);
        }
        else
        {
            if (local)
                timeData.Add(frame, new TDATA(transform.localPosition, transform.localRotation, transform.localScale));
            else
                timeData.Add(frame, new TDATA(transform.position, transform.rotation, transform.localScale));
        }
    }

    public void Play(ulong frame)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        foreach (Behaviour c in componentsToDisable)
        {
            c.enabled = false;
        }

        if (!timeData.ContainsKey(frame))
        {
            return;
        }

        destPos = timeData[frame].position;
        destRot = timeData[frame].rotation;
        destSiz = timeData[frame].scale;
    }

    private void Update()
    {
        if (TimeController.instance.playbackMode)
        {
            if (local) 
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, destPos, Time.deltaTime * TimeController.instance.smoothingSpeed);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, destRot, Time.deltaTime * TimeController.instance.smoothingSpeed);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, destPos, Time.deltaTime * TimeController.instance.smoothingSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, destRot, Time.deltaTime * TimeController.instance.smoothingSpeed);
            }
            transform.localScale = Vector3.Lerp(transform.localScale, destSiz, Time.deltaTime * TimeController.instance.smoothingSpeed);
        }
    }
}
