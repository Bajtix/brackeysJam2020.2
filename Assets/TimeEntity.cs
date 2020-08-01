using System.Collections.Generic;
using UnityEngine;

public class TimeEntity : MonoBehaviour
{
    public Behaviour[] componentsToDisable;

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
            timeData[frame] = new TDATA(transform.position, transform.rotation, transform.localScale);
            Debug.Log("swapping frame");
        }
        else
        {
            Debug.Log("adding frame");
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
            transform.position = Vector3.Lerp(transform.position, destPos, Time.deltaTime * 2);
            transform.rotation = Quaternion.Lerp(transform.rotation, destRot, Time.deltaTime * 2);
            transform.localScale = Vector3.Lerp(transform.localScale, destSiz, Time.deltaTime * 2);
        }
    }
}
