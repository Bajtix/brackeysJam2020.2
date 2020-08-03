using System.Collections;
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
        motion = lastPos - transform.position;
        lastPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!slicing)
            return;
        Material mat = /*collision.gameObject.GetComponent<MeshRenderer>().material*/ swordMat;
        if (collision.gameObject.GetComponent<Interactable>() != null)
            collision.gameObject.GetComponent<Interactable>().Slice();
        if (collision.gameObject.CompareTag("Sliceable"))
        {
            GameObject[] slicedObjects = SliceObject(collision.gameObject, transform.position, new Vector3(motion.y,motion.x,motion.z), mat);
            if(collision.gameObject.GetComponent<TimeEntity>() != null)
            {
                AddRigidbodyAndExplosions(slicedObjects, collision.gameObject.GetComponent<TimeEntity>().locked);
            }
            else
                AddRigidbodyAndExplosions(slicedObjects);
            collision.gameObject.SetActive(false);          
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

    public void AddRigidbodyAndExplosions(GameObject[] slicedObjects, bool timelocked = false)
    {
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
    }
}
