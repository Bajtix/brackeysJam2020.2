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
        Material mat = collision.gameObject.GetComponent<MeshRenderer>().material;

        if(collision.gameObject.CompareTag("Sliceable"))
        {
            GameObject[] slicedObjects = SliceObject(collision.gameObject, transform.position, new Vector3(-motion.y,-motion.x,-motion.z), mat);
            
            AddRigidbodyAndExplosions(slicedObjects);
            collision.gameObject.SetActive(false);
            
        }
        else if(collision.gameObject.CompareTag("SliceReactor"))
        {
            collision.gameObject.GetComponent<Interactable>().Slice();
        }

  
    }
    public GameObject[] SliceObject(GameObject obj, Vector3 worldPos, Vector3 worldDir, Material crossSectionMaterial)
    {
        return obj.SliceInstantiate(worldPos, worldDir, crossSectionMaterial);
    }

    public void AddRigidbodyAndExplosions(GameObject[] slicedObjects)
    {
        foreach (GameObject obj in slicedObjects)
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            MeshCollider col = obj.AddComponent<MeshCollider>();
            col.convex = true;
            obj.tag = "Sliceable";
            rb.AddExplosionForce(100, transform.position, 20);
        }
    }
}
