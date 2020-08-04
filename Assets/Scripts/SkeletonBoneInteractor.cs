using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SkeletonBoneInteractor : Interactable
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    MeshRenderer meshRenderer;
    TimeEntity timeEntity;
    Transform ogParent;

    private void Start()
    {
        ogParent = transform.parent;

        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        
        timeEntity = GetComponent<TimeEntity>();
    }

    private void Update()
    {
        if(timeEntity.metadata == "d")
        {
            transform.parent = null;
            if(meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.material = skinnedMeshRenderer.sharedMaterial;
            }
            skinnedMeshRenderer.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            transform.parent = ogParent;
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(meshRenderer);
            skinnedMeshRenderer.enabled = true;
        }
    }

    public override void Slice(Vector3 motion)
    {
        if (timeEntity.metadata != "d")
        {
            timeEntity.metadata = "d";
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(-motion / Time.deltaTime * 50);
            base.Slice(motion);
        }
    }
}
