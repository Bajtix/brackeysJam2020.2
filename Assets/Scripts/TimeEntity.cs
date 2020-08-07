using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeEntity : MonoBehaviour
{
    public Behaviour[] componentsToDisable;

    public bool local = false;

    public bool isInstance = false;

    public bool controlRigidbody = true;
    public string metadata;
    public class TDATA
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public bool active;
        public string additionalData = "";


        public TDATA(Vector3 position, Quaternion rotation, Vector3 scale, bool active = true, string additionalData = "")
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.active = active;
            this.additionalData = additionalData;
        }
    }

    private Rigidbody rb;
    private GameObject statusObject;

    private bool rbKinematic = false;
    public Dictionary<ulong, TDATA> timeData;

    private Vector3 destPos;
    private Quaternion destRot;
    private Vector3 destSiz;

    public bool locked = false;

    public bool canBeLocked = true;
    public Material[] mats;

    private void Start()
    {
        /*mats = new Material[0];
        Debug.Log("Creating material dictionary");
        if(GetComponent<MeshRenderer>() != null)
        {
            mats = new Material[GetComponent<MeshRenderer>().materials.Length];
            for(int i = 0; i< mats.Length; i++)
            {
                mats[i] = GetComponent<MeshRenderer>().materials[i];
                Material wm = Instantiate(mats[i]);
                mats[i] = wm;

                mats[i].SetColor("_EmissionColor", Player.instance.stopGlowColor);
            }
            GetComponent<MeshRenderer>().materials = mats;
        }*/

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
        if (rb != null && controlRigidbody)
        {
            rb.isKinematic = rbKinematic;
        }

        foreach (Behaviour c in componentsToDisable)
        {
            c.enabled = true;
        }

        if (timeData.ContainsKey(frame))
        {
            if (local)
            {
                timeData[frame] = new TDATA(transform.localPosition, transform.localRotation, transform.localScale, gameObject.activeSelf, metadata);
            }
            else
            {
                timeData[frame] = new TDATA(transform.position, transform.rotation, transform.localScale, gameObject.activeSelf, metadata);
            }
        }
        else
        {
            if (local)
            {
                timeData.Add(frame, new TDATA(transform.localPosition, transform.localRotation, transform.localScale, gameObject.activeSelf, metadata));
            }
            else
            {
                timeData.Add(frame, new TDATA(transform.position, transform.rotation, transform.localScale, gameObject.activeSelf, metadata));
            }
        }
    }

    public void Play(ulong frame)
    {

        if (!locked)
        {
            if (rb != null && controlRigidbody)
            {
                rb.isKinematic = true;
            }

            foreach (Behaviour c in componentsToDisable)
            {
                c.enabled = false;
            }

            if (!timeData.ContainsKey(frame))
            {
                if (isInstance)
                {
                    gameObject.SetActive(false);
                }

                return;
            }

            metadata = timeData[frame].additionalData;
            destPos = timeData[frame].position;
            destRot = timeData[frame].rotation;
            destSiz = timeData[frame].scale;

            gameObject.SetActive(timeData[frame].active);
        }
    }

    private void Update()
    {
        if (statusObject != null && Player.instance.GetComponent<Player>().cam.GetComponent<Camera>() != null)
        {
            statusObject.transform.position = Player.instance.GetComponent<Player>().cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
            Vector3 viewPos = Player.instance.GetComponent<Player>().cam.GetComponent<Camera>().WorldToViewportPoint(transform.position);
            if (!(viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0))
            {
                statusObject.SetActive(false);
            }
            else if (gameObject.activeSelf)
            {
                statusObject.SetActive(true);
                statusObject.transform.position = Player.instance.GetComponent<Player>().cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
            }
            else
            {
                statusObject.SetActive(false);
            }
        }

        if (TimeController.instance.playbackMode && !locked)
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

    public void Lock(bool set)
    {
        if (!canBeLocked)
        {
            return;
        }

        if (set)
        {
            statusObject = Instantiate(TimeController.instance.padlockSprite, GameObject.FindObjectOfType<Canvas>().transform);
        }
        else
        {
            Destroy(statusObject);
        }

        Debug.Log("Locking");
        /*if (mats != null)
        {
            if (set)
                foreach (Material m in mats)
                {
                    m.EnableKeyword("_EMISSION");
                }
            else
                foreach (Material m in mats)
                {
                    m.DisableKeyword("_EMISSION");
                }
        }*/

        locked = set;
        foreach (TimeEntity te in transform.GetComponentsInChildren<TimeEntity>())
        {
            te.locked = set;
        }
    }

    private void OnDisable()
    {
        Destroy(statusObject);
    }

    public void SetLock(bool l)
    {
        canBeLocked = l;
    }
}
