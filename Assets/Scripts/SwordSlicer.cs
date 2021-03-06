﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.PlayerLoop;
using JetBrains.Annotations;

public class SwordSlicer : MonoBehaviour
{

    private Vector3 lastPos;
    private Vector3 motion;

    public Material swordMat;
    public SwordController controller;
    public bool slicing = false;

    

    private void Update()
    {
        if (Player.instance.dead)
            return;
        motion = lastPos - transform.position;
        lastPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Player.instance.dead)
            return;
        if (!slicing)
            return;
        Material mat = /*collision.gameObject.GetComponent<MeshRenderer>().material*/ swordMat;
        if (collision.gameObject.GetComponent<Interactable>() != null)
            collision.gameObject.GetComponent<Interactable>().Slice(motion);
        if (collision.gameObject.CompareTag("Sliceable"))
        {
            GameObject[] slicedObjects = SliceObject(collision.gameObject, transform.position, new Vector3(motion.y,motion.x,motion.z), mat);
            if(collision.gameObject.GetComponent<TimeEntity>() != null)
            {
                if(AddRigidbodyAndExplosions(slicedObjects, collision.gameObject.GetComponent<TimeEntity>().locked)) collision.gameObject.SetActive(false);
            }
            else
                if(AddRigidbodyAndExplosions(slicedObjects)) collision.gameObject.SetActive(false);

        }
        else if(collision.gameObject.CompareTag("SliceDeflector"))
        {
            controller.Deflect();
        }

  
    }
    public GameObject[] SliceObject(GameObject obj, Vector3 worldPos, Vector3 worldDir, Material crossSectionMaterial)
    {
        return obj.SliceInstantiate(worldPos, worldDir, crossSectionMaterial);
    }

    public bool AddRigidbodyAndExplosions(GameObject[] slicedObjects, bool timelocked = false)
    {
        if(slicedObjects == null)
        {
            Debug.LogWarning("Cutting failed");
            return false;
        }
        foreach (GameObject obj in slicedObjects)
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            TimeEntity entity = obj.AddComponent<TimeEntity>();
            entity.componentsToDisable = new Behaviour[0];
            entity.isInstance = true;
            entity.local = true;
            entity.Lock(timelocked);
            MeshCollider col = obj.AddComponent<MeshCollider>();
            col.convex = true;
            obj.tag = "Sliceable";
            rb.AddExplosionForce(100, transform.position, 20);
        }
        return true;
    }
}
