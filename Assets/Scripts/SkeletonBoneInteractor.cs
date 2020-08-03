using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SkeletonBoneInteractor : Interactable
{
    public override void Slice(Vector3 motion)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        gameObject.AddComponent<MeshFilter>().mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        gameObject.AddComponent<MeshRenderer>().material = GetComponent<SkinnedMeshRenderer>().material;
        Destroy(GetComponent<SkinnedMeshRenderer>());
        GetComponent<Rigidbody>().AddForce(-motion / Time.deltaTime * 50);

        Destroy(this);
    }
}
